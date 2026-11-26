---
id: DF-ASR-2026-C006
title: Unbound financial semantics adopted for period reopen, replay and idempotency
status: accepted
decided_by: owner — accepted with the baseline (DF-ASR-2026-0D10, ODP-01)
date: 2026-11-25
related_questions: []
---

# DF-ASR-2026-C006: Unbound financial semantics adopted for period reopen, replay and idempotency

## Context
The financial review found that FIN-BC-011 allowed approved reopening of any period, FIN-BC-021 'replay' could be read as re-executing commands and SBC-009 dead-lettered conflicting idempotency payloads, each weaker or less precise than Unbound's requirements.

## Decision (proposed)
BR-FIN-010 (no reopening after statements issue), BR-FIN-016 (replay rebuilds projections only; never posts or dispatches), BR-FIN-004 (payload conflict opens an owned case; versioned hold and sanctions fences at dispatch) adopt the Unbound semantics.

## Consequences
Strengthens financial integrity; no cost increase expected.

Proposed by the assurance consolidation (journal `JR-ASR-2026-C001`). Not
authorized until the owner accepts it; see `docs/decisions/owner-decision-package-2026-11-25.md`.

*Accepted in principle with the baseline (owner record DF-ASR-2026-0D10, ODP-01), subject to traceability and validation; read with owner direction DF-ASR-2026-0D0F. The sentence above is historical (note added 2026-11-26).*
