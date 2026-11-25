---
id: DF-ASR-2026-C005
title: Staged assurance investment: minimum viable assurance now; evidence-collection automation gated on auditor reliance and scale
status: proposed
decided_by: pending owner
date: 2026-11-25
related_questions: [OQ-802, OQ-312, OQ-108]
---

# DF-ASR-2026-C005: Staged assurance investment: minimum viable assurance now; evidence-collection automation gated on auditor reliance and scale

## Context
The economics model v2 (`economics/v2/generated/model.md`) was corrected over
two challenge rounds and now includes the recommended package as alternative A8.

- **One or two products:** the alternatives are close within wide ranges. No internal option is cheaper on cash.
- **Reliance and three or more products:** targeted automation (A5) is cheapest only when the auditor relies on system evidence and there are three to five or more products.
- **Non-reliance:** outsourced management (A3) is cheapest at every scale and uses the fewest owner hours. Minimum viable assurance (A4) is within a few percent of A3.
- **Prevention and testing automation** adds about USD 50–70k a year (A8 − A4). Its benefit is not monetized.
- **Owner capacity is likely binding** for every internal option.

## Decision (proposed)
1. **Now:** minimum viable Echelon assurance (A4): registers, validator, enforced release gates, evidence envelope, generated customer answers.
2. **Prevention and testing automation** (security-by-construction rules, financial invariant suites, continuous security testing) is recommended for risk reduction, not cost savings. The owner classifies it as baseline product engineering or as a discretionary assurance investment of about USD 50–70k a year (ODP-09).
3. **Evidence-collection automation** proceeds only after a written auditor position on reliance (WI-EXT-01, a no-cost inquiry in P2) and a credible path to three or more products.
4. **Capacity trigger:** when recurring assurance labor exceeds an owner-set threshold (proposed 25% of owner capacity), contract A3-style support or a fractional reviewer, or hire.
5. **Never without demonstrated demand:** a feature-parity internal platform (A7), custom anchoring, OSCAL or AI certifications.

## Consequences
All figures are hypothetical or vendor claims; no quotes exist. No purchase or engagement is authorized by this record.

Proposed by the assurance consolidation (journal `JR-ASR-2026-C001`). Not
authorized until the owner accepts it; see `docs/decisions/owner-decision-package-2026-11-25.md`.
