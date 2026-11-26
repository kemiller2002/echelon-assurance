---
id: ASR-FIN-UNB-INV-001
title: Inventory of existing Unbound and daf-landscape assurance work (reuse baseline)
status: draft
version: 0.1.0
created: 2026-11-25
updated: 2026-11-25
sources: [SRC-ECH2-016, SRC-ECH2-017, SRC-ECH2-018, SRC-ECH2-019, SRC-ECH2-020, SRC-ECH2-021, SRC-ECH2-022, SRC-ECH2-023]
open_questions: [OQ-753..OQ-766]
---

# Existing Unbound and daf-landscape assurance work

Purpose: let the Echelon assurance programme **reuse** the substantial
assurance design already present in `kemiller2002/unbound` (@`9b38de4`,
2026-11-24) and `kemiller2002/daf-landscape` (@`97dddb7`, 2026-11-25), and
see clearly what it is and is not. Unbound identifiers are preserved and
never renamed (`methodology/identifiers.md`).

**Headline.** Unbound holds a large, internally consistent *design* for
SOC 2-oriented control operations and continuous control monitoring, and
an emerging SOC 1 relevance tagging. **Nothing is implemented, operating
or tested**: every control row, test and monitoring requirement is marked
not executed / No. Unbound is in requirements discovery; its charter
forbids application implementation before a formal review gate
(`requirements/CHARTER.md`, principle 8). [observed; classification of
consequences: recommendation]

## 1. Artefact inventory

Paths relative to `/home/user/unbound` unless stated.

| Artefact | Content | Size | Recorded status | Reuse value |
|---|---|---|---|---|
| `requirements/governance/SOC2-CONTROLS-EVIDENCE-AND-ENTERPRISE-DIRECTIVE.md` | SOC2-001..036 directive; minimum evidence envelope; example register (CTRL-ACCESS-01 …); SOC-T01..T18 acceptance scenarios; 6 assurance gates; adopted refinements RR-SOC2-007-r1, UNB-RCN-PLT-008, UNB-RCN-PLT-013 | 124 lines | "No control is presently asserted operational" | **High** – programme-level principles match `echelon-assurance` rules; reuse as requirement source |
| `requirements/governance/MASTER-ENTERPRISE-CONTROL-MATRIX.csv` | UNB-CTRL-001..090; 30 domains × 3 | 90 rows | all `Designed`; operating/test/auditor = `No` | **Low as controls**, useful as a domain list (see §3) |
| `requirements/governance/SPECIFIC-EXECUTABLE-CONTROLS.csv` | UNB-SCTRL-001..046 with event type, failure condition, negative test, owner, independent reviewer | 46 rows | all `Specified`; producer verified / negative test / independent review = `No` | **High** – genuinely specific, testable assertions |
| `requirements/governance/MASTER-CONTROL-MATRIX-GOVERNANCE.md` | MATRIX-001..; lifecycle states | 38 lines | calls rows "proposed" (CSV says `Designed`) | Medium |
| `requirements/governance/EXECUTABLE-ASSURANCE-ARCHITECTURE.md` | ControlRegistry, EvidenceCollector, EvidenceIntegrity, ExpectedEvidenceMonitor, ControlEvaluator, NegativeTestHarness, IndependentReviewer | 47 lines | "Design only" | High – matches CCM architecture |
| `architecture/CONTINUOUS-CONTROL-MONITORING-AND-EVIDENCE-ASSURANCE.md` | canonical model, 7-step pipeline, assurance states, denominator discipline, missing-evidence algorithm, API boundaries, fail-closed policy | 47 lines | "Requirements specification, not implemented or audited" | **High** – near-direct basis for `architecture/evidence-architecture.md` |
| `requirements/governance/CONTINUOUS-CONTROL-MONITORING-REQUIREMENTS.csv` | MON-001..024 | 24 | `Specified`, implemented/tested `No` | High |
| `requirements/governance/CONTINUOUS-CONTROL-MONITORING-TESTS.csv` | CCM-T01..T20 | 20 | all `Not executed` | **High** – ready-made negative-test catalogue for ASR controls |
| `requirements/governance/CONTROL-OPERATIONS-REQUIREMENTS.csv` | OPS-001..032 | 32 | all `Proposed` | High (control lifecycle) |
| `requirements/governance/CONTROL-OPERATIONS-ACCEPTANCE-TESTS.csv` | LIFE-T01..T18 | 18 | all `Not executed` | High |
| `requirements/governance/CONTROL-OPERATIONS-LIFECYCLE.md` | narrative for OPS | 75 lines | design | Medium |
| `requirements/governance/HIGH-RISK-CONTROL-VALIDATION-WAVE-03.csv` + `HIGH-RISK-CONTROL-GAP-CLOSURE-WAVE-03.md` | VAL-001..024 risk-prioritised validation specs; acceptance rule for "Validated" mapping | 24 | validation/test/evidence = not done | High – but **ID collision** with `echelon-assurance` `VAL-NNN` (§4) |
| `requirements/governance/REQUIREMENT-TO-CONTROL-CROSSWALK-WAVE-01.csv` / `REQUIREMENT-CONTROL-RECONCILIATION-WAVE-01.md` | 494 requirement IDs from 15 docs | 494 | "Provisional traceability" | Low |
| `requirements/governance/DETAILED-REQUIREMENT-CONTROL-LINKS-WAVE-02.csv` / `MANY-TO-MANY-…-WAVE-02.md` | 1,893 keyword/domain candidate links to UNB-SCTRL | 1,893 | "Proposed, not validated"; RR-SOC2-007-r1 AC-1 says they contribute **zero** to coverage | Low – triage only |
| `requirements/governance/ENTERPRISE-CAPABILITY-TO-CONTROL-CROSSWALK.csv` | domain → UNB-CTRL triples | 30 | design | Low |
| `requirements/governance/CONVERGENCE-CLAUSE-TO-CONTROL-CROSSWALK.csv` | 79 rows: CNV clause → UNB-CTRL, operator (Unbound 37 / shared 30 / sponsor 12), report relevance (SOC1 38, SOC1+SOC2 20, SOC2 19, none 2), automated (50 yes / 29 no), CUEC id | 79 | `evidence_query` = "to be defined at implementation (IPE definition per CNV-GOV-001)" for all 79 | **High for SOC 1 scoping**; 22 distinct CUEC IDs across requirements |
| `requirements/governance/SPONSOR-AUTHORITY-AND-AUTONOMOUS-OPERATIONS.md` | sponsor authority, agent boundaries | 179 lines | design | High for AI/FIN |
| `architecture/TENANT-AWARE-PERSISTENCE-DESIGN.md`, `requirements/PARTNER-SEGREGATION-AND-TENANCY.md` | isolation profiles A/B/C, UNB-TEN-001.., mandatory Strata (UNB-TEC-STRATA-001..003) | — | "NOT implemented or validated" | High for TEN; Strata assumption contradicted (§4) |
| `requirements/FULL-ACCOUNTING-SYSTEM.md`, `requirements/accounting/CANONICAL-CHART-AND-POSTING-RULES.md` | UNB-ACC-001..036, ACC-MDL-*; CNV-FIN-* with SOC1 tags and CUECs | — | candidate requirements | **High for FIN** |
| `requirements/DEFERRED-PAYMENTS-AND-REMAINING-GAPS.md` | CNV-PAY-001 (sponsor originates, Unbound TPSP), CNV-PAY-002 (positive pay, NOC handling, destination proof) | — | adopted for design; activation gates name bank agreements | High for PAY |
| `requirements/ai/AI-NATIVE-INTERACTION-REQUIREMENTS.csv` + md | 36 AI interaction requirements | 36 | design | Medium (AI domain) |
| `requirements/discovery/2026-11-24-open-decision-resolution-review.md` and `…/simplification-economics/data/decisions.csv` | SPD-PLT-001 (is any SOC report needed in release 1?), SPD-ADV-003 (sponsor auditor ITGC assurance without SOC 1), XID-INV-008 (SOC 1 Type II before first white-label sponsor?) | — | resolved and owner-approved in Unbound @9b38de4, each with Unbound's own basis (SPD-PLT-001 and SPD-ADV-003 external-authorization-required; XID-INV-008 reasoned-interpretation) and gated on sponsor auditors and contracts (recorded here as open at inventory time) | **Directly relevant open decisions** |

daf-landscape (paths relative to `/home/user/daf-landscape`):

| Artefact | Content | Reuse value |
|---|---|---|
| `research/soc2-assessment-2026-11-11.md` | Public SOC 2 claims of 9 DAF ecosystem entities; **zero reports reviewed**; Ren entity-disambiguation warning; 2026-11-14 validation correction | High as market/customer-expectation evidence (vendor-claim class only) |
| `research/technology-control-map.md` | DAF transaction flow; accountability by boundary; evidence to request (incl. "SOC 1 relevance" for reporting); failure-mode diligence tests | High – control-boundary model for sponsor vs platform |
| `docs/assurance-methodology.md` | Claim taxonomy (report-reviewed … conflicting), source ranking A–D/U, diligence checklist incl. CUECs/CSOCs and "SOC 1 Type II where fund accounting and financial reporting controls are material; SOC 2 alone cannot substitute" | High – compatible with our evidence classes |
| `docs/financial-core-control-spec-v0.5.md` | 10 financial invariants, canonical command envelope and lifecycle, settlement conservation invariant, gates G0–G9; "NO-GO for production financial execution"; lists unsafe prototype behaviours to not reuse | **High** – best existing FIN control design |
| `data/assurance.csv`, `data/assurance-sources.csv` | 9 vendor/sponsor assertion rows (all `independent_report_reviewed=no`), 22 sources | Medium – vendor-claim evidence only |

## 2. Mapping of existing Unbound control IDs to `echelon-assurance` domains

Unbound domain codes → our control domains (`methodology/identifiers.md`).
UNB-CTRL ranges are the master matrix (3 per domain); UNB-SCTRL are the
specific controls; VAL (Unbound) are wave-3 validation specs (prefixed
`UNB-` here to avoid collision).

| Our domain | Unbound domain(s) | UNB-CTRL | UNB-SCTRL | Unbound wave-3 VAL | Other Unbound IDs |
|---|---|---|---|---|---|
| GOV | GOV | 001–003 | 001 (sponsor decision authority), 002 (SoD) | UNB-VAL-001, 003, 020 | SOC2-001, 007, 019–024, 033–036; OPS-001..008, 013, 021, 031, 032; MATRIX-* |
| IAM | IAM, API | 004–006, 067–069 | 003 (provisioning), 004 (revocation), 005 (privileged auth/MFA), 037 (API scopes) | — | SOC2-010 |
| TEN | TEN | 007–009 | 006 (tenant isolation) | UNB-VAL-018 | UNB-TEN-001.., MON-013, CCM-T07/T15, LIFE-T11 |
| DAT | PRV, CHD | 085–087, 010–012 | 007 (restricted export), 045 (retention/deletion), 008/009 (source rights, freshness) | UNB-VAL-022 | SOC2-003, 014, 028, 031, 032; MON-014 |
| SDL | CHG | 070–072 | 038 (approved/signed release), 039 (secret scanning) | — | SOC2-011, 016 |
| LOG | LOG | 073–075 | 040 (tamper-evident evidence), 041 (missing evidence heartbeat) | UNB-VAL-016, 017 | SOC2-002, 005, 006, 012, 025, 027 |
| IR | INC | 076–078 | 042 | — | SOC2-026; CCM-T12/T14 |
| BCR | BCP | 079–081 | 043 (restore exercise) | — | SOC2-015; CCM-T19 |
| VEN | VEN | 082–084 | 044 | — | SOC2-013 |
| AI | AI | 064–066 | 035 (agent tool authorization), 036 (prompt injection) | UNB-VAL-002, 019 | SOC2-017, 029; OPS-007; MON-012 |
| FIN | ACC, CLS, PAY, CON, FEE, TAX, NAV, INV, CUS, GRT, GEX, GRP, DON, SUC | 013–060 (various) | 015–033 | UNB-VAL-001, 008–015, 023, 024 | UNB-ACC-001..036; ACC-MDL-*; CNV-FIN-*, CNV-PAY-001/002, CNV-GRT-*; SOC2-018; OPS-028, 030; MON-019; CCM-T03/T17/T18 |
| CUS | DON, COM, SUC | 019–024, 061–063 | 034 (notification privacy) | — | — |
| ASR | REP, LOG (evidence) | 088–090 | 046 (auditor evidence package) | UNB-VAL-021 | SOC2-008, 009, 020, 031, 034, 035; MON-001..011, 015, 016, 018, 024; CCM-T01..T20; LIFE-T01..T18 |
| (eligibility/legal) | ELG, CHK | 013–018 | 010–014 | UNB-VAL-004–007 | MON-020; OPS-029 (no direct domain in our scheme; nearest GOV/FIN) |

Domains in our scheme with **no** Unbound counterpart: `HR`, `END`,
`NET`, `CFG`, `VUL` (only SOC2-016 narrative), `CRY` (only narrative in
SOC2-014 and EvidenceIntegrity). These are standard SOC 2 CC areas and
must come from elsewhere. [recommendation]

## 3. Quality observations

| # | Observation | Evidence | Consequence | Class |
|---|---|---|---|---|
| Q1 | **Every status field is negative.** 90/90 UNB-CTRL: operating evidence `No`, test executed `No`, auditor reviewed `No`. 46/46 UNB-SCTRL: producer verified, negative test executed, independent review all `No`. 20/20 CCM-T and 18/18 LIFE-T `Not executed`; MON-001..024 implemented/tested `No`; OPS-001..032 `Proposed`. | the CSVs listed in §1 | Honest, consistent with SOC2-008; nothing can be imported above `Designed`/`Specified`. | observed |
| Q2 | **Master matrix is templated, not designed.** All 30 domains carry the same three objectives ("Authority and segregation", "Evidence and audit integrity", "Independent review and exceptions"), the same evidence strings ("…; domain-specific source evidence"), and a SOC 2 mapping that is one of three fixed values (CC5/CC6, CC7, CC5/CC7). | `MASTER-ENTERPRISE-CONTROL-MATRIX.csv` | Rows do not state a risk-specific objective; an auditor would not accept them as control descriptions. Reuse domain list and IDs as `unbound_refs` anchors only. | recommendation |
| Q3 | **Status vocabulary drift.** Governance narrative calls the 90 rows "proposed"; CSV says `Designed`. | `MASTER-CONTROL-MATRIX-GOVERNANCE.md` vs CSV | Treat as `Proposed` in our catalog until objective-level redesign. | recommendation |
| Q4 | **Coverage illusion acknowledged by Unbound itself.** 1,893 keyword candidate links; Unbound's own wave-3 and RR-SOC2-007-r1 say they count zero. | wave-2/3 docs | Do not import as `requirement_ids`. | observed |
| Q5 | **Evidence queries undefined.** All 79 convergence crosswalk rows: "to be defined at implementation (IPE definition per CNV-GOV-001)". | `CONVERGENCE-CLAUSE-TO-CONTROL-CROSSWALK.csv` | SOC 1 relies heavily on information produced by the entity (IPE) completeness/accuracy; this is an unresolved design element. | recommendation |
| Q6 | **Assumed dependency capabilities do not exist.** Strata mandated for append-only persistence/integrity; Folio for chain-of-custody packages. | `TENANT-AWARE-PERSISTENCE-DESIGN.md`; SOC2-009; MON-015; see `integration/fragments/ECH2-assessment.md` F3 | Evidence-integrity and packaging controls have no implementing system. | recommendation |
| Q7 | **Identifier collision.** Unbound wave-3 uses `VAL-001..024`; `echelon-assurance` reserves `VAL-NNN` for validation procedures. | `HIGH-RISK-CONTROL-VALIDATION-WAVE-03.csv`; `methodology/identifiers.md` | Reference Unbound's as `UNB:VAL-0xx` in `unbound_refs`; never import into `registers/validation-procedures.csv`. | recommendation |
| Q8 | Owners are role placeholders (MATRIX-002 says so); SCTRL independent reviewer frequently "Compliance / Independent Control Reviewer" — no named individuals, appropriate for design stage. | CSVs | None now; must resolve before `Implemented`. | observed |
| Q9 | Strong, auditor-friendly principles already exist: evidence as by-product (SOC2-004), failures and denials as evidence (SOC2-005), missing heartbeat = exception (SOC2-027), synthetic vs production separation (SOC2-031), no false claims (SOC2-036, MON-024), denominator discipline. | directive and CCM architecture | Adopt verbatim as design principles with citation. | recommendation |
| Q10 | SOC 2 Trust Services Criteria mapping is coarse (CC5/CC6/CC7 only); no Availability, Confidentiality, Processing Integrity or Privacy criteria selected; SOC2-021/023 defer criteria selection to an auditor. | master matrix; directive | Correct to defer; our programme must do the scoping. | observed |

## 4. SOC 1 vs SOC 2 relevance of Unbound's business activities

### 4.1 What Unbound is (per its own requirements)

- A **reference DAF enterprise** design, explicitly "not a claim that a
  regulated charitable sponsor already exists" (`requirements/CHARTER.md`).
  [observed]
- The **sponsor** (the sponsoring charity) is the legal owner of DAF assets
  and holds grant decision authority; Unbound operates the platform for
  the sponsor. Release 1 = "one sponsor, platform operated for that
  sponsor" (SPD-PLT-001). Partners are "institutional distribution/
  servicing tenant[s], not automatically the legal accounting entity"
  (`accounting/CANONICAL-CHART-AND-POSTING-RULES.md` §1); Bank of America is
  named only as illustrative (`PARTNER-SEGREGATION-AND-TENANCY.md`).
  [observed]
- **Customers (user entities) are sponsors**, and potentially partners
  (white-label/servicing institutions). Donors and advisors are end users
  of sponsors, not Unbound's contracting customers. [assumption derived
  from the observed role definitions; OQ-753]

### 4.2 Financial administration activities

| Activity | Unbound role per requirements | Source | Feeds sponsor financial statements? |
|---|---|---|---|
| DAF program ledger (contributions, grants payable, fees, fund balances) | "financial system of record: Unbound program ledger"; sponsor ERP for entity statements; Unbound exports balanced summarized journals to the sponsor's system of record | `FULL-ACCOUNTING-SYSTEM.md` lines ~144, 317–378; SPD-FIN-002 | **Yes** – DAF contributions, grants and net assets are material line items of a sponsoring charity's statements [assumption; OQ-754] |
| Investment unitization / NAV | "Unbound unit ledger; custodian prices" | `FULL-ACCOUNTING-SYSTEM.md` ~361; `investments/NAV-UNITIZATION-VALUATION-ENGINE.md` | Yes (investment valuation of fund assets) |
| Fees (sponsor/partner schedules, accruals) | Unbound program ledger | ~328, 371 | Yes (sponsor revenue/fees) |
| Reconciliation to bank and custodian | operational owner "independent reconciler (outside payment chain)"; Unbound ledger vs bank/custodian | ~349; UNB-ACC-018 | Yes (completeness/accuracy) |
| Payments | Sponsor originates; Unbound is a **Third-Party Service Provider**; Unbound "SHALL NOT sign an origination agreement, hold funds or execute or block a payment" on its own; positive pay, NOC handling | `DEFERRED-PAYMENTS-AND-REMAINING-GAPS.md` CNV-PAY-001/002 | Indirect (cash disbursement completeness) |
| Grant lifecycle and approvals | Board-delegated deterministic approval; sponsor authority | CNV-GRT-001; `grants/*` | Yes (grant expense/payable recognition) |
| Tax/regulatory data (e.g. Form 990 DAF disclosures, donor acknowledgments) | Source data with human sign-off | UNB-ACC-021 | Supports sponsor filings |
| Bank relationships | Sponsor's ODFI and positive-pay agreements; activation gated on executed agreements | CNV-PAY-001/002 activation gates | — (relationship detail is confidential; not recorded here) |

### 4.3 Characterisation

1. **SOC 1 relevance is likely if the operator is separate from the sponsor (OQ-756; the sponsor auditor's judgment, OQ-754) if Unbound operates a sponsor's DAF
   program ledger.** A SOC 1 (SSAE 18, AT-C 320) report addresses controls
   at a service organisation relevant to user entities' internal control
   over financial reporting; Unbound processes transactions whose output
   the sponsor's statements depend on. Unbound itself tags 58 of 79
   convergence clauses SOC1 or SOC1+SOC2, and adopted UNB-RCN-PLT-013
   requires per-service SOC 1/SOC 2 scoping agreed with a CPA.
   [auditor-interpretation expected — the AICPA characterisation needs a
   SRC from the standards research batch; the Unbound tagging is observed;
   overall: open-question OQ-754, OQ-755]
2. **SOC 2 relevance** arises from security/availability/confidentiality
   of donor and sponsor data, tenant isolation and AI agents — commitments
   to sponsors, partners and banks. daf-landscape records that competitors
   publicly claim SOC 2 (vendor-claim class), supporting a customer
   expectation. [customer-expectation, low confidence: claims are vendor
   marketing; no report reviewed]
3. **Neither is required merely by the requirements.** Unbound's own decisions
   (SPD-PLT-001, XID-INV-008, SPD-ADV-003; resolved and owner-approved in Unbound, with professional gates still open there) leave timing to
   auditor reliance and contracts; "no universal launch rule". Without a
   SOC 1, the sponsor's external auditor would need to test Unbound's
   ITGCs and application controls directly (SPD-ADV-003). [open-question]
4. **Bank/TPSP**: as a TPSP in ACH, bank partners may impose audit or
   due-diligence obligations; this needs the Nacha-rules research batch
   and the bank partner. [open-question OQ-757]
5. **Release 1 nuance**: if the "sponsor" and the platform operator end up
   being the same or related organisation, Unbound would not be a service
   organisation to a separate user entity, and SOC 1 relevance changes to
   the sponsor's own ICFR and financial statement audit. [open-question
   OQ-756]

### 4.4 Evidence classes for SOC 1 relevant processes (design targets)

| Process (control objective area) | Evidence the design already names | Evidence class of the design claim | Missing |
|---|---|---|---|
| Journal posting (balanced, immutable, authorized) | `JournalPostingValidated` (UNB-SCTRL-025); ACC-MDL-002/003 | recommendation (designed) | IPE definition, period-lock on all write paths, report-level completeness |
| Financial close | `CloseReviewSigned` (UNB-SCTRL-027); break blocks close (028) | recommendation | close checklist evidence, reviewer independence proof |
| Reconciliation bank/custodian | `CustodyBalanceReconciled` (024); CCM-T03 independent mismatch overrides | recommendation | independent source statements ingestion, aged-break evidence |
| Grant approval → payable → payment | 015, 016, 017, 026, 029, 030; CCM-T17/T18 | recommendation | maker/checker evidence over real payment rail; NOC/return handling evidence |
| Contributions acceptance/valuation | 031 | recommendation | valuation source evidence, noncash gift appraisal |
| NAV/valuation | 021, 022 | recommendation | price source completeness, stale-price exception evidence |
| Fee calculation | 032 | recommendation | recalculation evidence |
| ITGCs (access, change, operations) | 003–005, 037–039, 040–043 | recommendation | everything operational (see `integration/fragments/ECH2-assessment.md` F5) |
| CUECs | 22 CUEC IDs referenced (e.g. CUEC-CNV-FIN-001..011, CUEC-CNV-GRT-*) | recommendation | CUEC wording, sponsor acceptance |

## 5. Reuse recommendations

1. Import UNB-SCTRL-001..046 as the primary `unbound_refs` anchors for
   CTL rows; use UNB-CTRL only as domain anchors. [recommendation]
2. Adopt the CCM architecture and CCM-T/LIFE-T scenarios as the
   specification for the ASR domain, citing Unbound, and mark them
   "derived from Unbound, not executed". [recommendation]
3. Adopt the convergence crosswalk's operator/report-relevance/CUEC
   columns as the starting SOC 1 scoping matrix; flag that 79/79 evidence
   queries are undefined. [recommendation]
4. Adopt daf-landscape's financial-core invariants and gates G0–G9 for
   FIN controls; do not reuse its earlier prototype SQL. [recommendation]
5. Propose to Unbound (via `integration/proposed-changes.md`): renaming
   its wave-3 `VAL-` IDs or qualifying them; correcting the Strata and
   Folio capability assumptions; reconciling "proposed" vs `Designed`.
   [recommendation]
