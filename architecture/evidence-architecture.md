---
id: RP-ASR-2026-PAC3
title: Continuous evidence architecture
status: draft
version: 0.1.0
owners: [assurance-governance]
created: 2026-11-25
updated: 2026-11-25
research_key: PAC
sources: sources/fragments/PAC.csv
schema: architecture/evidence-envelope.schema.json
evidence_types: registers/fragments/PAC-evidence-types.csv
confidence: medium
---

> **Consolidation note (2026-11-25, `JR-ASR-2026-C001`; resolves the overlap question OQ-809 / registry OD-GOV-020; accepted with DF-ASR-2026-C001 under DF-ASR-2026-0D10).** Governing requirements: BR-ASR-001 to BR-ASR-011, BR-DAT-001 and BR-IAM-001.
>
> **Reconciliation with `requirements/continuous-assurance-requirements.md` (PR #2):**
> - CA-PROV-01 and CA-REPRO-01 become envelope fields (BR-ASR-001).
> - CA-PROV-02 becomes the self-reported versus platform-authenticated label (BR-IAM-001).
> - CA-FRESH, CA-EXP and CA-REV become BR-ASR-004.
> - CA-DISC becomes BR-ASR-007 and BR-CUS-002.
> - **CA-PROV-03 (hash chain plus external anchoring) is replaced** by managed immutable storage.
>
> **Minimum architecture now (DF-ASR-2026-C002, accepted):**
> 1. Envelopes are emitted at enforcement points.
> 2. Sensitive payloads are kept apart from minimal integrity metadata.
> 3. Storage is managed object storage with a provider retention lock in a separate account.
> 4. Population reconciliation uses an independent read identity.
> 5. An independent managed heartbeat watches the collectors.
> 6. Packages are reproducible.
>
> **Deferred until demand is recorded:** the hash-chained ledger, witness cosignatures (EVT-024), RFC 3161 timestamps (EVT-025) and public anchoring described below. They are design history, not the plan of record.
>
> **Verified capability (2026-11-25):** Arca has content hashes but no chain, signing or WORM storage; Folio has no digest manifest; Vigila has no logging (`integration/capability-verification.csv`).


# Continuous evidence architecture

> **Status: design research.** Not implemented. No evidence exists yet. This
> document specifies how Echelon Foundry *would* collect, protect and prove the
> completeness of control evidence. Evidence classes in `[brackets]`.

## 1. Principles

1. **Integrity is not accuracy.** A signature, hash chain or transparency log
   proves that a record has not changed since it was written and who wrote it.
   It does **not** prove that the record was true when written. A collector
   that misreads GitHub will produce perfectly signed wrong evidence. Accuracy
   needs separate controls: source-of-truth reconciliation, collector change
   control, and independent re-performance (§6) [recommendation; Ordo
   ORDO-0505 "available is not true", SRC-PAC-041].
2. **Unknown is never success.** Missing, stale or unverifiable evidence
   renders as *unknown*, never as pass or zero (Specula rule) [recommendation].
3. **Observed vs derived vs inferred** are distinct (Ordo `EvidenceKind`). An
   agent's or model's conclusion is `inferred` and cannot satisfy a control
   that requires an observation [recommendation].
4. **Transactions emit their own evidence.** Prefer evidence produced by the
   system performing the controlled action (a merge, a deploy, a policy
   decision) over evidence assembled later by a separate scraper
   [recommendation].
5. **No personal data in the integrity layer.** The append-only layer holds
   hashes and pointers; personal data lives in separately keyed, deletable
   payload storage (§8) [recommendation].
6. **Internal ≠ independent.** Witnesses run by Echelon are internal
   controls. Independence requires a party Echelon does not control (§5.4)
   [recommendation; AGENTS rule 4].

### 1.1 Evidence by construction (TNT-17, owner direction DF-ASR-2026-0D10)

Every system is designed so that its normal operation emits the evidence its
controls need; evidence is not reconstructed afterwards (BR-ASR-012).

- **Proportionate.** Emit what a control or obligation needs, not everything
  observable. Each evidence type names the control it serves.
- **Privacy-preserving.** Evidence carries references, hashes and
  classifications rather than personal or financial content wherever the
  control allows (section 8).
- **Failure of evidence collection is itself detected.** Expected sources emit
  heartbeats through the shared observability interfaces (Vigila, BR-LOG-002);
  evidence populations are reconciled against system populations (section 6);
  missing evidence is a control failure, never a pass (RSK-066).
- **Design obligation.** A design review for a new system or material change
  records which evidence it emits and for which controls.

## 2. Reuse of existing Echelon systems

> Capability statements below are from an earlier read-only review. `integration/capability-verification.csv` is authoritative and supersedes them where they differ: it does not confirm signed run authorizations, `CheckProducer` pinning or hash-chained Praxis events, nor Tutela's Evidence record or authority policy. Nothing here is relied on until verified there.

| System | Reported capability (earlier read-only review; see the CSV) | Role here |
|---|---|---|
| Praxis | Execution records, receipts (expected vs observed), signed run authorizations bound to policy digest, `CheckProducer` pinning | Producer of SDLC/agent execution evidence (EVT-007, EVT-018) |
| Tutela | `Evidence` record with digest + provenance + validity window; deny-by-default authority policy over evidence types/issuers | Envelope fields `integrity`, `provenance`, `validity`; evidence-authority rule at ingest |
| Ordo | `Evidence` kinds Direct/Derived/Inferred; decision events (`ordo.decision-event/1`) | `collection.kind`; policy-evaluation evidence |
| Aegis | Reference store: "one immutable file per event in a GitHub repository" | Pattern only; a public/shared GitHub repo is **not** an acceptable evidence store for production evidence (confidentiality rules) |
| Specula | Read-only observer; SHA-bound check status; unknown never shown as success | Model for the read-side dashboard; candidate *independent-of-producer* reconciler |
| Dokimos | Immutable longitudinal observations attributable to revision/analyzer version | Producer of code-quality evidence |

Gap: none of these is an append-only, witnessed, retention-locked evidence
store with completeness reconciliation. That is the new component, subject to
a decision record per AGENTS rule 5 [recommendation]. The earlier
proposal `DF-ASR-2026-3A1E` (Arca with a hash chain, anchoring and a WORM
target) is superseded by DF-ASR-2026-C002, which adopts managed immutable
storage (OQ-805 answered).

## 3. Evidence envelope

Schema: [`evidence-envelope.schema.json`](evidence-envelope.schema.json)
(JSON Schema draft 2020-12). One envelope per evidence record. The payload
(the raw artifact) is stored separately and referenced by digest.

| Field | Purpose | Notes |
|---|---|---|
| `envelope_version` | Schema version | `echelon.evidence-envelope/1` |
| `evidence_id` | Globally unique, content-independent ID | UUIDv7 (time-ordered) |
| `evidence_type_id` | `EVT-NNN` | Registered type determines required fields |
| `control_ids` | `CTL-*` the record supports | May be empty at collection; mapping can be added by a *derived* envelope, never by mutation |
| `source` | `system`, `system_id` (`SYS-*`), `tenant_scope`, `object_ref` (stable pointer, e.g. PR URL / commit SHA / CloudTrail event ID) | |
| `actor` | Identity that performed the *controlled action* (human / agent+principal / workload) | Pseudonymous identifier; never email in clear |
| `collector` | Identity that *wrote the envelope* (workload OIDC subject or key ID) and collector software version/digest | Separates "who did it" from "who recorded it" |
| `occurred_at`, `observed_at`, `recorded_at` | Event time, observation time, ingest time | OTel distinguishes Timestamp vs ObservedTimestamp (SRC-PAC-036) |
| `policy` | `policy_id`, `policy_version`, `policy_digest`, `evaluator_fingerprint` | Binds the evidence to the rule in force (Praxis `EvaluatorIdentity`) |
| `collection` | `method` (event-push / api-poll / attestation / manual-upload / derived), `kind` (direct/derived/inferred), `derived_from` | |
| `sequence` | `stream_id`, `seq` (gap-free per stream), `prev_envelope_hash` | Completeness and ordering |
| `integrity` | `payload_digest` (sha256/sha512), `payload_size`, `envelope_digest`, `signature` (bundle ref: Sigstore bundle or KMS key + sig) | |
| `provenance` | `issuer`, `run_ref`, `workflow_ref`, in-toto/SLSA attestation ref | Tutela `Provenance` |
| `anchoring` | `log_index`, `checkpoint_ref`, `rfc3161_token_ref`, `witness_cosignatures` | Filled after inclusion; written as a *derived* envelope or receipt, not by mutation |
| `validation` | `status` (unvalidated / schema-valid / authority-accepted / reconciled / rejected / superseded), `validated_by`, `findings` | Status changes are new envelopes |
| `retention` | `retention_class`, `retain_until`, `legal_hold` | |
| `access_classification` | public / internal / confidential / restricted / customer-shareable | Default `restricted` |
| `personal_data` | `present` boolean, `payload_key_id` (crypto-shred key), `subject_refs` (pseudonymous) | §8 |
| `exceptions` | Exception IDs in force for this subject/time | |
| `supersedes` | Prior evidence ID corrected by this one | Corrections append, never edit |

Immutability rule: an envelope is never updated. Every status change,
mapping, anchor or correction is a new envelope referencing the original
(`derived_from`/`supersedes`) [recommendation].

## 4. Evidence sources (draft type list in `PAC-evidence-types.csv`)

GitHub commits, PRs, reviews, ruleset state, push-protection bypasses; CI
runs and test results; deployments; cloud audit logs (CloudTrail, Azure
Activity, GCP Audit Logs); IAM events and access reviews; vulnerability and
secret scans; SBOMs; signed artifacts and provenance; backup restore tests;
incidents; financial/ledger reconciliations; policy evaluations (OPA/Cedar/F#
decisions); agent execution records; customer configuration changes; training
records; policy approvals and exceptions; evidence-system self-checks
(heartbeats, reconciliations, witness results).

Notable source facts:
- GitHub organization audit log covers **180 days**; programmatic API access
  is stated for GitHub Enterprise Cloud [authoritative SRC-PAC-018]. Evidence
  must therefore be collected continuously, not retrieved at audit time; plan
  choice follows the approved internal resolution: existing GitHub
  capabilities first, upgrading only if necessary and economical (OQ-508;
  DF-ASR-2026-0D11).
- Required status checks are only trustworthy if the **expected source app**
  is pinned; otherwise any writer can set a passing status [authoritative
  SRC-PAC-016]. CI evidence must record the producing app ID.
- Push-protection bypasses generate alerts and audit-log events [authoritative
  SRC-PAC-017] — collect them as exception evidence.
- CloudTrail digest files are hourly, RSA-signed and chained, letting a
  verifier assert that files were modified/deleted or that none were delivered
  in a period [authoritative SRC-PAC-033]. Ingest them; do not re-implement.

## 5. Storage and integrity

### 5.1 Layers

```
 producers (Praxis, CI, cloud, apps, agents)
     │  signed envelope + payload
     ▼
 ingest gateway (authenticates collector identity; schema + authority check; assigns recorded_at)
     │                          │
     ▼                          ▼
 payload store (WORM)       evidence log (Tessera, tlog-tiles) — leaf = envelope_digest
 S3 Object Lock compliance        │ checkpoints (signed note)
 / Azure locked immutability      ├─► RFC 3161 TSA token on each checkpoint (≥ 2 TSAs)
 / GCS locked retention           ├─► external witnesses cosign (C2SP tlog-witness)
     │                            └─► checkpoint copies published to an independent location
     ▼
 index/query DB (rebuildable, NOT authoritative)
     ▼
 dashboards, OSCAL assessment-results, auditor export (with inclusion proofs)
```

### 5.2 Why each layer

| Layer | Property it gives | What it does **not** give | Class |
|---|---|---|---|
| WORM object store (S3 Object Lock *compliance* mode, Azure **locked** time-based retention, GCS **locked** retention) | Cannot delete/overwrite before retain-until, even by root/admins (S3: only by deleting the account); Cohasset assessments vs SEC 17a-4(f) exist for S3 and Azure | Does not prove what was *never written*; governance mode / unlocked policies are bypassable | authoritative SRC-PAC-030..032 |
| Transparency log (Merkle, append-only) | Inclusion proofs per record; consistency proofs show later tree extends earlier one (append-only) | Log operator can show different views to different clients (split view) unless witnessed | authoritative SRC-PAC-026 |
| Witness cosignatures | Witness only cosigns checkpoints consistent with its last view → defeats split views / rollback | Witness must be independent and actually checked | authoritative SRC-PAC-027 |
| RFC 3161 timestamps on checkpoints | Third-party proof the checkpoint existed before genTime | Trust in TSA; key compromise invalidates tokens; needs renewal over long retention | authoritative SRC-PAC-024 |
| Signatures (Sigstore keyless for CI workloads; KMS keys for collectors) | Authenticates the writer; Rekor records signing events | Proves writer identity, not record truth | authoritative SRC-PAC-021 |

**RFC 3161 vs witnesses:** they are complementary. RFC 3161 gives a trusted
*time* bound; witnesses give *append-only/no-fork* assurance without trusting
the log operator. Timestamp each checkpoint (not each record) so cost is
bounded by checkpoint frequency [recommendation].

**Rejected options:** Amazon QLDB reached end of support 2025-07-31
[summary-only SRC-PAC-034]; immudb is BSL-licensed [authoritative SRC-PAC-035];
a database with "append-only" enforced only by grants is not tamper-evident
against its administrators [recommendation].

SEC 17a-4 context (not known to apply to Echelon): the 2022 amendments
added an *audit-trail alternative* to WORM — a system that can re-create an
original record if altered/erased [summary-only SRC-PAC-038]. The tlog +
WORM design satisfies the spirit of both; applicability is not claimed.

### 5.3 Key management

Collector signing keys in a managed KMS/HSM with no export; CI uses keyless
OIDC signing so no long-lived secret exists; log and checkpoint signing keys
held by a role separated from evidence producers (with one human
administrator this separation does not exist yet; KMS key deletion and
evidence-account deletion are admin bypasses of WORM and crypto-shredding,
OQ-804); key rotation recorded as
evidence; TSA tokens and witness keys pinned in verifier configuration
[recommendation].

### 5.4 Independence of verification

An Echelon-run verifier is an internal control. Credible independence options,
in increasing cost: (a) publish checkpoints to public witness networks (exposes
only origin, tree size and root hash — no content); (b) give the auditor a
read-only verifier tool and checkpoint feed so they re-verify inclusion and
consistency themselves; (c) third-party monitoring service. Whether the
auditor relies on (b) is pending auditor acceptance (OQ-500; internal design
approved, DF-ASR-2026-0D11: managed immutable storage with independently
verifiable integrity and completeness, and no unjustified custom witness
network) [recommendation; external acceptance pending].

## 6. Completeness: proving evidence was not selectively omitted

Tamper-evidence protects what was written. Completeness asks whether
everything that *should* have been written was. Layered approach:

| Technique | Detects | Mechanism | Limit |
|---|---|---|---|
| **Per-stream sequence numbers** | Gaps inside a producer's stream | `sequence.stream_id` + gap-free `seq`, `prev_envelope_hash` chain; ingest rejects out-of-order, gap monitor alerts | Producer can omit *before* numbering |
| **Heartbeats** | Silent producer death / disabled collector | Each stream emits signed heartbeat envelopes at fixed cadence including its last seq; missing heartbeat ⇒ alert and "unknown" status | Proves liveness, not coverage |
| **Population reconciliation** | Events that happened in the source but never reached the store | Periodic job lists the source-of-truth population (GitHub REST/GraphQL merged PRs for repo R in window W; deployments; CloudTrail digest manifests; IAM user list) and diffs against evidence store; result is itself evidence (EVT-022) | Trusts the source API; reconciler must be independent of collector code path |
| **Platform-native digests** | Deleted/modified platform logs | CloudTrail digest chain validation; GitHub audit-log export counts | Platform-specific |
| **Merkle consistency proofs** | Removal or rewrite of already-logged records | Verifiers and witnesses check each new checkpoint is consistent with prior | Only covers logged records |
| **External anchoring/witnessing** | Log operator forking or rolling back history | Witness cosignatures + RFC 3161 tokens + independent checkpoint copies | Requires verifiers to actually check |
| **Expected-evidence from policy model** | Controls with no evidence | `EvidenceRequirement` per control generates expectations; absence after cadence ⇒ control status unknown | Only as good as the model |

Example — PR review control: population = GitHub "list pull requests"
(state=closed, merged) per protected repo per day; each merged PR must have an
EVT-002 (review) and EVT-003 (CI result at merge SHA from pinned app)
envelope; reconciliation emits counts {source N, evidence M, missing list};
any missing ⇒ finding [recommendation].

Residual risk: a compromised *source* system (e.g., GitHub org owner deleting
a repo) can shrink the population itself; mitigate with org-level audit-log
streaming and repository inventory reconciliation [recommendation].

## 7. Retention

| Retention class | Content | Proposed minimum | Basis |
|---|---|---|---|
| `R-AUDIT` | Control operating evidence | Audit period + 1 year, pending research on required periods | assumption; configurable retention by class, jurisdiction and contract approved (OQ-502; DF-ASR-2026-0D11) |
| `R-FIN` | Financial-transaction and reconciliation evidence | Undetermined (tax/banking rules) | period pending research; configurable retention approved (OQ-502; DF-ASR-2026-0D11) |
| `R-SEC` | Security logs, incidents | Undetermined | period pending research; configurable retention approved (OQ-502; DF-ASR-2026-0D11) |
| `R-INTEGRITY` | Envelopes, checkpoints, TSA tokens, witness cosigs | ≥ longest payload class + renewal of timestamps | recommendation |
| `R-PI` | Payloads containing personal data | Minimum necessary; crypto-shreddable | recommendation |

Retention is enforced by the storage layer (locked retention), not by
procedure. Legal holds use the platform legal-hold feature and are themselves
evidence [recommendation].

## 8. Privacy: append-only vs deletion obligations

Conflict: an append-only, WORM, witnessed store cannot delete; GDPR Art. 17
gives a right to erasure, subject to exceptions including compliance with a
legal obligation and establishment/defence of legal claims [authoritative
SRC-PAC-037; applicability of GDPR to Echelon not established].

Design:
1. **Keep personal data out of envelopes and the log.** Actors are recorded
   by stable pseudonymous ID; the mapping table (ID → person) is a mutable,
   access-controlled register outside the evidence store.
2. **Payloads with personal data are encrypted per subject (or per subject ×
   period) with a key in KMS.** The erasure *mechanism* is key destruction
   (crypto-shredding); it is **not assumed to satisfy legal erasure**
   requirements (owner requirement PEC-021; OQ-503);
   envelope, hash and log entry remain, proving that a record existed and was
   erased under procedure (EVT-027 erasure record).
3. **Hashes of personal data are themselves personal data if guessable**
   (low-entropy fields like emails). Never log unsalted hashes of identifiers;
   use keyed HMAC with a shreddable key [recommendation].
4. WORM retention on encrypted PI payloads is compatible with crypto-shredding
   only if the *key* is not WORM-locked. The approved internal design uses
   crypto-deletion where appropriate and does not presume it is legal erasure
   (OQ-503; DF-ASR-2026-0D11); whether it satisfies a given erasure obligation
   remains a legal question [legal classification not assumed].

## 9. Access

| Role | Can | Cannot |
|---|---|---|
| Producer/collector workload | Append to its own stream | Read others' payloads; delete; alter retention |
| Evidence-system operator | Operate ingest, log, index | Delete (WORM), sign as a producer, approve exceptions |
| Control owner | Read evidence for their controls | Append on behalf of producers |
| Internal assurance | Read all, run reconciliations | Modify |
| External auditor | Read-only scoped export + verifier tool | — |
| Customer | Customer-shareable derived summaries only | Raw evidence |

Break-glass access is itself logged as evidence and reviewed
[recommendation].

## 10. Failure modes

| Failure | Detection | Response | Mode |
|---|---|---|---|
| Collector down | Missing heartbeat | Control status → unknown; page owner; backfill from source within platform retention (GitHub 180 d) | detective |
| Ingest rejects schema/authority | Ingest error evidence | Producer retries; persistent ⇒ incident | fail-closed (record not accepted as evidence) |
| Log split view / rollback | Witness refuses cosign; verifier consistency failure | Freeze reliance; incident; auditor notified | detective, high severity |
| TSA unavailable | Missing token | Retry with second TSA; checkpoint still witnessed | fail-open-justified |
| Signing key compromise | KMS/OIDC anomaly | Revoke; mark envelopes signed in window as suspect; rely on RFC 3161 + witness to bound pre-compromise records | detective |
| Collector bug (wrong data) | Reconciliation mismatch; independent re-performance | Supersede affected envelopes; root-cause; collector change control | detective — integrity layer will *not* catch this |
| Source-of-truth API change/outage | Reconciliation error | Status unknown, not pass | fail-closed for status |
| Retention misconfigured (governance mode / unlocked) | Config evidence check (EVT-029) | Treat store as non-compliant until locked | detective → preventive via guardrail |
| PI written to envelope | DLP scan of envelopes at ingest | Reject; if already logged, envelope remains — **cannot be removed** — hence fail-closed ingest | fail-closed |
| Clock skew | `occurred_at` vs `recorded_at` bounds | Flag; TSA time is authoritative for checkpoints | detective |

## 11. Open questions

OQ-500 (auditor reliance on witnessed tlog evidence), OQ-502 (retention),
OQ-503 (crypto-shredding as erasure), OQ-504 (cloud provider), OQ-507
(qualified timestamps), OQ-508 (GitHub plan for audit-log API) and OQ-510
(agent evidence classification) now have owner-approved internal design
resolutions (DF-ASR-2026-0D11); requirements definition, implementation
verification remain pending; external acceptance remains pending where a
condition exists, notably auditor acceptance (OQ-500, OQ-510). Provider
selection for OQ-504 is pending as part of implementation; OQ-504 has no
external acceptance condition.
OQ-505 (public witness network) is answered as deferred (DF-ASR-2026-0D10).
See `registers/question-dispositions.csv` and
`registers/fragments/PAC-open-questions.csv`.

## Owner requirements PEC-013..028 (2026-11-25)

`requirements/executable-policy-and-continuous-evidence.md` maps onto this architecture as follows (catalog controls in
brackets): evidence emitted at the enforcement point (CTL-ASR-001), standard
envelope (CTL-ASR-001; `evidence-envelope.schema.json`), completeness
reconciliation (CTL-ASR-003), freshness and unknown-is-not-pass (new
CTL-ASR-011), separated privileges (CTL-ASR-006), managed immutable storage
with GitHub not the sole store (CTL-ASR-002), heartbeat (CTL-ASR-004),
reproducible auditor packages (CTL-ASR-010), self-verification (CTL-ASR-005,
008). Per the owner's dispositions, a public witness network and qualified
timestamp purchases are **deferred until justified** (OQ-504, OQ-505,
OQ-507): the storage interface is provider-neutral first, and anchoring
options in this document are evaluation candidates, not commitments.
