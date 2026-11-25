---
id: GV-ASR-METH-001
title: Research methodology
status: canonical
version: 0.1.0
created: 2026-11-25
updated: 2026-11-25
---

# Research methodology

## 1. Question discipline

Each research area starts from explicit questions (see
`context/RESEARCH-QUEUE.md`). For each conventional compliance practice we ask
the Section 21 challenge questions of the charter: what risk it addresses;
whether it reduces that risk; whether it is legally/contractually required or
customary; whether the risk can be removed architecturally; whether a
preventive control can replace a detective one; whether continuous validation
can replace periodic review; whether the transaction can emit its own
evidence; what new risks the alternative introduces; and whether an
independent auditor would accept the result — with the evidence for that last
answer recorded separately from our hope.

## 2. Evidence classes

Every significant conclusion is tagged with exactly one class.

| Class | Meaning | Example |
|---|---|---|
| `authoritative` | Text of a statute, regulation, binding standard, or attestation standard, read from the primary source | 16 CFR 314.4(c)(5) requires MFA |
| `auditor-interpretation` | Published guidance or practice of auditors/AICPA guides that is persuasive but not itself the standard | AICPA SOC 2 Guide illustrative tests |
| `industry-convention` | Common practice without binding force | Annual policy re-acknowledgement |
| `customer-expectation` | Recurring demand in questionnaires, contracts, bank TPRM | Banks request SOC 1 for financial processors |
| `vendor-claim` | Statements by vendors with commercial interest | "Continuous compliance in weeks" |
| `recommendation` | Our evidence-supported recommendation | Use transparency-log anchoring for evidence |
| `assumption` | Unverified premise we are acting on | Unbound will hold customer funds via a bank partner |
| `open-question` | Cannot be resolved internally; listed in `registers/open-questions.csv` | Whether a specific bank will accept SOC 2 in lieu of SOC 1 |

## 3. Source handling

- Prefer primary sources: AICPA, NIST, CISA, CIS, OWASP, ISO (catalog pages;
  standard text is paywalled), regulators (FTC, FFIEC, OCC/Fed/FDIC, CFPB,
  IRS, state AGs, EU), accreditation bodies (CREST, ANAB/UKAS), PCI SSC.
- Every source gets a `SRC-` row with URL, publication date, retrieval date,
  jurisdiction and limitations. Paywalled standards (ISO, some AICPA guides)
  are marked `access=paywalled` and conclusions drawn from summaries are
  capped at `auditor-interpretation` or lower unless the text was read.
- Record contradictory sources and alternative interpretations in the same
  record; do not discard them.
- Retrieval is performed by agents with web access on the date recorded. A
  later reviewer should re-verify any source older than 12 months or any
  regulatory source affected by known rulemaking.

## 4. Confidence

Praxis confidence labels (`very-low`, `low`, `medium`, `high`, `very-high`)
describe evidential support only, with a one-line rationale.

## 5. Clarity and EDF

Where a deliverable makes an organizational design claim, it is framed for a
Clarity review (problem, intended outcome, assumptions, challenges) and an
Echelon Diagnostic Framework (EDF) pass where the EDF repository's method
applies. The research program prepares inputs; it does not claim a Clarity or
EDF review was performed unless one was executed and recorded.

## 6. Review passes

The ten review passes in charter Section 22 are recorded in
`research/journals/` with findings and resulting edits. A pass that finds
nothing still records what was checked.

## 7. Professional judgment boundary

When a conclusion needs a CPA, attorney, auditor or bank to confirm, it goes to
`registers/open-questions.csv` with: the exact question, why internal research
cannot settle it, and the lowest-cost credible way to get an answer.
