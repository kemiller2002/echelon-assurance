---
id: DF-ASR-2026-3A1E
title: No new Echelon system for assurance; evidence ledger extends Arca, identity and monitoring use managed services
status: proposed
decided_by: pending owner
date: 2026-11-25
---

# DF-ASR-2026-3A1E

## Context
Charter §13 forbids creating a new Echelon system until existing systems are
evaluated and a genuine gap is demonstrated. The evaluation
(`integration/echelon-integration-matrix.md`) found four gaps: tamper-evident
evidence anchoring, identity lifecycle/access review, centralized runtime
logging/alerting/heartbeat, and auditor evidence packaging.

## Decision (proposed)
1. Do not create a new Echelon system for assurance at this stage.
2. Evidence ledger = Arca records + Percepta envelope, extended with a hash
   chain, periodic checkpoints anchored externally (transparency-log witness,
   RFC 3161 timestamp) and a WORM storage target (PC-12, PC-13).
3. Identity lifecycle, MFA evidence and access review come from a managed
   identity provider plus thin F# adapters that emit evidence envelopes.
4. Runtime logging and alerting use managed cloud logging; Vigila either adds
   heartbeat/gap detection or narrows scope (PC-10).
5. Auditor packaging is an export function over the ledger (Arca export
   + signed manifest), not a Folio responsibility.
6. Exceptions and SoD: generalize Tutela (PC-07).

## Consequences
Minimal new infrastructure, consistent with Echelon principles. Risk: Arca
and Percepta maturity is pre-production; the ledger becomes critical
infrastructure and itself needs ASR controls and independent verification
(`validation/assurance-system-resilience.md`).

## Revisit if
Arca's design cannot accommodate append-only chaining without breaking its
erasure model; or auditor feedback (OQ-500, OQ-759) rejects the anchoring
approach.
