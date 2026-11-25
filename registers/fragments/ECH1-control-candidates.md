# ECH1 control candidates (Echelon systems reuse)

Proposals only. Status of every candidate: `Proposed`. No CTL ids assigned.
Sources: `sources/fragments/ECH1.csv` (SRC-ECH1-nnn). Open questions:
`registers/fragments/ECH1-open-questions.csv` (OQ-700..OQ-710). Systems:
`integration/fragments/ECH1-system-register.csv`.

Evidence class of every "mechanism exists" statement below is
`authoritative` only in the narrow sense that it was read from the Echelon
repository's own code or docs (`echelon-internal`); whether an auditor accepts
it is `open-question` unless stated.

## CC-1 Platform-enforced merge gate (SDL)

- **Objective:** No change reaches the default branch of an in-scope repository
  unless the required validations pass on the exact candidate commit.
- **Risk:** Agent or human merges/pushes untested or unauthorized changes;
  Praxis/Dokimos gates bypassed because they are advisory.
- **Mechanism:** GitHub ruleset on `main`: require PR, require status checks
  `Praxis validation`, Dokimos quality gate (`actions/quality-gate`, `enforce: true`,
  `revision` = PR head SHA) and Praxis `independent-review`, from the GitHub
  Actions app only; block force-push and deletion (Praxis decision package D-2/D-3,
  SRC-ECH1-007; Dokimos gate SRC-ECH1-011).
- **Automation target:** `L4` - preventive enforcement is native to the
  platform; `L5` not appropriate (no safe automated remediation of a blocked merge).
- **Failure mode:** fail-closed (missing/failed check blocks merge). Ruleset bypass list must be empty or owner-only and logged.
- **Evidence:** ruleset JSON export (periodic, diffed); per-PR check-run records at head SHA; Praxis merge receipts (`praxis.merge-receipt/1`, hash-chained).
- **Independence:** enforcement by GitHub (outside agent control); configuration change is an owner act. Today: not evidenced (OQ-700).

## CC-2 Attested verified completion (SDL / ASR)

- **Objective:** A work item is recorded complete only when a host verifier,
  whose key the agent never holds, signed a passing receipt bound to the
  commit, work item, validation policy and Ordo contract.
- **Risk:** Agent forges test evidence (Praxis REL-001 C3b / GATE-3 showed unattested evidence is forgeable; SRC-ECH1-003).
- **Mechanism:** `ros.json` `workProtocol.requireVerifiedCompletion: true`, no
  `allowUnattestedEvidence`, `trustedVerifierKeyIds` pinned on the protected
  branch; Dokimos `completion verify` in attested mode (SRC-ECH1-005, SRC-ECH1-012).
- **Automation target:** `L4` - already implemented as preventive refusal in Praxis; needs only configuration, key custody (OQ-703) and version upgrade (OQ-704).
- **Failure mode:** fail-closed (`PRX.COMPLETION.*` refusals).
- **Evidence:** claim records with `attestation: attested` and `verifierKeyIds`; DSSE receipts; `work.completion.verified` events.
- **Independence:** automated, internal (not independent assurance). Residual: a process that can rewrite local protected refs controls the protected source (doc-stated) - pair with CC-1 and CC-5.

## CC-3 Authenticated actor binding for agents (IAM / AI)

- **Objective:** Every agent action in evidence is attributable to an authenticated, agent-specific principal, distinct from the owner.
- **Risk:** Self-reported provenance (SRC-ECH1-002) or shared owner credentials make SoD and access populations unreliable (Praxis SECURITY-REVIEW R-6).
- **Mechanism:** dedicated GitHub App/bot identity per agent class; record OIDC/App installation identity in the Praxis execution `executor` block (the remote-execution design already separates requester, executor and transport principal).
- **Automation target:** `L3` - automated validation that every merged PR's actor maps to a registered principal; preventive enforcement depends on platform.
- **Failure mode:** detective-only initially; fail-closed once CC-1 restricts merge to registered identities.
- **Evidence:** identity inventory; per-PR actor list reconciled with Praxis execution records.
- **Independence:** platform-authenticated. Acceptability: OQ-702, OQ-706.

## CC-4 Role separation for implement / verify / approve (AI / SDL)

- **Objective:** The execution that implements a change cannot record its verdict, accept its review, or change acceptance criteria.
- **Risk:** Self-approval by the same agent run.
- **Mechanism:** Ordo `RoleAuthority.defaultFor` matrix (Implementation prohibits `RecordVerdict`, `AcceptReview`; no role holds `ModifyAcceptanceCriteria`/`ModifyEvaluationAuthority`; widening needs a named policy decision) (SRC-ECH1-015), bound to authenticated identities from CC-3; EEF R2 "independent-verification" executor for independence obligations (SRC-ECH1-026).
- **Automation target:** `L4` for agent-to-agent separation; human approval steps stay `L0`-`L2` with recorded evidence.
- **Failure mode:** fail-closed when a host enforces it; today it is a library model only (enforcement not verified).
- **Evidence:** execution records showing distinct execution IDs/roles for implement vs verify vs review; Ordo receipts.
- **Independence:** same-organization automation; single-owner SoD acceptability is OQ-701.

## CC-5 Evidence-store integrity anchoring (ASR / LOG)

- **Objective:** Detect any rewrite or deletion of evidence held in Git (Praxis `.ros/events`, executions, Dokimos `dokimos-evidence` branch, Project Administration `installations/events`, echelon-assurance registers).
- **Risk:** Append-only-by-convention stores (SRC-ECH1-006, SRC-ECH1-013, SRC-ECH1-018) silently rewritten on an unprotected branch.
- **Mechanism:** scheduled job computes a digest manifest (per-file SHA-256 + commit SHA) and stores it outside the repository in write-once storage or an external timestamp/transparency log; next run verifies continuity. Reuses Praxis's existing content digests and the autonomy receipt chain pattern.
- **Automation target:** `L3` (automated validation); `L4` not appropriate because the store must remain writable.
- **Failure mode:** detective-only; missing anchor run raises an alert.
- **Evidence:** anchored manifests; continuity verification results.
- **Independence:** anchor storage must not be writable by agents. Auditor acceptance: OQ-705.

## CC-6 Signed, exact-scope approvals for consequential decisions (GOV)

- **Objective:** Policy approvals, risk acceptances and control exceptions carry a verifiable approver signature bound to the exact content and an expiry.
- **Risk:** Approval inferred from a commit, a chat or a JSON `approved: true` written by an agent.
- **Mechanism:** generalize the Praxis `ecir.approval/1` design (ECDSA P-256, host allow-list of signer keys, domain-separated canonical payload, max validity, revocation by removal) (SRC-ECH1-004); Clarity decision-record fields (owner, posture, reconsideration criteria) as the record body (SRC-ECH1-023).
- **Automation target:** `L3` - automated verification of signatures and scope; the decision itself stays human (`L0`).
- **Failure mode:** fail-closed for any automation consuming the approval.
- **Evidence:** signed approval receipts, signer key inventory, verification log.
- **Independence:** human approver; key custody OQ-703.

## CC-7 Toolchain supply-chain verification (SDL / VEN)

- **Objective:** Only pinned, integrity-verified releases of Echelon tooling run in CI and agent environments.
- **Risk:** Tampered or floating tool versions alter gates or evidence.
- **Mechanism:** pinned versions + SHA-256 (Dokimos action `package-sha256`; Praxis CI pins Dokimos nupkg SHA); verify Sigstore build provenance already emitted by Praxis, Ordo, Dokimos and Limen release workflows (SRC-ECH1-010); adopt Registry signed release attestations when implemented (SRC-ECH1-017, OQ-709).
- **Automation target:** `L4` - verification can block installation.
- **Failure mode:** fail-closed.
- **Evidence:** install logs with digests and attestation verification output.
- **Independence:** Sigstore/GitHub attestation is third-party-anchored; Registry signing is internal.

## CC-8 Tooling inventory and version-drift detection (CFG)

- **Objective:** Know which governance tool version (and therefore which control behaviour) applied to each repository at any date.
- **Risk:** Controls claimed from a Praxis feature that the repository's pinned version lacks (observed: assessed repos pin rosVersion 1.0.0-3.7.1 while attested completion is later).
- **Mechanism:** Project Administration installation events (append-only, artifact-digest conflict refusal) reconciled with each repo's `ros.json`/`.echelon/toolchain.json`; Registry catalog for expected versions (SRC-ECH1-017, SRC-ECH1-018).
- **Automation target:** `L3` (automated validation/report); upgrades remain change-managed.
- **Failure mode:** detective-only.
- **Evidence:** installation-current projection, drift report per period.
- **Independence:** internal automation.

## CC-9 Source-required validation of assurance records (ASR)

- **Objective:** Every requirement/evidence record in echelon-assurance cites a source and flags stale evidence.
- **Risk:** Unsupported or stale conclusions enter registers.
- **Mechanism:** existing `tools/validate_registers.py` in CI plus EKS-style rules (`evidence-without-source` blocking; `stale-evidence` >180 days advisory) (SRC-ECH1-021); Praxis `validate` for front matter/IDs/supersession once installed (SRC-ECH1-008).
- **Automation target:** `L4` in CI (blocks merge via CC-1).
- **Failure mode:** fail-closed.
- **Evidence:** CI validation logs per commit.
- **Independence:** internal automation; content review remains human.

## CC-10 Protected gate configuration (SDL)

- **Objective:** Changes that weaken gates (workflows, `ros.json` workProtocol, `.dokimos/policy.json`, verifier key pins, test deletions) require owner approval.
- **Risk:** Candidate weakens its own verifier.
- **Mechanism:** Praxis protected-policy comparison against merge base (`PRX.COMPLETION.POLICY_CHANGED`) and autonomy `REQUIRES_OWNER` classification (SRC-ECH1-003, SRC-ECH1-007); CODEOWNERS + ruleset required review on those paths (none observed in-repo).
- **Automation target:** `L4`.
- **Failure mode:** fail-closed.
- **Evidence:** refusal events; PR review records on protected paths.
- **Independence:** owner approval; doc-stated residuals (gutted tests, CI data files) need Dokimos coverage ratchets.

## CC-11 Agent authority limits by design (AI)

- **Objective:** Agents cannot grant themselves permissions, perform human-reserved operations, or clear a risk flag.
- **Risk:** Over-privileged autonomous agents.
- **Mechanism:** EEF agent-catalog checks (closed tool allowlist, no human-reserved operation, no clearance under `NoAiClearance`) applied to the real agent configuration, not only the generated model (SRC-ECH1-026); Praxis remote execution allow-listed child environment (SRC-ECH1-002).
- **Automation target:** `L3` - automated validation of agent configuration; runtime enforcement by the platform credential scope.
- **Failure mode:** fail-closed at configuration validation.
- **Evidence:** agent catalog, credential scope export, validation output.
- **Independence:** internal; credential scope must be set by the owner.
