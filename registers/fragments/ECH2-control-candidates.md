---
id: ASR-REG-ECH2-CC
title: Control candidates from Echelon-system and Unbound inventory research (ECH2)
status: draft
created: 2026-11-25
sources: [SRC-ECH2-001..SRC-ECH2-023]
---

# ECH2 control candidates

No CTL ids assigned (coordinator assigns). Status of every candidate:
`Proposed`. "Reuse" names the Echelon system or Unbound/daf-landscape
artefact to build on. Automation levels per `controls/README.md`.
Independence: who must perform validation for the result to carry weight
(internal automation is never "independent", AGENTS.md rule 4).

## A. Financial controls for Unbound (FIN) — primary SOC 1 candidates

### C-FIN-1 Balanced, immutable, authorized journal posting
- **Objective.** Every posted journal balances per book/currency/basis, is never updated or deleted, and is posted only by an authorized principal under a current posting-policy version.
- **Risk.** Misstatement of DAF fund balances, grants payable, fees in the sponsor's statements.
- **Mechanism.** Database-enforced constraints and insert-only privileges (no UPDATE/DELETE grant to any runtime role), posting through a single command boundary with server-resolved principal; corrections only by linked reversal (UNB-ACC-002/003, ACC-MDL-002/003, daf-landscape invariant 3). Strata invariants and drift detection keep the privilege baseline from drifting.
- **Automation.** Target **L4** (preventive enforcement): the database refuses the invalid action; L5 is inappropriate because automated remediation of ledger content is itself a risk.
- **Failure mode.** fail-closed (no posting if constraint or authorization cannot evaluate).
- **Evidence.** Constraint and privilege definitions (Strata artefact + drift report), rejected-posting events (`JournalPostingValidated` incl. denials, UNB-SCTRL-025), periodic full-ledger balance recomputation.
- **Independence.** Design walkthrough + negative tests by internal-independent reviewer; operating effectiveness by external attestation (SOC 1).
- **Reuse.** Summa.Ledger patterns; Strata grants/RLS; Census SQL privilege baseline; Unbound UNB-SCTRL-025, UNB-VAL-011.

### C-FIN-2 Period close lock on every write path
- **Objective.** No posting can be effective-dated into a closed period except through an approved reopen with history preserved.
- **Risk.** Backdated entries altering reported periods (daf-landscape "Critical corrections" notes this must hold on every write path).
- **Mechanism.** Period state checked inside the posting transaction for all entry kinds (operational, adjusting, imports, corrections); reopen requires two-person approval recorded as evidence.
- **Automation.** L4. **Failure mode.** fail-closed.
- **Evidence.** Period state history, reopen approvals, attempted-backdate denials.
- **Independence.** external-attestation for operating effectiveness.
- **Reuse.** Summa `Periods.closeBlockers`; Unbound UNB-ACC-017, UNB-SCTRL-027/028.

### C-FIN-3 Independent bank and custodian reconciliation with break management
- **Objective.** GL and subledgers agree with independent bank, custodian and payment-rail statements each cycle; breaks are aged, owned and block close above threshold.
- **Risk.** Undetected loss, duplicate or missing payments, mispriced holdings.
- **Mechanism.** Automated matching against statements obtained directly from the external source (not via the producer), break cases with owner/due date; the independent source overrides the producer's claim (CCM-T03).
- **Automation.** L3 (automated validation) for matching; break resolution stays human (L1 reminders) because judgment on breaks is required.
- **Failure mode.** detective-only for matching; fail-closed for close (unresolved material break blocks close).
- **Evidence.** Statement source manifest with hashes, match results, break aging, reviewer sign-off by a role outside the payment chain.
- **Independence.** Reviewer outside payment chain (OPS-028); external attestation.
- **Reuse.** Summa `Reconciliation.fs`; Unbound UNB-ACC-018, UNB-SCTRL-024/028, UNB-VAL-010; daf-landscape gate G7. Materiality threshold is an open owner/CPA decision.

### C-FIN-4 Grant approval, payable recognition and payment-instruction maker/checker
- **Objective.** No payable or payment instruction exists without a valid sponsor approval by an authorized, non-conflicted approver on the exact terms; material change after approval invalidates it.
- **Risk.** Unauthorized or altered grants; donor recommendation treated as authority.
- **Mechanism.** Approval bound to a hash of grant terms; SoD check (initiator ≠ approver, including agents); payment instruction requires approval reference; Unbound never originates (CNV-PAY-001).
- **Automation.** L4. **Failure mode.** fail-closed.
- **Evidence.** `SponsorGrantApprovalRecorded`, `DutyConflictEvaluated`, `PaymentInstructionApproved`, denials, invalidations (CCM-T17).
- **Independence.** external-attestation; SoD treatment of deterministic gate is OQ-763.
- **Reuse.** Tutela exception/SoD model; Unbound UNB-SCTRL-001/002/015/026/029, UNB-VAL-001/003/014/023.

### C-FIN-5 Payee/destination bank-detail change verification
- **Objective.** Changes to recipient bank details take effect only after out-of-band verification by someone other than the requester, before the first payment.
- **Risk.** Payment redirection fraud.
- **Mechanism.** Hold on new/changed destination; independent verification record; positive pay and NOC handling per CNV-PAY-002.
- **Automation.** L4 hold; verification itself L0/L1 (human). **Failure mode.** fail-closed.
- **Evidence.** `BankDetailsIndependentlyVerified`, hold/release events.
- **Independence.** external-attestation.
- **Reuse.** Unbound UNB-SCTRL-020, CNV-PAY-002.

### C-FIN-6 Live payment execution prohibition until authorised
- **Objective.** No code path transmits a real payment while the programme is in simulation.
- **Mechanism.** Network egress deny to rail endpoints plus adapter-level refusal and environment attestation; evidence of denied attempts.
- **Automation.** L4. **Failure mode.** fail-closed.
- **Evidence.** `LiveRailInvocationDenied`; egress policy config.
- **Independence.** automated + internal-independent review.
- **Reuse.** Unbound UNB-SCTRL-030, OPS-030, CCM-T18, LIFE-T18.

### C-FIN-7 Fee and NAV recalculation
- **Objective.** Fees and unit values are independently recalculated from source inputs and agree with posted amounts.
- **Mechanism.** Deterministic recompute job from price sources and fee schedule versions; discrepancies open cases; stale prices quarantined.
- **Automation.** L3. **Failure mode.** detective-only (with hold on NAV publication = fail-closed).
- **Evidence.** Recompute outputs, price-source manifests.
- **Reuse.** Unbound UNB-SCTRL-021/022/032.

### C-FIN-8 Journal export completeness to sponsor system of record
- **Objective.** Summarized journals exported to the sponsor ERP are complete, accurate and agree with Unbound's ledger for the period.
- **Risk.** Sponsor statements misstate DAF activity (core SOC 1 interface).
- **Mechanism.** Export control totals and hash manifest; sponsor-side import confirmation; reconciliation of totals. Corresponding CUEC: sponsor reconciles received totals.
- **Automation.** L3. **Failure mode.** detective-only.
- **Evidence.** Export manifest, control totals, acknowledgement.
- **Reuse.** Arca canonical export with per-object SHA-256; Unbound `FULL-ACCOUNTING-SYSTEM.md` (summarized journal export requirement), CUEC-CNV-FIN-*.

### C-FIN-9 IPE definition and completeness for SOC 1-relevant reports
- **Objective.** Each report used in a control has a defined query, parameters, source version and completeness check.
- **Mechanism.** IPE register; reports emitted with query hash, parameters, row counts and source revision (Percepta-style envelope).
- **Automation.** L2–L3. **Failure mode.** detective-only.
- **Reuse.** Percepta evidence envelope; resolves Unbound gap "evidence_query to be defined" (OQ-758).

## B. Assurance-system integrity (ASR)

### C-ASR-1 Hash-chained, externally anchored evidence ledger
- **Objective.** Recorded evidence cannot be altered, removed or rolled back without detection.
- **Risk.** Evidence tampering; rollback undetected (self-reported by Signal); Git history rewrite.
- **Mechanism.** Append-only evidence namespace where each entry carries the previous entry hash; periodic anchoring of the head hash to an external, independently operated timestamp/transparency service; verifier recomputes chain. Genuine gap: requires a decision record before building (AGENTS.md rule 5).
- **Automation.** L3 verification continuously; L4 writes refuse non-chained entries.
- **Failure mode.** fail-closed for writes; detective for verification.
- **Evidence.** Chain verification reports, anchor receipts.
- **Independence.** External anchor provides third-party time evidence; effectiveness still requires auditor testing.
- **Reuse.** Arca integrity primitives (ARCA-INT-003 designed to allow a chain); Unbound UNB-SCTRL-040, CCM-T06.

### C-ASR-2 Missing-evidence (heartbeat) detection
- **Objective.** Absence of expected evidence opens an exception; never reads as pass.
- **Mechanism.** Expected-evidence schedule per control version vs observed; zero-activity requires independent attestation.
- **Automation.** L3. **Failure mode.** fail-closed on assurance status (shows InsufficientEvidence).
- **Reuse.** Unbound SOC2-027, MON-002/004, CCM-T01/T04/T14, UNB-SCTRL-041; Specula/Strata "unknown is not success" convention.

### C-ASR-3 Control-exception governance with verified SoD
- **Objective.** Every exception has an approver who is a verified human, not the requester, holding the required role at evaluation, with expiry; expired exceptions revert to failing.
- **Automation.** L4. **Failure mode.** fail-closed.
- **Reuse.** Tutela GATE-SEMANTICS (directly), Unbound SOC2-033, OPS-019, LIFE-T03, CCM-T20.

### C-ASR-4 Evidence provenance and anti-fabrication checks
- **Objective.** Evidence is bound to the producing system, commit/version and environment; synthetic evidence cannot be presented as production.
- **Mechanism.** Envelope with producer identity, source revision, input digests, environment label; verifier rejects wrong-commit, impersonated or synthetic-labelled-production evidence.
- **Automation.** L3. **Failure mode.** fail-closed for assurance claims.
- **Reuse.** Tutela adversarial fixtures; Percepta evidence contract; Unbound SOC2-031, CCM-T09, SOC-T11.

### C-ASR-5 Status-claim reconciliation
- **Objective.** Status in context files, registers, dashboards and public claims never exceeds observed evidence.
- **Mechanism.** CI check comparing claimed status to evidence records; blocks merges that inflate status; covers stale CURRENT-STATE files (four contradictions found).
- **Automation.** L4 for registers; L2 for other repositories (advisory).
- **Failure mode.** fail-closed in this repository.
- **Reuse.** Unbound SOC2-008/036, MON-024; Tekmerion no-fabrication; `tools/validate_registers.py`.

## C. Platform controls from Echelon systems

### C-SDL-1 Signed-release verification and digest pinning
- **Objective.** Only attested, digest-matched components are installed/deployed.
- **Automation.** L4. **Failure mode.** fail-closed (Conditor has no unsigned fallback).
- **Evidence.** Conditor lock files and verify outputs, retained centrally.
- **Reuse.** Conditor ReleaseAttestation/NugetFeed; Unbound UNB-SCTRL-038.

### C-SDL-2 Branch protection continuously verified
- **Objective.** Protected branches require PR review and passing checks, apply to admins, forbid force-push/deletion — and this is re-read periodically, not just set at creation.
- **Automation.** L3 (read-back) with L1 ticket on drift; L5 auto-reapply is plausible but would mask who removed it, so prefer alert + manual. **Failure mode.** detective-only.
- **Reuse.** Conditor BranchProtection; Specula for visibility; OQ-761.

### C-SDL-3 Database privilege and RLS drift detection
- **Objective.** Production DB grants/RLS match the approved artefact.
- **Automation.** L3. **Failure mode.** detective-only (deploy gate is L4 fail-closed).
- **Reuse.** Strata `drift`, `noGrantsToPublic`; Census SQL baseline.

### C-LOG-1 Log and telemetry content hygiene
- **Objective.** No secrets, credentials or unnecessary PII in logs, faults or audit records.
- **Mechanism.** Classification-driven redaction at the boundary; closed event vocabularies with typed identifiers; leak-scan tests in CI.
- **Automation.** L4 (redaction) + L3 (scan). **Failure mode.** fail-closed for the record (dropped/masked).
- **Reuse.** Aegis Redaction; Signal Audit.fs; Fides leak scans; Unbound SOC2-003, SOC-T05.

### C-DAT-1 Honest deletion and erasure claims
- **Objective.** Deletion status reports only what the provider verified; erasure is a separate gated capability leaving a tombstone.
- **Automation.** L4. **Failure mode.** fail-closed (cannot claim "erased" without proof).
- **Reuse.** Arca Erasure; Signal Retention states; Unbound SOC2-032.

### C-BCR-1 Encrypted, verified backups with restore exercise
- **Objective.** Backups are encrypted, integrity-verified before trust and periodically restored with evidence that audit records survive restore.
- **Automation.** L3 verification; restore exercise L1 scheduled.
- **Failure mode.** detective-only.
- **Reuse.** Summa Backup (AES-256-GCM, verify before trust); Unbound UNB-SCTRL-043, CCM-T19.

### C-TEN-1 Isolation domain per tenant at the repository/App level
- **Objective.** Each isolation domain has its own GitHub App installation and repositories; cross-domain token use is impossible by construction.
- **Automation.** L4. **Failure mode.** fail-closed.
- **Reuse.** DF-FIDES-2026-0013 (accepted); Arca namespaces; Unbound UNB-TEN-001..007 (DB-level isolation for Unbound is a separate candidate once its stack is chosen).

### C-IAM-1 Workforce access review over GitHub org, AWS and data repositories
- **Objective.** Periodic review of who holds admin/write access to production repositories, the Fides exchange account and data stores, with removal evidence.
- **Automation.** L2 (automated export of principals) + human review; L3 for detecting access outside approved roster.
- **Failure mode.** detective-only.
- **Reuse.** Fides principal ids; app rosters; Unbound UNB-SCTRL-003/004, SOC-T07. No existing system implements this (gap F5).
