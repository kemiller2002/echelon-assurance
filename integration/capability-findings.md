# Capability verification findings (2026-11-25)

Scope: re-verification of the assurance responsibilities proposed for Echelon systems in
`echelon-assurance` (branch `claude/echelon-assurance-consolidation-jygy23`), against fresh
shallow clones in local read-only clones (all at depth 1). Read-only; builds and tests ran in
scratch copies outside the repository. Companion files: `integration/capability-verification.csv`,
work items in `roadmap/backlog.csv`; raw test logs were not committed.

Evidence discipline: "tests-executed" means a result observed in this session;
"source-inspected" means code was read or located but not executed; README claims without
code are "specified-only".

## 1. Executed validation (observed)

| System | Command | Observed result |
|---|---|---|
| Praxis | `dotnet run --project tests/Praxis.Tests` (custom runner) | 10-minute cap reached (exit 124). 1303 PASS, 0 FAIL lines before timeout. `AttestedCompletionTests` are last in the suite and were most likely not reached, so attested completion is source-inspected only. |
| Strata | `dotnet test tests/Strata.Tests` (SDK 10, roll-forward, global.json SDK-8 pin removed in copy) | 743 passed, 0 failed, 41 skipped. The skipped tests need PostgreSQL 16, which was not available. Deploy, catalog introspection and PREPARE type checking against a live database were not executed. |
| Arca | `dotnet test tests/Arca.Tests` | 415 passed / 415 |
| Tutela | `dotnet run tests/Tutela.Core.Tests`; `pytest tests` | F#: 246 of 246 checks passed. Python oracle: 101 passed, plus 113 subtests passed. |
| Dokimos | `dotnet test` on Domain, Core and Cli test projects | 4/4, 436/436 and 157/157 passed (597 in total) |

Environment notes: .NET SDK 10.0.404, with only the 10.0.15 runtime installed. Strata and Dokimos target
net8.0 and ran only with `DOTNET_ROLL_FORWARD=Major`. Praxis and Tutela test projects are
console executables, so `dotnet test` runs nothing on them and reports success. CI should therefore not
interpret a `dotnet test` exit 0 on those projects as evidence.

## 2. Contradictions with owner decisions

1. **Vigila is not a logging or observability system. Two owner records from the same day conflict.**
   - Vigila @bb6dabd is a follow-up and open-loop tracker (`PROJECT-CHARTER.md`, `src/Vigila.Semantic`).
   - It has no logging, metrics, tracing, alerting or heartbeat code or requirements.
   - `echelon-registry` research package `RP-REG-2026-0001` (2026-11-25) records an owner decision: "do not build an
     application-observability system yet". It lists "that Vigila is the observability system"
     as a failed assumption.
   - The package also finds that no repository uses OpenTelemetry, Serilog or MEL, and that no Echelon system owns
     logs, metrics, traces, health or alerting.
   - Assurance owner decision 13 (`docs/decisions/owner-approved-foundations-2026-11-25.md`)
     says the opposite.
   - **Status: absent.** The owner must reconcile the two records before CTL-LOG and heartbeat controls get an
     implementer (WI-VIGILA-01).
2. **Strata is not yet a database interaction layer.** Owner decision 12 assigns it controlled access,
   transactions, parameterization, testability and instrumentation. The code (@61f5ec9, v0.1.1 pilot) provides:
   - schema compile, diff and deploy;
   - a migration gate (`strata check`, exits 0/1/2);
   - grants, column grants and RLS drift;
   - invariants such as no grants to PUBLIC;
   - design-time query validation, including PREPARE in a rolled-back transaction;
   - transactional, parameterized reference-data deployment.

   It has **no runtime library for applications**: no typed query API, no app transaction scopes, no
   tenant/RLS session binding, and no instrumentation (no ActivitySource, Meter or OTel). The Registry
   research also states "Strata is a SQL/schema developer tool with no runtime consumers". The virtualization
   requirements (`docs/requirements/data-interaction-virtualization.md`, STRATA-DIV-001) are a
   "Proposed baseline" with no code. The earlier "schema CLI" assessment remains materially accurate.
   Decision 12 is new scope (L effort: WI-STRATA-01..04).
3. **Praxis provenance is self-reported. Only validation receipts are cryptographically attested.**
   - `docs/agent-provenance.md` says: "Everything described here is self-reported provenance … Praxis does
     not defend against a malicious process that lies about itself". Signed events and execution receipts
     are deferred.
   - What is implemented (source-inspected): verifier-signed ECDSA P-256 DSSE receipts
     (`src/Praxis.Infrastructure/Work/TrustedVerifier.fs`), and protected-policy comparison
     against the merge base of the protected branch, which refuses policy weakening
     (`docs/verified-completion-rollout.md` REL-GATE-003/004).
   - Mandatory verified completion is **not enabled** across Echelon repositories (line 3 of the same doc).
   - Owner decisions 7 and 8 (authenticated provenance, signed execution records) are therefore
     unsupported today (WI-PRAXIS-01..03).
4. **Dokimos independence is logical, not operational.**
   - `dokimos completion verify` treats the packet as untrusted. It binds claims to host-supplied authority
     (commit, fencing generation, digests), refuses replay and verifies signed receipts against pinned keys.
     The tests passed (597).
   - Dokimos does not sign its own verification receipts: there is no `SignData` in `src`.
   - It runs in the same CI trust domain as the implementer.
   - Whether its gate blocks a merge depends on GitHub required checks, and that is not evidenced in-repo.
5. **Arca does not provide immutability in the audit sense.**
   - What exists (tests executed, 415 pass): SHA-256 content hashes, `ImmutableChanged`/`HashMismatch`
     detection, refusal to write immutable records, export and erasure.
   - What is missing: a hash chain, anchoring, signatures, WORM storage and retention/legal hold.
   - Storage is GitHub only. Owner decision 10 says GitHub is not the sole production evidence store.
   - **Status: partial.**
6. **Folio has no evidence-package capability.** It is JavaScript print components and CSS. Its src and
   conformance tests contain no digest, manifest or reproducibility binding. Decision 11 (render only)
   fits, but an evidence-package profile must be built (WI-FOLIO-01).
7. **Conditor's signed-attestation path is unused.** `trust/registry-release-keys.json` has
   `"keys": []` ("Empty until the Echelon Registry signs release attestations"). Signature
   verification is implemented. Release signing by the Registry is specified only.

## 3. Responsibilities that are supported (with qualifications)

- **Tutela.** The posture gate, the exception separation of duties and a secret scan are implemented in F#
  (`src/Tutela.Core/Exceptions.fs`, which rejects a self-approval, a non-human approver or a missing
  security-owner role; `tutela scan-secrets`). The adversarial fixtures cover fabricated evidence, gate
  weakening and impersonation, and the tests passed.
  - Gap: the approver identity comes from repository-held records and is not authenticated against GitHub
    at gate time.
  - Gap: the secret scan is a regex/key catalog over files, not a history scan.
  - Gap: the Python oracle is a temporary deviation from F#-first and expires 2027-01-31.
- **Summa.** Double-entry posting with an `Unbalanced` refusal, reversal-based corrections and period
  close (`src/Summa.Ledger/Ledger.fs`, `Corrections.fs`, `Periods.fs`). This is a good pattern source, but
  it is not a shared ledger service or a DAF fund ledger.
- **Limen.** The binding-security model includes no HTML-string sinks, strict CSP plus Trusted Types,
  and URL scheme normalisation (`docs/29-binding-security.md`, `test/binding-security.test.ts`).
  It is TypeScript, an accepted browser-boundary exception.
- **Forma.** `src/workflow/Forma.Workflow/Markup.fs` is a typed HTML tree that escapes all text and
  attributes, with "deliberately no raw HTML case". The guarantee covers only Forma.Workflow. Consuming
  apps can still use raw sinks, and no analyzer prevents that.
- **Ordo.** A typed role and capability matrix with prohibitions (RecordVerdict, AcceptReview). Actors are
  host-supplied and not authenticated.
- **Aegis.** Fault capture, redaction before sinks and health derived from sink outcomes. It is
  self-declared as not a logging, metrics or tracing system, and has no trace, span or tenant fields.
  Source is at 2.0.0 (unreleased); consumers use 1.0.0.
- **Fides.**
  - Implemented: GitHub App OAuth with PKCE, a stateless token exchange on AWS Lambda, Secrets Manager.
  - Its own readiness report counts 44 requirements implemented and verified, 16 unverified, and 27 P0
    not yet verified.
  - Missing: a tenant model and authorization/roles. Organization and tenant isolation is largely
    specified only.
  - It provides no workload or agent identity, so it cannot yet back owner decision 8.
- **Echelon Registry.** A system catalog (32 systems) and release manifests with digests. It is a JSON
  spec repository with .fsx scripts and no compiled code. It is **not a work-item system**: work items live
  per repository in Praxis. Many systems have empty `provides`. echelon-assurance, Specula and Vitium are
  not registered.
- **Percepta.** A digest-bound evidence envelope (`src/Percepta.Core/EvidenceEnvelope.fs`). It is
  unsigned.
- **EKS.** A research-phase knowledge compiler with evidence-source and staleness rules. It is not a
  store.
- **echelon-organization-administration** (private; capability-level only). Prose documents: entity
  administration, one security policy, and a roles and authority section. It has a Node inventory script,
  no CI, and no machine-readable role or policy register.

## 4. Other relevant systems discovered

- **Chrona.** Audit, attestation and review records (`src/Chrona.Domain/AttestationRecord.fs`,
  `AuditRecord.fs`). It is the planned owner of canonical time primitives, which the Registry lists as
  "planned, not implemented". There is **no scheduler** anywhere in the portfolio.
- **Specula.** An independent, read-only SDLC observer with SHA-bound check display. It is useful for
  change-management monitoring. It has no retained evidence and is unregistered.
- **Project Administration.** Append-only installation events with actor and digest provenance. This is
  an asset/installation inventory candidate.
- **Causa.** Causal investigation, a candidate for incident root-cause records. Its evidence ingestion
  was not re-verified.
- **Signal and Tekmerion.** Pattern sources only: audit and retention records, and research provenance.
- **Iter** (routing only), **Mercatus** (domain model only) and **Census** (requirements and SQL only)
  provide no assurance capability.
- **Identity and policy evaluation.** No dedicated system exists beyond Fides (authentication only),
  Tutela (posture rules) and Ordo (role model). There is no general policy engine.

## 5. What changed since the prior assessments (`integration/system-register.csv`)

The clones are depth 1, so commit-level diffs against the prior SHAs are not possible. Observed changes:

- **Repository heads moved for nearly every system.** For example:
  - arca 8cfa064 to 0004c7d (now v0.7.1)
  - praxis 12bf995 to f83c251 (3.25.0)
  - strata c6a384e to 61f5ec9 (still v0.1.1)
  - dokimos 6ea8624 to 30dc1a7
  - tutela 476dc68 to 0634875
  - vigila feab755 to bb6dabd
  - fides f9b7291 to 762fff0
  - conditor cadd5c7 to 219b873 (still v0.10.29)

  In about 25 repositories the latest commit is the same portfolio-wide merge, "Upgrade to echelon-current 1.42.0
  (WI-0117)" (branch `claude/vigila-implementation-jdeps1`). It is a governance/toolchain upgrade, not a
  capability change. No new runtime capability for Strata, Vigila or Arca was found.
- **Folio and Forma are now first-hand.** They were secondhand in the prior register.
  - Folio is JavaScript print components with no evidence features.
  - Forma (1.1.0) includes the F# Forma.Workflow safe-markup primitive.
- **New evidence:** `echelon-registry` RP-REG-2026-0001 (2026-11-25), the observability gap analysis and
  owner deferral that contradicts decision 13.
- **Prior statements re-confirmed:**
  - Strata is a schema tool.
  - Praxis identity is self-reported.
  - The Registry release key set is empty.
  - Tutela's Python oracle expires 2027-01-31.
  - Arca has no hash chain.
- **Strengthened:** Dokimos completion verification is now confirmed by executed tests. The prior
  assessment called it a quality gate only.
- **Prior register test counts were not re-executed** for Fides, Summa, Chrona and the other systems not
  run here. Those rows say "source count, not executed".

## 6. Owner-assumed responsibilities that are unsupported today

| Responsibility | System | Verdict |
|---|---|---|
| Logging, observability, independent heartbeat | Vigila | Absent; conflicting owner records |
| DB interaction layer (runtime access, transactions, parameterization, instrumentation) | Strata | Absent at runtime; schema governance only |
| Authenticated provenance / signed execution records | Praxis | Specified-only (deferred); receipts only |
| Independently operated verification | Dokimos | Logic implemented; operational independence and signing absent |
| Immutable evidence storage, retention | Arca | Hashes only; no WORM, chain or retention |
| Evidence-package rendering | Folio | Renderer exists; package profile absent |
| Signed release provenance | Conditor/Registry | Verification exists; no signing keys |
| Work items in Registry | Echelon Registry | Not applicable: Praxis owns work items |
| Machine-readable roles and policies | EOA | Prose only |

## 7. Language and F#-first conformance

- **F#:** Praxis, Dokimos, Strata, Arca, Tutela (gate), Fides, Summa, Ordo, Aegis, Conditor, Vigila,
  Chrona, Specula, Percepta and EKS.
- **Deviations:**
  - Tutela's Python oracle (time-boxed).
  - Limen is TypeScript (browser boundary, accepted).
  - Folio is JavaScript (browser print components). Any evidence logic added there should be F# or kept
    out of Folio.
  - echelon-organization-administration uses Node scripts.
  - Strata and Dokimos still target net8.0 with an SDK-8 `global.json`, while Praxis, Tutela and Arca target
    net10.0. On a host with only the .NET 10 runtime, Strata and Dokimos need roll-forward to run tests.
    That is a toolchain consistency finding.

## 8. Security notes

- Do not present Praxis actor fields to auditors as authenticated identity.
- Tutela exception approvals and role registries are repository-writable unless protected by
  CODEOWNERS and rulesets.
- Branch protection drift is not continuously verified.
- With no observability system, detection controls (CC7.x) have no implementer.
- Evidence in GitHub can be rewritten by repository administrators without an external anchor.
