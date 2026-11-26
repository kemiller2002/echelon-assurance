---
id: RS-ASR-2026-REG1
title: Regulatory and framework applicability matrix (Echelon Foundry and Unbound)
status: draft
version: 0.1.0
owners: [assurance-governance]
created: 2026-11-25
updated: 2026-11-25
review_cycle: quarterly
---

> **Consolidation note (2026-11-25).** The funds-flow regulatory review, covering 12 flows with characterization-determining facts and prepared legal questions, is in `findings/financial-integrity-review.md`. Primary texts retrieved on 2026-11-25 include:
> - 31 CFR 1010.100(ff);
> - FIN-2014-R012;
> - 16 CFR 314.4(d)(2) and 314.6;
> - the 2023 Interagency Guidance;
> - 12 U.S.C. 1867.
>
> No legal conclusion is drawn. The basis of each baseline requirement is recorded in `requirements/baseline.csv` (`basis`): `conditional-legal` means a legal obligation if applicability is confirmed.


# Regulatory and framework applicability matrix

Batch B3, agent key `REG`. Machine-readable rows: `regulatory/fragments/REG.csv`
(`REQ-*`), sources `sources/fragments/REG.csv` (`SRC-REG-*`), counsel and partner
questions `registers/fragments/REG-open-questions.csv` (`OQ-200`–`OQ-217`).
Retrieval date for every source fetched: 2026-11-25.

**Nothing here is legal advice.** Classifications are research conclusions with
an evidence class and confidence. Anything that needs an attorney, CPA, QSA or
bank is routed to an open question. No requirement is claimed as satisfied.

## 1. Subjects of the analysis

| Subject | What it is (from read-only Echelon sources) | Why it matters |
|---|---|---|
| **Echelon Foundry** | An Indiana limited liability company (for-profit) building Echelon systems (SRC-REG-053). | Owns the assurance program; may hold employee or contractor data in future (one human today); would plausibly be the operator of a production Unbound. Its **for-profit** status means the FTC Act's nonprofit carve-out does **not** protect it. |
| **Unbound** | A reference DAF enterprise design: multi-sponsor, multi-tenant platform; synthetic data and simulated money only until gates pass; sponsors own funds and originate ACH while Unbound acts as a Nacha **Third-Party Service Provider** (CNV-PAY-001); card intake deferred from Release 1 (SRC-REG-052). | Production Unbound would process donor PII, payee bank details and payment instructions on behalf of DAF sponsors and possibly banks. |
| **DAF sponsors** | 501(c)(3) public charities that legally own DAF assets (26 U.S.C. 4966(d)(2)). | Bear most legal duties (IRS, state privacy, possibly GLBA); flow them to Unbound by contract. |
| **Banks** | ODFIs, disbursement/custody banks; Bank of America appears in Unbound only as an illustrative partner. | Bound by banking-agency guidance that flows down contractually. |

**Today (2026-11-25) Unbound processes no real personal or financial data** (PRINCIPLES
hard boundary). Every "legal" classification below therefore describes the
production enterprise, triggered by the first real donor record or money movement.

## 2. Classification vocabulary

`legal` (binds by statute/regulation once facts occur) · `contractual` (binds once
a contract or network-rule participation exists; listed before signature when
the term is effectively unavoidable) · `framework-selected` (here: **proposed**
for selection unless an owner decision record selects it, for example decision 0D02 (SOC design targets) and
decision 0D03 (GLBA design baseline)) · `customer-expectation` · `voluntary`
· `not-applicable` (with trigger) · `undetermined` (needs an open question).

## 3. The GLBA question (the contested point)

### 3.1 Three routes by which GLBA could reach Unbound

1. **Directly, because the operator is itself a "financial institution".**
   16 CFR 314.2(h) covers any institution "significantly engaged" in activities
   financial in nature or incidental to them (SRC-REG-002). Activities that are
   "closely related to banking" under 12 CFR 225.28 include data processing,
   storage and transmission of financial data ((b)(14)) and financial/investment
   advisory activities ((b)(6)) (SRC-REG-013); GLBA imports these via 12 U.S.C.
   1843(k) (link not read: general knowledge). A for-profit platform that
   processes donors' financial data, presents itself to donors under its own
   brand and administers investment allocations could be argued to be
   "significantly engaged". If so, 314.1(b) brings in customer information it
   holds about customers of *other* financial institutions too (SRC-REG-004,
   tool paraphrase — re-verify). → **OQ-202, P0.** Evidence: `open-question`
   (legal classification not assumed; owner-approved design: meet substantive
   safeguards without assuming legal classification, DF-ASR-2026-0D11).
2. **Contractually, because a sponsor is a covered financial institution.**
   314.4(f) requires covered institutions to select capable service providers,
   require safeguards by contract and periodically assess them (SRC-REG-001).
   That only bites if the sponsor is (a) a financial institution and (b) under
   FTC jurisdiction. Both are contested:
   - (a) A DAF contribution is an irrevocable charitable gift; the sponsor owns
     and controls the assets (SRC-REG-046). The donor receives advisory
     privileges, not an account or financial product in the ordinary sense.
     But the sponsor holds, invests and disburses assets on the donor's advice,
     which resembles custody/investment services. No FTC/CFPB guidance or case
     on DAF sponsors was found. → **OQ-200.**
   - (b) GLBA assigns the FTC enforcement over residual institutions "under the
     Federal Trade Commission Act" (15 U.S.C. 6805(a)(7), SRC-REG-007), and the
     FTC Act defines "corporation" as an entity organized to carry on business
     for its own profit or its members' (15 U.S.C. 44, SRC-REG-006). In *FTC v.
     Grand Canyon Education* (D. Ariz. Aug. 15, 2024, as reported, SRC-REG-009)
     the court dismissed FTC Act Section 5 claims against a nonprofit-chartered
     university because the FTC had not pleaded facts showing it was a
     "corporation" under 15 U.S.C. 44 (leave to amend granted); a March 2025
     order dismissed the amended claims without leave to amend, and the FTC
     later dismissed the whole case (August 2025). It is a pleading-stage
     district-court ruling with no appellate decision, and it concerned
     Section 5, not GLBA. **Contradicting interpretation:** the
     Safeguards Rule rests on GLBA rulemaking authority, and some commentators
     read 6805(a)(7) as reaching any financial institution no other agency
     supervises. The Department of Education requires (mostly nonprofit)
     colleges to comply with the Safeguards Rule **by contract** through Title IV
     Program Participation Agreements (SRC-REG-010) — evidence that nonprofits
     end up bound contractually even where FTC reach is doubtful. → **OQ-201.**
   - **Regulation P is different:** the CFPA's "person" has no for-profit
     limit (SRC-REG-012), so the nonprofit argument may not shield sponsors
     from privacy-notice duties if a DAF is a consumer financial product.
     → **OQ-203.**
3. **Contractually, because a bank flows it down.** Banks must contractually
   require service providers to meet the Interagency Information Security
   Guidelines and notify them of unauthorized access (12 CFR 30 App. B III.D and
   Supplement A, SRC-REG-056). This route is the most certain once Unbound
   handles a bank's customer information.

### 3.2 Conclusion

| Item | Echelon Foundry (today) | Unbound (production) | Evidence class | Confidence |
|---|---|---|---|---|
| GLBA Safeguards Rule 314.4(a)–(i) | undetermined (OQ-202); likely not-applicable today (no financial activity, no customer information) | **undetermined** legally; very likely **contractual** in practice via banks; recommended as **design baseline now** | open-question | low (legal) / medium (practical) |
| 314.4(f) service-provider oversight | n/a | **contractual** (mechanism of flow-down) | authoritative | medium |
| 314.4(j) FTC notice (≥500 consumers, ≤30 days) | n/a | undetermined; derived duty = fast contractual notice to the FI | open-question | medium |
| Privacy Rule / Reg P | n/a | sponsors: undetermined; Unbound: reuse/redisclosure limits as service provider | open-question | low |

**Why adopt it anyway:** the 314.4 elements are concrete, auditable and
overlap SOC 2 CC1–CC9 and bank questionnaires almost entirely; the only
expensive divergences are MFA for *any* individual (including donors,
314.4(c)(5)), the two-year disposal norm (314.4(c)(6), in tension with
append-only ledgers and IRS retention — resolvable through the "required by
law/legitimate business need" exception), and the board report (314.4(i)).
Do **not** design around the 5,000-consumer exemption (314.6): it is counted per
institution, and banks expect the exempted elements anyway. `recommendation`.

**Triggers that change the classification:** a recorded legal-classification determination for OQ-200–OQ-202 (internal research, or external advice obtained on a recorded trigger; DF-ASR-2026-0D11 approves only the internal design direction, not a classification);
choice of a donor-facing brand versus white-label; any lending, custody,
check-cashing, wire-transfer or tax-preparation service (FTC's own examples,
SRC-REG-005); a bank contract (makes it contractual immediately).

**Internal contradiction found:** many Unbound requirements cite
"GLBA safeguards" as their basis, labelled "unverified general knowledge"
(SRC-REG-052). This matrix does not confirm GLBA as a *legal* basis; those
citations should be read as "contractual/expected" until the legal classification in OQ-200–OQ-202 is determined (the safeguards design is owner-approved regardless of classification, DF-ASR-2026-0D11).
Recorded for `integration/proposed-changes.md` (coordinator).

## 4. Matrix

Columns: authority · who it binds · Echelon Foundry · Unbound · classification
(for Unbound unless stated) · evidence class / confidence · contradicting views ·
triggers. Requirement-level rows are in `REG.csv`.

### 4.1 Laws and regulations

| Framework (code) | What / authority | Binds | Echelon Foundry | Unbound (classification) | Evidence / conf. | Contradictions | Triggers |
|---|---|---|---|---|---|---|---|
| GLBA Safeguards (GLBA) | 16 CFR 314 (FTC), amended 2021, notice rule 2023/eff. 2024 | Non-bank financial institutions under FTC jurisdiction | undetermined (for-profit operator could itself be a financial institution; OQ-202) | undetermined → adopt as baseline | open-question / low | §3.1 | OQ-200–202; bank contract |
| GLBA Privacy (GLBA) | 12 CFR 1016 (CFPB); 16 CFR 313 (FTC, auto dealers) | Financial institutions re consumers' NPI | n/a | undetermined (sponsors); reuse limits as service provider | open-question / low | CFPA lacks nonprofit carve-out | OQ-203 |
| State breach notification (STATE) | 50 states + DC, GU, PR, VI (SRC-REG-042) | Anyone owning, licensing or maintaining residents' PI | **legal if Echelon holds residents' personal information** (for example future employees or contractors; none evidenced now) | **legal** (as maintainer: notify data owner; sponsors notify residents) | authoritative (law) / medium (Echelon facts) | Deadlines/triggers vary | First real PI |
| State "reasonable security" (STATE) | e.g., 201 CMR 17 (MA) (SRC-REG-058) | Owners/licensors of MA residents' PI | legal if Echelon holds MA residents' personal information (none evidenced now) | contractual via sponsors | authoritative / medium | Whether a processor "licenses" PI | MA residents in data |
| State comprehensive privacy (STATE) | ~20 state laws; CO, DE, NJ, OR (and per later commentary MD, MN) do not exempt 501(c)(3)s (SRC-REG-043); GLBA exemptions entity- vs data-level, MT narrowed 2025-10-01, CT 2025-10-01 or 2026-07-01 (sources disagree) (SRC-REG-044) | Controllers above thresholds; processors | likely below thresholds | undetermined (processor duties near-certain contractually) | auditor-interpretation / medium | Secondary sources disagree on lists | OQ-209; resident counts per sponsor |
| NYDFS Part 500 (STATE) | 23 NYCRR 500; final phase 2025-11-01 (SRC-REG-045) | DFS licensees | undetermined as possible operator (OQ-216, OQ-301) | undetermined (licensing or covered-entity status not determined, OQ-216, OQ-301; owner-approved design does not assume an NYDFS licence and reassesses on operating-model change and no receipt, custody or unilateral movement of customer funds initially, DF-ASR-2026-0D11); contractual if a DFS-licensed partner | auditor-interpretation / low | — | OQ-211, OQ-216 |
| GDPR (GDPR) | Reg. (EU) 2016/679 Art. 3(2) (SRC-REG-041) | Non-EU controllers/processors targeting or monitoring EU data subjects | not-applicable | not-applicable | authoritative / medium | Accessibility ≠ targeting (EDPB, not read) | OQ-208: EU/UK donors, marketing, tracking |
| IRS DAF rules (IRS) | 26 U.S.C. 4966, 4967, 170(f)(8), 170(f)(18), 6033(k); 26 CFR 1.6001-1 (SRC-REG-046–050) | Sponsors, fund managers, donors | n/a | **contractual** (duties and taxes fall on sponsors, fund managers and donors; platform must enable compliance and not cause violations) | authoritative (law) / medium (classification) | Platform personnel acting with sponsor authority could be argued to be "fund managers" (4966(d)(3)); 170(f)(8)/(18) are donor-deduction conditions, not sponsor duties (OQ-212) | Sponsor contract; Unbound becoming a sponsor itself would make it **legal** |
| IRS Pub 1075 / e-file (IRS) | FTI safeguards; e-file provider rules | Agencies receiving FTI; EROs/preparers | n/a | not-applicable | assumption / medium | — | Offering tax preparation (also makes operator a Safeguards FI) |
| OFAC (OFAC) | 31 CFR ch. V; 2019 Compliance Framework (SRC-REG-051) | All U.S. persons | **legal** | **legal** (no facilitation) + framework voluntary | authoritative / medium | Allocation sponsor vs platform | OQ-213; foreign grants |
| Bank incident notice (BANK) | 12 CFR 53.4 et al. (SRC-REG-017) | Bank service providers (BSCA) | n/a | undetermined | open-question / medium | — | OQ-205 direct bank contract |
| BSCA examination (BANK) | 12 U.S.C. 1867(c) (SRC-REG-018) | Services performed for depository institutions | n/a | undetermined | authoritative / medium | — | OQ-205 |

### 4.2 Supervisory guidance and network rules (flow down)

| Framework (code) | What / authority | Binds | Unbound | Class | Evidence / conf. | Notes / triggers |
|---|---|---|---|---|---|---|
| Interagency TPRM (TPRM) | 88 FR 37920, final 2023-06-06; life cycle: planning, due diligence, contract negotiation, ongoing monitoring, termination; governance alongside (SRC-REG-014/015) | Banks | Due-diligence and contract expectations | customer-expectation → contractual on signing | authoritative (text) / high | SOC reports named as a *may consider* input, not mandated. Payment and ledger services likely "critical activities". |
| Community bank TPRM guide (TPRM) | May 2024, voluntary (SRC-REG-016) | — | Questionnaire structure | customer-expectation | auditor-interpretation / medium | — |
| Interagency InfoSec Guidelines (BANK) | 12 CFR 30 App. B (SRC-REG-056) | Banks | Safeguards and incident notice by contract | contractual | authoritative / medium | Most certain GLBA-equivalent route |
| FFIEC IT Handbook (FFIEC) | Examiner handbook (SRC-REG-020, 403 — not verified) | Banks, examined TSPs | Questionnaire language | customer-expectation | customer-expectation / low | Examination only under BSCA |
| FFIEC CAT (FFIEC) | Sunset; removed 2025-08-31; CSF 2.0 and CISA CPGs cited (SRC-REG-019) | — | Do not build to CAT | not-applicable | authoritative / medium | Supports CSF 2.0 anchor |
| Nacha Operating Rules (NACHA) | ACH Security Framework (2013); Supplementing Data Security (≥2M entries, 2021/2022); Fraud Monitoring (2026-03-20 / 2026-06-22) (SRC-REG-021/022) | Originators, TPSPs, TPSs (and ODFIs/RDFIs, not relevant to Unbound) | Applies as TPSP once ACH is live | contractual (security framework, fraud monitoring); voluntary until 2M (account-number unreadability — adopt now) | authoritative / medium | Rule text is subscription-only; NOC timing taken from Unbound | OQ-206; becoming a Third-Party Sender |
| PCI DSS v4.0.1 (PCI) | PCI SSC; v4.0.1 June 2024; future-dated reqs 2025-03-31; SAQ A changed 2025-01-30 (SRC-REG-023/024) | Entities storing/processing/transmitting CHD or able to affect CDE | Cards deferred | not-applicable (R1) | authoritative / medium | 2026 FAQ revision reported, unverified | OQ-207: enabling cards; hosting the donation page |

### 4.3 Voluntary frameworks

| Framework (code) | Version / date | Echelon Foundry | Unbound | Class (proposed) | Rationale |
|---|---|---|---|---|---|
| NIST CSF 2.0 (CSF) | CSWP 29, 2024-02-26; 6 functions, 22 categories (SRC-REG-025) | framework-selected (proposed) | framework-selected (proposed) | recommendation / high | Outcome taxonomy; regulators point to it post-CAT; free; includes Govern and supply chain |
| NIST SP 800-53r5 (N53) | Release 5.2.0, 2025-08-27; OSCAL via CPRT (SRC-REG-026) | proposed vocabulary | proposed vocabulary | recommendation / medium | Mapping hub; public US-government material; voluntary for private sector |
| NIST SSDF (SSDF) | SP 800-218 v1.1 (2022-02-03); r1 draft v1.2 (2025-12-17) (SRC-REG-027/028) | proposed | proposed | recommendation / medium | Secure-SDLC evidence for SOC 2 CC8 and 314.4(c)(4) |
| SLSA (SLSA) | v1.2 (2025-11), build + new source track (SRC-REG-036) | Build L2 proposed; L3/source voluntary | same | recommendation / medium-low | Verifiable change evidence |
| OpenSSF baseline/Scorecard (OSSF) | OSPS Baseline 2025-02 (SRC-REG-037) | voluntary (public repos) | voluntary | recommendation / medium | Repos are currently public |
| OWASP ASVS (ASVS) | 5.0.0, 2025-05 (SRC-REG-033) | — | L2 proposed | recommendation / medium | Testable app requirements; pen-test scope |
| OWASP SAMM (SAMM) | v2 (SRC-REG-034) | voluntary | — | recommendation / medium | Maturity self-assessment only |
| OWASP API Top 10 (APISEC) | 2023 (SRC-REG-035) | — | proposed as checklist | recommendation / high | API-first, multi-tenant; BOLA = tenant breach |
| CIS Controls (CIS) | v8.1, 2024-06-24 (SRC-REG-031) | voluntary (IG1 lens) | voluntary (IG2 lens) | recommendation / medium | Licence limits commercial reuse (OQ-214) |
| CSA CCM / CAIQ (CCM) | v4.1, 2026-01-28 (SRC-REG-038) | — | customer-expectation | customer-expectation / low | STAR L1 as questionnaire substitute |
| ISO/IEC 27001 (ISO27001) | 2022 (+Amd 1:2024); paywalled (SRC-REG-039) | voluntary | voluntary; certification undetermined | assumption / low | US buyers usually ask for SOC |
| ISO/IEC 27701 (ISO27701) | 2025, standalone (SRC-REG-040) | — | voluntary | vendor-claim / low | No legal driver |

SOC 1 / SOC 2 (AICPA) are covered by batch B2 and are not classified here.

## 5. What this means for design (recommendations)

1. **Adopt the 16 CFR 314.4 elements as the minimum security-program baseline
   for production Unbound** regardless of how the legal classification in
   OQ-200–202 is determined (owner-approved design direction, DF-ASR-2026-0D11). Cost of
   adoption is low because SOC 2 and bank TPRM require nearly the same things;
   cost of later discovery that the rule applied directly is high. `recommendation`.
2. **Treat banks as the binding route** (12 CFR 30 App. B, TPRM, 12 CFR 53.4 if
   BSCA applies): design incident notification with a contractual fast path
   (hours, not days) and a 4-hour-disruption trigger. `recommendation`.
3. **Account-number unreadability and fraud monitoring now**, not at 2M ACH
   entries. `recommendation`.
4. **Keep card data out** by architecture (processor-hosted fields/redirect) and
   decide who serves the donation page before enabling cards. `recommendation`.
5. **Privacy is per-sponsor and per-state:** build a maintained, primary-source-cited
   applicability table rather than one national policy. `recommendation`.
6. **IRS duties are sponsor duties enforced through platform design**; the
   platform's outputs (acknowledgments, 6033(k) data) are SOC 1-relevant.
   `recommendation`.

## 6. Biggest uncertainties

- GLBA legal status of DAF sponsors and of a for-profit DAF platform (OQ-200–203;
  legal classification not assumed; internal design approved, DF-ASR-2026-0D11).
- Contracting structure with banks (direct BSCA provider vs sponsor's TPSP) (OQ-204–206;
  OQ-205/206 internal design approved, DF-ASR-2026-0D11; bank agreements pending).
- What sponsors and banks will actually demand (OQ-215).
- State privacy law lists change every session; secondary sources disagree.
- Several primary texts could not be read (eCFR blocked, FFIEC 403, Nacha and PCI
  standards subscription/registration, ISO paywalled); those rows are capped at
  `auditor-interpretation` or marked `assumption`.
