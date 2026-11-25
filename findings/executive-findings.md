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

- **One baseline.** The assurance program is now one traceable baseline instead of nine overlapping documents: 113 requirements approved in principle by the owner, plus BR-VAL-004 (the review protocol, added from owner direction DF-ASR-2026-0D0F), for 114 in total. They consolidate 208 source items: all 189 original owner requirement IDs, 18 reconciliation challenge findings and one validation paragraph. No mandatory obligation is removed; scope narrowing is labelled *simplify* and stated.
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
| Requirements | 189 IDs in 9 documents, 102 registered; heavy duplication (identity ×5, exceptions ×4, status ladder ×4) | `requirements/baseline.csv`: 114 baseline requirements, each with tenet, policy, control objective, risk, basis, owner, enforcement class, automation target, validation, triggers, evidence, failure behavior, residual risk, status, external dependencies, cost and disposition |
| Dispositions | None | `requirements/source-dispositions.csv` (208 items): 149 consolidate, 34 retain, 12 simplify, 8 strengthen, 2 replace, 2 defer, 1 remove (editorial) |
| Hierarchy | Implicit | 17 tenets → 83 policy entries → 121 controls → 114 baseline requirements → enforcement, validation (39 procedures) and evidence (35 types); generated traceability matrix |
| Questions | 113 rows; statuses partly wrong | 115 rows, each with classification, blocking tier, lowest-cost resolution and next action; none block requirements finalization; 15 block implementation, 39 production launch and 33 external examination |
| Capabilities | Responsibilities assigned from READMEs and earlier assessments | `integration/capability-verification.csv`: 27 systems, tests executed for 5 |
| Economics | Five scenarios, one product, baseline not separated | Activity-based v2: 8 alternatives × 1–15 products × 8 scenarios, marginal cost and break-even; v1 preserved |
| Roadmap | Narrative | `roadmap/backlog.csv`: 62 dependency-ordered work items across 15 repositories plus owner and managed-service actions; every live baseline requirement has a work item |

## 2. Most important findings

1. **Owner-assumed capabilities do not exist yet** (`integration/capability-findings.md`):
   - **Vigila** has no logging or observability code. The owner has since retained it as the observability initiative on managed infrastructure (DF-ASR-2026-0D10); the Registry record still needs reconciling.
   - **Strata** is schema governance only; it has no runtime data-access layer. A thin runtime is now approved (DF-ASR-2026-0D10; L effort).
   - **Praxis** provenance is self-reported; verified completion is implemented but enabled nowhere.
   - **Conditor** signing has no trusted keys.
   - **Arca** has hashes but no immutable storage.

   Requirements that rely on these are designed but unenforced (RSK-059).
2. **Governance checks are advisory.** As of 2026-11-25 the repositories did not enforce required checks on `main` (RSK-060). Enforcing them is the cheapest high-value action (WI-ASR-03).
3. **Funds flows.** Unbound is designed so that sponsors hold all accounts and Unbound only relays instructions.
   - **Our working interpretation, not a legal conclusion:** the first limb of FinCEN's definition (acceptance and transmission) is not met. The second limb ("any other person engaged in the transfer of funds") needs counsel's view for instruction and file transmission.
   - **No production activation while material money-transmitter and custody questions are unresolved.** Under owner direction DF-ASR-2026-0D0F these are worked by internal primary-source research; no counsel is engaged now. **Two flows need specific answers before release 1:** the payment-partner rail, where the partner holds funds in transit, and white-label for banks (`findings/financial-integrity-review.md`, ODP-05).
   - **New activation gate, BR-FIN-020:** refuses configurations that would put Echelon in the flow of funds, and records who holds each dispatch credential. Unbound must never hold a credential that alone can dispatch money.
4. **Contradictions resolved by strengthening, not weakening:**
   - AI agents never authorize money movement in any scope (DF-ASR-2026-C004).
   - Periods cannot be reopened after statements issue.
   - Replay never posts entries or dispatches instructions.
   - Idempotency payload conflicts open a case rather than going to a dead-letter queue (DF-ASR-2026-C006).
   - No user-supplied HTML is accepted as markup (DF-ASR-2026-C003).
5. **Simplified evidence architecture.** The custom hash-chained ledger with witness anchoring and timestamps is replaced by managed storage under a compliance-mode retention lock from the first production evidence, separately sourced population reconciliation and an externally hosted heartbeat. These detect failure and external compromise, not action by the single owner. Anchoring is deferred until someone requires it (DF-ASR-2026-C002).
6. **Economics** (`economics/v2/README.md`; all inputs hypothetical or vendor claims, corrected over four challenge rounds):
   - At one product, five-year midpoints run USD 1.17–1.44M (USD 2.02M for a full internal platform), and the ranges span about 3×.
   - Minimum viable assurance has the lowest or near-lowest midpoint at one or two products in every scenario, with outsourced management within about 4%.
   - Targeted automation is cheapest only with auditor reliance on system-generated evidence and about five or more products.
   - Under non-reliance, minimum viable and outsourced are tied, and evidence automation never pays.
   - Prevention and testing automation adds about USD 50–70k a year for risk reduction the model does not monetize.
   - Owner capacity, rather than cash, is likely the binding constraint.
   - Owner direction (DF-ASR-2026-0D0F) prioritizes reusable continuous assurance and operational evidence, without assuming auditor reliance. In that planning case (S8), minimum viable assurance is cheapest at every scale. Continuous-assurance automation costs more and saves no owner hours, so its case is risk reduction and operational evidence. Build first what has operational value and reuse, and defer audit-only evidence collection.
7. **Independence is the structural constraint.** With one human, organizational independence is unavailable. Technical separation (separate verifier identity) is designed, but it is not organizational independence. Eight internal-independent validation procedures have no independent performer today, and owner-administered alerts and reconciliation detect failures but not owner action. Under owner direction DF-ASR-2026-0D0F no fractional reviewer is engaged now; internal multi-agent review with deterministic verification is used for development review, and the 94 controls whose `Tested` status depends on internal-independent validation (VAL-025) cannot reach it until an organizationally independent reviewer exists.
8. **Questions.** No question was falsely closed:
   - 28 depend on the auditor, 21 on counsel and 16 on customers.
   - 18 are answered, 17 of them by owner decision; 2 proposals remain on commercial intent (OQ-114, OQ-766).
   - None blocks requirements finalization. 95 remain open, with their external dependencies preserved.

## 3. Owner decisions and what remains

All twelve owner decision package items are decided, by two owner records of
2026-11-25.

**DF-ASR-2026-0D0F** (internal review; external services deferred):

- internal multi-agent review instead of purchased independent review;
- no counsel engagement now;
- external testing deferred and reassessed before material production launch;
- no additional custodian;
- continuous assurance prioritized without assuming auditor reliance;
- no self-approval of material exceptions.

**DF-ASR-2026-0D10** (remaining approvals):

- baseline approved in principle;
- emergency-delegate and protected-reporting roles defined (`governance/operating-model.md`), with naming deferred;
- Vigila retained as the observability initiative on managed infrastructure;
- a thin Strata runtime;
- restricted research-agent design;
- US-first operations and deferred card payments;
- AICPA standards research;
- no AI authorization of money movement;
- mandatory Praxis governance and rulesets;
- a new tenet, evidence by construction (TNT-17).

Both are reconciled into the baseline, registers, economics and roadmap.

**Remaining owner actions:**

- Apply repository rulesets on `main` (WI-ASR-03). Praxis 3.25.0 was installed on 2026-11-25.
- Name the delegate and protected-reporting recipient when operationally necessary (proposed: no later than production launch; OQ-607).

## 4. Claims status

- No control, policy or system is claimed to be operating, effective, compliant, certified or attested.
- Internal automated testing is never described as a penetration test or attestation.
- This consolidation and its review passes were performed by AI agents controlled by Echelon. They are a structured self-review, not independent assurance (`findings/validation-report.md`).
