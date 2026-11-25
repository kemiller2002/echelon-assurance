---
id: RS-ASR-2026-SOC2
title: SOC applicability to Unbound and Echelon Foundry
status: draft
version: 0.1.0
owners: [assurance-governance]
created: 2026-11-25
updated: 2026-11-25
research_key: SOC
confidence: medium
---

# SOC applicability to Unbound and Echelon Foundry

This document builds on `soc/soc-reporting-landscape.md`. Evidence classes
appear in brackets. Source IDs refer to `sources/fragments/SOC.csv`; OQ IDs
refer to `registers/fragments/SOC-open-questions.csv`.

**Nothing here is a CPA's scoping decision.** The scope of any examination is
agreed between management and a licensed service auditor, and user auditors
decide independently whether to rely on it.

## 1. Facts about Unbound that drive the analysis

All facts in this section are read from Unbound's requirements (read-only,
SRC-SOC-028/029).

- **Status.** Unbound is a **reference enterprise** that uses synthetic data
  and simulated money movement. It has **no production operations** and
  provides no real sponsor representation, custody, bank transfers or grant
  payments (PRINCIPLES.md hard boundaries).
- **System of record.** Unbound intends to keep the **sponsor's DAF program
  ledger** as the financial system of record. That ledger includes:
  - an integrated general ledger and subledgers for DAF balances, cash and
    banks, custody and securities, grants payable, contributions, investments
    and fees (UNB-ACC-004);
  - reconciliation to bank, custodian and payment-rail statements
    (UNB-ACC-018);
  - accounting segregation of duties (UNB-ACC-022).
- **SOC 1 tagging already in progress.** Many accounting clauses are already
  tagged "service-organization report relevance SOC1" and carry named CUECs
  (FULL-ACCOUNTING-SYSTEM.md, CNV-FIN-001..013).
- **Customer types.** Customers would be DAF **sponsoring organizations**,
  which are public charities and legally own the assets. They may also include
  **institutional partners or financial institutions** in embedded or
  white-label programs (PARTNER-SEGREGATION-AND-TENANCY.md). The operator has
  instruction-only authority and never takes title to money (UNB-XIN-REG-001).
- **Owner-approved Unbound decisions (2026-11-24) already in place:**
  - **SPD-PLT-001:** no report is legally required. Evidence engineering
    continues from inception. SOC description and period work is conditional
    on contract demand or a second user entity. Before go-live, obtain the
    sponsor auditor's written planned approach.
  - **XID-INV-008:** there is no universal rule. Offer right-to-audit and
    direct-testing support. Obtain a SOC 1 Type 2 once any user auditor plans
    to rely on Unbound's controls or two or more user entities exist. Skip a
    stand-alone Type 1 unless a contract asks for it.
  - **SPD-ADV-003:** a SOC 1 Type 2 is warranted when reliance by more than
    one user auditor makes it cheaper than repeated direct testing.
  - **RCD-031:** design for SOC 1 Type II (fund-accounting controls) and
    SOC 2 Type II (platform security). This is readiness only.

These decisions are Unbound's. This analysis tests them against primary
sources; it does not override them.

## 2. Would Unbound's services be relevant to user entities' ICFR?

**Yes, strongly, once Unbound operates for real sponsors**
`[authoritative]` reasoning applied to `[assumption]` facts.

**The test (AU-C 402.03, SRC-SOC-005).** A service is relevant to a user
entity's audit when it affects how significant classes of transactions are:

- initiated, recorded, processed and corrected;
- posted to the general ledger;
- reflected in the accounting records and the financial reporting process;
- supported by the related IT environment.

**How Unbound meets it.** Unbound, as the sponsor's program ledger, would
initiate, record and post the sponsor's most significant transaction classes:

| Sponsor transaction class | Unbound function | Likely financial statement assertions |
|---|---|---|
| Contributions (cash, securities, noncash) | records gift completion and acceptance, reversals, acknowledgments (CNV-FIN-003) | occurrence, completeness, cut-off, valuation of noncash gifts |
| Grants | recommendation intake, eligibility, approval workflow, grants payable, disbursement instructions | occurrence, authorization, completeness, cut-off of grants payable |
| Fees | administrative and investment fee calculation and posting | accuracy, completeness |
| Investments | custodian feed ingestion, position and income recording, allocation to funds (CNV-FIN-013) | existence (with custodian), valuation, allocation |
| Fund balances (net assets with donor restrictions or DAF liabilities, per sponsor policy) | subledger per fund, statements to donors | completeness, accuracy, presentation |
| Reconciliations | ledger to bank, custodian and rail (UNB-ACC-018) | existence, completeness |

**The exclusion does not apply.** AU-C 402.05 excludes services that only
process transactions the entity itself specifically authorizes, such as a
bank processing checking-account transactions. Unbound does more than that:
it maintains the books and computes fees and allocations. So it is a
**service organization** for the sponsor's auditor `[authoritative]`,
applying 402.03 and 402.05.

**Who is audited.** DAF sponsors are public charities that typically have
audited financial statements, because of state charitable solicitation
thresholds, lenders or boards `[assumption]`. Which sponsor audits exist, and
under which standards, is OQ-115.

**Conclusion.** SOC 1 demand would arise from **sponsors' external
auditors** wherever they plan to rely on Unbound's controls. Otherwise those
auditors must test Unbound directly (AU-C 402.12c/.16b) or perform more
substantive work `[authoritative]` for the mechanism, `customer-expectation`
for the demand. This matches Unbound's SPD-ADV-003.

**Financial institutions** add a second source of demand:

- The 2023 TPRM guidance names SOC reports as something banks "may consider"
  in due diligence and contract for (SRC-SOC-020) `[authoritative]` for the
  text.
- Bank TPRM teams commonly ask for **both** SOC 1, where the vendor touches
  financial records, and SOC 2 `[customer-expectation]`. This is not verified
  against any specific bank (OQ-105).
- The 2026 proposed replacement guidance is more tailored and risk-based
  (SRC-SOC-021) `[authoritative]` for the proposal. It may soften the
  expectation but does not remove it.

## 3. Is SOC 2 alone sufficient?

**Probably not, for sponsors whose auditors rely on Unbound's controls**
`[authoritative]` (AU-C 9402 says "unlikely", not "never"; the user
auditor's judgment decides).

- **Primary-source basis.** AU-C 9402 (issued December 2022, revised March
  2024) calls SOC 1 "the preferred report". It says a SOC 2 "is unlikely to
  achieve the intent" of AU-C 402, because:
  - SOC 2 tests address security and availability, not completeness and
    accuracy of transaction processing;
  - SOC 2 descriptions may omit ICFR-relevant reports;
  - SOC 2 CUECs differ from SOC 1 CUECs.
- **What SOC 2 is still for.** SOC 2 remains the expected report for
  **security due diligence** by sponsors, partners and banks
  `[customer-expectation]`. It is not legally required `[authoritative]`:
  no statute or rule found in this batch mandates it.
- **Can PI1 in SOC 2 replace SOC 1?** No. Adding Processing Integrity to
  SOC 2 does not convert it into a SOC 1. PI1 tests achievement of the
  entity's *processing commitments*, not user-entity control objectives over
  financial statement assertions. The AU-C 9402 reasoning still applies
  `[auditor-interpretation]`.
- **Recommendation.** Plan a **SOC 1 Type 2** for the fund-accounting service
  and a **SOC 2 Type 2** for platform security. Both should share one control
  set and one evidence base, and preferably one service auditor and one
  period `[recommendation]`. Whether one firm can efficiently perform both,
  and at what cost, is OQ-111.

## 4. Which TSC categories should be in scope?

| Category | Recommendation | Reasoning | Evidence class |
|---|---|---|---|
| **Security (CC1–CC9)** | In scope, always | It is mandatory in every SOC 2, and the Unbound directive already mandates it | `auditor-interpretation` (TSC not read; OQ-100) |
| **Availability (A1)** | **Include** | Grant timing, donor access and payment-instruction windows are service commitments. Unbound already defines RPO/RTO, SLO and recovery semantics (CNV-ARC-001). Banks and partners will ask about continuity | `recommendation` |
| **Confidentiality (C1)** | **Include** | Sponsor financial data, partner-segregated data, charity diligence documents and contractual confidentiality commitments to sponsors and banks | `recommendation` |
| **Processing Integrity (PI1)** | **Include only if** SOC 1 is not obtained, or if partners demand it; otherwise **defer** | It overlaps with SOC 1 control objectives. It adds value for non-financial outputs, such as donor statements and tax acknowledgments, that may matter more to donors and partners than to auditors. Double-testing cost is an issue | `recommendation`, `open-question` (OQ-103) |
| **Privacy (P1–P8)** | **Undetermined; lean exclude for the first report** | The privacy criteria fit when the entity itself makes privacy commitments to data subjects. When Unbound acts for the sponsor, the sponsor is the party making notice and consent commitments, so many P criteria become CUECs or "not relevant". In white-label programmes Unbound may collect personal information directly, which changes this | `open-question` (OQ-104) |

**Echelon Foundry's own platform.** If Echelon Foundry is the legal operator,
its corporate controls (HR, governance, vendor management) fall inside the
Security common criteria for the Unbound system. Which legal entity is the
"service organization" is unresolved (OQ-102).

## 5. Type 1 first, or straight to Type 2?

**What each type gives the user auditor `[authoritative]`:**

- A **Type 1** gives the user auditor **no evidence of operating
  effectiveness** (AU-C 402.16). It supports only the auditor's understanding
  of design and implementation (402.14).
- For SOC 1 reliance, the Type 2 is what reduces sponsor audit effort.

**SOC 1 Type 1 first?** Generally not worth it, consistent with XID-INV-008.

- An exception is a contract that requires a Type 1 at launch.
- Another exception is a first user auditor who explicitly says a Type 1
  plus their own tests is their planned approach `[recommendation]`.

**SOC 2 Type 1 first?** Possibly worth it, as a sales accelerator.

- Prospective sponsors and banks often accept a Type 1 while a Type 2 period
  runs `[customer-expectation]`, though no specific counterparty was
  verified.
- Costs:
  - a second engagement fee;
  - a description and assertion to prepare twice;
  - the risk of marketing it as "certified".
- Whether to do it is a commercial owner decision (OQ-112 adjacent)
  `[recommendation]`.

**Hard precondition: no report until production exists `[authoritative]`.**

- A Type 1 or Type 2 needs a system "designed and implemented" (AT-C
  320.07/.08), management's assertion, and appropriate subject matter (AT-C
  105.29).
- A reference environment using synthetic data cannot be the subject of a
  report on a production service.
- The earliest meaningful milestone is a **readiness assessment**. A CPA firm
  can do this as a consulting service; independence matters if the same firm
  later examines Unbound.

**Type 2 period.** No minimum exists `[authoritative]`.

- A first 3–6 month period aligned to end at or near the first sponsor's
  fiscal year-end is common practice `[industry-convention]`.
- After that, a contiguous 12-month period gives the best overlap for user
  auditors (AU-C 402.A34) `[recommendation]`.
- The acceptable length to the first actual user auditor is OQ-110.

## 6. Carve-out or inclusive for subservice organizations

| Third party | Likely status | Recommended method | Reasoning |
|---|---|---|---|
| **Cloud provider (IaaS/PaaS, e.g. managed PostgreSQL)** | Subservice organization for both SOC 1 (IT general controls (ITGC) supporting ledger processing) and SOC 2 | **Carve-out** | Hyperscalers do not provide assertions or representations for inclusive treatment `[industry-convention]`. Monitor their SOC reports and map their CSOCs `[recommendation]` |
| **Custodians (securities custody, investment feeds)** | Likely a **subservice organization** for SOC 1 if Unbound's investment and fund-allocation objectives depend on custodian controls (existence, valuation feeds). Alternatively the **sponsor's own service organization** if the sponsor contracts the custodian directly | **Carve-out** with CSOCs. Or treat the custodian as the sponsor's separate service organization, with a CUEC that the sponsor obtains the custodian's SOC 1 | Depends on who contracts the custodian. Unbound's FundsFlow puts accounts in the sponsor's name. Requires CPA judgment (OQ-106) |
| **Banks (deposit accounts, ACH/wire origination)** | **Possibly not a subservice organization at all.** AU-C 402.05 excludes bank processing of transactions the entity specifically authorizes. Where Unbound only instructs the sponsor's bank on authorized grants, the bank processes authorized transactions | Describe the interaction. Treat bank statements as independent reconciliation sources. Name a CSOC only if a control objective depends on a bank control | `authoritative` for the 402.05 principle, `auditor-interpretation` for its application (OQ-106) |
| **Payment processors (card/ACH contributions via the sponsor's processor)** | Possibly a subservice organization, depending on whether Unbound relies on the processor's data completeness | **Carve-out** | `recommendation` (OQ-106) |
| **AI model providers** | Subservice organization if model outputs feed in-scope controls; otherwise a vendor under CC9.2 | Carve-out, plus compensating controls at Unbound: deterministic validation and human approval | `recommendation`. The 2027 TSC AI points of focus may raise scrutiny (SRC-SOC-009; OQ-113) |
| **Echelon Foundry shared systems (Folio, Praxis and others), if a separate legal entity from the operator** | Affiliated subservice organization | **Inclusive** is feasible for an affiliate, because management can sign. Otherwise carve-out | `recommendation` (OQ-102) |

## 7. Recommendation

`[recommendation]`, confidence **medium**.

1. **Keep Unbound's existing owner decisions.** They are consistent with the
   primary sources:
   - SPD-PLT-001 (no legal requirement; evidence engineering from day one);
   - XID-INV-008 (SOC 1 Type 2 when reliance or two or more user entities;
     no stand-alone Type 1 by default).
2. **Plan two reports on one control and evidence base:**
   - **SOC 1 Type 2** on the DAF fund-accounting and grant/fee processing
     service. Control objectives cover contributions, grants, fees,
     investments, fund balances and reporting, plus ITGC (access, change,
     operations).
   - **SOC 2 Type 2** covering **Security + Availability + Confidentiality**.
   - Processing Integrity and Privacy remain undetermined (OQ-103, OQ-104).
3. **Defer the SOC 2 criteria mapping.** Do not finalise it before the 2027
   TSC are issued (comment deadline 15 January 2027). Map controls to both
   structures in the meantime.
4. **Use carve-out for cloud and custodians.** Treat banks as reconciliation
   sources unless a CPA concludes otherwise. Build CSOC and CUEC registers now.
   Unbound already names CUECs.
5. **Engineer evidence for IPE testing by an independent auditor:**
   - complete populations;
   - reproducible queries;
   - integrity hashes;
   - access logs showing auditor-extracted data.
   
   Do not market platform-generated dashboards as assurance.
6. **Choose the service auditor carefully:**
   - a licensed CPA firm;
   - with peer-review results reviewed;
   - independent of any readiness vendor or platform that designed the
     controls;
   - not a "quick-turn" provider.

   This answers the 2026 AICPA and peer-review warnings (SRC-SOC-008, -013,
   -014).

## 8. Uncertainties

| # | Uncertainty | Why it matters | OQ |
|---|---|---|---|
| 1 | Exact TSC and DC text not read (login gate) | Summaries in `REQ-TSC-*` may be imprecise | OQ-100 |
| 2 | First sponsor auditor's planned approach | Determines whether a SOC 1 is needed at all at launch | OQ-101 |
| 3 | Legal entity that is the service organization; system boundary | Determines whose controls are in scope, and whether EF systems are inclusive | OQ-102 |
| 4 | PI and Privacy in or out | Cost and description complexity | OQ-103, OQ-104 |
| 5 | Bank and financial-institution partner demands | Could force earlier or both reports | OQ-105 |
| 6 | Status of custodians, banks and processors as subservice organizations | Description, CSOCs and user-auditor work | OQ-106 |
| 7 | Final 2027 TSC content and effective date | Criteria mapping | OQ-107 |
| 8 | Auditor acceptance of automated/Folio evidence and IPE basis | Core to the Echelon thesis of continuous evidence | OQ-108 |
| 9 | Whether agent-performed monitoring counts as an internal audit function | Audit fee and the reasonable basis for the assertion | OQ-109 |
| 10 | Acceptable first period for user auditors | Timing | OQ-110 |
| 11 | Combined SOC 1 + SOC 2 cost and feasibility | Budget | OQ-111 |
| 12 | Sponsors' audit regimes | Whether SOC 1 demand materialises | OQ-115 |

**Contradicting view.** One could argue that early sponsors are small and
their auditors will take a substantive-only approach, so SOC 1 demand never
materialises. AU-C 402.15 allows substantive evidence from records at the
user entity. But because Unbound **is** the record-keeper, those records live
at Unbound. That argument therefore pushes the auditor towards direct
procedures at Unbound, not away from Unbound. The XID-INV-008 position
(direct testing until reliance or scale justifies a report) captures this
correctly `[auditor-interpretation]`.

## Owner decision (2026-11-25)

Owner decision `DF-ASR-2026-0D02` sets the **design target** at SOC 1 Type II
and SOC 2 Type II covering **all five** trust services categories (Security,
Availability, Confidentiality, Processing Integrity, Privacy) on shared
controls. Type I or SOC 3 only with demonstrated commercial benefit. The
analysis above, which leaned toward deferring Processing Integrity and
Privacy, is retained as the reasoning record. Actual examination scope
remains subject to customer and auditor confirmation (OQ-101, OQ-103,
OQ-104 stay open).
