---
id: ASR-FND-CLAR-002
title: Clarity and EDF assessment of the assurance program (consolidation)
status: draft for owner review
created: 2026-11-25
supersedes: ASR-FND-CLAR-001 (previous version of this file, in git history at 37cbdc5)
methods:
  edf: echelon-diagnostic-framework v0.3 (frozen baseline, docs/edf/specification.md) @ 710d54c
  clarity: clarity-framework @ 200a9eb — Clarity Result template (status Candidate), assessor guide Level 3
---

# Clarity and EDF assessment

This version replaces the previous assessment. The previous one cited EDF v0.3
and the Clarity Result template, but it did not conform to them:

- System Context was not split into its three parts.
- Its outcome was a goal rather than an observed fact.
- It typed no origins and no propagation paths.
- It had no Confidence, Unknowns or Next Responsible Action.
- It had no "Framework Concepts Observed" section.
- It named no owners.
- Its monitoring had no thresholds or triggers.

The conformance gaps are listed in the journal `JR-ASR-2026-C001`.

**Method caveats (required by both methods' own evidence records).**

- EDF v0.3 is a working grammar. Its value over neutral structure is unproven: the 2026-11-16 improvement research found no measurable advantage on synthetic cases.
- The Clarity Result format has Candidate status.
- Both assessments were written by the same agent that performed the consolidation. They are a structured self-review, not independent assurance (BR-VAL-001). An owner or external read is the calibration step.

---

## Part A — EDF-2 diagnosis

**Level.** EDF-2: several interacting systems (nine requirement documents,
fifteen Echelon repositories and external obligations), disputed origins and
high consequence.

**Diagnostic Calibration.** Not performed. The only independent second analysis
available is another model run with the same inputs, which EDF says is not
independent. It is recommended when an owner or external reviewer can provide
one (ODP-02; no external reviewer is engaged now, DF-ASR-2026-0D0F).

### System Context
- **Primary System:** the Echelon Foundry assurance program as recorded in
  `kemiller2002/echelon-assurance`: its requirements, controls, decisions and
  registers, and the assurance responsibilities it assigns to Echelon systems.
- **Context:** owner-approved foundations (0D01–0D0E) and Unbound and daf-landscape
  requirements. The Context also includes the Echelon system repositories as they
  stood on 2026-11-25. External obligations (SOC 1/2 design target, GLBA baseline,
  payment and tax rules) are another part, and a one-human organization with AI
  agents is the last.
- **Focus:** why the program, before this consolidation, could not serve as an
  implementation-ready baseline.

### Outcome (observed, 2026-11-25, before consolidation)
- 189 owner requirement IDs sat across nine documents, and 102 of them were registered.
- The documents overlapped: for example, exception rules were stated four times and evidence status ladders four times.
- All 121 controls were at `Designed`.
- Owner decisions assigned evidence, logging, database and provenance responsibilities to systems that do not implement them. Source inspection and executed tests confirm this (`integration/capability-verification.csv`).
- Governance checks required by decision 0D07 were advisory, not platform-enforced.
- The economic model compared alternatives without separating baseline engineering, without the same reviewer costs, and without scaling by product count.

### Manifestations (observable in the Primary System)
1. The same requirement appears under several IDs, for example identity: SBC-008, AIG-BC-001, AIG-BC-009, CA-PROV-01 and CA-PROV-02.
2. Contradictions:
   - FIN-BC-011 allows reopening a period that Unbound forbids reopening.
   - The OQ-763 disposition implies agents may move money within scope.
   - CTL-ASR-002 calls for a custom anchored ledger, against owner decision 0D0A.
   - Decision 0D0D (Vigila) conflicts with the Registry deferral of observability.
3. Questions were wrongly marked: OQ-802 and OQ-803 were "answered" without a key-control list, and OQ-216 and OQ-217 were described as missing although they existed.
4. Integration targets were described as responsibilities, not verified capabilities, in the PR #2 document.
5. Two control catalogs exist (642 ECTL controls in due diligence and 121 CTL controls here) with no crosswalk.

### Origin Network (typed)
| Origin | Type | Evidence |
|---|---|---|
| Requirements written per topic batch, each mapping to controls but none consolidating across batches | Organizational | Nine documents dated the same day, each ending with "reconcile catalog, roadmap…" |
| Integration responsibilities assigned from READMEs and earlier assessments rather than executed verification | Technical | Capability matrix: Vigila absent; Strata has no runtime layer; Praxis provenance is self-reported |
| A single human is the approver for every decision class, so independence rules collide | Human / Organizational | DF-ASR-2026-7B20; OQ-607, OQ-608 and OQ-800 |
| Agents produce requirements faster than the owner can approve them, and proposals accumulate as "proposed" | Organizational | 18 proposed decisions awaiting approval in question dispositions |
| External obligations are undetermined (auditor reliance, legal applicability), so designs hedge in prose instead of branching explicitly | Regulatory | 28 questions depend on an auditor and 21 on legal research (no counsel is engaged; prepared questions only, ODP-12) |
| Cost estimates are not activity-based and not scaled by product | Financial | `economics/parameters.csv` (original) |

### Propagation (typed)
- **Informational:** each batch's prose dispositions of questions were copied into later batches without re-reading the register, so the wrong statuses for OQ-802 and OQ-803 persisted.
- **Organizational:** owner decisions recorded in one repository (Registry, Unbound) are not visible in another, so the Vigila and Strata conflicts arose.
- **Financial:** estimates that are not scaled by product make internal build look either always or never worthwhile, depending on the reader.

### Evidence
| Item | Observation or inference |
|---|---|
| 189 IDs, 102 registered, overlap counts | Observed (`requirements/source-dispositions.csv`, generated by script) |
| System capabilities | Observed: tests executed for Arca, Dokimos, Strata, Tutela and partially Praxis; source inspection for the rest. Inference: README claims are excluded. |
| Governance checks advisory | Observed through a read-only API check of branch rules on 2026-11-25 (question disposition agent) |
| Root causes listed in the Origin Network | Inference from the observed artifacts and commit history; not confirmed with the owner |
| Disputed point | Whether the Vigila decision (0D0D) or the Registry deferral reflects the owner's current intent; both records are dated 2026-11-25 |

### Control Points and ranking
| Rank | Control point | Influence | Controllability | Cost | Confidence |
|---|---|---|---|---|---|
| 1 | One authoritative baseline register with a validator that enforces bidirectional traceability and status ceilings | High: stops recurrence of duplication and status inflation | High: internal | Low: done in this initiative | High |
| 2 | Verify capabilities before assigning responsibility; gate claims on executed tests | High | High | Low | High |
| 3 | Owner decision package limited to genuinely owner-reserved items | High: unblocks 8 finalization questions | Medium: depends on owner time | Low | Medium |
| 4 | Platform-enforced rulesets on main | Medium–high | High: owner action | Very low | High |
| 5 | Activity-based, product-scaled economics with an auditor-reliance branch | Medium | High | Low | Medium: inputs hypothetical |
| 6 | Cross-repository decision synchronization (Registry ↔ assurance ↔ Unbound) | Medium | Medium | Low–medium | Medium |

**Confidence:** Medium. The outcome and manifestations are observed. The origins
are reasoned inferences.

**Diagnostic Sufficiency:** sufficient for the next action named below. That
action is reversible and documentation-only, and it changes no production system.
It is **not** sufficient for implementation authorizations, which depend on
capability work and external answers.

**Unknowns:**
- The owner's current intent on observability (0D0D versus the Registry record).
- Auditor reliance on system-generated evidence.
- The legal characterization of the payment-partner rail (internal research; external advice only after an owner reassessment).
- The actual cost of maintaining internal automation.

**Next Responsible Action:** the owner applies the protected-branch ruleset (WI-ASR-03); the decision package is decided (DF-ASR-2026-0D0F, DF-ASR-2026-0D10).

---

## Part B — Clarity Result (Level 3 Formal)

**Effort level:** Level 3 (Formal). The program is externally reviewable,
regulated-adjacent and of high consequence, and some of its commitments have low
correctability (retention destruction, funds-flow characterization).

### 1. Executive Understanding
*Updated 2026-11-26 for the final reconciliation (owner directions DF-ASR-2026-0D0F and DF-ASR-2026-0D10).*
The program now has one baseline: 117 requirements (113 approved in principle
plus four traced additions) that consolidate 222 source items, including all 189 original owner requirements; no mandatory obligation is removed. The baseline is traced
to tenets, policies, controls, validation and evidence, and a validator enforces
the tracing. Nothing operates yet. The largest uncertainties are external:
legal characterization of funds flows (worked by internal research only; no
counsel engaged, ODP-12), and insurer, customer and examiner requirements.
Production activation is gated (`registers/production-gates.csv`, all eleven
gates `not-met`). The largest internal gaps are capabilities that the owner decisions
assumed exist.

### 2. Clarity Snapshot
| Claim | Posture | Confidence | Owner |
|---|---|---|---|
| C1. The baseline is complete for the owner requirement set | commitment | High (validator-enforced) | Owner (accountable), assurance coordinator agent (maintains) |
| C2. Security-by-construction requirements eliminate the named vulnerability classes within verified boundaries | discovery | Medium (design only; no boundary implemented) | Engineering lead |
| C3. Unbound's design keeps Echelon out of custody and money transmission | discovery | Medium–Low (flow F9 and bank white-label unresolved) | Owner (internal research; prepared questions, ODP-12; activation gated, PG-02) |
| C4. Continuous internal testing reduces external testing cost | discovery | Low (no data; external fee unaffected by assumption) | Engineering lead |
| C5. Assurance investments are justified by the purpose of their evidence (ODP-09) | discovery | Low–Medium (recalculated by purpose and reliance case, S9–S12; A4 lowest at 1–2 products in every case; prevention package about USD 245k over five years at one product, justified by unmonetized risk reduction; evidence automation depends on measured manual evidence hours, not on product-count thresholds) | Owner |
| C6. One human can operate the program with AI agents, multiple-agent review and deterministic verification (no contracted reviewer, DF-ASR-2026-0D0F) | reassessment | Medium–Low (separation-of-duties conflicts persist) | Owner |
| C7. Existing Echelon systems can provide the assurance capabilities | reassessment | Low (capability verification contradicts 4 of the assumptions) | Owner |

### 3. Current Understanding
See the baseline (`requirements/baseline.csv`), the decisions (`DF-ASR-2026-C001`
to `C006`) and `findings/executive-findings.md`.

### 4. Evidence Landscape
- Strong: executed test results for five systems; structural validation of the registers; primary regulatory texts for the FTC Safeguards Rule and for FinCEN's definition of a money transmitter.
- Weak: every cost figure (hypothetical or vendor claim); auditor positions (none obtained); SIG, CAIQ and VSA content (summaries only).

### 5. Assumption Landscape
- **Explicit:**
  - Additional products add about 30% to examination fees.
  - Labor costs USD 100–175 per hour.
  - Auditors will test and possibly rely on system evidence.
  - Sponsors hold all accounts.
- **Shadow (surfaced here):**
  - The owner has time to approve decisions at the rate agents propose them.
  - One combined SOC report boundary can include future products.
  - Managed cloud services' retention locks are acceptable to auditors.
  - Customers accept "Designed" answers during early sales.

### 6. Calibration Review
Confidence labels were lowered where earlier documents implied capability: C7
went from implied-high to Low. On 2026-11-26 six separately initialized
reviewers (A–F) found that several summaries still overstated the validator
and that agent review had been allowed to support `Tested`; both were
corrected (`findings/reviews/RV-ASR-2026-11-26.md`). C1 stays High because the
validator now rejects the corruptions they demonstrated. The economics claim was raised from the original
"no conclusion" to Low–Medium, with an explicit branch on auditor reliance.

### 7. Correctability Review
- Highly correctable: the baseline, decisions, economics and backlog.
- Low correctability:
  - Funds-flow characterization once live; mitigated by BR-FIN-020 activation gate.
  - Irreversible retention disposal; mitigated by BR-DAT-001 hold checks.
  - Public disclosure of sensitive evidence; mitigated by BR-CUS-002 classification filter.

### 8. Monitoring Plan
| Signal | Threshold | Trigger claim | Response | Trigger reassessment |
|---|---|---|---|---|
| Measured manual evidence hours per request (from the first customer or auditor request) | Recorded for two quarters | C5 | Decide evidence-collection automation by purpose and measured hours (ODP-09); auditor reliance is one input, not the gate | Each examination plan |
| Capability work item completion (WI-STRATA-01, WI-PRAXIS-01, WI-VIGILA-02) | Each reaches `done` with executed tests | C7, C2 | Update capability register; raise baseline status only with evidence | Each release of those systems |
| Recorded resolution of a funds-flow legal question (internal research, or external advice after an owner reassessment under 0D0F) | Recorded | C3 | Adjust BR-FIN-020 perimeter, PG-02 and the roadmap | Any new rail or bank customer |
| Owner decision latency | More than 10 proposed decisions older than 30 days | C6 | Reduce proposal rate; batch decisions | Quarterly |
| Actual maintenance hours versus estimate | More than 1.5× estimate for two quarters | C5 | Stop expanding internal automation; owner reassesses capacity relief (no consulting budget is assumed, TNT-18) | Annually |
| Traceability findings in CI | Above zero | C1 | Merge blocked (once WI-ASR-03 is done) | Continuous |

### 9. Reassessment Plan
- Reassess C3 before any production activation.
- Reassess C5 when manual evidence hours have been measured, or when an examination is planned.
- Reassess C6 when a second human joins, when a customer, examiner or bank partner requires organizational independence, or when a material exception cannot be approved without self-approval.
- The watcher is the owner. The assurance coordinator agent prepares the evidence.

### 10. Learning Priorities
1. Measured manual evidence hours for operations, customers and any examination.
2. A recorded resolution of the payment-partner rail and bank white-label questions (internal research first; external advice only on a recorded trigger).
3. The actual maintenance cost of the first automated controls.
4. The actual diligence questions asked by the first sponsor and bank partner.

### 11. Framework Concepts Observed
- **Confidence Debt:** capability claims written before verification.
- **Resolution Debt:** questions marked answered without the required artifact (OQ-802, OQ-803).
- **Governance Debt:** proposed decisions accumulating.
- **Assumption Drift:** the Strata role diverged across three repositories.
- **Burden Externalization:** proposals that shift approval load onto the single owner.
- **Proportional Certainty:** applied in the risk-tiered validation (BR-VAL-002).
- **Verification Debt (2026-11-26):** summaries claimed validator coverage that the rules did not enforce; corrected by corruption experiments and regression tests.
- **Evidence by construction (TNT-17):** the EDF control point "evidence collected after the fact" is replaced by evidence emitted at enforcement points, with failure of collection detected (RSK-066).

### 12. Recommended Next Posture
- **Commitment** on the baseline structure and the security- and financial-by-construction requirements.
- **Discovery** on economics direction, funds-flow characterization and auditor reliance.
- **Reassessment** on system responsibility assignments (Vigila, Strata, Praxis provenance).

### 13. Current Best Understanding
The program is now simpler and internally consistent, but not yet
implementation-ready, because:

- Key capabilities it depends on are not built.
- External answers (legal characterization, customer and examiner requirements) determine when gated capabilities may be activated.

The cheapest high-value steps are:

- enforcing the existing checks;
- building prevention and testing automation that has value regardless of auditor reliance;
- deciding evidence-collection automation by purpose and measured manual evidence hours (ODP-09); an auditor position is sought only if a reassessment under DF-ASR-2026-0D0F authorizes it.
