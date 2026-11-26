---
id: JR-ASR-2026-5C01
title: Review passes 2 and 3 - source accuracy and regulatory applicability (2026-11-25)
status: complete
created: 2026-11-25
provenance:
  contributions:
    EXE-20261125T233256557Z-3f771e74:
      operations: [origin-unrecorded]
      at: 2026-11-25T23:33:03.651Z
      actor:
        kind: agent
        id: claude-code
        provider: anthropic
        model: unknown
        runtime: claude-code
      reason: "Created by an earlier agent session on branch claude/fervent-tesla-ytnxz6 before Praxis was installed in this repository; origin is recorded in git history"
---

# Journal: review passes 2 (source accuracy) and 3 (regulatory applicability)

Independent reviewer pass on branch `claude/fervent-tesla-ytnxz6`. I checked
each claim against the best source I could reach on 2026-11-25 (web search and
page fetch). eCFR redirected to a bot wall, so CFR text came from the Cornell
LII mirror. Nothing here is legal advice. Registers were edited with
row-targeted substring replacement (register and matching fragment kept
identical), then `python3 -I tools/validate_registers.py` printed
`OK: all registers valid` and `tools/merge_fragments.py --check` reported no
drift.

Verdicts: **confirmed**, **corrected** (the repo text was wrong or overstated
and has been edited), **unverifiable** (softened or annotated in place).

## Claims checked

| # | Claim | Verdict | Source used | Where in repo | Edits |
|---|---|---|---|---|---|
| 1 | AU-C 9402: SOC 1 is "the preferred report"; SOC 2 "unlikely to achieve the intent" of AU-C 402 | Confirmed (substance). The "revised March 2024" date is unverifiable here because the AICPA PDF is login-gated; the AICPA page shows a posting date of 2022-12-09 | https://www.aicpa-cima.com/resources/download/interpretation-no-1-of-au-c-section-402 (title/scope only); secondary corroboration via search (prepi.to, cpahalltalk) | `soc/soc-reporting-landscape.md` §3/§4; `soc/unbound-soc-applicability.md` §3; REQ-SOC1-023; REQ-TSC-031/039/040; SRC-SOC-005 | **Corrected (overstatement):** `unbound-soc-applicability.md` §3 answer changed from "No" to "Probably not", noting that 9402 says "unlikely" and the user auditor decides |
| 2a | 16 CFR 314.4(j): FTC notice as soon as possible, no later than 30 days after discovery, for notification events involving ≥500 consumers | Confirmed | https://www.law.cornell.edu/cfr/text/16/314.4 | REQ-GLBA-018; matrix §3.2; POL-08; CTL-IR-002 | None |
| 2b | 314.6 exempts institutions with fewer than 5,000 consumers from 314.4(b)(1), (d)(2), (h), (i) | Confirmed. (j) is **not** exempted, and the repo does not say it is | https://www.law.cornell.edu/cfr/text/16/314.6 | REQ-GLBA-019; REQ-PENTEST-012; matrix §3.2; pentest research | None |
| 2c | 314.4(d)(2): continuous monitoring, or else an annual pen test plus vulnerability assessments every six months and after material changes | Confirmed | https://www.law.cornell.edu/cfr/text/16/314.4 | REQ-PENTEST-011; pentest research §1 | None |
| 3 | 12 CFR 53.4: a bank service provider notifies at least one bank-designated contact at each affected banking-organization customer "as soon as possible" after an incident that materially disrupts, or is reasonably likely to disrupt, covered services for four or more hours; pre-communicated scheduled maintenance is excepted | Confirmed. Repo wording in REQ-BANK-003 matches. Note the notice goes to the **bank customer's designated contact** (or its CEO and CIO), not to bank end-customers | https://www.law.cornell.edu/cfr/text/12/53.4 | REQ-BANK-003; matrix §4.1 and §5.2; SRC-REG-017; OQ-205 | None |
| 4 | *FTC v. Grand Canyon Education* (D. Ariz. 2024) on nonprofit jurisdiction | **Corrected.** The Aug 2024 order granted GCU's motion to dismiss **with leave to amend** because the FTC had not pleaded facts showing GCU was a "corporation" (organized for its own or its members' profit) under 15 U.S.C. 44. The Tushnet blog headline ("even sham ones") overstates that holding. The Mar 2025 order dismissed the amended claims without leave to amend. On 2025-08-15 the FTC dismissed the case with prejudice. There is no appellate ruling, and the case concerned Section 5, not GLBA | https://blog.galalaw.com/post/102k27o/ftc-forges-ahead-in-court-battle-on-ftc-acts-scope-over-nonprofit-institutions ; https://www.ftc.gov/legal-library/browse/cases-proceedings/grand-canyon-universitygrand-canyon-education | matrix §3.1(b); SRC-REG-009; OQ-201; REQ-GLBA-001 | Matrix text rewritten; SRC-REG-009 limitations now record the subsequent history; OQ-201 qualified ("pleading stage … no appellate ruling") |
| 5 | Nacha fraud monitoring Phase 2 effective 2026-06-22 and applies to TPSPs | Confirmed. The rule date is 2026-06-19; because that is a federal holiday, the practical effective date is 2026-06-22. Phase 2 covers non-consumer Originators, TPSPs and TPSs below the Phase 1 threshold, and the remaining RDFIs | https://www.nacha.org/rules/risk-management-topics-fraud-monitoring-phase-2 | REQ-NACHA-003; matrix §4.2; SRC-REG-022 | SRC-REG-022 limitations updated (Phase 2 page now read). Matrix "Binds" cell changed from "(not FIs)" to "(and ODFIs/RDFIs, not relevant to Unbound)", because the rule also binds FIs |
| 6 | NIST CAISI (March 2026): every frontier model tested was hijacked | Confirmed as worded: "at least one successful attack was found against all of the target frontier models" (13 models, 250,000+ attempts, 400+ participants), published 2026-03-23. Success rates varied widely between models | https://www.nist.gov/blogs/caisi-research-blog/insights-ai-agent-security-large-scale-red-teaming-competition | `ai-governance/ai-governance-model.md` principle 1 and §injection; RSK-001; OQ-616; SRC-AIG-018 | **Corrected (overstatement):** OQ-616 "shows no model is robust" now states the actual finding with the variance caveat. In the AI governance doc, "static robustness is insufficient per CAISI" is now marked as our recommendation drawn from CAISI |
| 7a | AICPA TQA 9561 (Sept 2026) treats AI providers under the subservice-organization model | Confirmed via secondary sources: issued 2026-09-11. CPA-firm summaries say AI providers performing functions in the system get the existing subservice test and no new examination model is created. TQA text not read (login) | https://www.aicpa-cima.com/resources/download/tqa-section-9561-soc-examinations-effect-of-the-service-organizations-use-of-ai-on-soc1-and-soc2-examinations (title); Schneider Downs (SRC-AIG-022); Forvis Mazars PDF | `ai-governance-model.md`; `validation/…`; findings; OQ-604/605; SRC-AIG-022 | None. The repo already marks it as "per secondary summary" |
| 7b | Proposed 2027 TSC: news release 2026-10-28; comments due 2027-01-15; proposed effective for engagements beginning on or after 2027-12-31, early adoption permitted | Confirmed | https://www.aicpa-cima.com/news/article/aicpa-proposes-update-of-soc-2-r-trust-services-criteria-to-enhance-the-relevance-usability-and-value-of-cpas-reports ; Journal of Accountancy Oct 2026 | REQ-TSC-052; soc landscape §4.2; SRC-SOC-009; SRC-AIG-023 | None |
| 7c | Revised description criteria exposure draft 2026-11-17: keeps nine areas, materiality, AI/ML guidance, comments due 2027-01-15 | Confirmed (news release dated 2026-11-17). SRC-SOC-011 (download page) is dated 2026-11-16; this one-day gap is left as recorded | https://www.aicpa-cima.com/news/article/aicpa-seeks-feedback-on-proposed-updates-to-soc-2-r-description-criteria | REQ-SOCDC-010; soc landscape §4.2–4.3; SRC-SOC-010 | None |
| 8a | AWS QLDB end of support 2025-07-31 | Confirmed by several independent secondary sources. The cited AWS blog page did not show the date when fetched | Search results (InfoQ 2024-07 "aws-kill-qldb", Classmethod, Codenotary) | `policy-as-code/tool-evaluation.md`; `architecture/evidence-architecture.md`; SRC-PAC-034 | SRC-PAC-034 limitations annotated |
| 8b | immudb licensed under BSL | Confirmed with nuance. The LICENSE file applies BSL 1.1 to immudb 1.9DOM.2 and later, with an Additional Use Grant that permits production use except as a competing hosted or embedded offering. Earlier versions were Apache-2.0 | https://raw.githubusercontent.com/codenotary/immudb/master/LICENSE | tool-evaluation; evidence-architecture; SRC-PAC-035 | tool-evaluation row and SRC-PAC-035 now state the version scope and the use grant. The "defer immudb" recommendation stands: it rests on licence and vendor risk, not on a production-use ban |
| 9 | FedRAMP NTC-0009 (2026-03-25): machine-readable packages | Confirmed. Comprehensive machine-readable data is required only for Rev5 Class D; human-readable materials are generated from machine-readable ones; OSCAL is not named as mandatory. One third-party summary (Knox Systems) claims all Rev5 providers are covered. That conflicts with the notice, and the notice governs | https://www.fedramp.gov/notices/0009/ | tool-evaluation; policy-as-code spec; SRC-PAC-020 | None |
| 10 | HIPAA Security Rule NPRM (90 FR 898): pen test every 12 months, vulnerability scan every 6 months; final action projected July 2027 | Confirmed. In the 2026 Unified Agenda the rule moved to long-term actions with a July 2027 target. That date is an agency estimate, not a deadline | Search: Holland & Knight 2026-07, Fierce Healthcare, DWT; NPRM at federalregister.gov (not fetched) | REQ-PENTEST-018; pentest research §1; SRC-PTV-012/013 | None |
| 11 | Delve allegations (TechCrunch 2026-03-21) are worded as allegations and the denial is included | Confirmed. All three mentions say "alleged" or "unproven" and record Delve's denial. The article confirms these points: anonymous "DeepDelver" Substack; "virtually all" clients through two firms alleged to be "part of the same operation"; Delve called the claims "misleading" and said only independent licensed auditors issue reports | https://techcrunch.com/2026/03/21/delve-accused-of-misleading-customers-with-fake-compliance/ | `vendors/compliance-platform-analysis.md` (table row, §3, §4, §6 table); SRC-PTV-042 | Table cell "Alleged rubber-stamp affiliated auditors" changed to "Alleged rubber-stamp audit firms (alleged to be one operation)". The article does not allege a formal affiliation with Delve |
| 12a | NIST SP 800-63B: no forced periodic password rotation | Confirmed. SP 800-63B-4 §3.1.1.2: verifiers "SHALL NOT require subscribers to change passwords periodically" and SHALL force a change on evidence of compromise | https://pages.nist.gov/800-63-4/sp800-63b.html | POL-04; policy inventory; findings #8; CTL-IAM-004 | None |
| 12b | GitHub organization audit log retains 180 days; API access on Enterprise Cloud | Confirmed for retention and API. The org-audit-log page does not cover streaming, so streaming availability by plan was not verified (the repo already says "stated for Enterprise Cloud" and "verify current GitHub plan feature matrix") | https://docs.github.com/en/organizations/keeping-your-organization-secure/managing-security-settings-for-your-organization/reviewing-the-audit-log-for-your-organization | evidence-architecture §; OQ-508 | None |

## Pass 3: regulatory applicability (regulatory-applicability-matrix.md and REQ rows)

| Area | Assessment | Edits |
|---|---|---|
| GLBA Safeguards / Privacy | Defensible. Routes are analysed separately, contradicting readings are preserved (GLBA grant vs FTC Act limit; Reg P has no nonprofit carve-out), and "adopt as baseline" is labelled a recommendation. The Grand Canyon description was the only factual problem (see #4). Matrix §3.2 says "banks expect the exempted elements anyway"; this is a recommendation-level generalisation and is tagged as one | See #4 |
| PCI DSS | Defensible: not applicable in Release 1 while card intake is deferred. The trigger and the "can impact CDE security" caveat for a hosted donation page are preserved. Reported 2026 SAQ A FAQ change remains unverified (already flagged) | None |
| GDPR | Defensible: Art. 3(2) targeting/monitoring test with explicit triggers (OQ-208). Art. 28/33 rows already capped at `assumption` | None |
| State privacy laws | Defensible, and nonprofit coverage is consistent with my knowledge (CO, DE, NJ, OR, MD, MN do not generally exempt nonprofits). **Contradiction found and preserved:** for Connecticut's move to a data-level GLBA exemption, Orrick gives 2025-10-01 while FPF and Perkins Coie give 2026-07-01 (SB 1295 / PA 25-113). Both dates have passed, so there is no practical effect today | REQ-STATE-003 summary, SRC-REG-044 limitations and matrix §4.1 cell now show both dates |
| NACHA | Defensible: contractual for a TPSP via ODFI/originator agreements. Fraud monitoring is already in force; the account-number unreadability threshold (2M) is correctly described as voluntary until met | Matrix "Binds" cell clarified (#5) |
| IRS 4966 / 4967 / 170(f)(8) / 170(f)(18) | Classification (contractual for Unbound) is defensible, but three reasoning texts were **inaccurate**. (a) 4967 taxes donors/advisors and fund managers, not the sponsor. (b) 170(f)(8) and 170(f)(18) are conditions on the donor's deduction, not statutory duties of the sponsor (the sponsor issues the acknowledgment in practice; the 6115 quid-pro-quo disclosure is a charity duty). (c) No contradicting view was recorded: platform personnel acting with sponsor authority could be argued to be "fund managers" (4966(d)(3)). Evidence/confidence "authoritative / high" conflated the law (authoritative) with the classification (interpretation) | REQ-IRS-002/003/004 reasoning rewritten; contradiction added to REQ-IRS-002 and to the matrix IRS row; matrix confidence now reads "authoritative (law) / medium (classification)" |

## Remaining doubts

- AU-C 9402 "revised March 2024" and the exact quoted phrases could not be
  re-read: the PDF is login-gated, and the repo says B2 read it from the AICPA
  codification PDF.
- TQA 9561 and the 2027 exposure drafts are known only from news releases and
  CPA-firm summaries.
- The Grand Canyon opinions themselves were not read. The case history comes
  from a law-firm blog and the FTC case page.
- The Connecticut effective date conflict was not resolved against the
  enrolled act.
- Nacha rule text remains subscription-only.
- I did not check whether GitHub audit-log streaming is limited to Enterprise
  Cloud.
- The matrix IRS row has one fewer cell than its header: the Unbound and Class
  values are merged. This predates my pass and I left it as it was.
