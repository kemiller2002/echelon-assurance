# AIG control candidates (domains AI and ASR)

Proposed by research batch B6 (key AIG), 2026-11-25. Labels `AI-n` / `ASR-n`
are temporary; the coordinator assigns `CTL-` IDs. All candidates are
`Proposed`. Evidence class of each design: `recommendation` unless stated.
Automation levels per `controls/README.md` (L0–L5). Independence = minimum
validation independence needed before the control may be called `Tested`.

Design basis: `ai-governance/ai-governance-model.md` (G§),
`validation/independent-validation-architecture.md` (V§),
`validation/assurance-system-resilience.md` (R§).

## Domain AI — AI and autonomous agent governance

### AI-1 Agent identity and capability manifest
- **Objective:** every agent action is attributable to an authenticated, role-scoped workload identity with a declared capability manifest.
- **Risk:** RSK-011, RSK-020. **Requirements:** REQ-OWASPAI-005, REQ-OWASPAI-010, REQ-AIRMF-001, REQ-SSDFAI-004.
- **Mechanism:** per-role short-lived workload identity (OIDC/cloud workload identity); manifest (tools, data scope, tier ceiling, budget) as policy input; effective permission = intersection of role, delegator and task; Praxis provenance linked to identity subject (OQ-615). G§5.
- **Target automation:** L4 — enforcement is preventive at issuance; L5 not applicable.
- **Failure mode:** fail-closed (no identity → no tool access).
- **Evidence:** identity issuance logs; manifest versions; policy decision logs.
- **Validation / independence:** VAL-013, VAL-017; automated, plus internal-independent review annually.

### AI-2 No agent approval authority (self-approval prohibition)
- **Objective:** agents cannot approve, merge, accept risk or close findings; approvals bind to exact artifact hash and require approver independence.
- **Risk:** RSK-002, RSK-019. **Requirements:** REQ-AIRMF-005, REQ-OWASPAI-005.
- **Mechanism:** approval service accepting only human principals with phishing-resistant authentication; signed approval over proposal+evidence hash; SoD rule excludes requester, preparer and delegation chain. G§4 P1; reuses GOV-AUTH-003/009, RR-AIN-003-r1.
- **Target:** L4 (preventive). Approval itself stays human by design.
- **Failure mode:** fail-closed.
- **Evidence:** approval records with hashes; denial logs.
- **Validation:** VAL-004, VAL-015, VAL-017; key control → internal-independent (VAL-025).

### AI-3 Evidence class separation (anti-fabrication)
- **Objective:** only collectors write primary evidence; AI-interpreted content is labelled and never sole support for a control conclusion.
- **Risk:** RSK-003. **Requirements:** REQ-AIRMF-013, REQ-OWASPAI-006.
- **Mechanism:** typed evidence store; collector signing keys non-exportable; agents lack write permission to primary class; citation resolver. G§4 P2, G§13.
- **Target:** L4 for write separation; L3 for citation validation.
- **Failure mode:** fail-closed (uncited AI output rejected).
- **Evidence:** store ACLs; signature verification logs; citation-check results.
- **Validation:** VAL-011, VAL-019 (internal-independent), VAL-020.

### AI-4 Outbound independence-claim gate
- **Objective:** no outward statement presents internal validation as independent or external attestation.
- **Risk:** RSK-006. **Requirements:** (AGENTS.md rule 4; SOC batch to map).
- **Mechanism:** publishing pipeline lint against attestation vocabulary unless linked to an external-attestation record; human signatory. G§4 P5.
- **Target:** L4 within the pipeline; L1 outside it (training, answer library).
- **Failure mode:** fail-closed for pipeline publications.
- **Evidence:** lint results; signed publication records.
- **Validation:** VAL-024; internal-independent sampling of published text.

### AI-5 AI egress gateway and provider allow-list by data class
- **Objective:** confidential evidence reaches only providers/models with approved terms for that class.
- **Risk:** RSK-009, RSK-039. **Requirements:** REQ-OWASPAI-002, REQ-AIRMF-004.
- **Mechanism:** agents without direct egress; gateway enforcing (provider, model, region, terms) per classification; default-restricted labels; canary tokens; DLP. G§4 P8, G§12. Gap: no existing Echelon system identified.
- **Target:** L4.
- **Failure mode:** fail-closed (unknown class or provider → block).
- **Evidence:** gateway decision logs; allow-list versions with approval; provider terms reference (restricted store).
- **Validation:** VAL-004 (egress attempts), VAL-009; external-penetration includes gateway (VAL-027).

### AI-6 Credentials outside model context
- **Objective:** agent credentials are short-lived, task-scoped and never visible to the model.
- **Risk:** RSK-010. **Requirements:** REQ-OWASPAI-010.
- **Mechanism:** tool runtime injects credentials; secrets scanning on traces; centralised revocation. G§6.
- **Target:** L4. **Failure mode:** fail-closed.
- **Evidence:** credential issuance/revocation logs; scan results.
- **Validation:** VAL-004, VAL-013; pen test.

### AI-7 Agent execution sandbox and budgets
- **Objective:** agents execute only within bounded environments, tools and budgets.
- **Risk:** RSK-037, RSK-038, RSK-017. **Requirements:** REQ-OWASPAI-007, REQ-OWASPAI-009, REQ-OWASPAI-011, REQ-OWASPAI-012, REQ-SSDFAI-005.
- **Mechanism:** sandbox (reuse Praxis containment after B1 evaluation), egress proxy, writes via governed APIs/PRs, signed inter-agent messages, versioned memory corpora, budget stop with explicit `incomplete`. G§7.
- **Target:** L4. **Failure mode:** fail-closed.
- **Evidence:** sandbox launch records; budget events; corpus versions.
- **Validation:** VAL-004, VAL-008; automated.

### AI-8 Prompt-injection containment
- **Objective:** a hijacked agent cannot cause a consequential effect.
- **Risk:** RSK-001. **Requirements:** REQ-OWASPAI-001, REQ-OWASPAI-004, REQ-OWASPAI-008, REQ-AIRMF-016, REQ-SSDFAI-003.
- **Mechanism:** A0–A2 ceiling; quarantine/structured extraction of untrusted content; deterministic authorization; output validation; anomaly alerts. G§11. Evidence basis for assuming injection succeeds: SRC-AIG-018 (authoritative research finding).
- **Target:** L4 for bounds; L3 for detection.
- **Failure mode:** fail-closed for tool calls outside manifest.
- **Evidence:** denial logs; injection test results.
- **Validation:** VAL-005 (automated), VAL-006 (internal-independent), VAL-027 (external-penetration).

### AI-9 AI-generated code controls
- **Objective:** agent-authored code meets the same SDLC as human code plus agent-specific checks.
- **Risk:** RSK-012. **Requirements:** REQ-SSDFAI-001, REQ-SSDFAI-002.
- **Mechanism:** authenticated agent authorship; no self-merge; mandatory human review on security-relevant paths; test-weakening classified as control weakening; dependency verification; SBOM. G§9.
- **Target:** L4 (branch protection, CI gates); review remains human.
- **Failure mode:** fail-closed (merge blocked).
- **Evidence:** PR records, review approvals, CI results, SBOMs.
- **Validation:** VAL-007 (mutation score), VAL-023; internal-independent review.

### AI-10 Model and provider change control
- **Objective:** no model, prompt, tool or provider change reaches agents without evaluation and approval.
- **Risk:** RSK-016, RSK-015, RSK-036. **Requirements:** REQ-AIRMF-007, REQ-AIRMF-011, REQ-AIRMF-012, REQ-OWASPAI-003, REQ-SSDFAI-002.
- **Mechanism:** pinned model versions; evaluation suite gate; daily drift canary; two qualified providers per role; providers treated as subservice orgs (OQ-602). G§10, G§17.
- **Target:** L3 (automated evaluation and gating; approval human).
- **Failure mode:** fail-closed (regression blocks rollout); drift canary → detective-only for provider-side changes.
- **Evidence:** evaluation results; inventory versions; approvals.
- **Validation:** VAL-021; evaluators independent of developers (internal-independent per MEASURE 1.3).

### AI-11 Bounded autonomous remediation
- **Objective:** automated remediation only restores approved baselines within blast-radius limits and never hides incidents.
- **Risk:** RSK-013, RSK-030. **Requirements:** REQ-AIRMF-009.
- **Mechanism:** pre-approved playbook catalog; restore-only; snapshot first; rollback; circuit breaker; alert preserved. G§14.
- **Target:** L5 for qualifying playbooks only; otherwise L4. Reasoning: L5 is appropriate only where action is reversible, idempotent and baseline-restoring.
- **Failure mode:** fail-closed (no action when bounds cannot be evaluated).
- **Evidence:** remediation records with before/after state.
- **Validation:** VAL-008, VAL-012; internal-independent review of all L5 actions quarterly.

### AI-12 Agent kill switch and degraded mode
- **Objective:** all agents can be suspended from a plane they cannot reach, and controls continue without AI.
- **Risk:** RSK-014, RSK-041. **Requirements:** REQ-AIRMF-008, REQ-AIRMF-004.
- **Mechanism:** centralised revocation + gateway disable; no control failure mode depends on AI; templates and manual runbooks. G§16, G§18; reuses GOV-AUTH-024.
- **Target:** L4 (one-action suspension); exercise L0/L1 scheduled.
- **Failure mode:** fail-closed for agents; controls unaffected.
- **Evidence:** exercise records.
- **Validation:** VAL-022 (internal-independent, witnessed).

### AI-13 Human approval quality
- **Objective:** human approvals are substantive, not rubber stamps.
- **Risk:** RSK-019. **Requirements:** REQ-AIRMF-005, REQ-OWASPAI-013.
- **Mechanism:** approval UI shows diff, weakening class, unchecked items; sampled re-review; metrics (review time, rejection rate).
- **Target:** L2 (metrics collection) — judgment cannot be automated.
- **Failure mode:** detective-only.
- **Evidence:** approval metrics; sample review records.
- **Validation:** VAL-032; internal-independent.

### AI-14 AI incident and error tracking
- **Objective:** AI incidents, errors and near-misses are recorded, responded to and learned from.
- **Risk:** RSK-001, RSK-003, RSK-009. **Requirements:** REQ-AIRMF-010, REQ-AIRMF-014.
- **Mechanism:** incident categories in G§16; error/near-miss register; after-action reviews.
- **Target:** L2 (automatic case creation from denials/alerts); response human.
- **Failure mode:** detective-only.
- **Evidence:** incident records; after-action reports (restricted).
- **Validation:** tabletop within VAL-022; internal-independent.

### AI-15 AI inventory and accountable ownership
- **Objective:** every agent, model, provider and AI use is inventoried with an accountable human owner role.
- **Risk:** RSK-020, RSK-036. **Requirements:** REQ-AIRMF-001, REQ-AIRMF-002, REQ-AIRMF-015, REQ-ISO42001-002.
- **Mechanism:** inventory generated from identity/manifest registry (not hand-maintained); owner field mandatory; agents never owners.
- **Target:** L3 (inventory derived and checked automatically).
- **Failure mode:** fail-closed (unregistered agent gets no identity).
- **Evidence:** inventory snapshots.
- **Validation:** VAL-013 reconciliation of identities vs inventory.

### AI-16 Regulatory change monitoring with human interpretation
- **Objective:** authoritative changes are detected deterministically and interpreted by an accountable human.
- **Risk:** RSK-018. **Requirements:** REQ-AIRMF-003, REQ-ISO42001-005.
- **Mechanism:** scheduled collector fetch, hash and diff of watched sources; agent drafts impact; regulatory owner accepts. G§3 F1.
- **Target:** L3 (detection), interpretation L1.
- **Failure mode:** fail-open-justified (source unreachable → stale flag and task, not silent pass).
- **Evidence:** fetch logs with hashes; acceptance records.
- **Validation:** VAL-020; seeded source change test.

## Domain ASR — assurance-system integrity

### ASR-1 Immutable, externally anchored evidence log
- **Objective:** historical evidence cannot be altered undetectably.
- **Risk:** RSK-005, RSK-042. **Mechanism:** compliance-mode retention lock; hash chain; external timestamp/transparency anchoring; linked corrections. G§4 P4, R§2 R4.
- **Target:** L4 (immutability) + L3 (verification). **Failure mode:** fail-closed (write rejected if chain cannot extend).
- **Evidence:** anchor receipts; verification results. **Validation:** VAL-018; VAL-026 external-technical.

### ASR-2 Expected-evidence completeness
- **Objective:** missing evidence is a failure, never a pass.
- **Risk:** RSK-025. **Mechanism:** expected-evidence schedule and missing-evidence algorithm (reuse Unbound CCM, SRC-AIG-033).
- **Target:** L3. **Failure mode:** fail-closed (status `insufficient-evidence`).
- **Evidence:** schedule vs receipts. **Validation:** VAL-008, VAL-011.

### ASR-3 Append-only control results
- **Objective:** results and findings cannot be deleted or silently downgraded.
- **Risk:** RSK-004. **Mechanism:** append-only result store; closure requires retest + human. G§4 P3.
- **Target:** L4. **Failure mode:** fail-closed.
- **Evidence:** result store; closure records. **Validation:** VAL-015, VAL-031.

### ASR-4 Control-weakening classifier and dual approval
- **Objective:** weakening changes to controls, policies, tests and monitoring require dual human approval.
- **Risk:** RSK-007, RSK-008. **Mechanism:** semantic diff classifier in CI; protected branches; signed policy bundles; admission check. G§4 P6/P7.
- **Target:** L4. **Failure mode:** fail-closed (unclassifiable change treated as weakening).
- **Evidence:** classifier verdicts; approvals. **Validation:** VAL-023, VAL-009.

### ASR-5 Separation of implement / validate / approve
- **Objective:** no single principal, repository or pipeline implements, validates and approves the same control.
- **Risk:** RSK-021, RSK-022. **Mechanism:** separate identities, repositories, pipelines, provider families and direct-source evidence paths; CI rule forbidding control and its validation in one PR. V§5.
- **Target:** L4 (permission and CI enforcement). **Failure mode:** fail-closed.
- **Evidence:** permission analysis; PR records. **Validation:** VAL-017, VAL-013; internal-independent review VAL-025.

### ASR-6 Control mutation testing program
- **Objective:** each key control is shown to detect deliberately introduced failures.
- **Risk:** RSK-034, RSK-007. **Mechanism:** mutation harness, operator catalogue, kill-rate tracking. V§4.1.
- **Target:** L3. **Failure mode:** detective-only.
- **Evidence:** mutation run results. **Validation:** VAL-007; harness changes approved outside implementing team.

### ASR-7 Sentinel and canary controls
- **Objective:** detect a broken or compromised evaluator.
- **Risk:** RSK-024, RSK-034. **Mechanism:** permanently failing sentinel; synthetic known-good/known-bad canaries.
- **Target:** L3. **Failure mode:** detective-only (alert).
- **Evidence:** sentinel/canary outcomes. **Validation:** VAL-010, VAL-031.

### ASR-8 Out-of-band heartbeat and monitor-of-monitors
- **Objective:** silent loss of collectors, evaluators or alert routes is detected.
- **Risk:** RSK-026, RSK-029. **Mechanism:** signed heartbeats to a receiver on separate provider/identity plane; dead-man switch.
- **Target:** L3. **Failure mode:** detective-only.
- **Evidence:** heartbeat logs. **Validation:** VAL-030, VAL-008.

### ASR-9 Privileged access to assurance infrastructure
- **Objective:** no standing admin over evidence, validation or monitoring; every elevation visible out of band.
- **Risk:** RSK-023. **Mechanism:** just-in-time elevation with second-person approval; sealed break-glass; out-of-band alerts. R§2 R1.
- **Target:** L4. **Failure mode:** fail-closed (break-glass is the justified exception, alarmed).
- **Evidence:** elevation logs. **Validation:** VAL-013; VAL-026.

### ASR-10 Independent reconciliation and re-collection
- **Objective:** evidence populations and samples match the source system as read by someone other than the control owner.
- **Risk:** RSK-003, RSK-025. **Mechanism:** reconciliation job and monthly re-collection under separate credentials.
- **Target:** L3 (reconciliation) + L0 (human sample). **Failure mode:** detective-only.
- **Evidence:** reconciliation reports. **Validation:** VAL-011, VAL-019 (internal-independent).

### ASR-11 Assurance infrastructure diversity
- **Objective:** a single cloud or AI provider event cannot remove both function and verification.
- **Risk:** RSK-029, RSK-031, RSK-015. **Mechanism:** anchors, backups and heartbeat receiver outside primary cloud; second AI provider qualified; deterministic controls.
- **Target:** L2. **Failure mode:** n/a (architecture).
- **Evidence:** architecture records; restore tests. **Validation:** VAL-012, VAL-022.

### ASR-12 Independence labelling of every validation result
- **Objective:** each result states who designed and ran it and its independence level; status `Tested` requires an appropriate level.
- **Risk:** RSK-006, RSK-021. **Mechanism:** schema-enforced `independence_level`; status rule (V§5).
- **Target:** L4 (validator rejects unlabeled results). **Failure mode:** fail-closed.
- **Evidence:** validation register. **Validation:** register validator; VAL-024.

### ASR-13 Compliance-theater metrics review
- **Objective:** detect optimisation for appearance over risk reduction.
- **Risk:** RSK-033, RSK-032. **Mechanism:** quarterly indicator review (R§4) with annual external participation.
- **Target:** L2 (metrics) + human review. **Failure mode:** detective-only.
- **Evidence:** review minutes. **Validation:** VAL-032.

### ASR-14 External independent validation schedule
- **Objective:** the assurance machinery and key controls receive external technical review, penetration testing and attestation at defined intervals.
- **Risk:** RSK-021, RSK-035. **Mechanism:** contracted external reviews (OQ-612), pen test incl. agent surface, SOC exam.
- **Target:** L1 (scheduling/reminders). **Failure mode:** n/a.
- **Evidence:** engagement letters and reports (restricted). **Validation:** VAL-026, VAL-027, VAL-028.
