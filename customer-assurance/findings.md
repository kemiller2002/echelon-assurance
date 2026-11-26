# Customer assurance knowledge base: research findings

Date: 2026-11-25. Scope: Echelon Foundry, first product Unbound (white-label DAF servicing
platform). Research only; no repository was modified. Companion files:
`question-bank.csv` (97 canonical questions, CAQ-001..CAQ-097) and `sources.csv`
(SRC-CAQ-001..024, all retrieved or read on 2026-11-25).

Current-state baseline used for every answer: Unbound has no production deployment; all 121
controls in `echelon-assurance/controls/control-catalog.csv` are `Designed`; all 83 policy
entries in `policies/policy-inventory.csv` are `drafted` (none adopted); no SOC report,
certification, external penetration test or insurance policy exists.

## 1. What was consolidated from echelon-due-diligence

Repository read at `echelon-due-diligence@721ebe1` (branch `claude/echelon-assurance-consolidation-jygy23`).

| Path | What it contributes | How it was used |
|---|---|---|
| `AGENTS.md`, `docs/DUE-DILIGENCE-OPERATING-CONTRACT.md` (DD-GOV-001) | Integrity rules: never present a proposed control as implemented, label independence, do not copy licensed questionnaire text, human approval before release | Adopted as answer-writing rules for the bank |
| `questions/*.jsonl` (2,404 questions, 67 categories) | Canonical question bank with framework locators and wording alternatives | Source of topic coverage; CAQ items are a deduplicated customer-facing subset (one CAQ often covers several DDQ items) |
| `answers/*.jsonl` (535 answer units) | `current_practice`, `intended_practice`, `provable`, `customer_statement`, `evidence_mechanism`, `evidence_freshness` (ISO 8601), disclosure class | Wording style and honesty model; none are human-approved (`review` absent on all 535) |
| `controls/*.jsonl` (642 ECTL-*), `evidence-requirements/*.jsonl` (617 EVR-*) | Parallel control and evidence vocabulary | Not used as ids in the bank (task requires echelon-assurance ids); see conflict C1 |
| `questionnaires/REAL-WORLD-QUESTIONNAIRE-RESEARCH.md` (DD-QNR-001) | 63 public sources: bank supplier standards, insurer applications, DAF sponsor documents; 38 high-friction topics | Topic list cross-checked; key sources re-retrieved here (Truist, Morgan Stanley, At-Bay, Beazley, TMHCC, Indiana, NYDFS, Ohio University, Fidelity Charitable) |
| `industry-profiles/donor-advised-funds/DAF-SPONSOR-TECHNOLOGY-PROCUREMENT-PROFILE.md` | No DAF sponsor or community foundation publishes a vendor questionnaire; expectations inferred from parent-company supplier policies | Cited as SRC-CAQ-023; DAF-specific questions are marked inferred |
| `industry-profiles/financial-services/...`, `industry-profiles/enterprise-software/INSURANCE-UNDERWRITING-PROFILE.md` | Bank TPRM and insurer underwriting expectations | Topic coverage for FI and insurer questions |
| `trust-center/TRUST-CENTER-RESEARCH.md`, `trust-center/TRUST-CENTER-REQUIREMENTS.md` (DD-TC-002) | Six-tier disclosure model; TC-HON-001 (no current claim unless operating); TC-RED-003 (planned or absent preventive controls never public) | Drove `disclosure_class` assignment |
| `reports/EXECUTIVE-RESEARCH-FINDINGS.md` | 0 of 535 answers verified operational; gap dominated by cheap organizational decisions | Confirms the honest baseline |

Also read: `echelon-assurance` trust-center spec (ASR-TRUST-SPEC-001), control catalog, policy
inventory, evidence types, `requirements/organizational-governance.md` (GOV-BC-019/020),
`requirements/continuous-assurance-requirements.md`, `SECURITY-AND-CONFIDENTIALITY.md`,
`registers/open-questions.csv`; `unbound/requirements/OPEN-QUESTIONS.md`; and
`daf-landscape@0c8bd5f:research/mission-2026-11-14/queue-agentic-controls-and-sponsor-audits.md`
(peer DAF and grantmaking software publicly claim SOC 1 Type II and SOC 2 Type II).

## 2. Overlaps and conflicts with existing assurance artifacts

| # | Issue | Evidence | Recommended resolution |
|---|---|---|---|
| C1 | Two control catalogs. echelon-due-diligence has 642 `ECTL-*` controls and 617 `EVR-*` evidence requirements; echelon-assurance has 121 `CTL-*` controls and 35 `EVT-*` evidence types. No crosswalk exists. | `controls/*.jsonl` vs `controls/control-catalog.csv` | Make echelon-assurance `CTL-*` authoritative for answers (as this bank does); add an `ECTL -> CTL` crosswalk; retire or mark ECTL as research-only |
| C2 | Status vocabularies differ: DD `proposed-target-state`/`partially-implemented`/`verified-operational`; assurance `Designed`. Bank uses `designed`/`not-started`/`unknown`/`not-applicable`. | DD answer schema; catalog `status` | Define one lifecycle (for example Designed, Implemented, Operating, Independently-verified) and map both |
| C3 | Disclosure tiers differ: assurance spec has 4 tiers (Public, Prospect, Customer, Bank partner); DD-TC-002 has 6 (public, authenticated-prospect, NDA, existing customer, auditor-only, internal). Bank uses 3 (public, customer-nda, restricted) as instructed. | Both trust-center specs | Adopt DD's 6 tiers as the storage model; derive the 3-class view for questionnaires |
| C4 | Pen-test letter tier: assurance spec puts it at Prospect (NDA click-through); DD TC-RED-002 says NDA tier only. Compatible but worded differently. | ASR-TRUST-SPEC-001 §2; DD-TC-002 | Harmonize wording on NDA |
| C5 | Publicity of planned controls: the assurance spec does not restrict it; DD TC-RED-003 forbids publishing absent or planned preventive and detective controls (MFA, EDR, monitoring, backups). | DD-TC-002 TC-RED-003 | Followed TC-RED-003: those answers are `customer-nda` or `restricted`; only program approach, independent-assurance status, VDP, AI use and subprocessor posture are `public` | *(Superseded for disclosure classes by the question bank, which marks only CAQ-014 public.)*
| C6 | Dangling cross-repo ids: echelon-assurance cites "due diligence BND-06" and "DDT-04" (`requirements/continuous-assurance-requirements.md` CA-TRACE-01, CA-IND-02) but neither id exists anywhere in echelon-due-diligence on any branch (git grep across refs). | grep, git grep | Replace with real ids (likely DD-GOV-001 rule 3/4, DD-TC-002 TC-INF-001/TC-HON-002) or define them |
| C7 | DD marks 7 answers `partially-implemented` with `qualified-current-fact` statements about developer tooling (for example ANS-SCS-005 signed build provenance for published developer tools; ANS-AIA-001 Praxis attribution). echelon-assurance says Praxis attestation is "implemented, tested, not enabled" and agent provenance is self-reported (OQ-702). | `answers/SCS.jsonl`, `answers/AIA.jsonl`; continuous-assurance-requirements.md | Verify each claim against a commit before reuse; this bank only states that repository attribution exists and is self-reported (CAQ-030, CAQ-048) |
| C8 | SOC timing: unbound `OPEN-QUESTIONS.md` XID-INV-008 is resolved as "no universal rule" (its "Type I at launch, Type II within 12 months" text is the original recommendation, not the resolution) while echelon-assurance OQ-101/OQ-110 (auditor approach, period) are open and SPD-PLT-001 asks whether any SOC is needed in release 1. | unbound OPEN-QUESTIONS.md lines 100, 159, 194 | Owner decision record; bank answers CAQ-012/013 say "not yet determined" until then |
| C9 | Freshness vocabulary: DD uses ISO 8601 durations (`P1D`, `P30D`); assurance uses `validation_frequency` prose; this bank uses prose rules. | DD answers; catalog | Store machine-readable duration plus rendered prose |
| C10 | Evidence access classes (`restricted`, `internal`, `confidential`) in `evidence-types.csv` are a fourth classification scheme beside the three above. | `registers/evidence-types.csv` | Map to STD-ORG-CLS-001 once and reference it |

## 3. Design recommendations: answers generated from authoritative records

1. **One answer record per concern, many question variants.** Keep the CAQ question as the
   canonical concern and attach customer wordings (SIG, CAIQ, VSA, bank forms) as mappings by
   locator only. Never store SIG or VSA question text (both licensed; SRC-CAQ-010, 011).
2. **Generate, do not author.** `proposed_answer` should be a template rendered from: control
   objective, status, latest validation result, evidence freshness and open-question state
   (as ASR-TRUST-SPEC-001 §4 already proposes). Hand-authored text only for `manual` and
   `owner-decision` bases, which must carry an approver and date.
3. **Status gates wording.** A renderer may emit present-tense capability language only when
   the control status is Operating and evidence is within its freshness rule (DD TC-HON-001).
   With all controls at Designed, every generated answer must begin with "Designed, not yet
   operating" or "Not yet determined". Add a lint that fails on present-tense verbs
   ("we enforce", "is encrypted") when status is not Operating.
4. **Unknown is not yes (GOV-BC-019).** Expired evidence, an `indeterminate` check (CA-AUTO-02)
   or an open OQ cited by the answer forces the answer back to "Not yet determined" text.
5. **Freshness per answer, derived from evidence.** Each answer's `evidence_freshness_rule`
   should be computed from its controls' `validation_frequency` (for example daily controls
   imply 1-day evidence); until operating, a 90-day catalog-snapshot rule applies.
6. **Approval routing from content.** `owner+legal-research` (owner approval after recorded legal research; external advice only on a recorded trigger, ODP-12) whenever an answer touches notification
   deadlines, data location, retention, insurance, audit rights, legal applicability or
   contract terms; `owner` otherwise. No answer has `none`, because GOV-BC-019 and DD-GOV-001
   require human approval before any release, and agents draft but do not send (POL-11.02).
7. **Version and trace.** Each released questionnaire records answer id, version, as-of date,
   catalog commit and evidence ids (DD TC-INF-001); CTL-GOV-012 reconciles released claims.
8. **Novel questions create gaps, not ad hoc answers** (ASR-TRUST-SPEC-001 §4 step 4).

## 4. Public assurance interface

> *Research proposal (superseded for disclosure classes by `customer-assurance/question-bank.csv`, which marks only CAQ-014 public under BR-CUS-002 and TC-RED-003). Do not build the public page from the examples below.*

| Class | Contents | Examples in bank |
|---|---|---|
| public | Security program approach and framework alignment; honest status of independent assurance (no SOC report, no certifications, no pen test yet); encryption design principle; VDP and security.txt (once published); AI use disclosure and data-use position; subprocessor list (once it exists); trust-center description | CAQ-003, 011-015, 018, 031, 042, 048, 049, 052, 057, 069, 079, 081, 084, 088 |
| customer-nda | Control-level design detail, including planned or absent preventive and detective controls (MFA, access reviews, backups, logging, IR), open legal and contractual items, DAF financial-integrity design | most items |
| restricted | Items that would map defenses or weaknesses: EDR coverage, 24x7 monitoring gaps, fraud detection rules, key-person dependency, evidence-store admin roles; financial statements | CAQ-009, 054 (EDR), 059 (monitoring), 068 (key person), 076 (fraud), 095 |

Never published at any tier (SECURITY-AND-CONFIDENTIALITY.md, GOV-BC-020): vulnerability
details, unremediated findings, pen-test reports (letters only), network addressing,
credentials, production evidence, other customers' information. SOC 1 and SOC 2 reports
follow their use restrictions (customer and auditor tiers only).

The public view is safe to publish today only as statements of approach and status; it must
not include a list of controls that are not yet operating (TC-RED-003).

## 5. Gaps

Research gaps:
- FFIEC IT Examination Handbook returned HTTP 403 and was not used; the bank relies on the
  2023 Interagency Guidance (read in full) and Federal Reserve letters SR 24-7 and SR 21-14.
- SIG (Shared Assessments) is licensed; only a licensee's public summary (SRC-CAQ-011) was
  read. VSA requires registration; only its landing page was read (SRC-CAQ-010). The CAIQ
  v4.1 workbook was not downloaded; only the CSA landing page was read (SRC-CAQ-009).
- SEC Regulation S-P: only the Federal Register abstract was read (fact sheet HTTP 403).
- No DAF sponsor or community foundation vendor questionnaire is public; DAF-sponsor
  questions are inferred (SRC-CAQ-023) and need validation with real prospects (OQ-215).
- Bank expectations after the FFIEC CAT sunset are unknown (OQ-217). The 2026 proposed
  interagency TPRM guidance (SRC-CAQ-002) may change bank due diligence; only its abstract
  was read.

Coverage gaps in the authoritative catalogs (questions answered `manual` or with no control):
- No control or policy for: insurance (cyber, E&O, crime; CAQ-085..087), incident response
  retainer (CAQ-063), government data access requests (CAQ-043), personnel and agent
  location restrictions (CAQ-022), accessibility conformance (CAQ-093), phishing
  simulation, customer SSO federation for tenant users (CAQ-028), explicit end-of-life
  software detection, customer-run testing rights, financial-condition disclosure.
- Numeric commitments absent from drafted policies: deprovisioning deadline, vulnerability
  remediation SLAs by severity, breach notice window, RTO and RPO, availability SLA,
  subprocessor notice period, retention periods (OQ-502, OQ-762).
- Policies are all `drafted`; no approval evidence (EVT-021) exists, so no policy can be
  described as adopted.
- Owner decisions blocking many answers: accountable roles and deputy (OQ-607), SoD staffing
  (OQ-608), service-organization entity (OQ-102, OQ-756), workforce IdP (OQ-764), cloud
  provider (OQ-504), AI provider terms (OQ-603).
- The bank has no human approval yet; per GOV-BC-019 and DD-GOV-001 nothing in it is
  customer-releasable until a named human approves each answer.

Recommended next steps: (1) owner resolves C1-C3 vocabulary and makes the CAQ bank a
registered artifact in echelon-assurance with an answer schema; (2) add controls or
policy sections for the coverage gaps above; (3) interview 3 to 5 prospective sponsors and
one bank partner (OQ-215, OQ-217) and map their actual questionnaires to CAQ ids;
(4) build the status-gated renderer and wording lint before any answer is released.
