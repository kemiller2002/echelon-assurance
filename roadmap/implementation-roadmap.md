---
id: ASR-ROADMAP-001
title: Prioritized implementation roadmap
status: draft
version: 0.1.0
created: 2026-11-25
inputs: [controls/control-catalog.csv, roadmap/generated/controls-by-phase.md, registers/open-questions.csv, integration/proposed-changes.md, economics/compliance-economics.md]
---

# Implementation roadmap

Phases are gates, not dates. Unbound is pre-production and its financial
core is NO-GO for production financial execution; no phase below claims any
control is implemented. Per-control assignments are generated in
[`generated/controls-by-phase.md`](generated/controls-by-phase.md) from the
catalog (P0 13, P1 67, P2 13, P3 10, P4 1 controls; P5 holds improvements, not controls).

## P0 — Immediate organizational requirements

Actions (owner unless stated):

1. **Make `echelon-assurance` private** (GitHub settings). Until then, keep
   applying `SECURITY-AND-CONFIDENTIALITY.md`.
2. **Answer the P0 questions that shape everything else** (cheapest
   resolution in brackets):
   - OQ-753 / OQ-756: who Unbound's contracting customers are; whether
     sponsor and operator are separate entities [owner decision].
   - OQ-607 / OQ-608: named roles for policy approval, exception approval and
     independent review given current headcount [owner decision; consider a
     contracted part-time independent reviewer].
   - OQ-200..202: GLBA status of sponsors and of the Echelon entity [one
     scoped counsel memo; fixed-fee].
   - OQ-754 / OQ-101: whether a first sponsor's auditor expects SOC 1 [a 30-minute
     call with the sponsor's audit firm; no fee].
   - OQ-204 / OQ-215: what bank partners and prospective sponsors will
     actually request [ask for their vendor due-diligence questionnaire].
   - OQ-700: are rulesets actually requiring checks today [owner looks at
     GitHub settings].
3. **Approve the policy set** (`policies/drafts/`) after review, or record
   changes. Approval makes CTL-GOV-002 operable; until then policies are drafts.
4. **Accept or reject decision record `DF-ASR-2026-3A1E`** (no new system;
   ledger extends Arca).
5. **Install Praxis** in this repository and require the `assurance-validation`
   check on `main`.
6. Turn on the cheap preventive controls in existing GitHub/IdP settings:
   MFA (CTL-IAM-004), push protection (CTL-SDL-010), protected branches with
   non-author review and no admin bypass (CTL-SDL-001).

## P1 — Before production

The bulk of the catalog. Order of work inside P1:

1. **Identity and change foundations**: CTL-IAM-001..008, CTL-SDL-001..010,
   CTL-CFG-001/002, CTL-NET-001/002, CTL-END-001, CTL-CRY-001/002. Mostly
   configuration of managed services plus PC-03, PC-05, PC-08.
2. **Unbound financial kernel controls** (CTL-FIN-001..008, 010, 011;
   CTL-TEN-001/002): built into Unbound and passing the daf-landscape gates
   G1–G7 before any production financial execution.
3. **Evidence at source** (CTL-ASR-001, 004, 009): envelopes emitted by
   source systems, heartbeat monitoring outside the primary account. The
   anchored ledger (CTL-ASR-002) can follow in P3.
4. **AI controls** (CTL-AI-001..008): agent registry, platform-enforced
   permissions, untrusted-content isolation, kill switch, evaluation suites.
   PC-19 decides where receipts and the AI egress gateway live.
5. **Incident, vulnerability, logging**: CTL-IR-001..003, CTL-VUL-001..004,
   CTL-LOG-001..003. The **first independent pen test** (CTL-VUL-002) runs
   on the production-equivalent environment before launch, following
   `pentest/unbound-minimum-cost-testing-strategy.md`.
6. **Vendors**: CTL-VEN-001, 003, 005: vendor register, AI provider approval,
   cloud provider report review.

Exit criterion: each P1 control is at least `Implemented` with a recorded
negative test, and production-separated evidence exists (CTL-ASR-009).

## P2 — Before onboarding financial institutions

CTL-VEN-004 (bank obligations mapped), CTL-FIN-009, 012..014 (valuation,
reproducible reports, fraud triage, bank integration controls),
CTL-BCR-002/003 (exercised DR, availability monitoring), CTL-DAT-002/003
(retention set with counsel — **before** any WORM lock), CTL-CUS-001/002,
CTL-IAM-009, CTL-AI-009. Prepare the bank due-diligence pack from the
trust-center tier "Bank partner" (`trust-center/`). Re-run the TPRM
questionnaire answers through CTL-GOV-012 claim linting.

## P3 — Before beginning an audit observation period

1. **Engage the CPA firm early** for scoping: system boundaries, SOC 1
   control objectives, TSC categories, carve-out of cloud provider and
   custodians, and — critically — whether they will rely on system-generated
   populations with completeness proofs (OQ-500, OQ-601, OQ-759). Run a
   readiness assessment (economics scenario 5).
2. Controls: CTL-ASR-002/003/005/006/007/008 (anchored ledger, completeness
   reconciliation, mutation/sentinel testing, evidence store isolation,
   internal independent review, external validation of the assurance
   machinery), CTL-VEN-002 (CUEC mapping incl. GitHub), CTL-CUS-003/004
   (system description from one source, trust center), CTL-GOV-013.
3. Exit criterion: every in-scope control is `Operating` with production
   evidence and `Tested` at `internal-independent` level, and the
   observation-period start date is agreed with the auditor.
4. **Decision point**: if the auditor will not rely on Echelon-generated
   evidence, buy a platform for the SaaS/cloud evidence layer
   (economics §3).

## P4 — For external attestation

CTL-ASR-010: SOC 1 Type 2 (fund accounting objectives) and SOC 2 Type 2
(Security, Availability, Confidentiality) over the agreed period, with a
read-only auditor interface (`auditor/`). No standalone SOC 1 Type 1 (it
gives user auditors no operating-effectiveness evidence; see
`soc/unbound-soc-applicability.md`). Publish a SOC 3 only if marketing value
justifies the extra cost.

## P5 — Longer-term improvements

- Formal model checking (TLA+) of the approval and evidence-sealing state
  machines (VAL-016) after a time-boxed pilot; analysable authorization
  policy language checked in CI.
- Raise selected L3 controls to L4/L5 once false-positive rates are known.
- Processing Integrity and Privacy TSC categories if customer demand appears.
- ISO 27001 or ISO 42001 certification only if a customer or market requires
  it; the unified catalog already maps toward both.
- OSCAL export of catalog and assessment results if an auditor or customer
  consumes it.
- Comment on the AICPA proposed 2027 TSC and description criteria (comment
  deadline 2027-01-15) if Echelon wants to influence AI points of focus.
- Second product onboarding using the shared control layer (economics §4).
