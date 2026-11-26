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

- **One baseline.** The assurance program is now one traceable baseline instead of nine overlapping documents: 113 requirements approved in principle by the owner plus four additions traced to owner items (BR-VAL-004 from ODP-02; BR-LOG-002, BR-APP-014 and BR-ASR-012 from ODP-06, ODP-07 and OT-01), for 117 in total. They consolidate 222 source items: all 189 original owner requirement IDs, 18 reconciliation challenge findings, one validation paragraph and the 14 owner decision items of 2026-11-25. No mandatory obligation is removed; scope narrowing is labelled *simplify* and stated.
- **Validated by tooling.** F# tooling enforces the structure. It fails on:
  - orphaned requirements and controls not supported by a baseline requirement;
  - missing owners, validation or evidence;
  - policies, validation or evidence that do not match the requirement's controls;
  - status inflation, including in customer answers and capability status;
  - undispositioned questions;
  - requirements without work items, and backlog ordering errors;
  - open questions without a reason, responsible role and trigger, or answered without evidence;
  - production activation gates that omit a gating requirement or blocking question, or are met while a cited question is open;
  - owner decision items without bidirectional traceability.

  The checks are structural: they cannot prove that a link is semantically right, which is why the challenge passes reviewed the content (`findings/validation-report.md`).
- **Nothing operates yet.** No control is above `Designed`, and no compliance, certification or operating-effectiveness claim is made.
- **Missing capabilities.** The program is not yet implementation-ready, because several capabilities the owner decisions assumed exist do not.
- **Economics by purpose.** Under the ODP-09 recalculation, investments are judged by the purpose of the evidence they produce and by measured manual evidence hours, not by auditor reliance or product-count thresholds (`economics/v2/README.md`).

## 1. What changed

| Area | Before | After |
|---|---|---|
| Requirements | 189 IDs in 9 documents, 102 registered; heavy duplication (identity ×5, exceptions ×4, status ladder ×4) | `requirements/baseline.csv`: 117 baseline requirements, each with tenet, policy, control objective, risk, basis, owner, enforcement class, automation target, validation, triggers, evidence, failure behavior, residual risk, status, external dependencies, cost and disposition |
| Dispositions | None | `requirements/source-dispositions.csv` (222 items): 149 consolidate, 36 retain, 16 strengthen, 13 simplify, 5 replace, 2 defer, 1 remove (editorial) |
| Hierarchy | Implicit | 18 tenets → 84 policy entries → 121 controls → 117 baseline requirements → enforcement, validation (41 procedures) and evidence (35 types); generated traceability matrix; 11 production activation gates (`registers/production-gates.csv`) |
| Questions | 113 rows; statuses partly wrong | 115 rows, each with classification, blocking tier, lowest-cost resolution and next action, and for every open question the reason it is unresolved, the responsible role and the resolution trigger; none block requirements finalization. After owner record DF-ASR-2026-0D11, 25 remain open (2 block implementation, 12 production launch, 9 external examination, 2 none), and each question records its architectural decision, requirements definition, implementation verification and external acceptance separately |
| Capabilities | Responsibilities assigned from READMEs and earlier assessments | `integration/capability-verification.csv`: 27 systems; tests executed for 5 (the Praxis run timed out before its attested-completion tests, so Praxis is partially tested) |
| Economics | Five scenarios, one product, baseline not separated | Activity-based v2: 8 alternatives × 1–15 products × 13 scenarios (including the ODP-09 no-examination, partial-reliance, full-reliance and customer-specific cases), cost by evidence purpose, marginal cost; v1 preserved |
| Roadmap | Narrative | `roadmap/backlog.csv`: 64 work items (63 live, 1 withdrawn), dependency-ordered across 15 named repositories, a placeholder for the observability owner repository, and owner and managed-service actions; every live baseline requirement has a work item |

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
   - **Our working interpretation, not a legal conclusion:** the first limb of FinCEN's definition (acceptance and transmission) is not met. The second limb ("any other person engaged in the transfer of funds") needs a recorded legal resolution (internal research; external advice only after an owner reassessment under DF-ASR-2026-0D0F) for instruction and file transmission.
   - **No production activation while material money-transmitter and custody questions are unresolved.** Under owner direction DF-ASR-2026-0D0F these are worked by internal primary-source research; no counsel is engaged now. **Two flows need specific answers before release 1:** the payment-partner rail, where the partner holds funds in transit, and white-label for banks (`findings/financial-integrity-review.md`, ODP-05).
   - **New activation gate, BR-FIN-020:** refuses configurations that would put Echelon in the flow of funds, and records who holds each dispatch credential. Unbound must never hold a credential that alone can dispatch money.
4. **Contradictions resolved by strengthening, not weakening:**
   - In financial domains AI agents are capped at A2: they never prepare, alter or release payment instructions and never authorize money movement in any scope (BR-AI-001, BR-FIN-006, DF-ASR-2026-C004).
   - Periods cannot be reopened after statements issue.
   - Replay never posts entries or dispatches instructions.
   - Idempotency payload conflicts open a case rather than going to a dead-letter queue (DF-ASR-2026-C006).
   - No user-supplied HTML is accepted as markup (DF-ASR-2026-C003).
5. **Simplified evidence architecture.** The custom hash-chained ledger with witness anchoring and timestamps is replaced by managed storage under a compliance-mode retention lock from the first production evidence, separately sourced population reconciliation and an externally hosted heartbeat. These detect failure and external compromise, not action by the single owner. Anchoring is deferred until someone requires it (DF-ASR-2026-C002).
6. **Economics** (`economics/v2/README.md`; all inputs hypothetical or vendor claims; recalculated under ODP-09 on 2026-11-26):
   - At one product, five-year midpoints run USD 1.27–1.54M (USD 2.12M for a full internal platform) in the base case (S0, which assumes auditor reliance and still charges the contracted-reviewer lines the owner declined; in the owner-direction case S8, minimum viable assurance is USD 1.19M), and the ranges span about 3×. No ranking at one or two products is reliable.
   - Minimum viable assurance (A4) has the lowest midpoint at one and two products in every scenario, including the ODP-09 cases.
   - Costs are now separated by evidence purpose. The prevention and testing package (A8) costs about USD 245k more than A4 over five years at one product: prevention USD 192k, financial integrity USD 34k and test infrastructure USD 19k. Its case is risk reduction, which the model does not monetize.
   - Auditor packaging is the cost most sensitive to examination plans: it disappears without an examination and falls by about a fifth under partial reliance.
   - Evidence-collection automation (A5) has a lower midpoint than A4 at five or more modelled products in the no-examination, customer-specific and full-reliance cases, because manual evidence preparation is needed for operations and customers too. This rests on a hypothetical estimate of manual evidence hours, to be measured before deciding. It is a description of results, not a threshold.
   - Owner capacity, rather than cash, is likely the binding constraint. No external consulting budget is authorized.
7. **Independence is the structural constraint.** With one human, organizational independence is unavailable. Technical separation (separate verifier identity) is designed, but it is not organizational independence. Eight internal-independent validation procedures have no independent performer today, and owner-administered alerts and reconciliation detect failures but not owner action. Under owner direction DF-ASR-2026-0D0F no fractional reviewer is engaged now; internal multi-agent review with deterministic verification is used for development review, and `Tested` rests on protected deterministic verification (VAL-040, technically separated) for executable controls. Multiple-agent review (VAL-041) is approved for development review only and never supports `Tested`, so judgment controls cannot reach `Tested` until a technically separated or organizational validator exists. Technical separation is not organizational independence and is never described as such; a stakeholder who requires organizational independence (an examiner, a bank partner) is told so (ODP-02).
8. **Questions.** No question was falsely closed:
   - Owner record DF-ASR-2026-0D11 (2026-11-26) approved internal design resolutions for 73 questions, 71 of them previously open. It did not resolve their requirements definition, implementation verification or external acceptance; those are tracked separately in `registers/question-dispositions.csv`, and external acceptance remains pending for most of them (auditor, legal, customer and vendor dependencies preserved).
   - 88 are answered (87 by owner decision, one, OQ-807, by implementation evidence); 2 are superseded; 25 remain open: 8 depend on customers, 5 on implementation or deployment facts, 4 on the auditor, 4 on future standards and 4 on vendors or insurers.
   - None blocks requirements finalization.

## 3. Owner decisions and what remains

All twelve owner decision package items are decided, by two owner records of
2026-11-25.

**DF-ASR-2026-0D0F** (internal review; external services deferred):

- internal multi-agent review instead of purchased independent review;
- no counsel engagement now;
- external testing deferred and reassessed before material production launch;
- no additional custodian now;
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

**DF-ASR-2026-0D11** (2026-11-26): internal design resolutions approved for 73 questions; verification dimensions tracked separately; listed questions kept open until evidence exists; validate the exact final commit under Praxis before merge. Defining requirements for the approved resolutions is WI-ASR-15.

**Remaining owner actions:**

- Apply repository rulesets on `main` (WI-ASR-03). Praxis 3.25.0 was installed on 2026-11-25.
- Name the delegate and protected-reporting recipient when operationally necessary (proposed: no later than production launch; OQ-607).
- Provision a Praxis verifier with its own identity and pinned keys (WI-PRAXIS-03) for later work. PR #3 was merged without verified completion on the owner's instruction, and plain completion was accepted under the owner's delegation (DF-ASR-2026-0D12).
- Review and merge the fixes made after the merge (pull request #4).

## 4. Claims status

- No control, policy or system is claimed to be operating, effective, compliant, certified or attested.
- Internal automated testing is never described as a penetration test or attestation.
- This consolidation and its review passes were performed by AI agents controlled by Echelon. They are a structured self-review, not independent assurance (`findings/validation-report.md`).
