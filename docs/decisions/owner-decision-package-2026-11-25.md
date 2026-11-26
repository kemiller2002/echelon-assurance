---
id: ASR-ODP-2026-11-25
title: Owner decision package — assurance consolidation
status: decided (DF-ASR-2026-0D0F, DF-ASR-2026-0D10); retained for history
created: 2026-11-25
---

# Owner decision package

> **Owner dispositions (2026-11-25).** Owner direction DF-ASR-2026-0D0F (`docs/decisions/owner-review-direction-2026-11-25.md`) decided ODP-02, 03, 05, 09, 11 and 12, as follows.
>
> - **ODP-02:** internal multi-agent review and deterministic verification; no fractional reviewer now; self-approval of material exceptions not authorized.
> - **ODP-03:** architectural safeguards and documented concentration risk; no additional custodian now; production risk acceptance remains separate.
> - **ODP-05:** continue with internal regulatory research; sponsor-controlled funds-flow safeguards and activation gates retained.
> - **ODP-09:** prioritize reusable continuous assurance and operational evidence; no consulting budget; costs and benefits recalculated without assuming auditor reliance.
> - **ODP-11:** comprehensive continuous internal security testing; external engagement decisions deferred and reassessed before material production launch.
> - **ODP-12:** no legal engagement now; primary-source research and precise questions continue.
>
> The direction waives no obligation and authorizes no production activation with unresolved material legal risk.
>
> **Remaining items, decided by owner record DF-ASR-2026-0D10** (`docs/decisions/owner-remaining-approvals-2026-11-25.md`):
> - **ODP-01:** baseline approved in principle, subject to traceability and validation.
> - **ODP-04:** roles and procedures defined; naming individuals deferred until operationally necessary.
> - **ODP-06:** Vigila retained on managed infrastructure; capabilities to be verified.
> - **ODP-07:** thin Strata runtime approved.
> - **ODP-08:** restricted research-agent design approved; this is not production residual-risk acceptance.
> - **ODP-10:** US-first operations; deferred card payments; AICPA research; no AI money authorization; mandatory Praxis governance and rulesets.
>
> The two owner records govern over conflicting recommendations in the table below.
>
> The table below is preserved as the original recommendation record.

This package contains only decisions that are genuinely reserved to the owner:

- changes to approved tenets or decisions;
- residual-risk acceptance;
- naming people;
- expenditure;
- resolving conflicts between owner records.

Everything else has been reconciled and documented without owner involvement
(journal `JR-ASR-2026-C001`). Each item gives a recommendation and states what
it blocks. No item authorizes spending, external engagements or changes to
other repositories.

| # | Decision | Recommendation | Blocks |
|---|---|---|---|
| ODP-01 | Approve the requirements hierarchy and the consolidated baseline: 113 baseline requirements from 208 dispositioned source items. The source items are 189 owner requirement IDs from 8 documents, 18 reconciliation challenge findings (REC-01..18) and the security-by-construction "validation and claims" paragraph. Dispositions: 149 consolidate, 34 retain, 12 simplify (scope narrowed, stated), 8 strengthen, 2 replace (the custom anchored ledger and PR #2's hash chain), 2 defer (formal methods; commercial-impact measurement), 1 remove (REC-04, an editorial identifier instruction). No mandatory obligation is removed. The baseline already reflects the proposed decisions; it reverts item by item if a decision is rejected. | Accept DF-ASR-2026-C001, C002, C003, C006. | Requirements finalization (OQ-802) |
| ODP-02 | Independence with one human. Organizational independence is not available internally. Choose: (a) contract a fractional independent reviewer, budgeted at USD 10–30k a year plus about USD 3–10k per product (hypothetical), for the **key controls** listed below only, starting before an observation period; or (b) accept that those controls cannot be claimed as organizationally independently assessed until a reviewer exists. | (a), deferred until P3; until then, (b) applies and no such claim is made. Approve the key-control list, and the one-human exception rule in BR-GOV-004: owner-originated material exceptions are self-approved, disclosed, and reviewed retrospectively, and are blocked where an obligation requires independent approval. Set the capacity threshold that triggers contracting or hiring (proposed: recurring assurance labor above 25% of owner capacity). | OQ-608, OQ-701, OQ-800, OQ-803 |
| ODP-03 | Concentration of privileged access. The owner holds cloud root, organization owner, verifier keys and evidence-store administration. Accept as residual risk with independent alerting and break-glass logging (BR-IAM-002), or designate a second custodian for verifier and evidence keys. | Accept with compensating alerts now. These detect failures and external compromise, not owner action. Apply a compliance-mode retention lock from the first production evidence (BR-ASR-006). Designate a second custodian before an observation period. | OQ-703, OQ-804 |
| ODP-04 | Succession and protected reporting. Name an emergency delegate, and an external recipient for whistleblowing and complaints about the owner. | Name both. These are no-cost appointments, and only the owner can make them. | OQ-607, BR-GOV-013, BR-HR-002 |
| ODP-05 | Funds-flow perimeter. Approve BR-FIN-020 (activation gate refusing any configuration where Echelon receives, holds or can unilaterally move money). Decide whether the payment-partner rail (F9: partner holds funds in transit) and white-label for banks (F12) stay in release 1. Counsel's position is needed before any production launch (ODP-12), and before F9 or F12 specifically. | Approve BR-FIN-020. Keep F9 behind a counsel-status gate. Exclude F12 from release 1 until counsel answers CQ-1 and CQ-3. | OQ-216, OQ-205, OQ-206 |
| ODP-06 | Conflict between owner records on observability. Decision 0D0D says Vigila is the observability initiative; Registry research package RP-REG-2026-0001 (same date) says "do not build an application-observability system yet". Capability verification found that Vigila has no logging code. | Amend 0D0D: use managed cloud logging plus a managed external heartbeat now (BR-LOG-001, BR-ASR-008); Vigila keeps follow-up tracking only; revisit an Echelon observability system on demand. | OQ-806, WI-VIGILA-01 |
| ODP-07 | Conflict on Strata scope. Decision 0D0C makes Strata the database interaction layer; Unbound CNV-ARC-004 limits it to schema governance; no runtime layer exists (L effort). | Build a thin `Strata.Runtime` (parameterized commands, transactions, tenant binding) before Unbound implementation, and record the same decision in Unbound. The alternative, an analyzer plus database-role confinement with no shared runtime, is cheaper but weaker for reuse. | OQ-760, WI-STRATA-04 |
| ODP-08 | Prompt-injection residual risk. Accept residual risk for research agents limited to no privileged write, no consequential tools and no sensitive egress (BR-AI-004), or restrict further. | Accept with those restrictions. Reassess on any tool-permission increase. | OQ-616 |
| ODP-09 | Economic direction (DF-ASR-2026-C005, corrected after four challenge rounds). At one or two products the options are close: minimum viable assurance has the lowest midpoint, with outsourced management within about 4%. Under auditor non-reliance the two are tied at every scale, and evidence-collection automation never pays. Decide: (a) adopt minimum viable assurance (A4) now; (b) classify prevention and testing automation (about USD 50–70k a year, benefit not monetized) as baseline engineering or as a discretionary security investment; (c) gate evidence-collection automation on a written auditor position and three or more products; (d) authorize the no-cost auditor scoping inquiry (WI-EXT-01, P2). | Approve (a), (c) and (d). For (b), treat financial invariant suites and security-by-construction rules as baseline engineering for a financial product. | OQ-802, OQ-312 |
| ODP-10 | Quick confirmations: (a) US-only initial scope (OQ-208); (b) card payments deferred, processor-hosted only if added (OQ-304); (c) a named human creates a free AICPA account to verify the TSC summaries (OQ-100); (d) AI agents never authorize money movement in any scope (DF-ASR-2026-C004, which tightens the OQ-763 disposition); (e) apply rulesets on `main` and install Praxis (WI-ASR-03). | Confirm all five. | OQ-208, OQ-304, OQ-100, OQ-763, OQ-700 |
| ODP-11 | Independent external penetration test before first production launch (BR-VUL-012, WI-EXT-04). No law requires it for Echelon as far as researched; sponsors, banks and insurers commonly expect one (OQ-302, OQ-303, OQ-307). Vendor-claim cost USD 15–35k per test including retest (SRC-PTV-036); no quotes exist. | Approve as a launch gate, with quotes from three qualified firms before engagement (WI-EXT-02). | Production launch |
| ODP-12 | Counsel engagement before production launch on the funds-flow and regulatory questions (CQ-1, CQ-3, CQ-6, CQ-8, CQ-10, CQ-11 in `findings/financial-integrity-review.md`; packages C1–C8 in `findings/counsel-and-auditor-questions.md`). The core money-transmitter question (31 CFR 1010.100(ff)(5)(i)(B)) cannot be resolved by research alone. Bundle the questions into one fixed-scope engagement. | Approve one bundled counsel review before production activation; no engagement before a sponsor is near contract. | Production launch (BR-FIN-020, BR-GOV-009) |

## Key controls proposed for organizationally independent review (ODP-02)

These controls protect money, tenant data, release integrity and the integrity of
the assurance system itself, consistent with the challenge review's mutation-test
scope:

| Area | Controls |
|---|---|
| Ledger | CTL-FIN-001, CTL-FIN-002, CTL-FIN-003, CTL-FIN-004, CTL-FIN-005, CTL-FIN-006, CTL-FIN-010 |
| Tenant isolation | CTL-TEN-001 |
| Application security by construction | CTL-APP-001, CTL-APP-005 |
| Release | CTL-SDL-001, CTL-SDL-002 |
| Access | CTL-IAM-004, CTL-IAM-005 |
| Assurance system | CTL-ASR-003, CTL-ASR-011 |
| AI authority | CTL-AI-002 |

All other controls use automated and internal validation proportionate to risk
(BR-VAL-002).

## What is not in this package, and why

- **Legal conclusions.** The counsel questions are prepared in `findings/counsel-and-auditor-questions.md` and `findings/financial-integrity-review.md`. They are not owner decisions, but engaging counsel requires the owner's approval of spending.
- **Routine consolidation choices.** These are recorded in `requirements/source-dispositions.csv` and the journal. They are reversible.
- **Work items in other repositories.** These are proposals (`roadmap/backlog.csv`). Each repository's own governance decides on them.
