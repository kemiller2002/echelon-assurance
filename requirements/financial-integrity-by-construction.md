# Financial integrity by construction
Date: 2026-11-25
Status: Proposed detailed requirements under owner-approved assurance direction. No legal opinion, implementation or operating effectiveness claim.

## Normative requirements and verification
Map FIN-BC identifiers to existing CTL-FIN and Unbound requirements; do not replace existing IDs. All tests use synthetic data and F# under Praxis. Each test produces artifact-bound evidence with scope, configuration, verifier and limitations.

| ID | Requirement | Enforcement and validation |
|---|---|---|
| FIN-BC-001 | Balanced posted journals by accounting entity and currency rules | Atomic posting invariant; reject unbalanced journals, partial writes and invalid FX legs; property tests |
| FIN-BC-002 | Posted entries immutable through normal application paths | Restricted update/delete privileges; attempt bypass through APIs, workers and DB roles |
| FIN-BC-003 | Corrections via linked reversal/adjustment | Preserve original and reason, authorization and effective date; test reversals and partial adjustments |
| FIN-BC-004 | Explicit monetary precision and rounding | Decimal or integer minor units with currency metadata; test rounding, overflow, allocation and FX |
| FIN-BC-005 | Idempotent financial obligations | Scoped keys and payload binding, atomic uniqueness, durable outbox; concurrent retries and crash tests |
| FIN-BC-006 | Distinct available/reserved/pending/settled balances | Atomic reservations and state machine; test concurrent spending, holds, returns and settlement |
| FIN-BC-007 | Sponsor/fund/account ownership boundaries | Trusted tenant context and scoped ledger accounts; cross-tenant negative tests |
| FIN-BC-008 | Explicit allowed financial transitions | Typed commands and transition rules; reject invalid transitions and replay |
| FIN-BC-009 | Traceable origin, authorization and evidence | Authenticated principals, correlation IDs, policy version and event linkage; test missing/forged authorization |
| FIN-BC-010 | Two-way external reconciliation | Compare independent bank/custodian/processor populations, balances and transactions; inject missing, duplicate, late and mismatched records |
| FIN-BC-011 | Controlled accounting periods | Lock closed periods; approved reopen or subsequent-period adjustments; test backdating and unauthorized reopening |
| FIN-BC-012 | Reproducible historical reports | Versioned rules, effective and recorded dates, immutable source references; replay and compare reports |
| FIN-BC-013 | No unauthorized movement of assets | Separate recommendation, eligibility, authorization, release and settlement; enforce sponsor authority and risk thresholds; test bypass |
| FIN-BC-014 | No automatic retry of ambiguous external payment outcome | Query provider status, reconcile and quarantine uncertainty; test timeout after external acceptance; do not claim exactly-once external delivery |
| FIN-BC-015 | Fee and investment allocation correctness | Versioned fee schedules, valuation dates, explicit allocation and rounding residuals; independent reference calculations |
| FIN-BC-016 | Contribution lifecycle and tax acknowledgment correctness | Distinguish pledge, receipt, acceptance, return and settlement; sponsor-approved templates; test noncash and corrected receipts |
| FIN-BC-017 | Grant eligibility and restrictions | Current charity evidence, sponsor policy, donor affirmations, prohibited-benefit checks, exception queue; test expired eligibility and restricted grants |
| FIN-BC-018 | Recurring and scheduled transactions revalidated at execution | Recheck funds, authority, eligibility and current rules; test changed circumstances and duplicate schedules |
| FIN-BC-019 | External payment destination integrity | Verify beneficiary and destination changes, risk-based approval, authenticated provider messages; test substitution and forged callbacks |
| FIN-BC-020 | Complete financial event populations | Reconcile accepted commands, ledger postings, outbox, provider events and reports; detect gaps, duplicates and missing evidence |
| FIN-BC-021 | Recoverability and controlled replay | Restore and replay synthetic histories, reconcile before release, preserve idempotency; test outages and schema upgrades |
| FIN-BC-022 | Configurable sponsor-specific accounting and compliance policies | Versioned effective dates, validated configuration, approval and regression tests; no tenant override of mandatory legal or platform safeguards |
| FIN-BC-023 | Independent exception management | Route unresolved discrepancies to accountable owner, prohibit silent write-offs and self-approved material exceptions; track age and disposition |
| FIN-BC-024 | Data retention and legal holds | Configurable record-class/jurisdiction/sponsor schedule, hold precedence, approved disposal and evidence; test hold conflicts |

## Regulatory question dispositions (proposed)
These are conservative design decisions, not final legal conclusions. Keep external confirmation dependencies in the question register.

- OQ-104, 208, 209: design privacy role and jurisdiction configuration; initial US-focused offering, no deliberate EU/UK targeting until applicability assessed. Do not equate nationality with GDPR territorial scope. Support data minimization, rights workflows, retention, subprocessors and sponsor-specific notices.
- OQ-105, 106, 204, 205, 206, 757: maintain counterparty/contract/role register for banks, custodians and processors; initial sponsor-owned accounts and sponsor-authorized flows, no assumed custody or direct bank service relationship. ACH roles and volume thresholds require confirmation; support configurable contractual obligations and incident windows. Do not claim bank acceptance.
- OQ-207: defer card processing until required; use hosted processor redirect or approved hosted integration, no cardholder data in Unbound. PCI scope and service-provider obligations remain open.
- OQ-210, 211: maintain jurisdiction and contract notification matrix, configurable deadlines and escalation, customer regulatory classification at onboarding; no universal legal deadline.
- OQ-212, 762: research IRS acknowledgments under 170(f)(8)/(18), Schedule D, noncash dispositions/Form 8282 and information-return triggers; implement sponsor-approved artifacts and configurable retention with legal holds. No universal retention period or tax advice.
- OQ-213: risk-based sanctions screening of relevant parties and transactions, sponsor accountability plus Unbound's own applicable obligations, rescreen at release, documented disposition and escalation. Screening is not a guarantee of legality; legal allocation remains open.
- OQ-115, 116, 758: draft SOC 1 objectives and IPE register from actual transaction flows; include completeness, accuracy, source, transformations, reconciliation and report version. Sponsor materiality and auditor reliance remain external.
- OQ-763: automated gates enforce approved rules but are not automatically independent human approval. Require elevated authorization for material exceptions and policy changes; no autonomous agent movement of money outside approved scope.

## Architecture and evidence
Use Summa ledger patterns only after capability verification; Strata owns controlled data interaction and transactions; Vigila operational events; Arca evidence integrity; Folio rendering. Durable queues accept requests only after persistence, with explicit ordering per relevant financial aggregate, bounded retries and reconciliation. Do not impose global ordering unless justified. External financial systems remain independently authoritative for their records. Evidence must distinguish internal correctness from external settlement and legal compliance.

## Follow-up
Reconcile control catalog, question register, policies, Unbound traceability, risk register, roadmap and economics. Research primary statutes, IRS guidance, OFAC, bank regulator guidance and applicable payment rules before upgrading regulatory conclusions. Record source dates and uncertainties; do not purchase consultations without authorization. No other repository changes authorized. Run required governance and register validation before merge.
