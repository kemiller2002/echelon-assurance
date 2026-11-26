---
id: DF-ASR-2026-C004
title: AI agents never authorize, approve or release movement of funds
status: accepted
decided_by: owner — accepted (DF-ASR-2026-0D10, ODP-10)
date: 2026-11-25
related_questions: [OQ-763]
---

# DF-ASR-2026-C004: AI agents never authorize, approve or release movement of funds

## Context
The financial-integrity question disposition for OQ-763 said 'no autonomous agent movement of money outside approved scope', implying movement inside a scope. Unbound's CNV-AIO-001 limits agents to reversible A2 actions in release 1. Its general rule lets agents act up to the recorded authority of their principal, so the prohibition is not permanent there. daf-landscape (invariant 9) gives agents no money-movement authority. Unbound cannot execute payments at all (CNV-LEG-001).

## Decision
BR-FIN-006: AI agents never authorize, approve or release movement of funds or material financial exceptions in any scope. Deterministic workers may act only under recorded sponsor (board) delegation. Unbound relays sponsor-authorized instructions and never executes or settles payments.

## Consequences
Tightens, not weakens, the owner direction. Making the prohibition permanent in Unbound is proposed through WI-UNB-08; no other repository is changed by this record. Agents in financial domains are capped at A2 and may not prepare or alter payment instructions (BR-AI-001, BR-FIN-006).

Proposed by the assurance consolidation (journal `JR-ASR-2026-C001`). Not
authorized until the owner accepts it; see `docs/decisions/owner-decision-package-2026-11-25.md`.

*Accepted: owner record DF-ASR-2026-0D10 (`docs/decisions/owner-remaining-approvals-2026-11-25.md`), with owner direction DF-ASR-2026-0D0F; the sentence above is historical (note added 2026-11-26).*
