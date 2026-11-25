---
id: DF-ASR-2026-C004
title: AI agents never authorize, approve or release movement of funds
status: proposed
decided_by: pending owner
date: 2026-11-25
related_questions: [OQ-763]
---

# DF-ASR-2026-C004: AI agents never authorize, approve or release movement of funds

## Context
The financial-integrity question disposition for OQ-763 said 'no autonomous agent movement of money outside approved scope', implying movement inside a scope. Unbound (CNV-LEG-001, CNV-AIO-001) and daf-landscape (invariant 9) give agents no money-movement authority and Unbound cannot execute payments at all.

## Decision (proposed)
BR-FIN-006: AI agents never authorize, approve or release movement of funds or material financial exceptions in any scope. Deterministic workers may act only under recorded sponsor (board) delegation. Unbound relays sponsor-authorized instructions and never executes or settles payments.

## Consequences
Tightens, not weakens, the owner direction; aligns three repositories.

Proposed by the assurance consolidation (journal `JR-ASR-2026-C001`). Not
authorized until the owner accepts it; see `docs/decisions/owner-decision-package-2026-11-25.md`.
