---
id: ASR-INT-001
title: Echelon integration and responsibility matrix
status: draft
version: 0.1.0
created: 2026-11-25
inputs: [integration/fragments/ECH1-assessment.md, integration/fragments/ECH2-assessment.md, integration/system-register.csv]
---

# Echelon integration and responsibility matrix

Evidence for every row is in the two system assessments (file paths read in
each repository). "Implemented" means code and tests exist in the
repository; **no system was observed operating in production**, and no test
suite was executed during this research (`assumption` on deployment state).

## 1. Headline

1. **Most of the assurance architecture can be assembled from existing
   Echelon systems.** The genuinely missing capabilities are: (a)
   tamper-evident evidence anchoring (hash chain + external witness/WORM),
   (b) identity lifecycle and access review, (c) centralized runtime
   logging, alerting and heartbeat monitoring, (d) auditor evidence
   packaging. None justifies a *new* Echelon system yet; (a) and (d) are
   extensions of Arca/Percepta, (b) and (c) are best met by managed
   services (identity provider, cloud logging) plus thin F# adapters.
   (`recommendation`; decision record `DF-ASR-2026-3A1E`, proposed.)
2. **The strongest existing assets are not switched on.** Praxis/Dokimos
   verifier-signed test receipts and fail-closed completion are implemented
   and tested but enabled in no repository examined; merge blocking depends
   on GitHub rulesets whose state is not evidenced (OQ-700, OQ-704).
3. **GitHub is a subservice organization** for most Echelon applications
   (system of record, identity, authorization, CI). Its SOC 2 report and the
   CSOCs it implies must be reviewed under POL-10 (OQ-750).
4. **Two assumed capabilities do not exist**: Strata is a PostgreSQL schema
   CLI, not an append-only integrity store; Folio is a document renderer, not
   an evidence packager. Unbound requirements that assume otherwise need
   correction (proposed change PC-01, PC-02).

## 2. Responsibility matrix

Columns follow charter §13. "Proposed" = proposal recorded in
`proposed-changes.md`, not a change made.

| System | Verified responsibility | Existing assurance capability (level) | Missing | Overlaps | Evidence produced | Controls it can enforce | Validation role | Trust boundary | Failure modes | Proposed |
|---|---|---|---|---|---|---|---|---|---|---|
| **Praxis** | Repository operating system: work protocol, canonical records, provenance, validation, CI | Append-only execution/event records with digests; validation gates; verifier-signed attested completion (implemented, tested, not enabled) | Authenticated provenance; signing of event logs; enablement | Ordo (actor semantics), Project Admin (provenance) | Work/execution records, validation results, completion receipts | SDL change-process conformance; record integrity checks | Records completion; not independent of the implementer | Repository + CI runner; trusts agent-reported actor identity | Self-reported provenance; disabled gates; history rewrite by repo admins | Enable `requireVerifiedCompletion` in assurance-relevant repos; bind actors to workload identity (PC-03, PC-04) |
| **Dokimos** | Validation and compliance gates; snapshot and receipt verification | Fails CI on violations; verifies signed receipts (implemented, tested) | Merge blocking (needs rulesets); org-wide adoption | Praxis validation | Gate results, verification receipts | L4 when required by rulesets: tests run, evidence present | **Independent verifier role** (separate key never held by agent) | Verifier host key | Check not required → advisory only | Require Dokimos checks via rulesets-as-code (PC-05) |
| **Ordo / SDE** | Engineering delivery model; role authority matrix | Typed SoD model (implementer ≠ verdict recorder) | Binding to authenticated identities | Praxis, EEF | Role decisions | SoD rules (design) | Defines independence semantics | — | Model-only, unenforced | Use as SoD source for authority matrix (PC-06) |
| **Tutela** | Derived security posture gate with exceptions | Verified, non-self, role-checked, expiring exceptions; adversarial fixtures (implemented, tested) | Org-wide use; evidence-store integration | Aegis | Posture decisions, exception records | GOV exception rules; release gating | Gate; adversarial tests are reusable validation | CI | Gate not required | **Generalize as the exception and SoD engine** (PC-07) |
| **Conditor** | Signed attestations, digest-pinned supply chain, branch/CI policy | Signed release attestations (implemented, only tagged release) | Central retention of attestations | Strata release provenance | Attestations, provenance | SDL signing/provenance (L4) | — | Release pipeline | Key compromise; policy not applied repo-wide | Adopt as supply-chain baseline (PC-08) |
| **Aegis** | Security-related primitives: redaction, fault records | Log/secret redaction; immutable fault files (implemented) | Aggregation, alerting | Signal audit records | Fault events | LOG hygiene (no secrets in logs) | — | App process | Redaction gaps | Shared redaction library for evidence emitters (PC-09) |
| **Vigila** | Personal/business follow-up and open-loop tracker with GitHub-backed records (SYS-VIGILA; ECH2 §2.2: at most L1 for control tasks). Not an observability or alerting system today | Per-app audit/observability records | Central aggregation, alerting, heartbeat (SOC2-027) | Signal, Chrona | Activity records | Detective only | — | GitHub | Missing events undetected | Heartbeat/gap detection; or replace with managed cloud logging (PC-10) |
| **Fides** | Identity/trust exchange (pilot) | Authentication (implemented, pilot; deployment state contradictory) | MFA evidence (delegates to GitHub), lifecycle, access review, tenancy | GitHub identity | Auth events | IAM authentication | — | Identity boundary | Pilot maturity; single point of failure | Clarify deployment (OQ-752); identity lifecycle via managed IdP (PC-11) |
| **Arca** | Record/archive store | Content hashes, actor trailers, export with SHA-256, erasure (implemented; context file says otherwise) | Hash chain (explicitly deferred), external anchor, WORM | Signal, Percepta | Archived records, exports | DAT retention/erasure | Integrity primitive | GitHub-backed | Rollback undetected without anchor | **Base of the evidence ledger**: add chain + anchoring (PC-12, decision record) |
| **Percepta** | Evidence/observation envelope | Input digests bound to source revision (implemented) | Signing, completeness | Arca | Evidence envelopes | — | Evidence format | — | — | Align with `architecture/evidence-envelope.schema.json` (PC-13) |
| **Strata** | PostgreSQL schema management CLI; grants/RLS | Schema migration control, grants (implemented) | Not an append-only store (contrary to Unbound assumption) | Census baseline | Migration records | CFG/TEN DB grants and RLS as code | — | DB admin | Misassigned responsibility | Correct Unbound requirements (PC-01) |
| **Summa** | Accounting ledger | Double-entry, period close blockers, reconciliation, sealed verified backups (implemented, not deployed) | Production operation | Unbound FIN design | Ledger, recon results, backup verification | FIN invariants; BCR backup verification | Reference implementation for FIN invariants | App | Not deployed | Reuse invariant and reconciliation patterns in Unbound (PC-14) |
| **Signal** | Notifications/records with audit | PII-free closed-vocabulary audit records; honest deletion states | Rollback detection (own review) | Chrona, Arca | Audit records | LOG/DAT | — | GitHub | Own verdict NOT ACCEPTED | Reuse audit vocabulary pattern |
| **Chrona, Causa, Census, Tekmerion, Specula, Limen, Registry, Project Admin, EKS, EES, Foundry** | See assessments | Peripheral or inventory roles; Specula observes GitHub activity read-only; Registry specifies (not implements) release signing; Project Admin holds installation inventory | — | Inventory overlap (Registry vs Project Admin vs Praxis) | Inventory, activity | Inventory completeness (asset inventory, CC6.1) | Specula as an independent read-only observer is a candidate external-witness feed | — | — | Use Registry + Project Admin as system inventory source (PC-15); evaluate Specula as completeness cross-check (PC-16) |
| **Clarity, EDF, EEF** | Decision and diagnostic methods | Decision records; EEF governance invariants (human authority, no AI clearance) | — | Approvals (three designs) | Decision records | GOV decision accountability | Method for review passes | — | — | One signed-approval pattern across systems (PC-17) |
| **Echelon Organization Administration** | Organizational administration records and templates | Business plans, entity records (private) | Policy/HR/training system of record not observed | — | Org records | HR/GOV (candidate) | — | Private repo | — | Candidate home for authority matrix and personnel role records (OQ register) |
| **Forma / Folio** | Rendering (secondhand only) | Folio renders documents | Evidence packaging, manifests, chain of custody | — | Rendered documents | — | — | — | Misassigned responsibility | Correct Unbound SOC2-009/MON-015 (PC-02) |
| **Unbound** | Reference product (requirements only) | Extensive control design (`UNB-SCTRL-*`, CCM) | Everything operational | — | — | Consumer of all of the above | — | — | — | First adopter |

## 3. Interfaces required

| Interface | From → To | Purpose |
|---|---|---|
| Evidence event contract | Every emitter → evidence ledger (Arca-based) | One envelope (`architecture/evidence-envelope.schema.json`) replacing three audit-record shapes |
| Policy model → enforcement | F# policy model → repository rulesets, Dokimos gate config, Tutela exception rules, cloud org policies | Single source of truth; drift check fails CI |
| Authority matrix | Policy model → application authorization (Unbound), Ordo SoD, Tutela | Same approvals enforced everywhere |
| System inventory | Registry + Project Admin → control catalog applicability | Which systems are in scope of which controls |
| Independent observation | GitHub API / Specula → completeness reconciliation | Cross-check that evidence covers every PR, deploy, access change |
| External anchoring | Evidence ledger checkpoints → transparency log witness + RFC 3161 TSA | Tamper evidence that does not depend on Echelon administrators |

## 4. Trust boundaries that matter most

1. **GitHub organization owners** can change rulesets and rewrite history;
   evidence of ruleset state over time and external anchoring of evidence
   checkpoints are the compensating controls.
2. **Verifier keys** (Dokimos/Praxis attested completion) must be held where
   implementing agents cannot reach them.
3. **Cloud account administrators** can disable logging; organization-level
   guardrails plus external heartbeat monitoring compensate.

## 5. Owner decisions of 2026-11-25 (`docs/decisions/owner-approved-foundations-2026-11-25.md`)

These set intended responsibilities going forward; they do not change what
was observed in each repository.

| System | Owner decision | Effect on this matrix |
|---|---|---|
| Vigila | 13 (DF-ASR-2026-0D0D): Vigila is the logging and observability initiative integrating managed infrastructure; heartbeat must be independently operated | Vigila is the intended implementer for CTL-LOG-001/003 and integrates CTL-ASR-004. The current repository is a follow-up tracker, so this is new scope (proposed change PC-10 revised). |
| Folio | 11 (0D0B): Folio renders authoritative evidence packages; it is not responsible for evidence collection, validity or authoritative storage | Folio is the renderer for auditor packages (CTL-ASR-010, CTL-CUS-003); manifests and chain of custody belong to evidence storage. |
| Strata | 12 (0D0C): Strata is the database interaction layer (controlled access, transactions, parameterization, schema governance, testability, instrumentation), not the evidence system | Strata implements database-level controls (TEN, DAT, CFG); evidence storage is separate (0D0A). |
| Arca | 10 (0D0A): integrity capabilities subject to verification; GitHub not the sole production evidence store; interfaces before custom ledger | Supersedes DF-ASR-2026-3A1E. |
| Praxis, Dokimos | 6, 7, 8 (0D06–0D08): adopt Praxis and F# standards; require verified completion, enforced checks, authenticated provenance | PC-03, PC-04, PC-05 become owner-approved direction; actual settings still to be verified (OQ-700). |
