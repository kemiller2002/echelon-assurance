---
id: ASR-INT-ECH2-001
title: Echelon system assurance-capability assessment (research fragment ECH2)
status: draft
version: 0.1.0
created: 2026-11-25
updated: 2026-11-25
sources: [SRC-ECH2-001..SRC-ECH2-023]
open_questions: [OQ-750..OQ-766]
---

# Echelon system assurance-capability assessment (ECH2)

Fragment for the coordinator. Machine-readable summary:
`integration/fragments/ECH2-system-register.csv`. Sources: `sources/fragments/ECH2.csv`.

## 0. Method and evidence discipline

- Every repository was read from a shallow single-commit clone under
  `/tmp/.../scratchpad/ref/<repo>` (commit and date in the system register).
  Unbound and daf-landscape were read from `/home/user/{unbound,daf-landscape}`.
- For each claim three levels are kept apart:
  **documented intent** (requirements/design docs), **implemented** (code
  that exists under `src/`, read by this agent), **tested** (test files that
  exist and name the behaviour). **No test suite was executed by this
  agent**, so "tested" means "a test exists that the repository says covers
  it", never "observed passing".
- Repository self-assessments (status tables, readiness reports, gap
  analyses) are management assertions by Echelon-controlled agents. Per
  `AGENTS.md` rule 4 they are not independent evidence.
- Several `context/CURRENT-STATE.md` files are stale and contradict the code
  (Arca, Signal, Vigila; Summa's README). Code was preferred over context
  files when they disagreed; the disagreement is itself a finding (§3.6).
- Weakness detail that a repository records about itself is summarised at
  design level only (per `SECURITY-AND-CONFIDENTIALITY.md`); the owning
  repository holds the detail.

Evidence classes are attached to conclusions in square brackets.

## 1. Summary of findings

| # | Finding | Class |
|---|---|---|
| F1 | **No Echelon system provides tamper-evidence beyond per-record content hashes.** No hash chain, signature over evidence, external anchor (transparency log, RFC 3161 timestamp, WORM storage) exists in any repository searched. Arca explicitly defers a hash chain (`ARCA-INT-003`), and Signal's own security review records that a rolled-back but validly sealed record is undetected without an external anchor. This is a genuine gap for an evidence store. | recommendation (gap is observed; remedy is recommendation) |
| F2 | **GitHub is the de facto system of record, identity provider and authorization enforcement point** for Arca, Summa, Chrona, Signal, Vigila, Causa and Aegis' reference store. Repository permissions, branch protection, org-owner powers to rewrite history and GitHub's audit log therefore become in-scope infrastructure (subservice organisation) for any SOC report covering these apps. | recommendation |
| F3 | **Unbound's requirements assume capabilities that two Echelon systems do not have.** Strata is a PostgreSQL schema-management CLI, not an append-only persistence/integrity API (contradicts `UNB-TEC-STRATA-001..003`, `UNB-ACC-026`, CCM architecture "Integrity and security"; Fides' readiness report independently notes the same). Folio is a print/document renderer; nothing seen shows evidence packaging, manifests, signing or chain of custody (contradicts `SOC2-009`, `MON-015`). | recommendation (contradiction observed in sources) |
| F4 | **Strongest reusable assurance assets:** Tutela's derived-posture gate with verified, non-self, role-checked, expiring exceptions and adversarial fixtures (fabricated evidence, wrong-commit evidence, producer impersonation); Conditor's signed-attestation and digest-pinned supply chain plus branch-protection/CI policy; Arca's integrity primitives and actor trailers; Aegis redaction; Percepta's evidence envelope (input digests bound to source revision); Signal's PII-free closed-vocabulary audit records and honest deletion states; Summa's double-entry ledger, period close blockers, reconciliation and AES-GCM sealed verified backups. | recommendation |
| F5 | **No system covers identity lifecycle or monitoring.** Nothing implements provisioning/deprovisioning workflows, periodic access review, MFA enforcement evidence (Fides delegates to GitHub), centralised log aggregation, alerting, SIEM or on-call. These are the bulk of SOC 2 CC6/CC7 evidence. | recommendation |
| F6 | **Maturity is uneven and mostly pre-production.** Only Conditor carries a release tag; Fides is "fit for a single-application pilot"; Signal's own verdict is NOT ACCEPTED; Chrona and Summa are not deployed; Unbound and Census are requirements-only. No system has operating evidence in a real environment that this agent observed. | assumption (deployment state not verified live) |

## 2. Per-system assessment

Format per system: Purpose · Implementation evidence · Capabilities
(intent / implemented / tested) · Gaps · Trust boundary · Failure modes ·
Evidence produced · Controls it can enforce · Proposed enhancements ·
Overlaps.

### 2.1 Aegis (SYS-AEGIS) — SRC-ECH2-001

- **Purpose.** Typed operational-fault handling for .NET/F# (capture,
  classification, redaction, recovery proposals, escalation, sinks).
- **Implementation evidence.** `src/Aegis.Core/*.fs` (20 files incl.
  `Redaction.fs`, `Sinks.fs`, `Store.fs`, `Escalation.fs`),
  `src/Aegis.Store.GitHub/GitHubStore.fs`, `tests/Aegis.Core.Tests` (16
  files), `docs/aegis/REQUIREMENTS-STATUS.md` (137 done / 4 partial / 2 not
  started of 143).
- **Capabilities.** Redaction before any sink, secret keys dropped and
  sensitive keys masked with an observable outcome — *implemented*
  (`Redaction.fs`), *tested* per status table (`BoundaryTests.fs`).
  Immutable one-file-per-event GitHub store that refuses overwrite —
  *implemented*, *tested* (`GitHubStoreTests.fs`). Required-sink failure
  is a reported outcome, not silent — *implemented* (docs/aegis/README.md
  "Explicit failure semantics"). Environment, deployment and revision
  correlation on fault events — *implemented*. Retention — *intent only*
  (`AEG-ADD-052` partial).
- **Gaps.** Scope is faults, not security/audit events; no hash chain or
  signature; store integrity = GitHub permissions; no browser sink; DB
  adapters out of scope; consumers pin 1.0.0 while source is 2.x
  (secondhand via Fides report).
- **Trust boundary.** In-process; trust ends at the sink.
- **Failure modes.** Sink unavailable → reported (`RequiredSinkFailed`);
  detached capture in short-lived processes handled since 2.0.2.
- **Evidence produced.** Fault events (fingerprint, scope, breadcrumbs,
  environment, revision).
- **Controls it can enforce.** Log-content hygiene (no secrets in fault
  telemetry) — preventive at L4 within apps that use it; supports incident
  detection inputs (detective).
- **Proposed enhancements.** Security-event profile (authn/authz
  decisions) reusing redaction; evidence-store sink with hash chaining
  (shared with the evidence ledger, F1); retention enforcement in an
  adapter. Record in `integration/proposed-changes.md`.
- **Overlaps.** Signal `Audit.fs` and Chrona `AuditRecord.fs` implement
  separate audit-record shapes; Unbound SOC2-003 logging hygiene.

### 2.2 Vigila (SYS-VIGILA) — SRC-ECH2-002

- **Purpose.** Follow-ups/open loops tracker, per-user GitHub data repo.
- **Implementation evidence.** `src/Vigila.{Semantic,Transition,Host.GitHub,Application,Wasm,Cli}`; 19 test files; `docs/requirements/SECURITY.md` (VIG-SEC-001..013). CURRENT-STATE still bootstrap boilerplate.
- **Capabilities.** Token containment and validate-before-trust are
  *intent* with partial *implementation*; item history *implemented*.
- **Gaps.** No control-task semantics (owner, independent reviewer, SLA
  escalation, evidence attachment, attestation).
- **Trust boundary.** User's GitHub repo and token.
- **Evidence produced.** Item history.
- **Controls it can enforce.** At most L1 (reminders) for control tasks.
- **Proposed enhancements.** None recommended for assurance until a
  control-task use case is decided; Unbound's control-operations model
  (OPS-004/017, LIFE-T08) is the requirement source if adopted.
  [recommendation]
- **Overlaps.** Summa/Praxis work items; Unbound deficiency cases (MON-007).

### 2.3 Fides (SYS-FIDES) — SRC-ECH2-003

- **Purpose.** Authentication/SSO boundary (GitHub App, stateless
  exchange, PKCE, WASM client, Arca token bridge). Authentication only
  (DF-FIDES-2026-0010); organisation membership not in Fides
  (DF-FIDES-2026-0011).
- **Implementation evidence.** `src/Fides/{Exchange,Pkce,Secrets,Provider}.fs`, `src/Fides.Hosting.Aws/SecretsManagerPort.fs`, `src/Fides.Client`, `tests/Fides.Tests` (incl. `SecretsTests.fs`, `ExchangeSecurityTests.fs`, `TrustBoundaryModelTests.fs`, acceptance scenarios vs a GitHub simulator), `docs/requirements/READINESS-AND-GAP-REPORT.md`.
- **Capabilities.** PKCE and exchange flows *implemented and tested
  against a simulator*; client secret held only by the exchange via
  Secrets Manager *implemented*; leak scans over responses/audit lines
  *tested*; provider-qualified principal id (`github:<subject>`) *tested*
  (FID-IDN-003); service-principal audit (FID-SVC-004) *intent*.
- **Gaps (self-reported, design level).** No tenant model; known
  high-severity token-scoping issues between applications sharing an
  environment (detail in Fides' own report); provider properties verified
  only against the simulator; no IR runbooks, key-rotation rehearsal or
  deployment smoke test; tokens are browser-resident in all retention
  modes; no authorization engine; MFA is GitHub's, unevidenced.
- **Trust boundary.** Exchange (AWS Lambda) ↔ GitHub ↔ browser; Arca
  receives a token provider, never the token.
- **Failure modes.** Fail-closed sign-in on exchange failure (intent);
  self-reported inconsistencies in revocation/refresh handling.
- **Evidence produced.** Exchange audit lines and Aegis faults (intent and
  partial implementation). No provisioning/access-review evidence.
- **Deployment.** CURRENT-STATE says nothing is deployed; a Signal
  production config (`signal/deploy/pages/signal.deployment.production.json`)
  references a Fides exchange endpoint. Liveness not verified
  [open-question, OQ-752].
- **Controls it can enforce.** Authentication to Echelon apps (preventive,
  L4) once deployed; secret custody for the client secret.
- **Proposed enhancements.** Access-review export of principals and
  installations; MFA-required evidence from GitHub org settings; one App
  per isolation domain (already accepted DF-FIDES-2026-0013).
- **Overlaps.** Summa/Chrona/Signal rosters do authorization; Unbound
  UNB-SCTRL-003..005, SOC2-010.

### 2.4 Arca (SYS-ARCA) — SRC-ECH2-004

- **Purpose.** Shared data layer: canonical JSON records in GitHub repos.
- **Implementation evidence.** `src/Arca.Core/{Integrity,Commit,Erasure,Export,Credentials,Secrets,OfflineQueue}.fs`, `tests/Arca.Tests` (27 files incl. `IntegrityTests.fs`, `ErasureTests.fs`, `PersistenceSafetyTests.fs`), requirements §6 (ARCA-COMMIT) and §9 (ARCA-INT). Version 0.7.0.
- **Capabilities.** Content hash + typed integrity failures
  (`HashMismatch`, `ImmutableChanged`, identity/type mismatch) —
  *implemented, tested*. One operation = one atomic commit with actor
  kind/id, execution id, correlation id, idempotency trailers; agent never
  recorded as human — *intent ARCA-COMMIT-001..003; implementation in
  `Commit.fs`*. Credential-shaped content refused — *implemented*.
  Erasure with tombstone, separate capability — *implemented, tested*.
  Canonical export with per-object SHA-256 — *implemented*. Refuse write
  when integrity unknown (ARCA-INT-004) — *intent + implementation*.
- **Gaps.** No hash chain/anchor (F1); detection only relative to a
  previously seen hash; no at-rest encryption (DF-ARCA-2026-0002); erased
  content remains in Git history; history rewrite by repo owner defeats
  evidence; isolation = repository permissions.
- **Trust boundary.** Browser + token; GitHub repo ACL.
- **Failure modes.** Fail-closed on integrity failure; unknown outcomes
  modelled explicitly (ARCA-OUT).
- **Evidence produced.** Commits with structured trailers, content hashes,
  tombstones, exports.
- **Controls it can enforce.** Change attribution per record (preventive
  against unattributed in-app writes), detection of out-of-band edits
  where an expected hash is held (detective), erasure governance.
- **Proposed enhancements.** Append-only hash-chained namespace mode with
  periodic external anchoring; signed commits; export manifest signed and
  anchored for evidence packages. [recommendation]
- **Overlaps.** Signal `RecordIntegrity.fs` re-implements per-record body
  hashes above Arca; Unbound UNB-SCTRL-040, CCM evidence ledger (MON-003).

### 2.5 Strata (SYS-STRATA) — SRC-ECH2-005

- **Purpose.** Declarative PostgreSQL schema management with a
  deterministic deployment gate and read-only drift detection.
- **Implementation evidence.** `src/Strata.{Application,Analysis,Cli,Host.Postgres,Semantic}`, 28 test files incl. golden schemadiff and "awkward/refused" corpora; README "What Strata compares" table; attested releases with checksums. Pilot v0.1.1, PostgreSQL 16 only.
- **Capabilities.** Grants, column grants and RLS policies created and
  drift-compared — *implemented, tested*; `noGrantsToPublic` invariant;
  gate exit 0/1/2 with approval requirement; drift monitor read-only;
  explicit "not-compared/unverifiable" states — *implemented*.
- **Gaps.** Not a runtime persistence/event store (contradicts Unbound,
  F3); approval via flag lacks verified approver identity; untested against
  large migration histories; data-interaction virtualization
  (STRATA-DIV-001) proposed only.
- **Trust boundary.** Operator/CI with DB credentials.
- **Failure modes.** Unverifiable never rendered as pass (core design).
- **Evidence produced.** Plans, verdicts, drift reports, release
  attestations.
- **Controls it can enforce.** Database change gating (preventive, L4 in
  CI); privilege/RLS drift detection (detective, L3); evidence of
  configuration baseline for DB-backed systems (Census, future Unbound).
- **Proposed enhancements.** Record approver identity and reason in the
  approval artefact; emit drift results as evidence events. [recommendation]
- **Overlaps.** Census SQL baseline; Unbound UNB-TEN-005; Conditor drift
  (repo level).

### 2.6 Summa (SYS-SUMMA) — SRC-ECH2-006

- **Purpose.** Small-business accounting (customers, invoices, AR,
  payments, ledger, periods, reports) on GitHub via Arca. Echelon's own
  book-keeping candidate — not the Unbound DAF ledger.
- **Implementation evidence.** `src/Summa.Ledger/{Ledger,Periods,Payments,Corrections,Invoicing}.fs`, `src/Summa.Storage/{Reconciliation,Verification,Retention,Governance,Sensitive}.fs`, `src/Summa.Operations/Backup.fs`; 87 test files; gap analysis rows: 165 tested, 34 partial, 4 missing (enforced by `GapAnalysisTests.fs`).
- **Capabilities.** Balanced double-entry, trial balance, P&L/balance
  sheet — *tested*; period close blockers, adjusting/closing entries —
  *implemented*; reconciliation findings incl. ledger-vs-index agreement —
  *implemented*; history audit flagging changes to immutable records —
  *implemented*; AES-256-GCM sealed backups verified before trust, keys
  from operator key store — *implemented*; actor provenance (agent never
  as human, INV-PROV-001) — *intent + implementation*; authentication
  separate from authorization, roster-based — *tested* (`AccessTests`).
- **Gaps.** Integrity inherits F1; SoD between preparer and approver of
  journals not verified by this agent; not deployed; README stale.
- **Trust boundary.** Browser, GitHub repo ACL, Summa roster; backup keys
  external.
- **Evidence produced.** Journals, invoices, payments with provenance;
  reconciliation and verification diagnostics; sealed backups.
- **Controls it can enforce.** For Echelon's own finance (SOC 1 is not
  implied for Echelon itself): balanced posting (preventive), period close
  checks, backup integrity. Pattern source for Unbound ACC/CLS controls.
- **Proposed enhancements.** Explicit maker/checker on manual journals;
  period lock enforced on every write path; reconciliation sign-off record
  by an independent role. [recommendation]
- **Overlaps.** Unbound UNB-ACC-002/003/017/018, UNB-SCTRL-025/027/028;
  daf-landscape financial-core spec invariants.

### 2.7 Echelon Signal (SYS-SIGNAL) — SRC-ECH2-007

- **Purpose.** Survey/assessment engine with anonymous submissions and an
  administrator flow storing datasets via Arca.
- **Implementation evidence.** `src/Echelon.Signal.Admin/{RecordIntegrity,Audit,Retention,Access,Credential}.fs`; 136 test files; `docs/modernization/SECURITY-REVIEW.md` and `FINAL-REPORT.md` (2026-11-08: NOT ACCEPTED, 76/204 independently verified).
- **Capabilities.** Dependency/secret scans, CSP, least-privilege CI
  permissions — *performed and recorded once (2026-11-08)*; per-record
  body hash verified on every read — *implemented* (fix pending
  re-verification per Signal's review); PII-free closed-vocabulary audit
  records — *implemented*; retention lifecycle that refuses to claim
  permanent deletion unless the provider verified it — *implemented*.
- **Gaps.** Rollback undetected without external anchor (self-reported);
  no server-side authorization; one-off scans, not a programme.
- **Evidence produced.** Audit records, retention transitions, scan
  outputs (point in time).
- **Controls it can enforce.** Privacy-safe audit logging pattern;
  honest-deletion pattern for SOC2-032.
- **Proposed enhancements.** Promote `Audit.fs` vocabulary approach and
  `Retention.fs` states into a shared library (Arca or Aegis); scheduled
  scans with retained results. [recommendation]
- **Overlaps.** Arca integrity; Chrona audit; Unbound SOC2-003/032.

### 2.8 Tekmerion (SYS-TEKMERION) — SRC-ECH2-008

- **Purpose.** Evidence-faithful research publication with provenance.
- **Implementation evidence.** `src/Tekmerion.Domain/Provenance.fs`, `src/Tekmerion.Core/Hashing.fs`, 40 test files; requirement TEK-FID-001 (no fabrication), TEK-PRV-001..003.
- **Capabilities.** Provenance per published fact — *slice-level
  implementation*; commit provenance — *target only*.
- **Gaps.** Not an evidence store; no access control or retention.
- **Controls it can enforce.** Publication-integrity for this repository's
  own research/trust-center content (no unsourced claim published).
- **Proposed enhancements.** Use for the public trust-center projection of
  `echelon-assurance` with a "claims must cite SRC/CTL ids" rule. [recommendation]
- **Overlaps.** Unbound SOC2-020/036 (claims reconciliation).

### 2.9 Chrona (SYS-CHRONA) — SRC-ECH2-009

- **Purpose.** Authority for recorded business time; publishes approved
  billable time to Summa.
- **Implementation evidence.** `src/Chrona.Domain/{AuditRecord,AttestationRecord,Access,Reconcile,Review}.fs`; 80 test files; gap analysis 53 tested / 11 partial.
- **Capabilities.** Immutable audit entries in the same commit as the
  audited change — *implemented*; immutable daily attestations —
  *implemented*; capability-based authorization per roster —
  *implemented*; reconciliation of outside edits — *implemented*.
- **Gaps.** Not deployed; integrity inherits F1; attestation is a user
  statement, not independent review.
- **Controls it can enforce.** Time-record integrity feeding Summa billing
  (relevant to Echelon revenue, not to Unbound).
- **Proposed enhancements.** None specific; reuse audit-in-same-commit
  pattern as the reference for Unbound evidence-as-byproduct (SOC2-004).
- **Overlaps.** Summa provenance; Arca commit format.

### 2.10 Conditor (SYS-CONDITOR) — SRC-ECH2-010

- **Purpose.** Establishes and verifies a governed repository: pinned
  components, lock file, scaffolds CI/release/branch protection.
- **Implementation evidence.** `src/Conditor.Core/{ReleaseAttestation,BranchProtection,CiPolicy,Drift,AcceptancePreflight,LockFile,NugetFeed}.fs`, `trust/registry-release-keys.json`, release v0.10.29.
- **Capabilities.** ECDSA P-256 Registry release-attestation verification
  against pinned, revocable keys with no unsigned fallback —
  *implemented*; SHA-256 digest pinning and `conditor verify` —
  *implemented, used* (cited by Signal review); branch-protection policy
  (required checks, enforce admins, reviews, no force push/deletion) —
  *implemented at creation*; CI-policy static check that fails closed —
  *implemented*; drift advisory — *implemented*.
- **Gaps.** No continuous verification that protection remains applied;
  registry signing-key management not reviewed; outputs not retained
  centrally as evidence.
- **Controls it can enforce.** Software supply-chain integrity (preventive
  L4), change-management baseline (branch protection), CI hygiene.
- **Proposed enhancements.** Periodic `conditor verify`/branch-protection
  read-back emitting evidence events. [recommendation]
- **Overlaps.** Unbound UNB-SCTRL-038 (unsigned release blocked), SOC2-011.

### 2.11 Causa (SYS-CAUSA) — SRC-ECH2-011

- **Purpose.** Causal investigation workspace, append-only event history.
- **Implementation evidence.** `src/Causa.Domain/{Events,Evolve,Decide}.fs`, `src/Causa.Persistence`, 117 test files; README: preview, 0/203 integrated on main.
- **Capabilities.** Append-only history — *implemented on slice branch*;
  evidence ingestion with evidence chain and redaction — *intent*
  (`docs/EVIDENCE-INGESTION-PIPELINE.md`).
- **Gaps.** Cannot add evidence items yet; not governed-accepted.
- **Controls it can enforce.** Post-incident root-cause analysis records
  (IR lessons learned, SOC2-026) once mature.
- **Overlaps.** Unbound INC controls; Tutela findings.

### 2.12 Percepta (SYS-PERCEPTA) — SRC-ECH2-012

- **Purpose.** UI semantic-contract verifier.
- **Capabilities.** Evidence envelope binding each result to input
  digests, configuration digest, source revision (inputs must match Git
  blobs) and running artifact; verify recomputes, never trusts supplied
  hashes — *specified and implemented* (`docs/PERCEPTA-EVIDENCE-CONTRACT.md`).
- **Gaps.** UI-only scope; envelope unsigned and unanchored.
- **Proposed enhancements.** Adopt the envelope shape as the template for
  automated-test evidence in the control catalog (VAL- results). [recommendation]
- **Overlaps.** Unbound SOC2 "minimum evidence envelope"; Tutela
  wrong-commit-evidence fixture.

### 2.13 Tutela (SYS-TUTELA) — SRC-ECH2-013

- **Purpose.** Security engineering discipline and release-posture gate.
- **Implementation evidence.** `src/Tutela.Core/{Gate,Policy,Rules,Verification,SensitiveData}.fs`, `tests/fixtures/adversarial/*` (8 fixtures), parity corpus, `assessments/`, `security/ROLE-REGISTRY.json`.
- **Capabilities.** Derived posture (PASS/CONDITIONAL/BLOCKED/INDETERMINATE)
  — *implemented*; exceptions require a verified human approver who is
  not the requester and holds `security-owner` at evaluation time; expired
  or self-approved exceptions rejected — *implemented, tested by fixtures*;
  detection of fabricated evidence, wrong-commit evidence, producer
  impersonation, secret leakage, gate weakening — *tested by fixtures*.
  Its own self-assessment posture is INDETERMINATE.
- **Gaps.** Repository-scoped; approver identity trust rests on a
  repo-held role registry; Python oracle bridge expires 2027-01-31.
- **Controls it can enforce.** Release gating on security invariants
  (preventive L4), exception governance with SoD (directly reusable for
  SOC2-033 / OPS-019), evidence-freshness and provenance checks.
- **Proposed enhancements.** Generalise the exception model to all
  control exceptions; bind role registry to Fides principals. [recommendation]
- **Overlaps.** Unbound OPS-005/019, MON-008, LIFE-T03; Conditor gates.

### 2.14 Census (SYS-CENSUS) — SRC-ECH2-014

- **Purpose.** Personal/multi-entity financial intelligence (Plaid,
  ledger, PostgreSQL/AWS). Requirements only.
- **Implementation evidence.** `database/security/001_roles_and_schemas.sql` (35 lines) and `verify.sql`; no tests.
- **Capabilities.** Function-only DB access, NOBYPASSRLS runtime roles,
  PUBLIC revocations incl. default privileges — *designed SQL baseline*.
- **Gaps.** Not implemented; per-principal tenant context explicitly not
  implemented.
- **Controls it can enforce.** Pattern for DB least privilege for Unbound
  profile A (shared DB, separate schemas).
- **Overlaps.** Strata grants/RLS management; Unbound UNB-TEN-005;
  daf-landscape financial-core spec.

### 2.15 Specula (SYS-SPECULA) — SRC-ECH2-015

Not empty: 123 files, live site. Read-only observer of public repos:
SHA-bound checks, unknown never shown as success. Useful as a change-
management **visualisation** of evidence that GitHub already holds; it
retains nothing, so it is not itself evidence. Its README's "independent"
means read-only, not auditor independence. [recommendation]

### 2.16 Unbound (SYS-UNBOUND)

See `financial/unbound-existing-assurance-inventory.md`. Requirements
only; no controls implemented.

### 2.17 Forma and Folio (SYS-FORMA, SYS-FOLIO) — secondhand only

Neither repository was cloned. **Forma**: zero-runtime CSS design system
(`@echelon-foundry/design-system`, 0.13–0.14 cited by Percepta, Specula and
Fides). No assurance function. **Folio**: print/document components and
`print.css`; Signal treats it as its only document path rendering
engine-produced data (`signal/docs/modernization/FOLIO-AUDIT.md`); Unbound
vendors 0.4.2 for design review only and records layout gaps
FOLIO-GAP-01..05 (`unbound/requirements/ui/designs/documents/FOLIO-FIT-REPORT.md`).
**Unbound's assumption that Folio produces reproducible, chain-of-custody
evidence packages (SOC2-009, MON-015, UNB-ACC-025) is unsupported by any
material seen**: packaging, manifests, signing, redaction and
reproducibility would need another component, with Folio at most the
human-readable rendering. [open-question, OQ-751]

## 3. Cross-cutting observations

### 3.1 Capability coverage against the assurance architecture

| Architecture element | Best existing source | Level | Gap |
|---|---|---|---|
| Security controls / release gating | Tutela, Conditor | implemented | org-wide coverage |
| Identity (authn) | Fides | implemented, pilot | tenancy, MFA evidence, lifecycle |
| Authorization | per-app rosters (Summa, Chrona, Signal) + GitHub ACL | implemented per app | no shared model, no access review |
| Secrets | Fides (Secrets Manager), Arca/Summa credential refusal, Aegis redaction | implemented | org secret inventory, rotation evidence |
| Logging/observability | Aegis (faults), Signal/Chrona audit records | implemented per app | aggregation, alerting, heartbeat (SOC2-027) |
| Tamper-evidence | Arca content hash, Signal bodyHash, Aegis immutable files | implemented, partial | hash chain + external anchor (F1) |
| Data access controls | Strata grants/RLS, Census SQL baseline | implemented / designed | runtime evidence |
| Trust/attestation (supply chain) | Conditor signed attestations, Strata/Conditor release provenance | implemented | central retention |
| Archival/retention | Arca erasure, Signal retention states, Summa sealed backups | implemented | legal-hold, retention schedule (owner/counsel) |
| Tenant isolation | Arca namespaces, repo-per-boundary (DF-FIDES-2026-0013) | implemented as repo ACL | DB-level isolation for Unbound |
| Evidence export | Arca export (+SHA-256), Percepta envelope | implemented | signed manifest, redaction, auditor packaging |

### 3.2 GitHub as subservice organisation

Because GitHub holds records, enforces access, authenticates users and
runs CI, a SOC 2 for any of these apps would treat GitHub as a subservice
organisation (carve-out or inclusive) and rely on its SOC report plus
complementary subservice-organisation controls. [auditor-interpretation
expected; open-question OQ-750]

### 3.3 Duplication to consolidate

Three audit-record shapes (Signal, Chrona, Arca trailers + Aegis events),
two record-hash schemes (Arca, Signal), three authorization rosters. A
single evidence-event contract (Unbound SOC2 envelope + Percepta digests +
Arca trailers) would remove the duplication. [recommendation]

### 3.4 Contradictions preserved

- Unbound mandates Strata for append-only persistence/integrity; Strata
  0.1.1 is a schema CLI (Strata README; Fides READINESS §5).
- Unbound `MASTER-CONTROL-MATRIX-GOVERNANCE.md` calls the 90 rows
  "proposed"; the CSV marks all 90 `Designed`.
- Fides CURRENT-STATE says nothing is deployed; Signal production config
  references a Fides exchange.
- Arca CURRENT-STATE says no storage behaviour exists; `src/Arca.Core`
  implements it.

### 3.5 Recommended reuse decisions (for coordinator)

1. Evidence ledger: build on Arca records + Percepta envelope; add hash
   chain and external anchoring (genuine gap → decision record required by
   AGENTS.md rule 5).
2. Exception and SoD governance: generalise Tutela's exception model.
3. Supply chain and change: Conditor + Strata gates as L4 preventive.
4. Log hygiene: Aegis redaction + Signal closed-vocabulary audit.
5. Do not assign Folio or Strata responsibilities they do not have.

### 3.6 Status-hygiene finding

Stale context files contradict code in at least four repositories. An
auditor sampling repository records would find inconsistent management
assertions. A check that context status claims match code/test inventory
would be a cheap ASR-domain control. [recommendation]
