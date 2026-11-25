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
The economics model v2 (`economics/v2/generated/model.md`) shows the alternatives within overlapping ranges at one product because external examinations and testing dominate cost; targeted automation (A5) is cheapest from two to three products when auditors rely on system evidence, but under auditor non-reliance it never beats a commercial platform or outsourced management within 30 products, while the hybrid (A6) remains competitive in every scenario. A full internal platform (A7) does not break even against commercial or outsourced alternatives before 5-8 products in the base case and never under non-reliance.

## Decision (proposed)
1. Immediately: minimum viable Echelon assurance (A4) — registers, validator, enforced release gates, evidence envelope, generated customer answers — plus prevention and testing automation that has value regardless of auditor reliance (security-by-construction rules, financial invariant suites, continuous security testing).
2. Gate evidence-collection automation (the part whose value depends on auditor reliance) on a written auditor position from the scoping inquiry (WI-EXT-01). If reliance is not obtainable, adopt the hybrid (A6) with a commercial platform for generic evidence.
3. Never build a feature-parity internal compliance platform (A7) without demonstrated demand.

## Consequences
All figures are hypothetical or vendor claims; no quotes exist. No purchase or engagement is authorized by this record.

Proposed by the assurance consolidation (journal `JR-ASR-2026-C001`). Not
authorized until the owner accepts it; see `docs/decisions/owner-decision-package-2026-11-25.md`.
