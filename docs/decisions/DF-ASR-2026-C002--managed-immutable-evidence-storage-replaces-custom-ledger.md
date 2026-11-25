---
id: DF-ASR-2026-C002
title: Managed immutable evidence storage replaces custom ledger and external anchoring
status: proposed
decided_by: pending owner
date: 2026-11-25
related_questions: [OQ-505, OQ-805]
---

# DF-ASR-2026-C002: Managed immutable evidence storage replaces custom ledger and external anchoring

## Context
`CTL-ASR-002` designed a tamper-evident hash-chained ledger with external witness anchoring, RFC 3161 timestamps (`EVT-024`, `EVT-025`) and the PR #2 requirement CA-PROV-03 repeated it. Owner decision 0D0A requires interfaces before any custom ledger; the mission forbids custom transparency logs, witness networks or specialized timestamping without demonstrated requirement. Capability verification found Arca has content hashes but no chain, signing or WORM storage.

## Decision (proposed)
Production evidence uses managed object storage under a compliance-mode retention lock, applied from the first production evidence, in a separate trust domain. Separately sourced population reconciliation and an externally hosted heartbeat complete the design (BR-ASR-003, BR-ASR-006, BR-ASR-008). While one owner administers every account, these mechanisms detect failure and external compromise, not owner action; they are technical separation, not independence. Hash chaining, external anchoring, witness cosignatures and qualified timestamps are deferred until a customer, auditor or regulator requirement is recorded (WI-SCALE-02; OQ-505, OQ-507). `CTL-ASR-002` keeps its ID; its mechanism of record changes.

## Consequences
A compliance-mode retention lock at a conservative minimum period is applied from the first production evidence and is a precondition for any observation period. Until it is applied, evidence is deletable by the owner. Reduces build and maintenance; residual risk is provider-level trust and an administrator who controls the cloud account (BR-IAM-002, ODP-03). Reversible: anchoring can be added later without changing the envelope.

Proposed by the assurance consolidation (journal `JR-ASR-2026-C001`). Not
authorized until the owner accepts it; see `docs/decisions/owner-decision-package-2026-11-25.md`.
