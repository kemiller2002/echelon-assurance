# ECH1 - Echelon systems assessment for the assurance architecture

Agent key ECH1, 2026-11-25. Read-only review of shallow clones (HEAD commits in
`ECH1-system-register.csv`). Nothing was built, run or executed except `grep`,
`find`, `git log` and reading files. "Implemented" = code present; "tested" =
test cases present in the repository (counted, **not run**); "documented" =
docs/specs only. Sources: `sources/fragments/ECH1.csv` (SRC-ECH1-001..026).
Questions: OQ-700..OQ-710. Control proposals: `registers/fragments/ECH1-control-candidates.md` (CC-1..CC-11).

## Headline findings

1. **Praxis and Dokimos already implement the hard part of an evidence-integrity
   design** - DSSE ECDSA-P256 verifier-signed test receipts, pinned trust root
   on the protected branch, fail-closed completion, replay and freshness
   checks (implemented and tested: `praxis/tests/Praxis.Tests/AttestedCompletionTests.fs`, 54 cases;
   `dokimos/tests/*/CompletionAttestation*Tests.fs`). [authoritative (internal code); SRC-ECH1-003, -005, -012]
2. **None of it is switched on.** No assessed repository sets
   `requireVerifiedCompletion`/`requireAttestedEvidence`; assessed consumers pin
   Praxis `rosVersion` 1.0.0-3.7.1, earlier than the attested-completion work
   (Praxis is at 3.25.0). `praxis/docs/verified-completion-rollout.md` says
   verified completion "is not enabled across Echelon repositories". [authoritative (internal); OQ-704]
3. **Gates do not by themselves block merges.** Dokimos fails the CI job; Praxis
   validation fails CI; Praxis autonomy evaluates merges fail-closed. Whether a
   failed check *blocks a merge* depends on GitHub rulesets, which repository
   contents cannot show. Praxis's own diagnosis (dated 2026-10-28) describes the
   merge gate as self-governance of a cooperating agent until owner decision
   D-2 makes it mandatory. Current state: **not evidenced**. [open-question; OQ-700; SRC-ECH1-007]
4. **Provenance is self-reported, not authenticated** (Praxis says so explicitly).
   Events/executions are committed append-only JSONL/JSON with content digests
   and content-derived `eventId`s that `validate` checks, but they are not
   signed and (except autonomy merge receipts) not hash-chained. [authoritative (internal); OQ-702, OQ-705]
5. **Segregation of duties exists as a typed model** (Ordo role authority matrix;
   EEF R2) and as signed scoped approvals (Praxis ECIR, owner-signed run
   authorization). Binding roles to authenticated identities, and the
   single-owner question, are unresolved. [recommendation/open-question; OQ-701, OQ-706]
6. **No assessed system provides production runtime logging/monitoring, an
   auditor export package, or WORM evidence storage.** These are genuine gaps
   (reuse-before-build: record before proposing a new system). [recommendation]

## Per-system assessment

### SYS-PRAXIS - Praxis (repository operating system)

- **Verified purpose:** F# CLI installed per repository: work protocol, canonical
  records + registries, telemetry, agent provenance, verified completion,
  ECIR approvals, autonomous-merge evaluator. (`praxis/README.md`, `praxis/SDE-MAP.md`)
- **Implementation evidence:** 414 `.fs` in `src/`, 196 test files;
  `src/Praxis.Domain/Work/VerificationReceipt.fs` (DSSE PAE, ECDSA P-256, keyId = SHA-256 of SPKI);
  `src/Praxis.Infrastructure/Execution/ExecutionStore.fs` and `.../Autonomy/AutonomyStore.fs`
  (`File.AppendAllText` ledgers; merge receipts content-addressed and chained via `previous` digest);
  `.github/workflows/praxis-validation.yml` runs real Dokimos 0.7.0 (pinned by SHA-256) for attested tests;
  `native-release.yml` uses `actions/attest-build-provenance`.
- **Assurance-relevant capabilities:**
  - Actor provenance on executions, events, front matter (`provenance.contributions`,
    keyed by execution; refuses re-attribution; `provenance.enforce: true` in greenfield `ros.json`). *Implemented, tested* (`ProvenanceTests.fs`, `ProvenanceCliTests.fs`).
  - Attested completion with protected-policy comparison against the merge base
    (`PRX.COMPLETION.POLICY_CHANGED`, `UNTRUSTED_AUTHORITY`, `ATTESTATION_REQUIRED`). *Implemented, tested; not enabled.*
  - ECIR signed exact-scope approval receipts (24 h max, host allow-list, revocation by removal). *Implemented as library, tested (`EcirApprovalTests.fs`, 9 cases); no CLI route; ECIR groups still refused* (`docs/ecir-execution.md`).
  - Local ECIR host authority: revision-linked policy+audit document, mode 0600, atomic rename; doc states no rollback resistance against a privileged operator. *Library adapter.*
  - Autonomous merge: owner-signed `praxis.run-authorization/1`, check runs accepted only from the GitHub Actions app at the candidate SHA, protected paths `REQUIRES_OWNER`, deterministic independent-review check "never represented as a human review". *Implemented; enforcement depends on rulesets.*
  - `validate`: front matter, duplicate/invalid IDs, broken refs, supersession reciprocity, stale registries, reconciliation-event integrity.
- **Gaps:** self-reported identity; unsigned event logs; protected source read from local refs (doc-stated residual: a process able to rewrite `refs/remotes/origin/*` controls it); residual risks R-2 (spoofable check suites without required workflow), R-3 (gutted tests pass), R-6 (platform attribution) in `docs/autonomous-execution/SECURITY-REVIEW.md`; no auditor-facing export (Folio reports "conditional").
- **Trust boundary:** runs as the agent; all integrity rests on keys and policy outside agent-writable paths plus platform branch protection.
- **Failure modes:** opt-in flags left off (silent advisory mode); version drift (controls claimed that the pinned version lacks); key held where agent can read it; merges bypassing Praxis.
- **Evidence it could produce:** per-change execution records (who/what/model/run), completion records labelled `attested`/`unattested` with `verifierKeyIds`, chained merge receipts, signed approvals, validation logs.
- **Controls it could enforce:** CC-2, CC-4 (with Ordo), CC-6, CC-10; feeds CC-1, CC-3, CC-5.
- **Proposed enhancements (proposals only, record in `integration/proposed-changes.md`):** signed or hash-chained `events.jsonl`; record platform-authenticated identity (OIDC/App) in execution records; an evidence-export command producing a period-bounded, digest-manifested package for auditors; doctor finding when a repo claims verified completion but pins a pre-attestation version.

### SYS-DOKIMOS - Dokimos

- **Verified purpose:** code-quality measurement with longitudinal evidence; CI gate; independent verifier of Praxis completion claims.
- **Implementation evidence:** `actions/quality-gate/{action.yml,gate.sh,install.sh}`; `src/Dokimos.Core/CompletionWire.fs`; tests `CompletionVerificationTests.fs`, `CompletionAttestationTests.fs`, `EvidenceStoreTests.fs`, `QualityGateProofTests.fs`; `.github/workflows/ci.yml` dogfoods the gate; `release.yml` attests build provenance.
- **Capabilities:** pinned install verified by SHA-256; snapshot vs accepted baseline; `enforce: true` fails the job with the evaluation exit code; `revision` binding to PR head SHA; content-addressed immutable snapshots and baseline acceptances (with reason) on branch `dokimos-evidence`; exit 5 on evidence identity conflict; completion verify: host authority, exact required sets, missing/extra evidence fail closed, replay protection, freshness windows, attested mode.
- **Gaps:** merge blocking only via required checks (OQ-700); evidence branch writable by anyone with push rights (CI has `contents: write`); baseline acceptance via `workflow_dispatch` input without enforced second approver; measures quality not security; consumers observed: Aegis, Fides only.
- **Trust boundary:** CI token; host-supplied authority and verifier keys.
- **Failure modes:** `enforce: false` or `persist` misconfiguration; baseline re-accepted to hide regressions; evidence branch rewrite.
- **Evidence:** snapshots, dispositions, baseline acceptance records, completion-verification receipts, CI artifacts.
- **Controls:** CC-1 (gate), CC-2 (verifier), CC-7 (pinned install pattern).
- **Enhancements:** require baseline acceptance to carry a signed owner approval (CC-6 pattern); anchor evidence-branch digests (CC-5).

### SYS-ORDO - Ordo / SDE

- **Verified purpose:** SDE methodology and F# semantics for legal transitions, execution roles, receipts, decision gate; `sde` is the CLI alias (`ordo/README.md`).
- **Implementation evidence:** `src/Ordo.Core/ExecutionRole.fs` (default authority matrix), `Authorization.fs` (actor kind per transition), `Receipt.fs` (expected vs observed; narrative excluded; `Indeterminate` distinct), `src/Ordo.Decisions/Gate.fs` (authorization only via `Transition.evaluate` private constructor); tests `ExecutionGovernanceTests.fs`, `ExecutionContractTests.fs`.
- **Capabilities:** SoD model - Implementation prohibits `RecordVerdict`/`AcceptReview`; Verification/Review prohibit `ModifyImplementation`; no role holds `ModifyAcceptanceCriteria`/`ModifyEvaluationAuthority`; widening requires a named policy decision, widening into a prohibition refused.
- **Gaps:** library only; actor identity supplied by the caller; no storage/signing; runtime enforcement in hosts not verified.
- **Trust boundary / failure mode:** host mislabels actor or role -> separation is nominal.
- **Evidence:** resolution observations, ECIR validation digests, receipt comparisons.
- **Controls:** CC-4. **Enhancement:** emit a role-binding record per execution for evidence sampling.

### SYS-EES - Echelon Engineering System

Static F# documentation site (`src/EngineeringSystem.Site/Site.fs`). Useful as narrative source for the agent decision-boundary model (human authority / predetermined / delegated / stop-and-escalate / verification gate). No enforcement or evidence. Documented intent only.

### SYS-REGISTRY - Echelon Registry

- Catalog/spec of systems, capabilities, release manifests with artifact SHA-256 digests, profiles, freezes; F# `conformance/*.fsx`; CI resolves profiles twice and compares bytes.
- **Release attestation signing (REG-SIG-001..008) is specified, not implemented**: no `trust/` directory or signing code in the clone. Conditor's registry key set is empty per the spec.
- Useful for CC-7/CC-8 (expected versions and digests). Enhancement: implement REG-SIG (OQ-709).

### SYS-PROJADMIN - Project Administration

- Knowledge hub plus canonical **installation inventory**: `installations/events/IE-*.json` create-new, append-only, idempotent (`operationId`), refuses same-version/different-digest, privacy validator; projection checked in CI; transport `installation-request.yml` (`workflow_dispatch`, `contents: write`).
- Gaps: append-only by convention; covers Echelon tooling installations, not production assets; any principal able to dispatch the workflow can register.
- Controls: CC-8; evidence: tooling inventory history.

### SYS-LIMEN - Limen

Browser library separating browser capabilities from application authority; binding-security policy and CSP/Trusted Types smoke test (`test/binding-security.test.ts`); URL-secret parameter refusal (SAF-URL-5 / LCP-109); `limen verify --strict` CI; Sigstore provenance on publish. Relevant to application security controls for web products; produces no audit evidence beyond CI results.

### SYS-FOUNDRY - echelon-foundry

Public corporate site (Node build, Dockerfile) with a contact form posting to an external inquiry worker (`contact.js`); backend not in repo (OQ-710). Relevant only as an in-scope public asset for web security/privacy inventory. Praxis 3.7.1 pilot.

### SYS-EKS - Echelon Knowledge Store

Research-phase compiler of Markdown front matter into a validated graph; rules `evidence-without-source` (blocking), `stale-evidence` >180 days (advisory), supersession checks (`src/Echelon.KnowledgeStore.Core/Validation.fs`). Reusable as a validator for assurance records (CC-9); not a store (no persistence, access control or signing). EEF audit reports 78/78 tests pass (secondary; not re-run).

### SYS-EDF - Echelon Diagnostic Framework

Diagnostic grammar v0.3; own README: no established reproducibility or incremental value. Not a control. Reusable techniques: hash-verified frozen release bundle and preregistration integrity tests. Possible use: structured root-cause narratives for control failures/incidents (recommendation, unvalidated).

### SYS-CLARITY - Clarity Framework

Markdown methodology; `decision-record.md` (decision, owner, posture = impact-if-wrong x correctability, reconsideration criteria). Own research: current Clarity "no more accurate" than no method at a ceiling benchmark (`research/2026-framework-improvement/README.md`). Use: template body for risk acceptance / exception records (CC-6). No enforcement.

### SYS-EOA - Echelon Organization Administration

Entity administration documents and portfolio audits. Key finding (`application-governance/ENGINEERING-QUALITY-DECISION-INVENTORY.md`, 2026-10-04): "architecture decisions are often advisory while implementation can continue growing ... without a mechanical ratchet". `AEGIS-FORMA-FOLIO-COMPLIANCE.md` distinguishes required / installed / used / verified - same anti-status-inflation rule as this program. No CI. Source of approval authority (entity governance) for policy approvals; entity contents not reviewed or reproduced.

### SYS-EEF - AI-Native Enterprise Design Framework

F# design-time model; AI Responsibility Model R1-R7 (R1 authorized-human, R2 independent-verification "segregation of duties", R4 "AI may add a flag, never clear one"); agent catalog checks (closed tool allowlist, no human-reserved ops); EEF011 control coverage; CI fails empty/skipped tests and stale generated outputs. Useful to derive AI-governance control design (CC-11); does not enforce anything at runtime. `research/ecosystem-audit-2026-11.md` is consistent with these findings; this review additionally found Ordo's SoD matrix and Praxis's attested completion, which that audit did not cover.

## Overlaps and boundaries

| Concern | Systems | Note |
|---|---|---|
| Actor/provenance records | Praxis, Ordo (`ExecutionActor`), Project Administration (event provenance) | Praxis is the recorder; Ordo defines semantics; neither authenticates. |
| Test/validation evidence | Praxis (`praxis.validation-result/1`), Dokimos (snapshots, receipts) | Dokimos is the independent verifier; Praxis records completion. Docs agree on this split. |
| Approvals | Praxis ECIR, Praxis run authorization, Clarity decision record, EEF R1 | Three designs; CC-6 proposes one signed-approval pattern. |
| Inventory | Registry (what exists), Project Administration (installed where), Praxis `.echelon/ros.json` | Clearly separated in docs. |
| Record validation | Praxis `validate`, EKS validator, echelon-assurance `tools/validate_registers.py` | Overlap; reuse rather than extend the local validator further. |
| Supply-chain integrity | Release workflows (Sigstore), Registry (spec'd signing), Dokimos action SHA pin | Complementary. |

## Echelon repository governance standards - conformance checklist for echelon-assurance

From the Praxis greenfield profile (`praxis/starter/greenfield/manifest.json`, `ros.json`), `praxis/docs/installation.md`, `praxis/docs/00-governance/Engineering-Standards.md`, and observed practice in every Praxis-installed repo. Evidence class: authoritative (internal) for what `praxis init` installs; assumption that it is mandatory for every repo (OQ-708).

| Expected | echelon-assurance (observed 2026-11-25) |
|---|---|
| `AGENTS.md` with authority contract (GV-START-001) | Present (quotes GV-START-001) |
| `CLAUDE.md`, `GEMINI.md`, `.github/copilot-instructions.md` pointing to AGENTS.md | CLAUDE.md present; GEMINI.md and copilot instructions absent |
| `BOOTSTRAP.md`, `HANDOFF.md`, `PROJECT-CHARTER.md`, `README.md`, `LICENSE` | Charter, README present; **`HANDOFF.md` absent although AGENTS.md and context/CURRENT-STATE.md reference it**; BOOTSTRAP.md, LICENSE absent |
| `ros.json` (workProtocol `enforceAttribution`, `continuity.requireDurableCheckpoint`, `upstreamSync`, `provenance.enforce: true` from 2026-09-25, telemetry) and `.echelon/ros.json`, `.echelon/toolchain.json` | Absent (Praxis not installed, acknowledged in AGENTS.md) |
| `./praxis` launchers; `context/`, `registries/` (generated), `research/{journals,packages,theories,evidence}`, `templates/`, `schemas/`, `docs/00-governance/`, `framework/` | `context/`, `research/{journals,packages,evidence}` present; others absent |
| `.github/workflows/praxis-validation.yml` running `./praxis registry check` and `./praxis validate` on push/PR | **No `.github/` directory - no CI at all**; register validator runs only manually |
| `.editorconfig`, `.gitattributes`, `.gitignore` (incl. `.env*`, `*.pem`, `*.key`) | **Absent** (no `.gitignore`, `.gitattributes` or `.editorconfig` at repo root) |
| Decision records `DF-<AREA>-YYYY-xxxx`, identifiers `<PREFIX>-<AREA>-<YYYY>-<TOKEN>`, confidence vocabulary | Adopted manually (`methodology/identifiers.md`) |
| F#/.NET-only implementation (`implementationPolicy.prohibitPythonAutomation`) | Praxis-repo policy; echelon-assurance uses Python - applicability unresolved (OQ-708) |
| Definition of Done incl. durable pushed checkpoint; `main` protected, PR-only, no force-push | AGENTS.md states it; enforcement is a GitHub setting (OQ-700) |
| Shared foundations (Aegis/Forma/Folio, URL-state standard) | Apply only when the repo has application/UI/print surfaces - not applicable now |

## Biggest uncertainties

- GitHub enforcement state of every in-scope repo (OQ-700) - determines whether any gate here is preventive.
- Auditor acceptance of single-owner + agent SoD design and of self-reported provenance (OQ-701, OQ-702).
- Test counts were read, not executed; maturity statements rely on repository self-description plus code presence.
