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
three challenge rounds. Mandatory gates, maintenance, auditor non-reliance and
reviewer growth are now charged to every alternative, and the recommended
package is modelled as A8.

- **One or two products:** minimum viable assurance (A4) has the lowest or near-lowest midpoint in every scenario, with outsourced management (A3) within about 4%. The ranges overlap heavily.
- **Reliance and five or more products:** targeted automation (A5) is cheapest only when the auditor relies on system evidence and there are about five or more products.
- **Non-reliance:** A4 and A3 are effectively tied at every scale, and A5, A6 and A8 never overtake A3.
- **Prevention and testing automation** adds about USD 50–70k a year (A8 − A4). Its benefit is not monetized.
- **Owner capacity is likely binding.** The cost of contracting to relieve it is not yet modelled.

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
