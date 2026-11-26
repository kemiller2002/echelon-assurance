---
id: GV-ASR-VAL-002
title: Assurance-system resilience (how assurance itself fails)
status: draft
version: 0.1.0
owners: [assurance-governance]
created: 2026-11-25
updated: 2026-11-25
review_cycle: semi-annual
research_batch: B6 (key AIG)
---

> **Consolidation note (2026-11-25).** Scenarios R1, R3, R4 and R16 rely on mitigations that the consolidation removed or that cannot operate with one human:
> - hash chaining, external anchoring and daily chain verification, replaced under DF-ASR-2026-C002;
> - second-person approval and second-person review.
>
> Current design:
> - **R1 (compromised administrator):** compliance-mode retention lock from the first production evidence; out-of-band alerts; separately credentialed drift detection (BR-ASR-006, BR-IAM-002). This detects external compromise, not owner action (ODP-03).
> - **R3 (compromised CI/CD):** verifier keys in a protected environment that the implementer cannot read (WI-DOKIMOS-01). Pipeline-definition changes are elevated changes approved by the owner.
> - **R4 (evidence manipulation):** retention lock plus separately credentialed population reconciliation against sources Echelon does not write, such as sponsor bank statements and platform APIs (BR-ASR-003). There is no chain verification.
> - **R16 (anchoring lapse):** not applicable while anchoring is deferred.
> - **R7, R8, R10, R12 and R13:** cells that rely on external review samples (VAL-026), internal-independent review (VAL-025), a walkthrough by someone outside the team, or evidence anchors describe options, not the current plan: VAL-026 is deferred under DF-ASR-2026-0D0F, VAL-025 cannot be performed as independent with one human (self-assessment only), no outside person is engaged, and anchoring is deferred under DF-ASR-2026-C002.


# Assurance-system resilience

**Status.** Research draft (charter section 17 topic). Nothing here is
implemented. Meta-controls are proposed as `ASR-*` candidates in
`registers/fragments/AIG-control-candidates.md`; validations `VAL-0xx` and
risks `RSK-0xx` are in the AIG fragments.

## 1. Premise

The evidence and validation machinery is itself a system with privileged
operators, code, pipelines and dependencies. If it fails quietly, every
control it reports on appears healthy. The rule inherited from Unbound's
monitoring architecture is: **assurance is computed from expected
independent evidence and tested operation, never from the absence of
alarms** (SRC-AIG-033). *(recommendation)*

## 2. Failure modes and meta-controls

| # | Failure mode | How it shows up | Preventive meta-control | Detective meta-control (signal) | Risk |
|---|---|---|---|---|---|
| R1 | Compromised administrator | Retention shortened, controls disabled, evidence deleted, alert routes changed | Compliance-mode retention lock from the first production evidence (BR-ASR-006); break-glass accounts sealed and alarmed; one-person interim: no second-person approval exists (ODP-03) | Out-of-band alerts and separately credentialed drift detection (BR-IAM-002); detects external compromise, not owner action | RSK-023 |
| R2 | Compromised agent | Agent attempts prohibited actions, exfiltrates, drafts misleading content | Prohibitions P1–P8 enforced outside the model; A0–A2 ceiling for agents processing untrusted content (BR-AI-004); credentials not in context | Policy-engine denial rate and anomalous tool use alerts; negative tests (VAL-004); kill switch exercised (VAL-022) | RSK-001, RSK-002 |
| R3 | Compromised CI/CD | Checks skipped, results forged, unapproved control code deployed | Separate validation pipeline and verifier identity whose key the implementer cannot read (WI-DOKIMOS-01); pipeline-definition changes are elevated changes needing explicit owner approval (BR-SDL-003) | Validation pipeline recomputes key results independently; sentinel control must fail (VAL-031); config verification of pipeline definitions | RSK-024 |
| R4 | Evidence manipulation | Records altered, back-filled or forged | Primary store writable only by collectors; compliance-mode retention lock; stored digests in envelopes | Separately credentialed population reconciliation against sources Echelon does not write (BR-ASR-003); store integrity verification (VAL-018); re-collection sampling (VAL-019, self-assessment until a reviewer exists) | RSK-003, RSK-005 |
| R5 | Missing evidence | Collector down or scope gap; nothing to evaluate | Expected-evidence schedule per control/period; missing = `insufficient-evidence`, never pass | Heartbeats (VAL-030); population reconciliation (VAL-011); fault injection (VAL-008) | RSK-025 |
| R6 | Disabled or blinded monitoring | Log source off, detector rule disabled, alert route broken | Monitoring configuration is code under weakening classifier (P6) | Synthetic events must produce alerts end-to-end (VAL-010); dead-man switch on separate provider (VAL-030); mutation of log source must be detected (VAL-007) | RSK-026 |
| R7 | Collusion (human–human or human steering agent) | Two role-holders bypass SoD | SoD across distinct humans; approvals bound to artifact hash; no agent approvals | Pattern analytics (same pair repeatedly approving each other's weakening changes; approvals with minimal review time); external review samples (VAL-026). Only detectable, not preventable | RSK-027 |
| R8 | Wrong policies, faithfully enforced | All checks green; objective not met | Policy approval by accountable authority with requirement traceability | Outcome metrics (incidents, external findings) compared with control results; internal independent review (VAL-025); external findings that internal validation missed are a meta-finding | RSK-028 |
| R9 | Stale regulatory interpretation | Mappings drift from current law/standards | Source register with retrieval dates; re-verification rule for sources > 12 months (methodology §3); change monitoring (F1) | Source-age report; deterministic diff of watched sources; quarterly human review of material changes (OQ-613) | RSK-018 |
| R10 | Shared infrastructure failure | Control and its monitor fail together | Monitor, evidence anchor and heartbeat receiver on different account/provider/identity plane from production | Heartbeat absence; cross-provider canary | RSK-029 |
| R11 | Bad automated remediation | Outage, oscillation, baseline changed, incident evidence destroyed | L5 bounds (governance model §14): restore-to-baseline only, forensic snapshot first, circuit breaker | Remediation-rate alerts; repeated-drift alerts; post-incident review of all L5 actions | RSK-013, RSK-030 |
| R12 | Reliance on a single cloud or AI provider | One provider event removes function and verification | Deterministic controls independent of AI (degraded mode); second AI provider qualified; evidence backups and anchors outside primary cloud | Degraded-mode exercise (VAL-022); restore test in second location (VAL-012) | RSK-014, RSK-015, RSK-031 |
| R13 | Excess complexity | Nobody can explain why a control is green; auditor friction | Complexity budget: every meta-control must name the failure it detects and its test; retire unused ones; prefer few strong invariants | Annual "explain it" walkthrough by someone outside the team; count of controls without a mutation/negative test | RSK-032 |
| R14 | Compliance theater | 100% green, no negative tests, findings only ever come from outside | Status vocabulary forbids inflation (controls/README); `Tested` requires a VAL result with stated independence | Theater indicators (§4) reviewed quarterly (VAL-032) | RSK-033 |
| R15 | Vacuous validation suite | Tests that cannot fail | Mutation testing of the validation suite and the weakening classifier | Kill rate (VAL-007, VAL-023); sentinel control (VAL-031) | RSK-034 |
| R16 | Anchoring/witness lapse (not applicable while anchoring is deferred, DF-ASR-2026-C002) | External anchoring silently stops | Anchoring job is an expected-evidence item itself | Missing anchor = finding; external service's own log consulted | RSK-042 |

*(All rows: recommendation; mechanisms are industry-convention patterns
unless cited.)*

## 3. Independent verification strategy for the evidence and validation systems

The machinery that produces assurance must itself be verified by something it
does not control. In increasing order of independence:

1. **Self-checks with known answers.** Sentinel always-failing control
   (VAL-031), synthetic canaries (VAL-010), seeded faults (VAL-008), mutants
   (VAL-007). If any of these reports the *wrong* answer, the machinery is
   presumed broken until explained.
2. **Separate observer on separate infrastructure.** A read-only observer
   (Specula-style; Specula's own fitness not evaluated here, SRC-AIG-035)
   recomputes headline numbers from sources and compares. This is
   `automated`, not independent: same owner.
3. **Reperformance by a different person with different credentials.**
   Monthly samples re-collected from source (VAL-019), reconciliation owned
   outside the control team (VAL-011) — `internal-independent`. *Not available
   with one human: performed as self-assessment and not described as
   independent until a second person exists (DF-ASR-2026-0D0F, RSK-051).*
4. **External witnesses that require no trust in Echelon.** Periodic digests
   of the evidence log anchored to an external timestamping/transparency
   service; anyone holding the digest can later detect alteration (VAL-018).
   *Deferred under DF-ASR-2026-C002 until someone requires it.*
5. **External technical review** of evidence integrity, completeness and
   identity separation (VAL-026; provider and CPA acceptance pending, OQ-612; internal design
   approved, DF-ASR-2026-0D11: internal validation first, external review
   conditional).
   *Deferred under DF-ASR-2026-0D0F; only if a reassessment authorizes it.*
6. **Formal attestation** (VAL-028), where the CPA firm decides what of the
   above it relies on (OQ-601, OQ-604). *Only if an examination is planned
   (reassessed under DF-ASR-2026-0D0F; none planned now).*

## 4. Compliance-theater indicators (reviewed quarterly, VAL-032)

| Indicator | Healthy direction | Why |
|---|---|---|
| Share of key controls with at least one negative or mutation test | Rising toward 100% | A control never shown to fail is unproven |
| Mutation kill rate per control | High and stable | Surviving mutants = blind spots |
| Findings found by internal validation vs by external parties | Internal ≫ external | External-first discovery means the internal layer is theater |
| Evidence freshness (age vs schedule) | Within SLA | Stale evidence masquerades as current |
| Denominator stability (controls evaluated, population sizes) | No unexplained drops | Shrinking scope makes pass rates rise |
| Approval review time and rejection rate | Non-trivial rejections | Zero rejections suggests rubber-stamping (RSK-019) |
| Agent denial rate (prohibited-action attempts) | Low but non-zero in tests; investigated in prod | Zero in tests means tests are not attempting |

## 5. What this design cannot do

- It cannot prevent collusion between enough privileged humans; it can make
  it leave traces outside their control.
- It cannot make Echelon's own verification independent; only external
  parties provide independence (`independent-validation-architecture.md` §3).
- Tamper-evidence rests on the managed retention lock (external anchoring deferred under DF-ASR-2026-C002) continuing and on the
  storage provider's integrity.
- Retention-lock and storage immutability guarantees are provider features
  whose claims (vendor-claim) require verification during implementation.
