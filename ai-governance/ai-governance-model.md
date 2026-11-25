---
id: GV-ASR-AIG-001
title: AI governance model for assurance agents
status: draft
version: 0.1.0
owners: [assurance-governance]
created: 2026-11-25
updated: 2026-11-25
review_cycle: quarterly
research_batch: B6 (key AIG)
---

# AI governance model for assurance agents

**Status.** Research draft. Nothing in this document is implemented. No
control described here is above `Proposed`/`Designed`. Agents are described as
*intended* to operate under this model; none has been evaluated against it.

Registers this document cites: sources `SRC-AIG-*` (`sources/fragments/AIG.csv`),
requirements `REQ-AIRMF-*`, `REQ-ISO42001-*`, `REQ-OWASPAI-*`, `REQ-EUAI-*`,
`REQ-SSDFAI-*` (`regulatory/fragments/AIG.csv`), risks `RSK-001..042`
(`registers/fragments/AIG-risks.csv`), validations `VAL-001..032`
(`registers/fragments/AIG-validation-procedures.csv`), open questions
`OQ-600..616` (`registers/fragments/AIG-open-questions.csv`) and control
candidates (`registers/fragments/AIG-control-candidates.md`, labelled `AI-*`/`ASR-*`
pending catalog IDs).

---

## 1. Design position

Five premises drive every mechanism below. Each is tagged with its evidence class.

1. **Agents are untrusted principals, not trusted staff.** An assurance agent
   reads untrusted content (regulatory text, customer questionnaires, vendor
   documents, tickets). NIST CAISI's March 2026 analysis of a red-teaming
   competition (250,000+ attempts) found at least one successful hijack
   against *every* one of the 13 frontier models tested (SRC-AIG-018).
   NIST AI 600-1 §2.9 names direct and indirect prompt injection as a GAI
   information-security risk (SRC-AIG-003). *(authoritative for the risk;
   recommendation for the design consequence.)* **Consequence:** no
   prohibition may rely on the model choosing to comply. Every prohibition
   is enforced outside the model, by deterministic systems the agent's
   credentials cannot change.
2. **Agents propose, deterministic systems enforce, humans decide.** This
   reuses Unbound's authority tiers A0–A5 and `GOV-AUTH-021` ("AI explanations
   do not replace enforcement") (SRC-AIG-031) and the Unbound agent contract
   "agents gather, compare, classify, prepare, explain and propose"
   (SRC-AIG-032). *(recommendation, building on internal requirements.)*
3. **Provenance ≠ authentication.** Praxis records agent identity and
   provenance but states it is *self-reported* and "does not defend against a
   malicious process that lies about itself"; signed attestation is deferred
   (SRC-AIG-030). Assurance-grade attribution therefore needs an authenticated
   identity bound to each action (§5). *(authoritative for Praxis behaviour as
   documented; open-question OQ-615 for the fix.)*
4. **AI is an accelerator, never a dependency of a control.** Control
   enforcement, evidence collection and approval must work with every AI
   provider unavailable (§18), consistent with the Echelon Registry invariant
   that systems keep their core functions when optional providers are absent
   (SRC-AIG-034). *(recommendation.)*
5. **Internal ≠ independent.** Echelon-controlled agents are never
   independent auditors (AGENTS.md rule 4; analysis in
   `validation/independent-validation-architecture.md` §3). *(recommendation
   grounded in auditor-interpretation.)*

## 2. Reuse inventory (before building anything)

| Existing asset | What it gives this model | Gap for assurance use | Evidence basis |
|---|---|---|---|
| Praxis agent provenance (`docs/agent-provenance.md`) | Actor kind/id/provider/model/runtime; per-execution IDs; additive, refusal-checked contribution records; `previous/current.sha256` on events; requester/executor/principal separation for remote runs | Self-reported; no signatures; no defence against lying process; repository-scoped | Read in full (SRC-AIG-030) |
| Unbound authority tiers A0–A5 and `GOV-AUTH-001..026` | Authority registry, no self-grant (003), SoD across humans/agents (004), approval bound to exact proposal + evidence snapshot (009), tamper-evident logs (012), independent reconciliation (014), kill switch (024), deterministic enforcement (021); tests AUTH-T01..T12 | Written for DAF operations; needs an assurance-domain action catalog | Read (SRC-AIG-031) |
| Unbound `AIN-001..036`, `RR-AIN-003-r1` | Capability manifest (AIN-002), injection isolation (AIN-012), identity/attribution (AIN-013), no synthetic success claims (AIN-014), model/prompt change control (AIN-027), budgets (AIN-028), missing traces = deficiency (AIN-033); A4 approver must not have originated or modified the item | Requirements only, not implemented | Read (SRC-AIG-032) |
| Unbound continuous control monitoring architecture | Expected-evidence schedule, missing-evidence algorithm, "never from absence of alarms", linked corrections, append-only history | Specified, not executed; Strata API capabilities unverified | Read headings and key lines (SRC-AIG-033) |
| Specula | Read-only observer of repositories, CI and evidence; never directs agents | Echelon-controlled: useful as a *separate* observer, not an independent one | README only (SRC-AIG-035) |
| Praxis containment (`docs/containment.md`) | Host containment for local workers with probes and gating | Not evaluated in this batch (B1 scope) | Headings only |
| Tutela, Dokimos | Security-invariant and quality-observation disciplines (immutable observations) | Not evaluated in this batch | README only |

**Gap recorded** (for the coordinator's integration register): no Echelon
system was found that provides (a) authenticated, signed agent action
receipts, (b) an AI egress gateway enforcing provider allow-lists by data
class, or (c) a semantic "control weakening" classifier. These are recorded
as gaps, not as build decisions (AGENTS.md rule 5).

## 3. Operating model: what agents do

Authority tiers are Unbound's: **A0** observe, **A1** prepare, **A2** bounded
reversible internal action, **A3** controlled consequential action under
explicit policy, **A4** reserved human authority, **A5** prohibited
(SRC-AIG-031). No assurance agent is proposed above A2.

| # | Agent function | Tier | Agent output | Deterministic check before use | Human decision | Degraded mode (no AI) |
|---|---|---|---|---|---|---|
| F1 | Monitor authoritative requirement changes | A0/A1 | Change notice with source URL, retrieved hash, diff, proposed affected `REQ-` rows | Source fetched by collector (not by the model); diff computed deterministically; citation resolver (VAL-020) | Regulatory owner accepts/rejects interpretation | Scheduled fetch + deterministic diff still runs; humans read diffs; backlog flagged stale after SLA |
| F2 | Identify affected policies | A1 | Impact list linking `REQ-`→`CTL-`→`POL-` | Traversal over registers is deterministic; agent only ranks/explains | Policy owner | Register traversal query |
| F3 | Propose policy revisions | A1 | Draft diff on a branch | Weakening classifier (§4 P6); protected branch | Policy approval authority (OQ-607) | Human drafting |
| F4 | Detect control failures | A0 (interpretation only) | Triage notes on failures emitted by evaluators | **Detection is by deterministic evaluators, not agents.** Agents cannot change result status | Control owner | Unaffected (evaluators are deterministic) |
| F5 | Investigate evidence gaps | A1 | Gap analysis citing expected-vs-received evidence | Missing-evidence algorithm (SRC-AIG-033) produces the gap; agent explains | Evidence owner | Gap list still produced |
| F6 | Generate remediation items | A2 | Tickets with cited findings | Ticket schema validation; dedup key | Owner prioritises | Tickets created from finding templates |
| F7 | Evaluate (not approve) exceptions | A1 | Evaluation memo: compensating controls, risk, expiry proposal | Requester ≠ evaluator ≠ approver enforced by approval service | Exception approver (A4) | Human evaluation |
| F8 | Prepare auditor responses and questionnaires | A1 | Draft answers each citing evidence IDs and independence level | Citation check; independence lint (VAL-024) | Named human signs and sends | Library of approved answers; human assembly |
| F9 | Maintain mappings | A1 | Proposed mapping edits | Register validator; referential integrity | Reviewer merges | Manual edits |
| F10 | Prepare management review | A1 | Pack: metrics, findings, risks, changes | Metrics computed deterministically; agent narrates | Management decides (ISO 42001 cl. 9.3 analogue) | Metrics pack without narrative |
| F11 | Identify emerging risks | A1 | Risk proposals with sources | Source register entries required | Risk owner accepts into register | Periodic human horizon scan |

The pattern: **anything that determines a fact (did the control pass, is
evidence missing, what changed in the source) is computed deterministically;
agents interpret, draft and route.** *(recommendation)*

## 4. Hard prohibitions and the mechanisms that enforce them

"Impossible" below means impossible *for an agent identity* given correctly
configured infrastructure. Against a privileged human or a compromised
platform the honest claim is "detectable", and that limit is stated in each
row. *(All rows: recommendation; mechanism patterns are industry-convention
unless cited.)*

| # | Prohibition | Preventive mechanism (makes it impossible for agents) | Detective mechanism (makes it visible) | Residual / limit |
|---|---|---|---|---|
| P1 | Approve its own exceptions (or anything) | Approval is a separate service. Approve/merge/accept-risk actions are permitted only for `human` principals authenticated with a phishing-resistant authenticator; policy engine denies all `agent` and `service` principals on these actions (Unbound AUTH-T01). Each approval is a signed assertion over the hash of the exact proposal and evidence snapshot (GOV-AUTH-009); any change invalidates it. SoD rule: approver ∉ {requester, preparer, any principal in the delegation chain of the preparer} (RR-AIN-003-r1). | Daily query: approvals where approver kind ≠ human, or approver ∈ preparer set, or approval hash ≠ executed artifact hash (VAL-015, VAL-013). Policy analysis proves no agent principal satisfies approve actions (VAL-017). Negative test attempts self-approval (VAL-004). | A human can rubber-stamp (RSK-019): mitigated by sampled approval-quality review and approval metrics, not eliminated. Collusion (RSK-027) only detectable. |
| P2 | Fabricate evidence | Evidence classes are typed: **primary** (written only by collector identities that read the source system; agents hold no write permission to the primary store), **derived** (deterministic computations with recorded inputs), **AI-interpreted** (narrative; always labelled; never sufficient alone for a control conclusion). Collectors sign records with non-exportable keys and record source request IDs. | Independent re-collection sampling from the source (VAL-019); population reconciliation (VAL-011); citation resolver rejects any AI output that cites a non-existent evidence hash (VAL-020). | A compromised collector could sign false data; mitigated by reconciliation against an independent read path. |
| P3 | Conceal failures | Evaluators write results to an append-only store; there is no "delete" or "edit status" API for any principal; status changes are new linked records requiring retest evidence and human closure. Expected-evidence schedule means absence of a result is itself a failure (SRC-AIG-033; AIN-033). Alerts route through a channel the agent cannot configure. | Count reconciliation: evaluator emissions vs findings register; sentinel always-failing control must always appear (VAL-031); heartbeat/dead-man switch (VAL-030); Specula-style separate observer. | Admin with store-level access could suppress routing; detected by out-of-band heartbeat on separate provider. |
| P4 | Modify historical evidence | Write-once storage with a compliance-mode retention lock that cannot be shortened even by the account administrator; records hash-chained; periodic checkpoint digests anchored to an external timestamping or transparency service outside Echelon's control. Corrections are new records linked to the original (SRC-AIG-033). | Daily chain verification against external anchors from a separate account (VAL-018). | Destruction (not alteration) by cloud-provider compromise remains possible; detected by anchor mismatch; backups in second provider (RSK-031). Provider feature claims need verification (vendor-claim). |
| P5 | Represent internal validation as external attestation | Every validation result carries an `independence_level` from the fixed enum (`self-assessment` … `external-attestation`). Outbound publishing (trust center, questionnaires, sales) goes through a pipeline that blocks attestation vocabulary ("audited", "certified", "attested", "independently tested/verified", "SOC 2 compliant") unless the sentence links to an `external-attestation` record that references an external party's report ID. Agents cannot publish; a human signs outbound text. | Lint (VAL-024); periodic review of published text; the questionnaire answer library stores the independence level with every answer. | Free-text in email outside the pipeline; mitigated by training and the answer library. |
| P6 | Silently weaken controls | Controls, thresholds and policies are code in protected repositories. A deterministic **weakening classifier** computes a semantic diff (check removed, condition inverted, scope narrowed, threshold relaxed, severity lowered, log source or alert route removed, exception expiry extended, population filter added). Any weakening requires approval by the control owner *and* assurance governance; agents cannot merge. | Control mutation tests (VAL-007) fail when a check is removed; classifier itself is mutation-tested (VAL-023); denominator monitoring (number of controls evaluated, population sizes) alerts on drops (VAL-032). | Weakening via upstream data (e.g., a source stops emitting events) is caught by expected-evidence logic, not the classifier. |
| P7 | Change material policies without approval | Policy bundles are signed after approval by the authority in the decision register; the policy engine loads only bundles whose hash matches an approved, signed release (admission check). Agents can open pull requests only. | Configuration verification compares the loaded bundle hash with the approved release (VAL-009). | Wrong-but-approved policy (RSK-028): out of scope for this control; covered by review and outcome metrics. |
| P8 | Expose confidential evidence to unauthorised providers | Agents have no direct internet egress; all model calls go through an AI gateway that enforces an allow-list of (provider, model, region, contract terms) per data classification; evidence carries classification labels set by collectors, not agents; restricted classes are never sent off-platform. Provider terms are a prerequisite for listing (OQ-603). | Gateway logs reconciled with provider usage records; canary tokens embedded in restricted evidence alert if they appear in any egress; DLP on gateway. | Misclassified data (label wrong at source) can leak; mitigated by default-restricted labelling. |

Additional prohibitions inherited from Unbound A5 apply unchanged: no
disabling of required logs (AUTH-T08), no agent self-expansion of privileges
(GOV-AUTH-003), no backdating of approvals.

## 5. Agent identity and authorization

- **One workload identity per agent role** (not per model, not shared with
  humans). Identity is issued by the platform (CI OIDC federation or cloud
  workload identity), short-lived, and bound to the execution. Praxis
  `EXE-` IDs and actor records are retained as provenance, **linked** to the
  authenticated identity by recording the identity token's subject and the
  CI run ID in the execution record (proposal for Praxis: OQ-615).
- **Capability manifest per role** (AIN-002): tools, data scope, allowed
  actions, authority tier ceiling, escalation route, budget. The manifest is
  policy input; the model cannot alter it.
- **Effective permission = intersection** of (agent role permissions,
  delegating principal's permissions, task scope). Sub-agents receive a
  strictly narrower token (AIN-010; RSK-011). An agent acting "on behalf of"
  a human never inherits that human's approval rights.
- **Deterministic policy engine** (e.g., an analysable policy language) makes
  allow/deny decisions; decisions and denials are logged (GOV-AUTH-012).
  Analysability matters because it enables VAL-017.
- Maps to REQ-OWASPAI-005, REQ-OWASPAI-010, REQ-AIRMF-002.

## 6. Agent credentials

- No long-lived secrets in agent configuration. Credentials are minted per
  task and scoped to the manifest.
- **Credentials never enter the model context.** The tool runtime injects
  them into outbound calls; the model sees only tool names and results. This
  removes the "agent leaks its token in output" class (RSK-010).
- Revocation is centralised and does not depend on the agent's cooperation
  (kill switch, §16).
- Secrets scanning on agent outputs and traces before persistence.

## 7. Execution boundaries

- Agents run in sandboxes with: no direct internet (egress via AI gateway and
  an allow-listed fetch proxy), read-only mounts by default, writes only via
  governed APIs (GOV-AUTH-023) or pull requests.
- No agent write path to production configuration except through the change
  pipeline; no agent access to evidence-store administrative APIs.
- Budgets: tokens, tool calls, wall time, cost per task (AIN-028,
  REQ-OWASPAI-007); exceeding a budget stops the task with an explicit
  `incomplete` state, never a silent partial success (AIN-014).
- Inter-agent messages are signed by the sending identity and treated as
  untrusted data by the receiver (REQ-OWASPAI-012; RSK-038).
- Persistent memory/retrieval corpora are versioned, writable only by
  ingestion pipelines from registered sources, and diff-reviewed
  (REQ-OWASPAI-011; RSK-017).

## 8. Human approval requirements

| Action class | Tier | Approver | Binding |
|---|---|---|---|
| Accept interpretation of a requirement change | A4 | Regulatory owner | Approval over hash of change notice + diff |
| Adopt or materially change a policy | A4 | Policy approval authority (OQ-607) | Signed bundle release |
| Any control-weakening change (P6) | A4 | Control owner **and** assurance governance | Classifier verdict + diff hash |
| Approve exception / risk acceptance | A4 | Exception approver ≠ requester ≠ evaluator | Expiry mandatory |
| Close a finding | A4 | Control owner with retest evidence | Retest result ID |
| Send auditor response / questionnaire / trust-center text | A4 | Named human signatory | Text hash + independence lint pass |
| Raise an agent's authority tier or add a tool | A4 | AI governance owner, after VAL-006 and VAL-021 | Manifest hash |
| Add an AI provider/model to the allow-list | A4 | Security owner | Contract terms recorded (OQ-603) |
| Create remediation ticket, tag, route | A2 | none (logged) | — |

Approval UX shows the diff, the weakening classification, cited evidence and
what the agent did *not* check. Approval quality is sampled quarterly
(RSK-019, automation bias; REQ-OWASPAI-013). Feasibility of distinct humans
per role is open (OQ-608). Owner decision DF-ASR-2026-0D04 (superseding proposal DF-ASR-2026-7B20): standing human authorization + separate implementing agent + technical verifier + enforced release gate for routine low-risk work; elevated human approval for material changes; fractional human review where required. With one human owner, the P1 rule (approver outside
the preparer's delegation chain) leaves no eligible approver for owner-sponsored
agent work and conflicts with the POL-11.03 small-team fallback; which rule
governs is OQ-800.

## 9. AI-generated code governance

- NIST SP 800-218A "do[es] not distinguish between human-written and
  AI-generated source code, because it is assumed that all source code should
  be evaluated" (SRC-AIG-014, REQ-SSDFAI-001). *(authoritative.)* So the
  baseline is the normal secure SDLC, applied without exemption.
- Additions specific to agents *(recommendation)*:
  - Agent-authored commits carry authenticated agent identity (not just a
    co-author trailer) so review rules can key on it.
  - An agent never approves or merges its own pull request; a human review is
    required for agent-authored changes to security-relevant paths (identity,
    authorization, evidence, controls, CI, policy).
  - **Test-weakening protection**: changes that delete or relax tests,
    assertions, lint rules, coverage thresholds or CI steps are classified as
    control weakening (P6) and require human approval; mutation score is
    tracked so a test suite that stops killing mutants is visible (VAL-007).
  - Dependency allow-list/verification to block non-existent or typo-squatted
    packages suggested by models; SBOM and provenance per release (PS.3.2,
    PW.4.4; REQ-SSDFAI-002/004).
  - Input/output handling per PW.5.1 R2/R3 for any code that calls models
    (REQ-SSDFAI-003).

## 10. Model and provider risk management

- Inventory of agents, models (pinned versions, not floating aliases where the
  provider allows), providers, data classes and contract terms
  (REQ-AIRMF-001, REQ-AIRMF-015).
- Providers used in in-scope processes are evaluated as **subservice
  organizations** consistent with the AICPA approach reported for TQA 9561
  (carve-out or inclusive; CSOCs) (SRC-AIG-022, summary-only; OQ-602).
- Model, prompt, tool or provider change = change-controlled release gated by
  the evaluation suite (VAL-021; AIN-027). Provider-side silent updates are
  detected by a daily canary evaluation against a frozen golden set (RSK-016).
- At least two providers qualified for each agent role so that loss or
  compromise of one is survivable (RSK-015); the validation agents use a
  different provider/model family from the agents they validate (see
  validation architecture §5).
- Third-party contingency per GOVERN 6.2 (REQ-AIRMF-004) is the degraded mode
  in §18.

## 11. Prompt-injection containment

Prevention at the model layer is unreliable (SRC-AIG-018; REQ-OWASPAI-001,
REQ-AIRMF-016). The design therefore **assumes the agent may be hijacked** and
bounds what a hijacked agent can do *(recommendation)*:

1. Authority bound: agents are A0–A2; hijack yields at most a draft, a
   ticket or a reversible tag.
2. Deterministic authorization outside the model (§5) and no credentials in
   context (§6).
3. Separation of untrusted content from instructions: content fetched from
   outside is passed as data to a reader/quarantine step whose output is
   structured (fields, citations) and validated before a tool-using step acts
   on it; the tool-using step never sees raw untrusted text where avoidable.
4. Output handling: model outputs are never executed or used as commands;
   schema-validated and encoded (REQ-OWASPAI-004, REQ-SSDFAI-003).
5. Human approval for every consequential effect (§8).
6. Monitoring: anomalous tool-call patterns (new destination, unusual volume,
   prohibited-action attempts) alert; every denial by the policy engine is a
   signal (AIN-033).
7. Testing: automated injection corpus on every change (VAL-005) and human
   adaptive red-team quarterly (VAL-006), since the CAISI results show
   static robustness cannot be assumed *(recommendation)*.

Residual risk acceptance is an owner decision (OQ-616).

## 12. AI data handling

- Classification labels assigned at collection; default `restricted` when
  unknown.
- Gateway routing by class (P8). Minimise: send excerpts, not whole evidence
  packages; redact identifiers where the task does not need them.
- Provider prerequisites: no training on inputs, bounded retention, region,
  breach notice, subprocessor list (OQ-603). The CISA/NSA/FBI AI data
  security guidance (SRC-AIG-016, summary-only) is the reference for data
  supply-chain, poisoning and drift risks.
- Prompts/traces retained as evidence of agent actions, under the evidence
  retention schedule with privacy minimisation (OQ-614; RSK-039).

## 13. Validation of AI-generated evidence

| Evidence class | Who can create | Can support a control conclusion? | Validation |
|---|---|---|---|
| Primary | Collector identities only | Yes | Signature, source request ID, reconciliation (VAL-011), re-collection sample (VAL-019) |
| Derived | Deterministic jobs with recorded inputs and code version | Yes, if reproducible | Recompute from inputs |
| AI-interpreted | Agents | **No, not alone.** Must cite primary/derived hashes | Citation resolver + figure recomputation (VAL-020); sampled human review for confabulation (REQ-AIRMF-013, MS-2.5-003) |

Auditor acceptance of agent-collected or agent-summarised evidence is open
(OQ-601); PCAOB's 2024 amendments on technology-assisted analysis show that
even an auditor's own tools are expected to address reliability of the
information analysed (SRC-AIG-020, summary-only, analogy only).

## 14. Autonomous remediation bounds

L5 (bounded automated remediation) is allowed only when **all** hold
*(recommendation)*:

- The playbook is pre-approved (A4) and versioned; the agent selects, it does
  not author, remediation at run time.
- The action **restores to an approved baseline**; it never changes the
  baseline, never widens access, never lowers a threshold.
- Reversible and idempotent, with automatic rollback on failed post-check.
- Blast-radius limits (resources per run, runs per hour) and a circuit breaker
  that halts after N consecutive remediations of the same drift (oscillation
  usually means the baseline or a second actor is wrong).
- **Forensic snapshot before change** and the original alert is preserved and
  still raised, so remediation never hides an incident (RSK-030).
- Evidence of detection, action and post-check written as primary/derived
  records.

Everything else is L4 or lower (block and route to a human).

## 15. Accountability

- Every agent role has one accountable human owner (role, not person, in this
  repository); an agent is never the `owner_role` of a control, policy or
  risk (REQ-AIRMF-002, REQ-ISO42001-002).
- Agents are tools of management. Their errors are management's errors for
  assertion purposes; this is one reason they are not independent
  (validation architecture §3).
- Involvement labels from Praxis (`agent-created, human-approved`, etc.) are
  surfaced on every assurance artifact (SRC-AIG-030).

## 16. AI incident response

- Incident categories: successful injection/goal hijack; prohibited-action
  attempt (even if denied, above threshold); unauthorised data egress;
  fabricated output relied upon; model regression affecting controls;
  provider compromise or outage; agent credential exposure.
- **Kill switch**: revoke all agent identities and disable the AI gateway from
  a control plane the agents cannot reach; queued agent work suspends safely
  (GOV-AUTH-024, AUTH-T07; REQ-AIRMF-008). Exercised quarterly (VAL-022).
- Preserve traces, prompts, tool calls and gateway logs as evidence.
- After-action assessment and error/near-miss tracking per NIST AI 600-1
  MG-4.3-001/002 (REQ-AIRMF-010, REQ-AIRMF-014); notification per legal
  obligations via the general incident process.

## 17. Model evaluation and monitoring

- Pre-deployment per role: task accuracy on frozen golden sets, citation
  fidelity, refusal/denial of prohibited actions, injection resistance
  (VAL-021, VAL-005).
- Evaluators and red-teamers are not the agent's developers
  (REQ-AIRMF-006 MEASURE 1.3; REQ-AIRMF-012 MS-1.3-003).
- Ongoing: correction rate (human edits to agent drafts), denial rate,
  citation failure rate, cost, latency, drift canary; using Praxis metrics
  where they exist.
- Avoid extrapolating from anecdote (NIST AI 600-1 MS-2.5-001).

## 18. Degraded mode: AI providers unavailable

| Capability | Normal | Degraded | Fails |
|---|---|---|---|
| Preventive controls (authorization, approvals, retention locks, policy engine) | Deterministic | Unchanged | closed |
| Evidence collection | Deterministic collectors | Unchanged | — |
| Control evaluation | Deterministic evaluators | Unchanged | — |
| Requirement-change monitoring | Fetch + diff + agent analysis | Fetch + diff; human reads; SLA clock runs | open-justified (stale flag) |
| Triage, drafting, questionnaires, management-review narrative | Agents | Templates, answer library, humans | open-justified |
| Auto-remediation (L5) | Agent selects playbook | Deterministic rule selects playbook or routes to human | closed (no action) |
| Approvals | Human | Human (never auto-approve on timeout) | closed |

Requirement: no control's `failure_mode` may depend on an AI provider.
Verified by VAL-022. *(recommendation; REQ-AIRMF-004.)*

## 19. Framework coverage summary

| Framework | Rows | Applicability | Note |
|---|---|---|---|
| NIST AI RMF 1.0 + AI 600-1 | REQ-AIRMF-001..016 | voluntary | Recommended organising frame; revision announced (OQ-613) |
| ISO/IEC 42001:2023 | REQ-ISO42001-001..006 | voluntary | Clause titles verified from publisher preview; text not purchased; adopt only if demanded (OQ-606) |
| OWASP LLM Top 10 2025, Agentic Top 10 2026, Agentic T&M, AI Exchange | REQ-OWASPAI-001..015 | voluntary | Threat baseline for design and test |
| EU AI Act | REQ-EUAI-001..004 | undetermined | Applicability check only (§20) |
| NIST SP 800-218A | REQ-SSDFAI-001..005 | voluntary | Echelon is AI system producer/acquirer, not model producer |
| AICPA (SOC) developments | — (owned by SOC batch) | — | TQA 9561 (Sept 2026) and proposed 2027 TSC AI points of focus (exposure draft, comments due 2027-01-15) (SRC-AIG-022, SRC-AIG-023; OQ-605) |

## 20. EU AI Act applicability check

- Regulation (EU) 2024/1689; general application 2 Aug 2026; Chapters I–II
  from 2 Feb 2025 (Art. 113, consolidated rendering SRC-AIG-011). The Digital
  Omnibus on AI reportedly entered into force 27 Jul 2026 and moved Annex III
  high-risk obligations to 2 Dec 2027 and Annex I to 2 Aug 2028; sources
  disagree on the adoption date (SRC-AIG-012, SRC-AIG-013, summary-only).
  The official OJ text could not be retrieved in this batch.
- Internal assurance agents that do not interact with the public and do not
  make decisions about natural persons appear unlikely to be high-risk;
  however, territorial scope, Article 4 literacy and Article 50 transparency
  for any future donor-facing assistant are **undetermined** and go to
  counsel (OQ-600). *(open-question)*

## 21. Contradictions and limits preserved

- **Formal vs substantive human approval.** Human approval is the main
  backstop, yet automation bias is a known failure (REQ-OWASPAI-013). The
  model therefore pairs approval with deterministic checks; it does not claim
  approval alone is sufficient.
- **AICPA AI guidance is moving.** A CSA note (Aug 2026, SRC-AIG-036) said
  AICPA had not acted on AI in SOC 2; within weeks AICPA issued TQA 9561 and
  an exposure draft (SRC-AIG-022/023). Mappings here must be revisited when
  the 2027 TSC are final (OQ-605).
- **OWASP agentic numbering** changed between the T&M document (T-numbers)
  and the Agentic Top 10 (ASI IDs); only ASI IDs and LLM IDs from pages read
  are used.
- **ISO 42001** conclusions are capped at `auditor-interpretation` because the
  text was not read.

## Owner authority model and requirements AIG-BC-001..018 (2026-11-25)

`requirements/ai-governance-independent-validation.md` sets the authority model for Echelon agents:

| Tier | Meaning | Relation to Unbound tiers above |
|---|---|---|
| A0 | Read public or specifically authorized data | Same as Unbound A0 (observe) |
| A1 | Propose drafts and decisions | Same as A1 (prepare) |
| A2 | Execute bounded reversible, or otherwise explicitly authorized low-risk, operations | Same as A2; "otherwise explicitly authorized" aligns with owner decision 0D09 |
| A3 | Material operations only through independently enforced standing or transaction-specific human authorization and deterministic gates | Clarifies Unbound A3; no agent independently authorizes movement of funds or material exceptions; the sponsor retains financial authority |
| A4 | Reserved human authority: policy weakening, material exceptions, risk acceptance, privileged access, management assertions, other nondelegable decisions | Same as A4; Unbound's A5 (prohibited) corresponds to the hard prohibitions in §4 |

If independent approval is required and unavailable, the action is blocked or
redesigned; technical separation is never represented as human
organizational independence. The 18 AIG-BC requirements map to existing
controls (CTL-IAM-001, CTL-AI-001..008, CTL-IR-003, CTL-VEN-003,
CTL-LOG-002, CTL-DAT-002, CTL-GOV-007/008/012, CTL-APP-005/010,
CTL-FIN-002, CTL-ASR-006) plus one new control, CTL-AI-010 (AI involvement
disclosure and correction path). Residual prompt-injection risk (RSK-001) is
explicitly **not accepted** pending owner review.
