---
id: ASR-INT-CAP-001
title: Capability verification matrix — target responsibilities versus verified capabilities
status: draft
created: 2026-11-25
inputs: [docs/decisions/owner-approved-foundations-2026-11-25.md, requirements/*.md, integration/fragments/ECH1-assessment.md, integration/fragments/ECH2-assessment.md]
---

# Capability verification matrix

Owner reconciliation finding 15: responsibilities assigned to Echelon
systems are **targets until verified**. This matrix states, per system, the
target responsibility, what the research actually observed (by reading
repository code and tests; nothing was executed or observed deployed), and
what must happen before a catalog control may name the system as its
implementer at `Implemented` or above.

Status key: **observed** = code and tests present for the capability;
**partial** = some of the capability present; **not observed** = no
implementation found; **secondhand** = repository not read, only described
by other repositories.

| System | Target responsibility (owner) | Observed capability | Status | Verification needed before reliance |
|---|---|---|---|---|
| Praxis | Repository governance, authorization of work, verified completion, provenance (0D06–0D08) | Work protocol, append-only records with digests, verifier-signed attested completion (54 test cases) — enabled in no repository; provenance self-reported | partial | Upgrade pinned versions; enable `requireVerifiedCompletion`; bind actors to authenticated identities; inspect actual rulesets (OQ-700, 704, 615) |
| Dokimos | Validation gates, independent verifier | CI gate and signed receipt verification; merge blocking depends on rulesets | partial | Required-check configuration evidence; verifier key custody separate from implementers |
| Vigila | Logging and observability initiative integrating managed infrastructure (0D0D) | Follow-up / open-loop tracker with GitHub-backed records; no aggregation, alerting or heartbeat | not observed | Inspect current scope (OQ-806); otherwise managed monitoring with an independently operated heartbeat |
| Arca | Evidence integrity primitives (0D0A) | Content hashes, actor trailers, export with SHA-256, erasure; hash chain explicitly deferred; GitHub-backed storage | partial | Verify integrity functions; managed immutable storage interface (not GitHub as sole store) |
| Strata | Database interaction layer: controlled access, transactions, parameterization, schema governance (0D0C; SBC-001/005) | Declarative PostgreSQL schema CLI with grants/RLS; no runtime data-access adapter observed | partial | Runtime typed adapter is new work; verify before CTL-APP-001 names Strata as implementer |
| Fides | Application identity (AIG integration) | GitHub-App-based authentication, pilot; deployment state contradictory | partial | Owner evidence of deployment (OQ-752); workforce identity is a separate managed IdP (GOV-BC-011, OQ-764) |
| Tutela | Exceptions and release posture | Verified, non-self, role-checked, expiring exceptions with adversarial fixtures | observed | Org-wide adoption; integration with evidence store |
| Folio | Renders evidence packages (0D0B) | Secondhand: document/print renderer | secondhand | Read repository; confirm rendering of manifests produced elsewhere |
| Forma / Limen | Safe rendering, controlled JS boundary (SBC-002/003) | Limen: TypeScript browser-kernel library (observed); Forma secondhand | partial / secondhand | Sink inventory; safe-rendering API verification |
| Conditor | Signed release attestations, digest pinning | Signed attestations, one tagged release | partial | Apply to production-bound repos; central attestation retention |
| Summa | Ledger, reconciliation and backup patterns (reference) | Double-entry ledger, period close, reconciliation, sealed verified backups; not deployed | observed (not deployed) | Pattern reuse only; Unbound kernel implements its own |
| Specula | Independent read-only observation (candidate) | Read-only observer of GitHub activity | observed | Echelon-run, so not an external witness; usable as a completeness cross-check |
| Unbound | Reference product | Requirements and designs only | not observed (by design) | All FIN/TEN controls are designs |

**Rule applied in the catalog:** `echelon_systems` names the *intended*
implementer. A control's `status` cannot move beyond `Designed` on the
strength of a named system until the verification in this matrix is done
and evidenced. The repository acceptance test enforces that no control is
above `Specified` today.
