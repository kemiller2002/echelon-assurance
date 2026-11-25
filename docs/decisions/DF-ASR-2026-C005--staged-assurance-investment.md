---
id: DF-ASR-2026-C005
title: Staged assurance investment: minimum viable now, targeted automation gated on auditor reliance
status: proposed
decided_by: pending owner
date: 2026-11-25
related_questions: [OQ-802, OQ-312, OQ-108]
---

# DF-ASR-2026-C005: Staged assurance investment: minimum viable now, targeted automation gated on auditor reliance

## Context
The economics model v2 (`economics/v2/generated/model.md`) was corrected after
the challenge passes so that platform alternatives also bear maintenance and
auditor non-reliance.

- **At one or two products the alternatives are indistinguishable** within wide ranges.
- **Targeted automation (A5) has the lowest midpoint from about three to five products,** but only when the auditor relies on system-generated evidence.
- **Under non-reliance:**
  - Minimum viable assurance (A4) is cheapest or near-cheapest at every scale.
  - Outsourced management (A3) is within a few percent of A4.
  - A5 and the hybrid (A6) never overtake A3.
- **A feature-parity internal platform (A7) needs 6–9 products** to break even in the base case.
- **Owner capacity is likely binding.** Recurring assurance labor of 300–1,500 h a year at one to three products, for most alternatives, is a large share of one person.

## Decision (proposed)
1. **Immediately:** minimum viable Echelon assurance (A4). That means registers, validator, enforced release gates, evidence envelope and generated customer answers. Add the prevention and testing automation that has value regardless of auditor reliance: security-by-construction rules, financial invariant suites and continuous security testing.
2. **Evidence-collection automation is gated.** It proceeds only after a written auditor position on reliance on system evidence (WI-EXT-01, a no-cost inquiry in P2) and a credible path to three or more products. If either is missing, stay on A4.
3. **Capacity trigger.** If recurring assurance labor exceeds an owner-set threshold (proposed: 25% of owner capacity), contract outsourced support or a fractional reviewer, or hire. The choice is made on cost and independence value.
4. **Never without demonstrated demand:** a feature-parity internal platform (A7), custom anchoring, OSCAL or AI certifications.

## Consequences
All figures are hypothetical or vendor claims; no quotes exist. No purchase or engagement is authorized by this record.

Proposed by the assurance consolidation (journal `JR-ASR-2026-C001`). Not
authorized until the owner accepts it; see `docs/decisions/owner-decision-package-2026-11-25.md`.
