---
id: ASR-FIND-EXEC-001
title: Executive findings
status: draft for owner review
version: 2.0.0
created: 2026-11-25
updated: 2026-11-25 (assurance consolidation, journal JR-ASR-2026-C001; v1.1.0 in git history at 37cbdc5)
---

# Executive findings: assurance consolidation

**Bottom line.**

- **One baseline.** The assurance program is now one traceable baseline instead of nine overlapping documents: 113 requirements. They consolidate 208 source items: all 189 original owner requirement IDs, 18 reconciliation challenge findings and one validation paragraph. No mandatory obligation is removed; scope narrowing is labelled *simplify* and stated.
- **Validated by tooling.** F# tooling enforces the structure. It fails on:
  - orphaned requirements and controls not supported by a baseline requirement;
  - missing owners, validation or evidence;
  - policies, validation or evidence that do not match the requirement's controls;
  - status inflation, including in customer answers and capability status;
  - undispositioned questions;
  - requirements without work items, and backlog ordering errors.

  The checks are structural: they cannot prove that a link is semantically right, which is why the challenge passes reviewed the content (`findings/validation-report.md`).
- **Nothing operates yet.** No control is above `Designed`, and no compliance, certification or operating-effectiveness claim is made.
- **Missing capabilities.** The program is not yet implementation-ready, because several capabilities the owner decisions assumed exist do not.
- **External answers.** Whether internal evidence automation pays off depends on one external answer: auditor reliance on system-generated evidence.

## 1. What changed

| Area | Before | After |
|---|---|---|
| Requirements | 189 IDs in 9 documents, 102 registered; heavy duplication (identity ×5, exceptions ×4, status ladder ×4) | `requirements/baseline.csv`: 113 baseline requirements, each with tenet, policy, control objective, risk, basis, owner, enforcement class, automation target, validation, triggers, evidence, failure behavior, residual risk, status, external dependencies, cost and disposition |
| Dispositions | None | `requirements/source-dispositions.csv` (208 items): 149 consolidate, 34 retain, 12 simplify, 8 strengthen, 2 replace, 2 defer, 1 remove (editorial) |
| Hierarchy | Implicit | 16 tenets → 83 policy entries → 121 controls → 113 baseline requirements → enforcement, validation (39 procedures) and evidence (35 types); generated traceability matrix |
| Questions | 113 rows; statuses partly wrong | 115 rows, each with classification, blocking tier, lowest-cost resolution and next action; 9 block requirements finalization, all owner decisions; 19 block implementation |
| Capabilities | Responsibilities assigned from READMEs and earlier assessments | `integration/capability-verification.csv`: 27 systems, tests executed for 5 |
| Economics | Five scenarios, one product, baseline not separated | Activity-based v2: 8 alternatives × 1–15 products × 8 scenarios, marginal cost and break-even; v1 preserved |
| Roadmap | Narrative | `roadmap/backlog.csv`: 60 dependency-ordered work items across 15 repositories; every live baseline requirement has a work item |

## 2. Most important findings

1. **Owner-assumed capabilities do not exist yet** (`integration/capability-findings.md`):
   - **Vigila** has no logging or observability code, and the Registry records a same-day owner deferral of observability (ODP-06).
   - **Strata** is schema governance only; it has no runtime data-access layer (ODP-07, L effort).
   - **Praxis** provenance is self-reported; verified completion is implemented but enabled nowhere.
   - **Conditor** signing has no trusted keys.
   - **Arca** has hashes but no immutable storage.

   Requirements that rely on these are designed but unenforced (RSK-059).
2. **Governance checks are advisory.** As of 2026-11-25 the repositories did not enforce required checks on `main` (RSK-060). Enforcing them is the cheapest high-value action (WI-ASR-03).
3. **Funds flows.** Unbound is designed so that sponsors hold all accounts and Unbound only relays instructions.
   - **Our working interpretation, not a legal conclusion:** the first limb of FinCEN's definition (acceptance and transmission) is not met. The second limb ("any other person engaged in the transfer of funds") needs counsel's view for instruction and file transmission.
   - **Two flows need counsel before release 1:** the payment-partner rail, where the partner holds funds in transit, and white-label for banks (`findings/financial-integrity-review.md`, ODP-05).
   - **New activation gate, BR-FIN-020:** refuses configurations that would put Echelon in the flow of funds, and records who holds each dispatch credential. Unbound must never hold a credential that alone can dispatch money.
4. **Contradictions resolved by strengthening, not weakening:**
   - AI agents never authorize money movement in any scope (DF-ASR-2026-C004).
   - Periods cannot be reopened after statements issue.
   - Replay never posts entries or dispatches instructions.
   - Idempotency payload conflicts open a case rather than going to a dead-letter queue (DF-ASR-2026-C006).
   - No user-supplied HTML is accepted as markup (DF-ASR-2026-C003).
5. **Simplified evidence architecture.** The custom hash-chained ledger with witness anchoring and timestamps is replaced by managed storage under a compliance-mode retention lock from the first production evidence, separately sourced population reconciliation and an externally hosted heartbeat. These detect failure and external compromise, not action by the single owner. Anchoring is deferred until someone requires it (DF-ASR-2026-C002).
6. **Economics** (`economics/v2/README.md`; all inputs hypothetical or vendor claims, corrected over three challenge rounds):
   - At one product, five-year midpoints run USD 1.17–1.44M (USD 2.02M for a full internal platform), and the ranges span about 3×.
   - Minimum viable assurance has the lowest midpoint at one or two products in every scenario, with outsourced management within about 4%.
   - Targeted automation is cheapest only with auditor reliance on system-generated evidence and about five or more products.
   - Under non-reliance, minimum viable and outsourced are tied, and evidence automation never pays.
   - Prevention and testing automation adds about USD 50–70k a year for risk reduction the model does not monetize.
   - Owner capacity, rather than cash, is likely the binding constraint.
   - Recommendation (DF-ASR-2026-C005): minimum viable assurance now. Prevention automation is justified on risk, and the owner decides whether to treat it as baseline engineering. Evidence-collection automation is gated on a written auditor position and scale. A capacity threshold triggers contracting or hiring. Never a feature-parity platform without demand.
7. **Independence is the structural constraint.** With one human, organizational independence is unavailable. Technical separation (separate verifier identity) is designed, but it is not organizational independence. Eight internal-independent validation procedures have no independent performer today, and owner-administered alerts and reconciliation detect failures but not owner action. Fractional review is proposed for 17 key controls only (ODP-02).
8. **Questions.** No question was falsely closed:
   - 28 depend on the auditor, 21 on counsel and 15 on customers.
   - 22 are proposed decisions awaiting the owner. OQ-760 and OQ-806 were reopened because owner records conflict, and OQ-800 because its one-human rule awaits ODP-02.
   - Only 9 block requirements finalization, and all 9 are owner decisions that need no paid advice.

## 3. What the owner needs to do

Review `docs/decisions/owner-decision-package-2026-11-25.md`. It contains ten
items, each with a recommendation. Nothing else needs owner involvement to
proceed with specification work.

## 4. Claims status

- No control, policy or system is claimed to be operating, effective, compliant, certified or attested.
- Internal automated testing is never described as a penetration test or attestation.
- This consolidation and its review passes were performed by AI agents controlled by Echelon. They are a structured self-review, not independent assurance (`findings/validation-report.md`).
