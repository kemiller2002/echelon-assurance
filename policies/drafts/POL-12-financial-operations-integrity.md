---
policy_id: POL-12
title: Financial Operations Integrity Policy
version: 0.1.0-draft
status: draft — not approved, not in force
owner: Finance/controls owner role (defined; held by the owner until designated (repository proposal; ODP-04 itself defines only the delegate and protected-reporting roles); separation from the sole engineering approver is a recorded gap, RSK-065)
approval_authority: Board or sole owner
review: annual
scope: products that record, calculate, authorize or move money or financial positions on behalf of customers (Unbound first)
authoritative_references: [AT-C 320 / SSAE 18 (SOC 1 control objectives, if a SOC 1 is pursued), TSC PI1.1-PI1.5, customer and bank partner contracts, NACHA Operating Rules (if originating ACH), OFAC regulations 31 CFR Chapter V, IRC 4966-4967 (sponsor obligations supported by the product)]
---

# POL-12 Financial Operations Integrity Policy

## Principle
Financial correctness is enforced by invariants in the system and proved by independent-source reconciliation, rather than inferred from sampling individual transactions.

Detailed requirements: the BR-FIN rows of `requirements/baseline.csv` (DF-ASR-2026-C001), which consolidate FIN-BC-001..024 (historical source, `requirements/financial-integrity-by-construction.md`) and apply C006 where they differ; mapped to CTL-FIN controls in the catalog.

## 01 Transaction authorization
1. Every financial action (contribution acceptance, grant recommendation approval, disbursement release, fee assessment, investment instruction, adjustment) requires authority recorded in the customer's authority matrix and enforced by the application.
2. Above customer-defined thresholds, two distinct authorized humans approve; the initiator cannot approve. AI agents may recommend but never prepare, alter or release payment instructions, and never authorize, approve or release funds, in any scope (BR-AI-001, DF-ASR-2026-C004). Execution is deterministic: a deterministic worker acts only under a recorded sponsor (board) delegation or sponsor-user authorization, or an established recurring authority set by the sponsor (for example, a sponsor-approved recurring grant) revalidated at execution, and payment execution stays on the sponsor's rails (ODP-10, DF-ASR-2026-0D10; DF-ASR-2026-C004; BR-FIN-006, BR-FIN-015).

## 02 Accounting integrity
1. The ledger is double-entry; every posting balances; postings are immutable.
2. Invariants are enforced at write time and tested continuously: balanced journals, no negative fund balances where prohibited, fee calculations reproducible from versioned fee schedules, every disbursement traceable to an approved grant.
3. Period close is a recorded event. Ordinary postings to a hard-closed period are rejected and routed to a prior-period adjustment in the first open period; a period is never reopened after statements are issued (BR-FIN-010, DF-ASR-2026-C006).

## 03 Reconciliation
1. Ledger positions are reconciled daily against independent sources (bank, custodian, payment processor statements) obtained directly from the source, not via a user-editable channel.
2. Breaks are recorded with age and owner; aged breaks escalate; resolution requires a second person.
3. Completeness is checked both ways: every external movement has a ledger entry and every ledger movement has an external confirmation.

## 04 Fraud prevention
1. Payees (charities) are verified for eligibility from authoritative sources before disbursement; OFAC screening applies to payees and, where required, donors.
2. Changes to payee bank details require out-of-band verification and a cooling-off period.
3. Anomaly detection runs on contributions, grants and access patterns; alerts are triaged by a human.

## 05 Reversals, corrections and manual adjustments
1. Corrections are compensating entries referencing the original; history is never rewritten.
2. Manual adjustments require a reason code, supporting evidence and approval by someone other than the preparer; all manual adjustments are reported to the customer's designated reviewer.

## 06 Idempotency and completeness
Every externally triggered financial operation carries an idempotency key; duplicate submission cannot create duplicate effects. Event sequences are gap-checked.

## Change history
| Version | Date | Change | Approved by |
|---|---|---|---|
| 0.1.0-draft | 2026-11-25 | Initial draft | Not approved |
