---
id: ASR-FIN-UNB-001
title: Unbound financial controls — analysis and control design
status: draft
version: 0.1.0
created: 2026-11-25
inputs: [financial/unbound-existing-assurance-inventory.md, daf-landscape docs/financial-core-control-spec-v0.5.md, unbound requirements/FULL-ACCOUNTING-SYSTEM.md, unbound requirements/DEFERRED-PAYMENTS-AND-REMAINING-GAPS.md, soc/unbound-soc-applicability.md]
---

# Unbound financial controls

Nothing here is implemented. Unbound is in requirements discovery and its
financial core is explicitly **NO-GO for production financial execution**
(daf-landscape `financial-core-control-spec-v0.5.md`). This document
defines what the controls must be, which ones carry SOC 1 weight, and where
invariants and independent reconciliation should replace sampling.

## 1. Unbound's financial responsibilities (as designed)

From the existing-assurance inventory §4 (`observed` in Unbound requirements):

- Unbound keeps the sponsor's **DAF program ledger** (contributions, grants
  payable, fees, fund balances, unitized investment positions) and exports
  balanced summary journals into the sponsor's entity books.
- The **sponsor** owns assets and holds grant authority; it originates
  payments. Unbound is a **third-party service provider** in the payment
  chain and does not hold funds or sign origination agreements.
- Custodians and banks are the independent sources for cash and
  positions.

Consequence: Unbound's outputs flow into material line items of the
sponsor's financial statements. That is the defining condition for SOC 1
relevance (`auditor-interpretation`; see `soc/unbound-soc-applicability.md`).
Whether a SOC 1 is *demanded* depends on the sponsor's auditor and on
whether sponsor and operator are separate entities (OQ-754, OQ-756).

## 2. The core argument: invariants plus independent reconciliation beat sampling

A conventional SOC 1 test of, say, grant authorization selects 25–60 grants
from the period and inspects approvals (`auditor-interpretation`: sample
sizes are firm methodology, not standard text). Sampling gives probabilistic
assurance about the *population as recorded*. It cannot show that the
recorded population is complete, and it detects systematic errors only if
they happen to land in the sample.

A stronger structure, available because Unbound is designed rather than
inherited:

| Layer | What it proves | Coverage |
|---|---|---|
| **Write-time invariants** (type- and transaction-enforced) | Every posting balances; every disbursement references an approved grant; no posting into a closed period; idempotent commands | 100% of transactions, by construction |
| **Continuous invariant checks** (separate process, read-only) | Invariants still hold over stored data (detects bypass, migration error, direct DB writes) | 100%, daily |
| **Independent-source reconciliation** | Ledger cash and positions equal bank/custodian records, obtained directly from the source | 100% of balances and movements, daily |
| **Two-way completeness** | Every external movement has a ledger entry; every ledger movement has external confirmation | 100% |
| **Auditor sampling** | Remains the auditor's choice; now tests the controls above (configuration, exception handling) rather than re-performing arithmetic | Reduced, not eliminated |

Why this is stronger: an invariant enforced at write time cannot be
violated without bypassing the system, and bypass is detected by the
independent checks and by reconciliation against data Unbound does not
control. Reconciliation to bank and custodian is the key independence
point: it anchors the ledger to records produced by a third party.

What it does not prove: that the *inputs* were legitimate (a fraudulent but
correctly authorized grant still balances), that the invariants are the
right ones, or that the authority matrix reflects what the sponsor's board
actually delegated. Those remain authorization, fraud and governance
controls with human accountability. (`recommendation`, confidence `medium`;
auditor acceptance of reduced sampling is OQ-related and unconfirmed.)

## 3. Control set

Domain `FIN` (plus `TEN`). IDs are assigned in `controls/control-catalog.csv`;
Unbound references use the `UNB:` prefix. SOC 1 column: whether the control
would plausibly support a SOC 1 control objective (`recommendation`; the
objectives themselves are management's to define and the auditor's to accept).

| Area | Control | Mechanism | Fail mode | SOC 1 | Unbound refs |
|---|---|---|---|---|---|
| Ledger integrity | Postings immutable; corrections only as reversing/adjusting entries | Append-only journal table, no UPDATE/DELETE grant to application role; hash chain over journal batches | fail-closed | Yes | UNB:UNB-SCTRL-025, UNB-ACC-* |
| Double-entry | Every journal balances per currency and legal entity | Type-level journal construction + DB constraint + daily recheck | fail-closed | Yes | ACC-MDL-002/003 |
| Transaction authorization | Financial commands carry server-resolved principal, authority and policy version; dual control above threshold; initiator ≠ approver | Pure authorization function over sponsor's versioned authority matrix | fail-closed | Yes | UNB:UNB-SCTRL-001, 002, 015 |
| Transaction completeness | Command inbox and outbox commit with the mutation; event sequence gap detection | Transactional outbox; sequence checks | fail-closed (write) / detective (gaps) | Yes | financial-core invariant 8 |
| Idempotency | Same key + same request hash ⇒ same result; same key + different hash ⇒ rejected | Command envelope (`idempotency_key`, `request_hash`) | fail-closed | Yes | financial-core command envelope |
| Unknown external outcomes | Provider timeout = Unknown; query and reconcile before retry | Attempt state machine | fail-closed | Yes | financial-core invariant 7 |
| Reconciliation | Daily ledger↔bank, ledger↔custodian, positions↔custodian; breaks aged and escalated; resolution by second person | Reconciliation job reading source files/APIs directly; break register | detective | Yes (key control) | UNB:UNB-SCTRL-024, UNB-ACC-018 |
| Cash and asset movement | Settlement atomically applies cash, posting and reservation; no spendable cash from reservation consumption | Settlement conservation invariant | fail-closed | Yes | financial-core invariants 4–6 |
| Grant disbursement | Disbursement only for an approved grant to an eligible charity; amount ≤ approved; payee bank change verified out-of-band | Domain rule + eligibility check + payee-change workflow | fail-closed | Yes | UNB:UNB-SCTRL-015–017, 026, 029, 030 |
| Contribution processing | Contributions recorded on confirmed receipt; valuation from defined source; noncash gifts follow appraisal rule; acknowledgements generated from ledger | Workflow + valuation source evidence | fail-closed for posting; detective for valuation | Yes | UNB:UNB-SCTRL-031 |
| Fee calculation | Fees computed from versioned fee schedules; independent recalculation daily | Pure fee function + recalculation job | detective | Yes | UNB:UNB-SCTRL-032 |
| Investment accounting | Unit/NAV from custodian prices; stale or missing price raises exception; unit reconciliation to custodian | Price completeness check + reconciliation | detective | Yes | UNB:UNB-SCTRL-021, 022 |
| Financial reporting | Summary journals exported to sponsor are reproducible from ledger at a point in time; export hash recorded; sponsor-side receipt reconciled | Deterministic report function; export manifest | detective | Yes | FULL-ACCOUNTING-SYSTEM export design |
| Period close | Close is an event; closed period blocks every write path; close review signed by someone other than preparer | Period-lock enforced in the posting boundary, not per-function | fail-closed | Yes | UNB:UNB-SCTRL-027, 028 |
| Donor restrictions | Fund purpose/restriction metadata constrains eligible grants; restriction changes require sponsor approval | Domain rule | fail-closed | Partly (net asset classification) | UNB grants requirements |
| Charity eligibility | Payee eligibility from authoritative sources (e.g. IRS data) with freshness check before each disbursement; OFAC screening | Eligibility service + screening | fail-closed | Indirect (compliance more than ICFR) | UNB:UNB-SCTRL-010–014 |
| Fraud detection | Anomaly rules on grants, contributions, payee changes, access; human triage | Detection rules + case management | detective | Supports | — |
| Manual adjustments | Reason code, evidence, preparer ≠ approver, reported to sponsor's reviewer; volume monitored | Adjustment workflow | fail-closed (approval) | Yes | — |
| Historical corrections | No rewriting history; corrections reference originals; restatements of closed periods require sponsor approval | Append-only ledger | fail-closed | Yes | financial-core invariant 3 |
| Tenant-specific policies | Fee schedules, thresholds, grant rules are versioned, effective-dated, sponsor-approved configuration | Configuration governance (POL-13.02) | fail-closed | Yes (configuration is an application control input) | UNB-TEN-* |
| Financial institution integrations | Positive pay, NOC/return handling, destination proof, signed/encrypted file exchange, bank file completeness counts | Integration controls | fail-closed (submission) / detective (returns) | Yes | UNB CNV-PAY-001/002 |

## 4. IT general controls that a SOC 1 would also need

SOC 1 opinions on application controls depend on ITGCs over the systems
running them (`auditor-interpretation`): access (IAM), change management
(SDL), and operations (job scheduling, backup, incident). These are the same
controls as the SOC 2 Security criteria. **One ITGC implementation serves
both reports**; this is the main economy of the unified catalog.

## 5. What remains manual by design

Board/sponsor delegation of authority; approval of fee schedules; review
of aged reconciliation breaks; approval of manual adjustments and
restatements; fraud case decisions; CUEC acceptance by sponsors. These are
judgment and accountability controls (automation ceiling L2–L4 enforcement
of the approval step, never automated decision).

## 6. Complementary user entity controls (draft themes)

Unbound already references 22 CUEC identifiers. Themes a sponsor must
operate for Unbound's controls to achieve their objectives:
sponsor maintains its authority matrix and reviews changes; sponsor
reviews monthly summary journals and reconciles to its ERP; sponsor
approves fee schedules and restriction changes; sponsor reviews manual
adjustment reports; sponsor manages its bank relationship and positive pay
decisions; sponsor manages its own user access to Unbound.

## 7. Open questions (see register)

OQ-753 customers; OQ-754 sponsor auditor expectation; OQ-756 entity
separation; OQ-757 bank TPSP expectations; plus: whether the sponsor's
auditor accepts invariant + reconciliation evidence for reduced sampling,
and which control objectives the sponsor's auditor considers relevant.
