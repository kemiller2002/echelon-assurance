---
id: RP-ASR-2026-PAC1
title: Policy-as-code specification — authoritative policy model and generation pipeline
status: draft
version: 0.1.0
owners: [assurance-governance]
created: 2026-11-25
updated: 2026-11-25
research_key: PAC
sources: sources/fragments/PAC.csv
open_questions: registers/fragments/PAC-open-questions.csv
confidence: medium
---

> **Consolidation note (2026-11-25, `JR-ASR-2026-C001`).** The governing requirements are the baseline rows BR-GOV-001 to BR-GOV-006 in `requirements/baseline.csv`. They consolidate PEC-001 to PEC-012, CA-AUTO-01/02 and FIN-BC-022.
>
> Changes to this specification:
> - **Obligation classes.** The five classes (executable decision, configuration constraint, scheduled obligation, human judgment, external obligation) are now a column of every baseline requirement (`enforcement_class`), so the policy model can be generated from the baseline.
> - **Generation.** Generation is required only where it removes duplicated maintenance (BR-GOV-002, *simplify*).
> - **Authorization engine.** F# pure decisions stay the default; Cedar is not adopted (OQ-509).
> - **Precedence.** The order is: mandatory legal and platform safeguards; then Echelon policy; then sponsor policy; then tenant configuration (repository proposal for the full order; PEC-007 and BR-FIN-017 establish only that legal and platform safeguards cannot be weakened by tenant policy). Unresolved conflicts deny or route to review.
> - **Lifecycle.** Approval binds the policy digest. Activation, rollback and expiry are BR-GOV-003.
>
> The first implementation slice is WI-ASR-06. Where this document conflicts with the baseline, the baseline governs.


# Policy-as-code specification

> **Status: research draft.** Nothing here is implemented. The F# types below
> are a *specification* written in F# notation, not a buildable project. No
> control or policy is claimed as operating. Evidence classes are tagged
> `[class]` after each significant conclusion; source IDs are `SRC-PAC-NNN`
> (see `sources/fragments/PAC.csv`).

## 1. Problem

Echelon Foundry needs one place where a policy's *intent* (purpose, scope,
owner, approval, mandatory controls, exceptions) is bound to its *mechanisms*
(the GitHub ruleset, the cloud guardrail, the in-app authorization rule, the
CI check) and to the *evidence* that shows the mechanism operated. The failure
mode we are designing against is the conventional one: a Word policy, a
separate GRC spreadsheet, a separate set of Rego/SCP/ruleset files and a
separate evidence folder, each edited independently, drifting silently
[industry-convention].

Two requirements pull in opposite directions:

1. **One source of truth** for each fact (no duplicated definitions).
2. **Many runtimes** that each demand their own language (Rego for OPA, Cedar
   for Cedar, JSON for SCPs / Azure Policy / GCP Org Policy, YAML for GitHub
   rulesets and Kyverno, F# for Echelon applications) [authoritative —
   SRC-PAC-003, -007, -010, -011, -013, -014, -015].

The resolution proposed here is to divide *which fact lives where*, not to
generate everything from one file (§6).

## 2. Existing Echelon material reused

A grep of the read-only Praxis, Tutela, Ordo and Aegis checkouts (2026-11-25)
found policy and evidence representations that should be reused rather than
re-invented [authoritative for "what the code says"; SRC-PAC-039..041]:

| Echelon artifact | What it already does | Reuse here |
|---|---|---|
| Praxis `Autonomy/Model.fs` `ExecutionPolicy` (`praxis.autonomous-execution-policy/1`) | Versioned policy (`PolicyVersion`), `ProtectedClass` DU (security policy, execution policy, workflows, repository rules, credentials, permission expansion, infrastructure), `MergeRequirements` with `CheckProducer` (only a named GitHub App's checks count) and `IndependentReviewCheck` | Pattern for a versioned, typed policy whose evaluation is a pure function; `ProtectedClass` is the model for "changes to policy-as-code itself are never merged unattended" |
| Praxis `Autonomy/Authorization.fs` | Canonical, length-framed signing payload including `PolicyVersion` and `PolicyDigest` | Model for binding an approval to an exact policy digest (§3.4) |
| Praxis `Execution/Governance.fs` `EvaluatorIdentity` (fingerprint of evaluator inputs) and `ExpectedReceipt`/`ObservedReceipt` | Records *which* evaluator version judged, and compares expected vs observed facts | Model for `policy_version` + evaluator fingerprint in the evidence envelope |
| Tutela `Domain.fs` `Evidence`, `Provenance`, `AuthorityPolicy` (deny-by-default), `ExceptionRecord` + `ExceptionRejection` (`SelfApproval`, `ApproverNotHuman`, `Expired`, …) | Evidence with digest/provenance; exceptions validated against a role registry | Exception model (§3.3) and evidence-authority rule are adopted almost verbatim |
| Ordo `Evidence.fs` `EvidenceKind = Direct \| Derived \| Inferred` | "Evidence being available is not evidence being true"; a model's inference is never an observation | Adopted in the evidence envelope `collection.kind` |

None of these is a general organizational-policy model; the gap is the
policy-level record that binds intent → controls → mechanisms → evidence
[recommendation].

## 3. F# authoritative policy model (specification)

Design rules (inherited from Echelon F# practice observed in Praxis/Tutela):
discriminated unions for closed vocabularies, private constructors with smart
`create` functions returning `Result`, no nulls, pure validation, wire
serialization by explicit `toWire` functions (stable strings), deterministic
canonical form for hashing.

### 3.1 Identity and references

```fsharp
namespace Echelon.Assurance.Policy

open System

/// POL-NN or POL-NN.NN (methodology/identifiers.md). Smart-constructed.
type PolicyId = private PolicyId of string
type ControlId = private ControlId of string          // CTL-<DOMAIN>-NNN
type RequirementId = private RequirementId of string  // REQ-<FW>-NNN
type EvidenceTypeId = private EvidenceTypeId of string // EVT-NNN
type ValidationId = private ValidationId of string    // VAL-NNN
type SystemId = private SystemId of string            // SYS-<NAME>
type ExceptionId = private ExceptionId of string
type RoleId = private RoleId of string                // a role, never a person

/// Semantic version of the policy text and bindings. Major = obligation
/// change (re-approval + re-acknowledgement); minor = mechanism/evidence
/// change (re-approval); patch = editorial (owner only).
type PolicyVersion = { Major: int; Minor: int; Patch: int }

/// SHA-256 over the canonical serialization of the Policy record
/// excluding ChangeHistory and Approvals (which reference it).
type PolicyDigest = private PolicyDigest of string

type AuthoritativeReference =
    | Requirement of RequirementId                  // regulatory/requirements.csv
    | Decision of string                            // DF-ASR-2026-xxxx
    | ExternalStandard of publisher: string * reference: string * sourceId: string // SRC-*
```

### 3.2 Scope, applicability, obligations

```fsharp
type ScopeDimension =
    | Systems of SystemId list
    | Environments of Environment list
    | DataClasses of DataClass list
    | Personnel of PersonnelClass list        // employees, contractors, agents
    | Tenants of TenantScope
and Environment = Production | Staging | Development | Evidence
and DataClass = Public | Internal | Confidential | Restricted | PersonalData | FinancialRecord
and PersonnelClass = Employee | Contractor | AutonomousAgent | ServiceIdentity
and TenantScope = AllTenants | TenantClass of string

type Scope =
    { Includes: ScopeDimension list
      Excludes: ScopeDimension list
      Rationale: string }                       // why the boundary is here

/// How a mandatory control is enforced. One policy obligation may have
/// several mechanisms (defence in depth). The mechanism record never holds
/// the rule logic of an external engine; it holds a *binding* (§6.2).
type EnforcementMechanism =
    | FSharpDecider of module: string * function: string * testSuite: string
    | CedarPolicySet of path: string * schemaPath: string * digest: string
    | RegoBundle of path: string * entrypoint: string * digest: string
    | GitHubRuleset of org: string * rulesetName: string * expectedSpecPath: string
    | CloudGuardrail of provider: CloudProvider * kind: GuardrailKind * specPath: string
    | KubernetesAdmission of engine: string * specPath: string
    | CiCheck of workflowPath: string * checkName: string * producerApp: string
    | ProcessStep of runbookPath: string             // L0/L1 manual control
and CloudProvider = Aws | Azure | Gcp
and GuardrailKind =
    | AwsScp | AwsRcp | AwsConfigRule | AzurePolicyDeny | AzurePolicyAudit
    | GcpOrgPolicy | ObjectLockRetention

type FailureMode = FailClosed | FailOpenJustified of rationale: string | DetectiveOnly
type AutomationLevel = L0 | L1 | L2 | L3 | L4 | L5   // controls/README.md

type MandatoryControl =
    { Control: ControlId
      Statement: string                        // the obligation, normative "must"
      Enforcement: EnforcementMechanism list
      Validation: ValidationMechanism list
      Evidence: EvidenceRequirement list
      FailureMode: FailureMode
      AutomationTarget: AutomationLevel }

and ValidationMechanism =
    | ContinuousCheck of evaluator: string * frequency: TimeSpan
    | PopulationReconciliation of source: string * sink: EvidenceTypeId
    | PeriodicTest of ValidationId
    | IndependentTest of ValidationId * independence: Independence
and Independence =
    | SelfAssessment | Automated | InternalIndependent
    | ExternalTechnical | ExternalPenetration | ExternalAttestation

and EvidenceRequirement =
    { EvidenceType: EvidenceTypeId
      Cadence: EvidenceCadence
      Completeness: CompletenessRule
      RetentionClass: string
      AccessClass: string }
and EvidenceCadence = PerEvent | Periodic of TimeSpan | OnChange
and CompletenessRule =
    | EveryEventInPopulation of populationSource: string   // e.g. GitHub merged PRs
    | HeartbeatWithin of TimeSpan
    | SampleAccepted of rationale: string                   // only with auditor agreement
```

### 3.3 Exceptions (adopted from Tutela `ExceptionRecord`)

```fsharp
type ExceptionRequest =
    { Id: ExceptionId
      Policy: PolicyId
      Covers: ControlId list
      Scope: Scope
      Rationale: string
      CompensatingControls: ControlId list
      RequestedBy: Identity
      RequestedAt: DateTimeOffset
      ExpiresAt: DateTimeOffset                 // mandatory; no open-ended exceptions
      RiskAcceptance: RoleId }

type ExceptionDecision =
    | Honored of ExceptionRequest * approvedBy: Identity * approvedAt: DateTimeOffset
    | Rejected of ExceptionRequest * ExceptionRejection list
and ExceptionRejection =
    | Expired | NotYetValid | IncompleteRecord | SelfApproval
    | ApproverNotHuman | ApproverLacksRole | ExceedsMaximumDuration
and Identity =
    | Human of githubId: uint64 * login: string
    | Agent of agentId: string * principal: Identity  // agent always bound to a human principal
    | Workload of issuer: string * subject: string    // e.g. OIDC workflow identity
```

`ExceptionDecision` is produced by a pure function
`evaluateException : RoleRegistry -> DateTimeOffset -> ExceptionRequest -> ExceptionDecision`
(the Tutela pattern). An agent can never approve (`ApproverNotHuman`) and a
requester can never approve their own exception (`SelfApproval`)
[recommendation; mirrors SRC-PAC-040].

### 3.4 The policy record

```fsharp
type ReviewRequirement =
    { Cycle: TimeSpan                          // e.g. annual
      Triggers: ReviewTrigger list
      Reviewer: RoleId }
and ReviewTrigger =
    | RequirementChanged of RequirementId
    | IncidentInScope
    | ExceptionCountAbove of int
    | MechanismRetired

type RemediationRequirement =
    { Severity: string                          // severity class, not finding detail
      DetectToContain: TimeSpan
      DetectToRemediate: TimeSpan
      Escalation: RoleId list }

type Approval =
    { Version: PolicyVersion
      Digest: PolicyDigest                      // approval binds exact content
      ApprovedBy: Identity                      // must be Human, holding ApprovalAuthority role
      ApprovedAt: DateTimeOffset
      SignatureRef: string }                    // signed commit / signed tag / Sigstore bundle

type ChangeRecord =
    { From: PolicyVersion option
      To: PolicyVersion
      Summary: string
      PullRequest: string                       // URL of the reviewed PR
      Classification: ChangeClass }
and ChangeClass = Obligation | Mechanism | Evidence | Editorial

type Policy =
    { Id: PolicyId
      Version: PolicyVersion
      Title: string
      Owner: RoleId
      Purpose: string                           // authored prose (not generated)
      Scope: Scope
      ApplicableSystems: SystemId list
      AuthoritativeReferences: AuthoritativeReference list
      EffectiveDate: DateOnly
      Review: ReviewRequirement
      MandatoryControls: MandatoryControl list
      ApprovalAuthority: RoleId
      Remediation: RemediationRequirement list
      Exceptions: ExceptionId list              // live exceptions held in exception register
      Narrative: Map<string, string>            // authored sections keyed by heading
      Approvals: Approval list
      ChangeHistory: ChangeRecord list }
```

Note on fields requested by the charter: *enforcement mechanisms*, *validation
mechanisms* and *evidence requirements* live **per mandatory control**, not
per policy, because one policy carries controls with different mechanisms.
Policy-level views are projections.

### 3.5 Pure validation (sketch)

```fsharp
module Policy =
    /// Structural and referential checks; no I/O. Every rule has a stable code.
    val validate : Registers -> Policy -> Result<Policy, PolicyFinding list>
    // e.g. PAC-V001 every MandatoryControl references an existing CTL id
    //      PAC-V002 FailClosed controls have at least one L4+ mechanism
    //      PAC-V003 every EvidenceRequirement names an existing EVT id
    //      PAC-V004 latest Approval.Digest = digest of current content
    //      PAC-V005 Approval.ApprovedBy is Human and holds ApprovalAuthority
    //      PAC-V006 no Exception without ExpiresAt; none expired but listed live
    //      PAC-V007 Major version bump ⇒ ChangeClass.Obligation present
    val digest : Policy -> PolicyDigest
```

## 4. Can one definition generate everything? (honest evaluation)

| Output | Generate from model? | How / limit | Class |
|---|---|---|---|
| Human policy document | **Yes**, with authored prose | Render Markdown/PDF from `Narrative` + structured fields. Prose (purpose, rationale) is *authored inside the model*, not generated | recommendation |
| Auditor documentation | **Mostly** | Policy→control→mechanism→evidence traceability matrix, approval history, exception list. Auditors still need system descriptions written by management (SOC 2 description criteria) — authored, not inferred | recommendation; internal design approved: the versioned approved policy model is authoritative and readable documents are generated (OQ-501; DF-ASR-2026-0D11); auditor acceptance pending |
| Enforcement rules | **Only for parametric rules** | Simple, parameterised guardrails (allowed regions, required tags, retention days, required checks list) can be emitted as SCP/Azure Policy/Org Policy/ruleset JSON. Non-trivial logic (authorization, data-dependent rules) must be authored in its target language and *bound by digest* (§6.2). A Turing-complete DSL that compiles to Rego **and** Cedar **and** SCP is a new language with its own verification burden — rejected | recommendation |
| CI checks | **Yes** | Generate a conformance job that checks every binding: file exists, digest matches, tests exist, ruleset live state equals expected spec | recommendation |
| Agent instructions | **Yes, but as advisory only** | Emit AGENTS.md fragments ("changes to X require Y"). Instructions are not controls; the same obligation must have a non-agent enforcement point (Praxis `ProtectedClass`, rulesets) | recommendation |
| Runtime configuration requirements | **Yes** as expected-state specs | Desired-state documents consumed by AWS Config / Azure Policy audit / GitHub ruleset drift checks | recommendation |
| Dashboards | **Yes** from model + evidence | Coverage = controls with fresh, complete, verified evidence. "Unknown" never rendered as pass (Specula rule) | recommendation |
| Evidence collection requirements | **Yes** | `EvidenceRequirement` → collector configuration + completeness rule | recommendation |
| Control tests | **Partly** | Test *specifications* (what, population, frequency, pass criterion) are generated; test *logic* is code | recommendation |
| Customer assurance documents | **Partly** | Filtered projection (access class `customer-shareable`) of policy summaries and control statements; never auto-publishes status claims above what evidence supports (AGENTS rule 3) | recommendation |

Conclusion: a single model can be the source for **intent, bindings,
traceability, evidence requirements and every document**, and for the
*parameters* of simple guardrails. It should **not** attempt to be the source
of complex enforcement logic [recommendation, confidence medium].

## 5. Tool evaluation summary

Full detail in [`tool-evaluation.md`](tool-evaluation.md).

| Tool | Role it can play | Fit for Echelon | Verdict |
|---|---|---|---|
| **OSCAL** (catalog, profile, mapping, component-definition, SSP, assessment-plan, assessment-results, POA&M) | Exchange format for control catalogs, implementation statements and assessment results | Strong as an **output**; heavy as an authoring format; FedRAMP direction but not named as mandatory in NTC-0009 | **Integrate (emit) when justified** (OQ-506: portable structured exports; OSCAL when justified, DF-ASR-2026-0D11) |
| **OPA / Rego** | General decision engine (Kubernetes, CI, API gateways, Terraform plans) | Good for CI/infra checks over JSON; not needed for F# in-app decisions | **Integrate where an enforcement point already speaks Rego**; do not adopt as the policy model |
| **Cedar** | Application authorization language, analyzable, formally modelled in Lean | Strong candidate for tenant/customer-facing authorization where policies must be analysed or delegated; F# has no first-party binding (Rust core, WASM, Go port) | **Evaluate by prototype** for in-app authz (OQ-509) |
| **Kyverno** | Kubernetes-native admission/validation, CEL | Only if Echelon runs Kubernetes; not observed | **Not now** |
| **Cloud-native** (SCP/RCP, AWS Config, Azure Policy, GCP Org Policy) | Preventive and detective cloud guardrails | Required at the cloud layer regardless; they are the enforcement points | **Adopt as enforcement points**, bound from the model |
| **GitHub rulesets / branch protection / push protection** | SDLC enforcement | Already central to Praxis; required checks must pin producer app | **Adopt as enforcement points** |
| **compliance-trestle** (OSCAL Compass, CNCF sandbox, originally IBM) | OSCAL authoring/validation in git CI | Python; useful to *validate* emitted OSCAL; could replace our emitter if Echelon chooses OSCAL-first | **Use as validator of emitted OSCAL** |
| **Limited F# model** | Authoritative intent + bindings + evidence requirements | Matches Echelon F#-first, Praxis/Tutela patterns, pure validation | **Adopt (this spec)** |

## 6. Recommendation: F# model as authority, emit standards, bind engines

### 6.1 Fact ownership (no duplicated source of truth)

| Fact | Single owner | Others hold |
|---|---|---|
| Requirement text and applicability | `regulatory/requirements.csv` | IDs only |
| Control objective, owner, automation target | `controls/control-catalog.csv` | IDs only |
| Policy intent, scope, approvals, exception rules, control bindings, evidence requirements | **F# policy model** (serialized canonical JSON committed alongside) | — |
| Simple guardrail parameters (regions, retention days, required check names) | **F# policy model** → emitted to cloud/GitHub JSON | Generated files marked `GENERATED — do not edit` and checked by CI for drift |
| Complex decision logic (Cedar/Rego/F# deciders) | **The rule file in its native language** | Policy model holds path + digest + test suite reference only |
| Live enforcement state (what GitHub/AWS actually enforce) | The platform | Evidence store holds observed snapshots; drift = finding |
| OSCAL documents | **Generated** from model + registers + evidence | Never hand-edited |

This answers the duplication problem: each fact has exactly one owner; the
model *references* engine-native rules by digest instead of re-expressing them
[recommendation].

### 6.2 Binding rather than generating complex rules

A binding is `(path, digest, test suite, owner)`. CI fails closed if:
the file at `path` hashes to something other than `digest` without a model
version bump; the test suite is missing or failing; or a rule file exists in an
enforcement directory with no binding (orphan rule). Changing a bound rule thus
forces a policy-model change, which forces review under the policy's approval
authority. This reproduces Praxis's `ProtectedClass` idea at policy level
[recommendation].

### 6.3 F# deciders vs Cedar for in-app authorization

| Criterion | F# pure decider (`decide : Command -> State -> Result<Event list, Rejection>`) | Cedar |
|---|---|---|
| Language fit | Native; shares domain types; compiler-checked exhaustiveness | Separate language; F# would call Rust via FFI/WASM or a service — no first-party .NET SDK observed [vendor-claim/observation SRC-PAC-008] |
| Formal properties | Property-based tests (FsCheck); no proof of language semantics | Lean-modelled semantics with mechanized proofs; validator soundness proved; differential testing of Rust vs model [academic SRC-PAC-005, -006] |
| Policy analysis ("does change X widen access?") | Must be built per policy | Supported by design via sound and complete logical encoding [academic SRC-PAC-005] |
| Delegation to customers (tenant-authored rules) | Unsafe without a sandboxed DSL | Designed for it; default-deny, forbid-overrides-permit [authoritative SRC-PAC-007] |
| Error semantics | Explicit `Result`; fail-closed by construction | Erroring policies are **skipped** (not counted); with default-deny this fails closed for `permit` errors but a **`forbid` that errors does not deny** — must be monitored via diagnostics [authoritative SRC-PAC-007] |
| Operational dependency | None | Rust/WASM runtime or Amazon Verified Permissions |

Recommendation: use **F# deciders** for domain state-transition legality
(Ordo's territory) and internal authorization where policies are fixed by
Echelon; consider **Cedar** only where (a) customers or tenant admins author
permissions, or (b) an auditor/customer values analyzable policy diffs. If
Cedar is adopted, the policy model binds the Cedar policy set by digest and the
CI gate runs Cedar's validator against the schema [recommendation; OQ-509].

### 6.4 OSCAL emission

Map model → OSCAL as follows (no OSCAL authoring by hand):

| OSCAL model | Generated from |
|---|---|
| Catalog | Echelon control catalog (`controls/control-catalog.csv`) as an Echelon catalog |
| Mapping (added in OSCAL 1.2.0) | `requirement_ids` ↔ `control_ids` links to external catalogs (e.g., NIST SP 800-53) |
| Profile | Framework-selected subsets (SOC 2 scope, customer-specific baselines) |
| Component definition | One per Echelon system (`SYS-*`) from mandatory-control bindings |
| SSP | Policy scope + component definitions + authored narrative |
| Assessment plan | `ValidationMechanism` entries + `VAL-` procedures |
| Assessment results | Evidence envelopes (validation status) for a period |
| POA&M | Remediation register entries (restricted; never in this public repo) |

Validate emitted OSCAL with NIST schemas and/or compliance-trestle in CI
[recommendation]. Whether any customer requires OSCAL is unknown; the approved internal design is
portable structured exports, with OSCAL when justified (OQ-506; DF-ASR-2026-0D11);
FedRAMP's 2026 notice requires machine-readable data and that human-readable
materials be *generated from* machine-readable data, but does not itself name
OSCAL [authoritative SRC-PAC-020]. That direction independently supports the
"generate documents from the model" design [recommendation].

## 7. Generation pipeline

```
 policies/model/*.fsx|json ──► validate (pure, PAC-V0xx) ──► canonical JSON + digest
          │                                                          │
          ├─► render: policy docs (MD/PDF), auditor traceability matrix, customer extract
          ├─► emit:   OSCAL catalog/profile/mapping/component/SSP/AP  ──► schema + trestle validate
          ├─► emit:   guardrail params → SCP/Azure Policy/GCP Org Policy/GitHub ruleset JSON
          ├─► emit:   evidence-collector config + completeness rules
          ├─► emit:   AGENTS.md fragments (advisory)
          └─► check:  bindings (path+digest+tests) for Cedar/Rego/F# deciders
                                     │
 CI (required check, producer app pinned) fails closed on any finding
                                     │
 merge to main requires approval-authority review (CODEOWNERS/ruleset)
                                     │
 post-merge: signed release of the policy bundle (digest) → evidence envelope
             EVT "policy-version-published"; drift monitors compare live platform
             state with emitted specs → EVT "policy-drift-observation"
```

Properties: deterministic (same input → byte-identical output, so generated
files can be verified rather than trusted); generated files carry the model
digest in a header; drift between emitted spec and live state is evidence, not
silently corrected (L5 auto-remediation only where the control row justifies
it) [recommendation].

## 8. Risks introduced by this design

| Risk | Mitigation |
|---|---|
| Model becomes a bespoke DSL nobody else can read | Keep it a data model (records/DUs), not a language; emit OSCAL for interchange |
| Generator bug produces wrong enforcement config | Golden tests; emitted guardrails reviewed in PR diff; drift monitor compares live vs intended |
| "Generated" mistaken for "operating" | Dashboards derive status only from evidence envelopes; AGENTS rule 3 |
| Auditor unfamiliar with policy-as-code approvals | Approval = signed commit/tag binding digest; produce human-readable approval record; confirm acceptance (OQ-501) |
| Cedar error-skipping semantics weaken `forbid` | Treat any Cedar evaluation diagnostic error as a deny at the enforcement point (wrapper fails closed) |

## 9. Contradictions and uncertainties preserved

- OSCAL is described by NIST as the standard machine-readable control format,
  and FedRAMP is moving to machine-readable packages, yet NTC-0009 does not
  name OSCAL as required and FedRAMP "will not build or operate tools"
  [authoritative SRC-PAC-001, -020]. OSCAL adoption is therefore a bet on
  direction, not a present requirement for Echelon.
- OPA governance: OPA's core maintainers moved from Styra to Apple in August
  2025; the project states maintainers and governance are unchanged and OPA
  remains CNCF graduated. Commentary disagrees about the future of Styra's
  commercial products [summary-only SRC-PAC-004; vendor/news commentary not
  registered].
- Cedar paper venue: the language paper is OOPSLA 2024 (PACMPL, DOI
  10.1145/3649835); the *verification-guided development* paper is FSE
  Companion 2024, not OOPSLA [academic SRC-PAC-005, -006]. The OOPSLA paper's
  SRG seminar summary mentions Dafny models, while the arXiv v2 abstract says
  Lean — the project moved from Dafny to Lean; treat Lean as current
  [academic; low-impact discrepancy].

## Owner requirements PEC-001..012 (2026-11-25)

`requirements/executable-policy-and-continuous-evidence.md` confirms this specification's direction: one authoritative F#
model (PEC-001), explicit classification of obligations and of what is not
executable (PEC-002, new CTL-GOV-014), pure deterministic deciders with
policy version in every decision (PEC-003), generation with drift detection
and links for non-generated content (PEC-004), signed approval bound to the
policy digest (PEC-005), and operation without AI services (PEC-012). OQ-509
disposition (inferred from the owner's in-principle baseline approval,
DF-ASR-2026-0D10; the owner did not answer the question specifically): prefer F# pure deciders; Cedar only if a demonstrable
assurance or interoperability benefit outweighs integration cost; no new
runtime dependency is approved. OSCAL remains a later adapter (OQ-506).
