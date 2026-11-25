---
research_key: PAC
title: Control candidates — policy-as-code and evidence system
status: draft
created: 2026-11-25
sources: sources/fragments/PAC.csv
---

# PAC control candidates

Proposed only. No CTL ids assigned (the coordinator assigns them). No control
here is implemented or operating. Automation levels per `controls/README.md`.
"Independence" states who must validate the control for the validation to be
meaningful; Echelon-run automation is never independent attestation.

## A. Assurance-system integrity (ASR domain — controls over the evidence machinery)

### PAC-C01 Evidence ingest validation (schema + authority)
- **Objective:** Only well-formed envelopes from authorised producers for registered evidence types enter the evidence store.
- **Risk:** Forged, malformed or PI-bearing records polluting the permanent record; unauthorised producer writes.
- **Mechanism:** Ingest gateway validates against `evidence-envelope.schema.json`, verifies signature/OIDC identity, applies a deny-by-default evidence-authority policy (Tutela `AuthorityPolicy` pattern: evidence type × provenance kind × issuer × producer identity kind), DLP-scans envelope fields.
- **Target automation:** L4 — preventive rejection is cheap and deterministic; L5 unnecessary.
- **Failure mode:** fail-closed (record not accepted; producer retries; persistent failure = incident). Rationale: an accepted bad envelope can never be removed from an append-only log.
- **Evidence:** EVT-022 (rejection counts within reconciliation), ingest decision events (EVT-018).
- **Independence:** Gateway rule changes reviewed by someone other than the collector owner; auditor re-performance on sample.

### PAC-C02 Append-only evidence log with signed checkpoints
- **Objective:** Any deletion or alteration of logged evidence is detectable.
- **Risk:** Insider or compromised admin rewrites history before an audit.
- **Mechanism:** Merkle transparency log (Tessera / tlog-tiles) over envelope digests; signed checkpoints; verifier checks consistency proofs between successive checkpoints.
- **Target automation:** L3 (continuous automated validation of consistency).
- **Failure mode:** detective-only (cannot block a rewrite at the storage layer; detection is the control) — combined with PAC-C04 to make rewrites costly.
- **Evidence:** EVT-024 checkpoints; verifier results.
- **Independence:** Requires PAC-C03 for independence from the log operator.

### PAC-C03 External witnessing and timestamping of checkpoints
- **Objective:** Append-only property and time bounds are assured by parties Echelon does not control.
- **Risk:** Log operator presents split views or rolls back (RFC 9162 trust caveat).
- **Mechanism:** Submit each checkpoint to ≥2 external witnesses (C2SP tlog-witness) and ≥1 RFC 3161 TSA (prefer 2); publish checkpoints to an independent location; give auditor a verifier.
- **Target automation:** L3.
- **Failure mode:** fail-open-justified for TSA outage (witness still provides consistency; retry second TSA); witness refusal = high-severity incident.
- **Evidence:** EVT-024, EVT-025.
- **Independence:** External witnesses/TSA; auditor-run verification (OQ-500, OQ-505).

### PAC-C04 Retention-locked payload storage
- **Objective:** Evidence payloads cannot be deleted or overwritten during retention, including by administrators.
- **Risk:** Destruction of evidence; ransomware.
- **Mechanism:** S3 Object Lock compliance mode / Azure locked time-based retention / GCS locked retention, in a dedicated account isolated from production; guardrail (SCP/Org Policy/Azure Policy deny) preventing retention-config changes; daily config snapshot.
- **Target automation:** L4 (platform-enforced).
- **Failure mode:** fail-closed (writes without retention denied by bucket default + policy).
- **Evidence:** EVT-029, EVT-030.
- **Independence:** Cloud provider enforcement; config reviewed by internal assurance. Note: locked retention is irreversible — needs OQ-502 answered first.

### PAC-C05 Evidence completeness reconciliation
- **Objective:** Every event in a control's population has corresponding evidence; omissions detected.
- **Risk:** Selective omission (integrity controls only protect what was written).
- **Mechanism:** Per-source reconciler lists source-of-truth population (GitHub merged PRs, deployments, IAM principals, CloudTrail digest manifests) and diffs against evidence store; per-stream sequence gap detection; collector heartbeats. Reconciler code path separate from collector code path.
- **Target automation:** L3 (L4 not appropriate — blocking production because evidence is late would couple availability to assurance).
- **Failure mode:** detective-only; control status becomes "unknown", never pass.
- **Evidence:** EVT-022, EVT-023.
- **Independence:** Reconciler owned by a role other than collector owner; auditor re-performs a reconciliation from source.

### PAC-C06 Collector and evidence-system change control
- **Objective:** Changes to collectors, reconciler, ingest rules, schema and log configuration are reviewed and traceable (accuracy is not guaranteed by integrity).
- **Risk:** Collector bug or malicious change produces signed-but-wrong evidence.
- **Mechanism:** Evidence-system repos treated as Praxis `ProtectedClass` (never merged unattended); required checks with pinned producer app; collector `software_digest` recorded in every envelope; golden-data tests per collector.
- **Target automation:** L4.
- **Failure mode:** fail-closed (merge blocked).
- **Evidence:** EVT-002, EVT-003, EVT-006 for evidence-system repos.
- **Independence:** Reviewer ≠ author; periodic independent re-performance of sampled evidence against source.

### PAC-C07 Evidence signing-key management
- **Objective:** Envelope signatures are attributable and keys cannot be exfiltrated or misused.
- **Risk:** Forged evidence with valid signatures.
- **Mechanism:** CI uses keyless Sigstore (OIDC workflow identity, Rekor record); non-CI collectors use non-exportable KMS keys; log/checkpoint keys held by a role separated from producers; rotation logged.
- **Target automation:** L4.
- **Failure mode:** fail-closed (unsigned/unknown-key envelopes rejected by PAC-C01).
- **Evidence:** KMS audit events (EVT-008), EVT-030.
- **Independence:** Key custodians distinct from evidence producers.

### PAC-C08 Evidence-store access segregation and break-glass logging
- **Objective:** No single role can both produce and alter/hide evidence; privileged access is visible.
- **Risk:** Collusion or insider abuse.
- **Mechanism:** Role model in architecture §9; break-glass via time-bound elevation with mandatory evidence envelope; quarterly access review of evidence-store roles.
- **Target automation:** L4 for role enforcement, L2 for review evidence.
- **Failure mode:** fail-closed.
- **Evidence:** EVT-030, EVT-010.
- **Independence:** Review by someone outside evidence-system operations.

### PAC-C09 Evidence-store recovery test
- **Objective:** Evidence and log can be restored and re-verified after loss.
- **Risk:** Loss of evidence store destroys audit capability.
- **Mechanism:** Cross-region replica (retention-locked), periodic restore + full consistency re-verification from witnessed checkpoints.
- **Target automation:** L2 (scheduled test, evidence collected).
- **Failure mode:** detective-only.
- **Evidence:** EVT-015.
- **Independence:** Internal-independent.

### PAC-C10 Personal-data minimisation and crypto-shredding in evidence
- **Objective:** Evidence store satisfies deletion obligations without breaking integrity.
- **Risk:** Immutable store retains personal data beyond lawful basis.
- **Mechanism:** Pseudonymous actor IDs; keyed-HMAC subject refs; PI payloads encrypted per subject with shreddable KMS keys; erasure record envelope.
- **Target automation:** L4 at ingest (DLP rejection), L2 for erasure execution.
- **Failure mode:** fail-closed at ingest.
- **Evidence:** EVT-027.
- **Independence:** Privacy/legal review (OQ-503).

### PAC-C11 Unknown-is-not-pass status derivation
- **Objective:** Dashboards and assurance documents never present missing/stale/unverified evidence as compliant.
- **Risk:** Status inflation (AGENTS rule 3).
- **Mechanism:** Status function derives control status only from envelopes with `validation.status` ∈ {authority-accepted, reconciled}, `collection.kind` ≠ inferred where observation is required, `synthetic` = false, freshness within cadence; else `unknown`.
- **Target automation:** L4 (in the rendering pipeline).
- **Failure mode:** fail-closed (render unknown).
- **Evidence:** Rendered outputs carry evidence IDs; spot-check by internal assurance.
- **Independence:** Internal-independent review of status function changes.

## B. Governance and SDLC controls for policy-as-code

### PAC-C12 Policy model validation and approval binding (GOV)
- **Objective:** Every published policy version is structurally valid and approved by its approval authority for that exact content.
- **Risk:** Unapproved or drifted policy text in force.
- **Mechanism:** Pure `Policy.validate` (PAC-V001…V007) as required check; approval = signed commit/tag by holder of approval-authority role binding `PolicyDigest`; CODEOWNERS/ruleset requiring that role's review.
- **Target automation:** L4.
- **Failure mode:** fail-closed.
- **Evidence:** EVT-021.
- **Independence:** Approver ≠ author; auditor acceptance open (OQ-501).

### PAC-C13 Enforcement binding integrity (no orphan or drifted rules) (GOV/CFG)
- **Objective:** Every enforcement artifact (Cedar/Rego/F# decider/guardrail spec) is bound to a policy by path+digest+tests, and vice versa.
- **Risk:** Rule changes bypass policy approval; duplicated sources of truth.
- **Mechanism:** CI conformance job generated from the model (spec §6.2).
- **Target automation:** L4.
- **Failure mode:** fail-closed.
- **Evidence:** EVT-003 for the conformance check.
- **Independence:** Automated; reviewed in PR.

### PAC-C14 Live enforcement drift detection (CFG)
- **Objective:** Live platform enforcement (GitHub rulesets, SCPs, Azure Policy, Org Policy, retention locks) matches the emitted intended spec.
- **Risk:** Console changes or bypass lists silently weaken controls.
- **Mechanism:** Scheduled snapshots compared with emitted specs; drift → finding; auto-revert only where the control row justifies L5.
- **Target automation:** L3 (L5 selectively).
- **Failure mode:** detective-only.
- **Evidence:** EVT-006, EVT-028.
- **Independence:** Drift monitor not writable by platform admins.

### PAC-C15 Required-check producer pinning (SDL)
- **Objective:** Required status checks can only be satisfied by the designated CI app.
- **Risk:** Any writer (including an agent token) sets a passing status (GitHub docs, SRC-PAC-016).
- **Mechanism:** Ruleset required checks with expected source app; envelope records `producer_app_id`; reconciliation rejects mismatches (Praxis `CheckProducer`).
- **Target automation:** L4.
- **Failure mode:** fail-closed.
- **Evidence:** EVT-003, EVT-006.
- **Independence:** Automated; auditor inspects ruleset config.

### PAC-C16 Exception lifecycle with expiry and non-self, human approval (GOV)
- **Objective:** Exceptions are time-bound, approved by an authorised human who is not the requester, and visible in evidence.
- **Risk:** Permanent or self-granted exceptions erode controls; agents approving exceptions.
- **Mechanism:** Pure `evaluateException` (Tutela pattern: rejects SelfApproval, ApproverNotHuman, Expired, ApproverLacksRole); mandatory `ExpiresAt`; expiry alerts; push-protection bypasses ingested as exception evidence.
- **Target automation:** L4 for validity checks, L1 for expiry reminders.
- **Failure mode:** fail-closed (invalid exception does not suppress a control failure).
- **Evidence:** EVT-026, EVT-012.
- **Independence:** Approver independence enforced by role registry.

### PAC-C17 Agent instructions are never the sole enforcement (AI)
- **Objective:** Every obligation communicated to agents via generated instructions has a non-agent enforcement or detection point.
- **Risk:** Agents ignore or misread instructions.
- **Mechanism:** Model validation rule: a MandatoryControl whose only mechanism is agent instruction fails validation.
- **Target automation:** L4.
- **Failure mode:** fail-closed (policy cannot publish).
- **Evidence:** EVT-021 validation output.
- **Independence:** Automated.
