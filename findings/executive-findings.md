---
id: ASR-FIND-EXEC-001
title: Executive findings
status: draft for owner review
version: 1.1.0
created: 2026-11-25
---

# Executive findings

**Bottom line.** Echelon Foundry can make assurance an operating property
rather than an annual exercise, but the savings come from **enforcing
controls structurally and proving financial correctness by invariants and
reconciliation**, not from building a compliance platform. External
examination and penetration-testing fees are unavoidable and make up about
half of five-year cost under every model. Nothing in this repository shows
any control operating today, and no compliance claim is made.

## 1. What reports Unbound needs

- **SOC 1 Type 2 as well as SOC 2 Type 2 is the likely answer**, if Unbound
  operates sponsors' DAF program ledgers for legally separate sponsors. The
  AICPA's own interpretation (AU-C 9402) calls a SOC 1 the preferred report
  for a user entity's financial-statement auditor and says a SOC 2 is
  unlikely to serve that purpose. (`auditor-interpretation`, confidence
  `medium`; `soc/unbound-soc-applicability.md`.)
- **No law requires either report.** Demand comes from sponsors' auditors,
  banks' third-party risk programs and customers. Without a SOC 1, a sponsor's
  auditor would have to test Unbound directly.
- SOC 2 design target: all five trust services categories (owner decision
  DF-ASR-2026-0D02; the research had leaned toward deferring Processing
  Integrity and Privacy). Actual examination scope is confirmed with
  customers and the auditor. Carve out the cloud provider and custodians;
  GitHub is effectively a subservice organization for most Echelon systems.
- No standalone SOC 1 Type 1 (it gives user auditors no operating evidence),
  and no report is possible while Unbound runs only on synthetic data.

## 2. What law actually applies

- **Contested and undetermined:** whether GLBA/FTC Safeguards binds the
  nonprofit sponsors, and whether the for-profit Echelon entity is itself a
  "financial institution". Recommendation: adopt the Safeguards Rule
  elements as the design baseline now, regardless of the answer
  (`regulatory/regulatory-applicability-matrix.md`; counsel questions
  OQ-200–202).
- **Most certain obligations arrive by contract** through banks (12 CFR 30
  App. B, 2023 interagency TPRM guidance, possibly 12 CFR 53.4 incident
  notice), Nacha rules as a third-party service provider, OFAC, state breach
  notification and the IRS rules that bind sponsors and donors (which the
  platform must enable rather than violate).
- PCI DSS, GDPR and NYDFS are currently not applicable; their triggers are
  documented.

## 3. The operating model

1. **One control catalog** (`controls/control-catalog.csv`, 104 controls)
   to which 278 requirements from 31 framework and law codes map; every
   applicable requirement and every policy section traces to controls,
   evidence types and validation procedures, checked by CI.
2. **13 policies instead of 20–35**, with addressable sections, generated
   from one F# policy model; documents vendors recommend but no framework
   requires were not created (`policies/`).
3. **Prevent rather than detect**: 47 controls target L4 preventive
   enforcement (rulesets, IdP, cloud organization policies, application
   authorization). Only four target L5, each reversible and bounded.
   Judgment controls stay human by design (`automation/`).
4. **Evidence at the source**: a standard envelope emitted by the systems
   where events happen, heartbeat detection of missing evidence, and — once
   an auditor has said it will use it — an anchored, append-only ledger with
   completeness reconciliation. Integrity is never presented as accuracy
   (`architecture/`).
5. **Financial assurance by invariants**: write-time double-entry and
   authorization invariants, idempotent processing and daily two-way
   reconciliation against bank and custodian records cover 100% of
   transactions; auditors' sampling then tests these controls rather than
   re-performing arithmetic (`financial/unbound-financial-controls.md`).
6. **Agents operate, humans own, outsiders assure**: every agent
   prohibition is enforced outside the model; agents never approve, never
   write evidence, never represent internal validation as independent;
   controls keep working without AI providers (`ai-governance/`).
7. **Independence is bought, not simulated**: a contracted independent
   reviewer, external validation of the assurance machinery, an independent
   pen test and CPA attestation. A control cannot be marked `Tested` on
   automated or self-assessment results alone.

## 4. What Echelon already has, and what it lacks

- **Has (implemented, not operating):** verifier-signed test receipts and
  fail-closed completion (Praxis, Dokimos) — enabled in no repository;
  Tutela's non-self expiring exceptions; Conditor's signed releases; Arca's
  integrity primitives; Summa's ledger and reconciliation patterns; Aegis
  redaction.
- **Lacks:** tamper-evident anchoring; identity lifecycle and access review;
  central logging/alerting/heartbeat; auditor packaging. None justifies a
  new Echelon system. Owner decisions 0D0A–0D0D: separate evidence generation
  from managed, durable, immutable storage (GitHub is not the sole store;
  interfaces before any custom ledger); Folio renders evidence packages;
  Strata is the database interaction layer; Vigila becomes the logging and
  observability initiative with an independently operated heartbeat.
- **Wrong assumptions in Unbound:** Strata is not an append-only store; Folio
  does not package evidence; GLBA as a cited legal basis is unverified.
  Proposed corrections are in `integration/proposed-changes.md`.

## 5. Economics

Five-year totals (ranges; vendor-claim prices and assumed labor):
conventional $672k–$2.1M; commercial platform $584k–$1.6M; outsourced
$587k–$1.7M; full Echelon build $682k–$2.0M; hybrid $585k–$1.7M (SOC 2
scoped to all five trust services categories per owner decision 0D02).

- **Building feature parity is not cheaper than buying.**
- **The hybrid costs about the same as a platform** once real independent
  review is included; its value is covering Unbound's financial controls
  (which platforms do not), avoiding lock-in and bundled-auditor risk, and
  genuinely better assurance.
- **The hybrid's downside is large**: if the auditor will not rely on
  system-generated evidence it becomes the most expensive option
  ($785k–$2.4M). So build preventive and financial controls now (valuable in
  any case) and defer ledger and auditor tooling until a CPA firm has scoped
  their use.
- Shared controls (80 of 104) make each additional product cheap: mainly its
  own pen test, product-specific controls and a scope increment.

## 6. Penetration testing

Explicitly required only by specific regimes (PCI, NYDFS, the Safeguards Rule
with a continuous-monitoring alternative); for Unbound the driver is bank
and sponsor due diligence. Independence means organizational separation, so
it must be external. Minimum-cost strategy: heavy internal preparation, a
public vulnerability disclosure policy, then one grey-box web/API test on
tenant isolation and financial logic with a retest before launch (planning
range US$15k–35k, vendor claims) (`pentest/`).

## 7. What blocks progress (owner and adviser decisions)

The P0 open questions are cheap to answer and change the design:

| Question | Resolver | Cheapest route |
|---|---|---|
| Legal structure confirming the operating model of owner decision 0D01 (OQ-753, OQ-756) | owner + counsel | confirmation |
| Named approval roles and fractional reviewer (OQ-607, OQ-608; approval model set by owner decision 0D04) | owner | staffing decision |
| Whether sponsors' auditors expect SOC 1 (OQ-101, OQ-754) | sponsor's audit firm | one call |
| GLBA status of sponsors and Echelon (OQ-200–202) | counsel | one fixed-fee memo |
| What banks and sponsors will request (OQ-204, OQ-215) | bank / customer | ask for their questionnaire |
| Whether rulesets enforce gates today (OQ-700) | owner | look at GitHub settings |

112 questions remain open in total (14 P0), each with the reason research
cannot settle it and the lowest-cost resolution
(`registers/open-questions.csv`).

## 8. Immediate actions

1. Keep this repository to the public content classes of owner decision
   0D05 and stand up a restricted store for evidence and findings.
2. Answer the remaining P0 questions above.
3. Review and approve the 13 draft policies (now reconciled with the owner
   decisions of 2026-11-25).
4. Turn on what already exists: MFA, push protection, protected branches
   without admin bypass, Praxis verified completion.
5. Install Praxis here, migrate the register tools to F# (decision 0D06) and
   require the `assurance-validation` check.

The full phased plan is in `roadmap/implementation-roadmap.md`.

## 9. Confidence and limits

- Standards texts read in full: AT-C 105/205/320, AU-C 402/9402. TSC and
  description criteria, ISO standards, PCI DSS, Nacha rules and the FFIEC
  handbook were read only in summary or not at all; conclusions resting on
  them are capped at `auditor-interpretation`.
- Echelon systems were assessed by reading code and tests; no test suite was
  run and no deployment was observed.
- All prices are vendor claims or assumptions; quotes are needed.
- Clarity and EDF were applied by this program to its own conclusions, not by
  an independent assessor (`findings/clarity-and-edf-assessment.md`).

## 10. Owner decisions of 2026-11-25

Fourteen owner decisions (`docs/decisions/owner-approved-foundations-2026-11-25.md`,
registered as DF-ASR-2026-0D01..0D0E) set design direction and supersede the
two proposals DF-ASR-2026-3A1E and DF-ASR-2026-7B20. They are design
direction only: legal applicability, auditor acceptance, actual
configuration, staffing and operating effectiveness remain unresolved, and
the related open questions stay open with the decision noted.

## 11. Baseline approved (2026-11-25)

The owner approved the reconciled baseline, the requirement challenge review
and the capability verification matrix (DF-ASR-2026-0E01). Stage P0 is
complete; stage P1 starts by verifying actual platform settings. The draft
policies are not yet in force, and no control is above `Designed`.

