---
id: RP-ASR-2026-PAC2
title: Policy-as-code and evidence tooling evaluation
status: draft
version: 0.1.0
owners: [assurance-governance]
created: 2026-11-25
updated: 2026-11-25
research_key: PAC
sources: sources/fragments/PAC.csv
confidence: medium
---

# Tool evaluation

> **Note (DF-ASR-2026-C002, accepted).** Custom hash chaining, transparency-log anchoring, external witnesses and RFC 3161 timestamping are deferred until a requirement for them is recorded; evidence integrity rests on a managed store under a compliance-mode retention lock, separately credentialed population reconciliation and an externally hosted heartbeat. Text below that relies on anchoring describes an option, not the plan.

Retrieved 2026-11-25 unless stated. "Maturity" is what the primary source
says, not our judgment of production quality. Licences are as stated by the
project's own page. Where a fact came only from a search-result summary the
source is marked `summary-only` in `sources/fragments/PAC.csv` and the claim is
capped accordingly. No tool here has been run by this program.

## 1. Policy languages and decision engines

| Tool | What it is (primary source) | Maturity / governance | Licence | Strengths for Echelon | Weaknesses / risks | Fit | Class |
|---|---|---|---|---|---|---|---|
| **OPA / Rego** (SRC-PAC-003, -004) | General-purpose policy engine; decouples decision from enforcement; Rego over hierarchical JSON; structured (not only allow/deny) decisions | CNCF **graduated**; core maintainers moved Styra→Apple Aug 2025, project says governance unchanged | Apache-2.0 | Ubiquitous in CI/K8s/Terraform-plan checks; large library (Conftest etc.) | Rego is a second language for F# teams; dynamic semantics harder to analyse; Styra commercial product future disputed | Use only where an enforcement point already embeds OPA (e.g., IaC plan checks) | authoritative (docs) / summary-only (governance) |
| **Cedar** (SRC-PAC-005..009) | Authorization language: RBAC/ABAC/ReBAC; default-deny; forbid overrides permit; erroring policies skipped; validator; designed for sound and complete logical encoding enabling analysis | OOPSLA 2024 paper; Lean model with mechanized proofs, differential random testing of Rust impl; CNCF **sandbox** (accepted 2025-10-08 per CNCF page, summary-only); powers Amazon Verified Permissions | Apache-2.0 | Best-in-class analyzability; safe for tenant-authored rules; formal assurance story | No first-party .NET SDK observed (Rust core, WASM bindings, a Go port); error-skip semantics mean an erroring `forbid` does not deny | Option only: candidate for tenant/customer authorization if a demonstrated benefit exceeds integration cost; no new runtime dependency approved (OQ-509 disposition) | authoritative (docs) / academic |
| **Kyverno** (SRC-PAC-010) | Policy engine originally for Kubernetes; YAML + CEL; validate/mutate/generate/cleanup/image verification; OpenReport reports; can evaluate any JSON via CLI | Page does not state CNCF level or licence (not verified here) | not verified | Low learning curve if K8s used | No Kubernetes observed in Echelon systems | Not now | authoritative (docs) |
| **F# pure deciders** (SRC-PAC-039..041) | Echelon pattern: pure evaluation functions over typed inputs (Praxis execution policy, Tutela authority policy) | Internal; in use in Praxis/Tutela | Echelon licences (PolyForm NC for Tutela) | Same language and types as apps; exhaustiveness; no runtime dependency | No formal semantics proof; analysis must be hand-built | Default for in-app and pipeline decisions | echelon-internal |

## 2. Control/compliance representation

| Tool | What it is | Maturity | Licence | Fit | Class |
|---|---|---|---|---|---|
| **NIST OSCAL** (SRC-PAC-001, -002) | Layered models: Control layer (catalog, profile, control mapping); Implementation layer (SSP, component definition); Assessment layer (assessment plan, assessment results, POA&M). XML/JSON/YAML | Releases observed: v1.1.0…v1.1.3, v1.2.0 (adds Control Mapping model), v1.2.1–v1.2.3 (v1.2.3 dated 07 Aug; year shown on the release list inferred as 2026 — **verify**) | Public domain (NIST work; not re-verified here) | **Emit** from the F# model; validate in CI; do not hand-author | authoritative |
| **compliance-trestle / OSCAL Compass** (SRC-PAC-019) | Python toolkit: OSCAL split/merge, schema enforcement, markdown authoring governed by templates, CI on git, detached signing of JSON | CNCF sandbox; originally IBM; v5 stable (OSCAL 1.2.1); v4 maintenance until 2026-12-31 | Apache-2.0 | Validator for emitted OSCAL; fallback emitter if Echelon goes OSCAL-first | authoritative |
| **FedRAMP machine-readable packages** (SRC-PAC-020) | NTC-0009 (2026-03-25): comprehensive machine-readable authorization data required only for Rev5 Class D; text formats for A–C; human-readable materials must be generated from machine-readable materials; OSCAL Foundation named as partner, OSCAL not named as mandatory | Final rules in "Consolidated Rules for 2026" (not read) | — | Signals direction; Echelon has no federal scope identified | authoritative |
| **GRC SaaS (Vanta, Drata, Secureframe, etc.)** | Not researched in this batch | — | commercial | Out of scope for PAC; vendor batch should evaluate as evidence *consumers* of the envelope | open-question |

## 3. Cloud-native and platform enforcement points

| Mechanism | Semantics (primary source) | Preventive/detective | Key limit | Class |
|---|---|---|---|---|
| **AWS SCPs** (SRC-PAC-011) | Never grant; cap maximum permissions for member-account principals including root | Preventive | Do not affect the management account or service-linked roles | authoritative |
| **AWS Config rules / conformance packs** (SRC-PAC-012) | Collections of managed/custom rules + remediation actions, deployable org-wide | Detective (+ remediation) | Evaluation is after the fact | authoritative |
| **Azure Policy** (SRC-PAC-013) | JSON definitions; effects deny/audit/modify/deployIfNotExists/denyAction; 24h compliance cycle; identity-aware via `requestContext().identity` | Both | Assignments use the latest definition (definition changes take effect immediately — version via policy-as-code) | authoritative |
| **GCP Organization Policy** (SRC-PAC-014) | Constraints (managed/custom), hierarchy inheritance, dry-run mode | Preventive | New policy does not remediate existing violations | authoritative |
| **GitHub rulesets / branch protection** (SRC-PAC-015, -016) | Layered rules, most restrictive wins; bypass lists; readable by anyone with read access; required checks can pin an **expected source app**; "do not allow bypassing" applies to admins | Preventive | Org-wide rulesets need Enterprise; any writer can set a status unless source pinned | authoritative |
| **GitHub push protection** (SRC-PAC-017) | Blocks secrets on push; bypass requires reason and generates alert + audit-log event; delegated bypass available | Preventive with audited override | Requires Secret Protection for private repos | authoritative |

## 4. Evidence integrity and storage

| Tool | What it is | Maturity | Licence | Fit | Class |
|---|---|---|---|---|---|
| **Sigstore** (cosign, Fulcio, Rekor) (SRC-PAC-021) | Keyless signing: OIDC identity → short-lived Fulcio cert; signature recorded in Rekor append-only log | Widely used (not re-verified) | Apache-2.0 (not re-verified) | Sign CI-produced evidence and artifacts with workflow identity | authoritative |
| **in-toto / SLSA provenance** (SRC-PAC-022, -023) | Attestation framework (Statement + predicate); SLSA provenance predicate `https://slsa.dev/provenance/v1`; SLSA v1.2 current | Stable specs | Apache-2.0 / CC (not re-verified) | Use in-toto Statement as the *payload format* for build/deploy evidence | authoritative |
| **Tessera** (SRC-PAC-029) | Go library for tile-based transparency logs (C2SP tlog-tiles); successor approach to Trillian v1; drivers GCP/AWS/POSIX | "Production ready since Beta"; migration from Trillian still beta | Apache-2.0 | Candidate engine for the evidence log (§architecture) | authoritative |
| **C2SP tlog-checkpoint / tlog-tiles / tlog-witness** (SRC-PAC-027, -028) | Signed-note checkpoints; static tiles; witnesses verify consistency proofs before cosigning, preventing split views | Community specs | CC-BY (not re-verified) | Witnessing gives independent append-only assurance | authoritative |
| **RFC 3161 TSA** (SRC-PAC-024) | TSA signs a hash imprint + genTime: proves datum existed before a time; trusted third party | Proposed Standard (2001), updated by RFC 5816 | — | Timestamp checkpoints, not individual records | authoritative |
| **RFC 6962 / 9162 CT** (SRC-PAC-025, -026) | Merkle log; inclusion and consistency proofs; consistency proves append-only; logs still trusted third parties absent gossip/witnesses | Experimental RFCs; 9162 obsoletes 6962 | — | Design basis | authoritative |
| **S3 Object Lock** (SRC-PAC-030) | WORM; compliance mode: no user incl. root can delete/shorten; only account deletion; governance mode bypassable with permission; legal holds; Cohasset assessed for SEC 17a-4/CFTC/FINRA | GA | AWS service | Primary candidate store if AWS | authoritative |
| **Azure immutable blob** (SRC-PAC-031) | Time-based retention + legal hold; must **lock** policy for SEC 17a-4(f) compliant state; Cohasset assessed | GA | Azure service | Equivalent on Azure | authoritative |
| **GCS Bucket Lock / Object Retention Lock** (SRC-PAC-032) | Retention up to 100 years; locking irreversible; project lien | GA | GCP service | Equivalent on GCP | authoritative |
| **CloudTrail log file validation** (SRC-PAC-033) | SHA-256 per log file; hourly RSA-signed digest files chained to previous digest; detects modification/deletion and lets you assert no files were delivered in a period | GA | AWS service | Model for our own digest chain; source of IAM evidence | authoritative |
| **Amazon QLDB** (SRC-PAC-034) | Ledger DB; **end of support 2025-07-31**; AWS recommends Aurora PostgreSQL migration | Retired | — | **Do not use** | summary-only |
| **immudb** (SRC-PAC-035) | Append-only DB, Merkle commit log, client-side verification | Active | **Business Source License 1.1** for v1.9DOM.2+ (not OSI open source; Additional Use Grant permits production use except a competing hosted/embedded offering) | Possible but licence and single-vendor risk; prefer Tessera + object lock | authoritative |
| **OpenTelemetry logs** (SRC-PAC-036) | Log data model (Timestamp, ObservedTimestamp, TraceId, SpanId, Resource, Attributes) | CNCF | Apache-2.0 | Transport/correlation for evidence events; **provides no integrity guarantees** on the page read | authoritative |

## 5. Overall verdict

1. **Adopt** a limited F# authoritative policy model (intent + bindings +
   evidence requirements), extending Praxis/Tutela patterns [recommendation].
2. **Integrate**, when justified (OQ-506, DF-ASR-2026-0D11), OSCAL as an emitted interchange format validated by NIST
   schemas/trestle [recommendation].
3. **Adopt** cloud-native and GitHub controls as enforcement points, bound
   from the model, with drift monitoring [recommendation].
4. **Integrate** Rego only at enforcement points that already embed OPA;
   **prototype** Cedar for tenant-facing authorization [recommendation; option only under the OQ-509 disposition: F# pure deciders by default, no new runtime dependency approved].
5. For evidence integrity, **combine** WORM object storage (Object Lock
   compliance mode or equivalent) with a Tessera tlog over envelope hashes,
   external witnesses, and RFC 3161 timestamps on checkpoints; **reject QLDB**
   (retired) and **defer immudb** (BSL) [recommendation].
