---
id: GV-ASR-VAL-001
title: Independent validation architecture
status: draft
version: 0.1.0
owners: [assurance-governance]
created: 2026-11-25
updated: 2026-11-25
review_cycle: semi-annual
research_batch: B6 (key AIG)
---

> **Consolidation note (2026-11-25, `JR-ASR-2026-C001`).** The independent validation methodology is now:
> - **Status ladder:** BR-ASR-002.
> - **Role separation:** BR-VAL-001.
> - **Risk tiers:** BR-VAL-002.
> - **External assurance trigger:** BR-VAL-003.
> - **Validating the machinery:** BR-ASR-010.
>
> **Key controls for organizational review.** The controls proposed (not owner-approved) for organizationally independent review are listed in `docs/decisions/owner-decision-package-2026-11-25.md` (ODP-02). All others use automated and internal validation proportionate to risk. Blanket independent human review of routine operations is not required.
>
> **Small-team constraint.** With one human, `internal-independent` validations (VAL-006, VAL-011, VAL-012, VAL-016, VAL-019, VAL-022, VAL-025, VAL-032) have no independent performer today. They are not performed as if independent. Under owner direction DF-ASR-2026-0D0F, `Tested` rests instead on protected deterministic verification (VAL-040, `technically-separated`) for executable controls. The multiple-agent review protocol (VAL-041, `agent-review`) is approved for internal development review only and never supports `Tested`, so judgment controls (L0–L2) cannot reach `Tested` until a technically separated or organizational validator exists. Technical separation is not described as organizational independence (see `controls/README.md`). VAL-025 is used only if an organizationally independent reviewer is later engaged on a recorded trigger.
>
> **Continuous security testing** is specified in `validation/continuous-security-testing-specification.md`.


# Independent validation architecture

**Status.** Research draft. No validation procedure here has been executed.
Validation procedures are `VAL-001..032` in
`registers/fragments/AIG-validation-procedures.csv`; risks `RSK-0xx` in
`registers/fragments/AIG-risks.csv`; sources `SRC-AIG-*`. Resilience of the
assurance system itself is in
[`assurance-system-resilience.md`](assurance-system-resilience.md).

## 1. The question

A control is worth what its validation can show. Automated validation is
cheap and continuous but is built and run by the same organization that owns
the control. The architecture below separates **how much a test can find**
(the layers, §4) from **how much anyone outside should believe it** (the
independence level, §2). Both are recorded for every result.

## 2. Independence spectrum

The levels are the `independence_level` enum in `src/Echelon.Assurance.Registers/Schema.fs`.

| Level | Performed by | What it can support | What it must never be called | Evidence class of the distinction |
|---|---|---|---|---|
| `self-assessment` | Control owner (human or the owner's agents) | Readiness; management's own monitoring | "tested", "validated", "independent" | recommendation |
| `automated` | Echelon-controlled pipelines and agents | Continuous detection; management monitoring; evidence that a test ran | "independent", "audited" | recommendation |
| `agent-review` | Separately initialized AI agents under the multiple-agent review protocol (VAL-041, `methodology/review-protocol.md`) | Internal development review only (DF-ASR-2026-0D0F); never `Tested` | "independent", "organizational review", "tested" | recommendation |
| `technically-separated` | Echelon-controlled verifier with separate credentials and bound receipts (VAL-040, Dokimos planned under WI-DOKIMOS-01) | Deterministic re-execution supporting `Tested` for executable controls | "independent", "organizationally independent" | recommendation |
| `internal-independent` | Echelon person outside the implementing/operating team, with separate credentials | Internal assurance comparable to internal audit; NIST AI RMF MEASURE 1.3 "internal experts who did not serve as front-line developers" (REQ-AIRMF-006) | "external", "attestation" | authoritative (for the AI RMF basis) |
| `qualified-professional` | Qualified professional (for example counsel or a CPA) engaged on a recorded trigger; none is engaged now (DF-ASR-2026-0D0F) | Professional judgment within the engagement scope | "attestation" unless it is one | recommendation |
| `external-technical` | Third party engaged for a technical review (not a CPA attestation) | Expert opinion on design/operation of specific mechanisms | "audit", "attestation", "certification" | recommendation |
| `external-penetration` | Independent penetration testing firm | Point-in-time adversarial findings | evidence of operating effectiveness over a period | industry-convention |
| `external-attestation` | Independent CPA firm under AICPA attestation standards (SOC 1/SOC 2) | Opinion on controls over a period, for report users | — | authoritative (attestation standards; owned by SOC batch) |

Rule: a result's level is set by **who controlled the test design, the
credentials and the evidence path**, not by who ran the button. An external
firm executing an Echelon-written script against Echelon-supplied evidence
is not external validation of that evidence.

## 3. Why Echelon-controlled AI agents are not independent auditors

*(auditor-interpretation for the AICPA concepts, which were read only via
secondary sources SRC-AIG-021; recommendation for the application.)*

1. **Self-review threat.** The AICPA Conceptual Framework for Independence
   (ET 1.210.010) names self-review — evaluating one's own work — as a threat,
   and some self-review threats cannot be reduced by safeguards. An Echelon
   agent validating an Echelon control is the organization reviewing its own
   work, regardless of which agent instance runs.
2. **Management-participation threat.** Agents operate on management's
   instructions, with management's credentials and objectives. They are an
   instrument of management, i.e., they *are* the control environment, not an
   observer of it.
3. **Same control owner.** The agents, their prompts, their models, their
   pipelines and their pay-masters are owned by the party whose assertions
   are being evaluated. Changing the model provider changes the tool, not the
   owner.
4. **Professional obligations do not transfer.** Attestation requires a
   licensed practitioner bound by independence rules, quality management and
   professional liability. Per the reported content of AICPA TQA 9561, even
   a service auditor using AI remains responsible for procedure design,
   evidence evaluation and the opinion (SRC-AIG-022, summary-only).
5. **Correlated failure.** Agents built on similar models share blind spots;
   an agent validating another agent's work can share its errors (RSK-022).
6. **Analogy from issuer audits.** PCAOB's 2024 amendments to AS 1105/2301 on
   technology-assisted analysis address the reliability of information
   analysed by tools even when the auditor controls the tool
   (SRC-AIG-020, summary-only). An entity-controlled tool is further from
   independence, not closer.

Therefore: Echelon agents can raise the *quality and coverage* of
management's monitoring and reduce the cost of external work; they cannot
substitute for it. Whether the auditor will use any of it is pending auditor
acceptance (OQ-601, OQ-604; internal design approved, DF-ASR-2026-0D11: the
original source remains authoritative, and mutation and synthetic tests are
management monitoring and internal validation).

## 4. Layered validation strategy

| Layer | What it detects | Echelon example | VAL | Typical independence | Cost |
|---|---|---|---|---|---|
| Unit | Predicate logic errors | `requester != approver` predicate, missing-data → `insufficient-evidence` | VAL-001 | automated | low |
| Integration | Broken evidence chain | Action → evidence record → signature → hash chain → evaluator | VAL-002 | automated | low |
| Property-based | Edge cases humans don't write | Ledger always balances; no generated approval sequence lets an agent approve (cf. QuickCheck, SRC-AIG-029) | VAL-003 | automated | low–medium |
| Negative security | Prohibited action succeeds | Each hard prohibition P1–P8 attempted with real agent identity; expect denial + alert | VAL-004 | automated | low |
| Adversarial | Model-level and workflow bypass | Injection corpus (automated), adaptive human red-team | VAL-005, VAL-006 | automated / internal-independent | medium |
| **Control mutation** | Control that cannot fail (vacuous) or detection gap | Remove a check, relax a threshold, drop a log source; detection must fire | VAL-007, VAL-023 | automated | medium |
| Fault injection | Pipeline reporting green when broken | Kill collector, skew clock, duplicate events, provider outage (cf. chaos engineering, SRC-AIG-028) | VAL-008 | automated | medium |
| Configuration verification | Drift from approved baseline | Branch protection, retention lock, IAM, policy bundle hash | VAL-009 | automated | low |
| Synthetic transactions / canaries | Control silently off in production | Known-bad synthetic action must be blocked; sentinel control must always fail | VAL-010, VAL-031 | automated | low–medium |
| Independent reconciliation | Incomplete/inaccurate populations | Recount from source system vs evidence pipeline | VAL-011 | internal-independent | low |
| Recovery exercises | Untested backups/kill switch | Restore evidence store; revoke all agents | VAL-012, VAL-022 | internal-independent | medium |
| Access control testing | Privilege creep, agent with approver rights | Enumerate effective permissions daily | VAL-013 | automated | low |
| Cross-tenant isolation | Tenant leakage | Two synthetic tenants attack each other | VAL-014 | automated | low–medium |
| Runtime invariants | Violations in live event streams | Approval precedes execution; sealed periods immutable | VAL-015 | automated | medium |
| Formal methods (selected) | Design-level protocol flaws | §6 | VAL-016, VAL-017 | internal-independent / automated | medium–high |
| External layers | Everything Echelon cannot see in itself | Technical review, pen test, SOC exam | VAL-026..028 | external-* | high |

### 4.1 Control mutation testing (design)

Code mutation testing seeds small faults to measure whether tests catch them;
Google runs a diff-based form in code review at scale, surfacing results to
thousands of engineers, and reports that limiting mutants to non-"arid" lines
made results reviewable (SRC-AIG-027). Applying the idea to **controls** is
our analogy, not the paper's claim *(recommendation)*:

- **Mutation operators for controls:** delete the check; invert the
  condition; widen scope filter to exclude a population; relax a numeric
  threshold; disable a log source; remove an alert route; extend an exception
  expiry; make the evaluator return a cached result; drop the signature step.
- **Where:** an isolated replica environment by default; in production only
  through pre-approved, reversible canary mutations on synthetic tenants.
- **Oracle:** each control declares its detecting mechanism(s) and SLA; a
  mutation is *killed* if the declared detector raises within SLA.
- **Metric:** per-control kill rate; a surviving mutant is a finding about the
  validation layer, not the control. Track over time (Dokimos-style immutable
  observations).
- **Independence:** the mutation harness lives in a separate repository and
  pipeline, runs under a validation identity, and its mutant catalogue is
  changed only with approval by someone other than the control implementer
  (§5).
- **Limit:** a killed mutant shows detection of *that* fault class; it does
  not prove operating effectiveness over a period (RSK-040).

## 5. Preventing the same system from implementing, validating and approving its own controls

*(recommendation; builds on Unbound GOV-AUTH-004 and -014 and CNV-GOV-003,
SRC-AIG-031.)*

| Separation axis | Implementer | Validator | Approver |
|---|---|---|---|
| Role (human) | Control owner/engineer | Person outside the implementing team where one exists; today multiple-agent review and protected deterministic verification, with no retainer (OQ-608, DF-ASR-2026-0D11) | Authority from decision register (OQ-607) |
| Agent identity | Builder agent identities | Separate validation agent identities with read-only source access | none (humans only) |
| Credentials | Write to control code via PR | Read-only to sources and evidence; write only to validation-results store | Approval service, phishing-resistant authenticator |
| Repository | Control/policy repositories | Validation repository (tests, mutants, reconciliations) | Decision register |
| Pipeline | Build/deploy pipeline | Validation pipeline on separate runner pool/account | — |
| Model/provider | Provider A family | Different provider/model family B (reduces correlated error; does **not** create independence) | — |
| Evidence source | Evidence pipeline | Reads the **source system directly**, not the implementer's pipeline output | Sees both and the diff |
| Environment | Production + staging | Isolated validation environment + read-only prod | — |
| External witness | — | External timestamp/transparency anchoring; external technical review; pen test; CPA | — |

Enforcement mechanisms:

- Branch protection: changes to the validation repository require approval by
  someone who did not author the change *and* is not the implementer of the
  controls it tests (not achievable with one human today; see the small-team
  limits later in this section); validation code cannot be changed in the same pull
  request as the control it tests (a deterministic CI rule).
- Policy analysis (VAL-017) proves no single principal holds both
  control-write and validation-write permissions, and no agent holds approval.
- The validation pipeline publishes results only to an append-only store the
  implementing pipeline cannot write.
- *(Superseded for the development stage by owner direction
  DF-ASR-2026-0D0F: `Tested` now requires `technically-separated` or higher;
  `agent-review` never supports it; see `controls/README.md`.)*
  Original proposal: a control's `Tested` status requires a validation result whose
  `independence_level` is at least `internal-independent`; `controls/README.md`
  applies this to every control, not only key controls, and `automated`
  results alone keep it at `Operating` *(recommendation for the
  status rules in `controls/README.md`; coordinator decision)*.
- Small-team reality: where two distinct humans are not available, record the
  gap and block or redesign the action rather than pretending separation
  exists (OQ-608). No external reviewer is contracted at the development stage
  (DF-ASR-2026-0D0F).

## 6. Formal methods: where they pay

*(recommendation; evidence vendor-authored and self-reported.)*

What the evidence says:

- AWS reported TLA+ found subtle, serious design bugs (one in DynamoDB needed a
  35-step trace) that reviews and testing had missed; engineers reached
  useful results in two to three weeks; but "TLA+ does not verify the
  executable code" — the authors say they do not know whether code matches
  the verified design (SRC-AIG-024, excerpt). No quantitative cost data.
- Amazon's Cedar authorization language was built with a Lean-proven model
  plus differential random testing against the Rust implementation; proofs
  found 4 validator bugs and DRT/PBT found 21 more (SRC-AIG-025). A secondary
  summary reports a large validator soundness proof took on the order of
  weeks of person-effort (unverified).
- Zelkova encodes IAM policy semantics in SMT to answer access questions
  soundly and is used in production analysers (SRC-AIG-026, summary-only).
- F*: considered but no primary source was read in this batch; not
  recommended for Echelon at current scale *(assumption)*.

Candidate invariants and recommendation:

| Invariant | Technique | Recommendation | Phase |
|---|---|---|---|
| No agent principal can approve, merge protected branches, write primary evidence or alter retention | Analysable policy language + automated policy analysis (Cedar/SMT pattern) | **Yes** — cheap once the policy language is analysable; runs in CI (VAL-017) | P1 |
| Approval binds to exact artifact hash; any change invalidates | TLA+ model of approval/exception state machine | Deferred; a time-boxed pilot only if later justified (OQ-611) | P2 |
| Evidence log: append-only, sealed periods immutable, corrections linked | TLA+ model + property-based tests on implementation | Property-based tests; TLA+ only if a pilot is later justified (OQ-611, deferred) | P2 |
| Ledger conservation / double entry; settlement state machine | Property-based tests now; TLA+ for settlement protocol only if later justified (OQ-611, deferred) | PBT P1; TLA+ conditional | P1–P3 |
| Tenant isolation in authorization | Policy analysis | Yes with first item | P1 |
| Full code-level proof (Lean/F*/Dafny) | — | **No** for now: cost and skills out of proportion; revisit if Echelon builds its own policy engine | — |

Realistic cost/benefit: the high-value, low-cost move is choosing an
**analysable** authorization policy language so that prohibitions P1/P2/P4/P8
become machine-checkable statements. Design-level model checking is
worthwhile only for the two or three protocols where a subtle bug would
silently break assurance (approvals, evidence sealing, settlement). Formal
results are design evidence; they are never presented as operating
effectiveness (RSK-040).

## 7. Recommended minimum by phase

| Phase | Validation layers |
|---|---|
| P0–P1 | VAL-001..004, 009, 010, 011, 013, 014, 017, 018, 020, 024, 029, 030, 031 (VAL-014 moved from P2 in review pass 4: CTL-TEN-001 is a P1 control and daf-landscape gate G2 requires tenant-isolation tests before production) |
| P2 | Add VAL-005, 006, 007, 008, 012, 015, 016 (formal-methods part only if justified, OQ-611), 019, 021, 022, 023, 025, 032 |
| Before material production launch | VAL-027 (pen test incl. agents): deferred under ODP-11; the need is reassessed against risk and actual obligations before material production launch, whether or not an examination is planned (WI-EXT-04, PG-04, PG-10) |
| Only if an examination is planned (reassessed under DF-ASR-2026-0D0F; none planned now) | VAL-026 (external technical review of evidence machinery; deferred under 0D0F), then VAL-028 |

## 8. Open items

OQ-601 (auditor reliance on agent evidence), OQ-604 (mutation testing as
management monitoring), OQ-608 (staffing for separation) and OQ-612 (external
technical validation provider) have owner-approved internal design resolutions
(DF-ASR-2026-0D11); requirements definition and implementation verification
remain pending for all four,
and auditor or provider acceptance remains pending for OQ-601, OQ-604 and
OQ-612 (OQ-608 has no external acceptance condition). OQ-611 (formal methods investment) is answered as
deferred (inferred from the owner's baseline approval, DF-ASR-2026-0D10).

## Owner requirements VAL-BC-001..014 (2026-11-25)

`requirements/ai-governance-independent-validation.md` confirms this architecture's independence vocabulary and adds a risk
tier rule: routine deterministic checks for routine controls; independent
technical verification plus accountable review for material controls;
external human review where required or justified (VAL-BC-009). A
different model alone is not independence (VAL-BC-008). Status is never
promoted automatically between internally tested, technically independently
verified, organizationally independently assessed and externally attested
(VAL-BC-010; CTL-ASR-009, CTL-ASR-011). Formal methods only for critical
state machines where the benefit justifies the cost, and a model proof is not
a proof of the executable code (VAL-BC-013; OQ-611: state machines and
property tests first, any modeling pilot only after cost-benefit review).
