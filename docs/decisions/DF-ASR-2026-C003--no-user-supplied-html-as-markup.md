---
id: DF-ASR-2026-C003
title: No user-supplied HTML accepted as markup
status: accepted
decided_by: owner — accepted with the baseline (DF-ASR-2026-0D10, ODP-01)
date: 2026-11-25
related_questions: []
---

# DF-ASR-2026-C003: No user-supplied HTML accepted as markup

## Context
SBC-002 prohibited only untrusted raw HTML rendering; the owner's direction (reconciliation finding 2 and the mission) is that no user-supplied HTML is accepted as markup.

## Decision (proposed)
BR-APP-002: user and external content is plain text encoded by context; rich formatting, if needed, uses a structured non-HTML representation rendered by approved Forma components; raw markup sinks fail the build.

## Consequences
Eliminates the sanitizer class rather than maintaining one; may constrain future rich-text features.

Proposed by the assurance consolidation (journal `JR-ASR-2026-C001`). Not
authorized until the owner accepts it; see `docs/decisions/owner-decision-package-2026-11-25.md`.
