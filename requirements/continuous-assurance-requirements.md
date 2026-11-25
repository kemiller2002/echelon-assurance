---
id: ASR-CA-REQ-001
title: Continuous assurance requirements (cross-system)
status: proposed
version: 0.1.0
classification: public
created: 2026-11-25
updated: 2026-11-25
origin: echelon-registry work item GOV-TRN-001 (organizational transparency and portfolio governance)
builds_on:
  - architecture/evidence-architecture.md (research branch claude/fervent-tesla-ytnxz6 @ 77bf3a8, unmerged on 2026-11-25)
  - integration/fragments/ECH1-assessment.md, ECH2-assessment.md (same branch)
  - organization policies POL-ORG-TRN-001, STD-ORG-CLS-001, STD-ORG-PUB-001
---

# Continuous assurance requirements (cross-system)

**Status: proposed.** This document states *requirements* and *responsibility assignments*. The
evidence architecture (envelope, storage layers, key management, completeness, retention, privacy)
is specified in `architecture/evidence-architecture.md` on the research branch; this document does
not restate it and defers to it where they overlap. Reconciliation of the two is open decision
OD-GOV-020 (registry open decisions register).

Nothing here asserts that any control operates effectively.

## 1. Model

Assurance evidence is produced as a by-product of normal engineering and operations: a merge, a
release, a deployment, an access change, a test run or a reconciliation emits an evidence record with
provenance at the moment it happens. Periodic collection becomes the exception for controls that
cannot be automated.

## 2. Responsibilities: implemented vs proposed

Verified capabilities come from the integration assessments on the research branch (read-only review
of repository HEADs on 2026-11-25; "implemented" = code present; "tested" = tests present, counted,
not run). **Proposed** responsibilities are not capabilities.

| System | Implemented capability relevant to assurance (verified basis) | Proposed responsibility | Not responsible for |
|---|---|---|---|
| Praxis | actor provenance on executions and records; attested completion with DSSE/ECDSA receipts (implemented, tested, **not enabled** in any assessed repository); signed approval receipts; validation (ECH1) | governance and provenance evidence for every change; evidence export command (period-bounded, digest-manifested) | proving identity of the actor (provenance is self-reported, ECH1 finding 4) |
| Aegis | operational fault capture with redaction (ECH2 §2.1) | fault and diagnostic evidence for applications | security rule registry (see Dokimos/Tutela; DUP-03 in the registry duplication analysis) |
| Dokimos | quality snapshots against accepted baselines; CI gate; independent verification of Praxis completion claims (ECH1) | verification evidence; independent verifier for completion | security assessment |
| Tutela | release-posture gate; exception governance with separation of duties; secret scanning; fabricated-evidence detection fixtures (ECH2 §2.13) | security control evidence; organization-wide exception model for all controls | general security claims about consumers (Tutela CURRENT-STATE) |
| Vigila | follow-up items with history (ECH2 §2.2) | follow-up of findings and exceptions (reminders, at most level L1) | **runtime monitoring** (the Secure Construction Standard's assignment is not supported by Vigila's charter; unowned, registry GAP-05) |
| Strata | schema management, grants/RLS (ECH2) | evidence of database access constraints in consumers | append-only persistence/integrity (contradiction preserved in ECH2 §3.4) |
| Limen | browser capability boundary | evidence that application code does not bypass the boundary (SEC-UI-002) | — |
| Forma | accessible, safe rendering primitives | accessibility conformance evidence inputs | — |
| Folio | deterministic print/PDF | presentation of evidence packages and reports | producing or verifying evidence (ECH2 §3.5 item 5) |
| Arca | records with content hashes and export with SHA-256 (ECH2 §3.1) | substrate of the evidence ledger (with hash chain and external anchor — a genuine gap) | — |
| Conditor | signed release attestations, release provenance (ECH2) | supply-chain evidence | — |
| Echelon Assurance | research, control catalog, evidence architecture | assurance coordination: control-to-evidence mapping, evidence freshness policy | storing production evidence (SECURITY-AND-CONFIDENTIALITY.md) |
| Echelon Knowledge Store | compilation and task packages (research) | evidence relationship representation (KSI-03/04 `evidences` edges) | evidence storage |
| Echelon Registry | system and repository identity; release records | identity of evidence producers (system ids) | evidence |

## 3. Requirements

### Automated control verification
- **CA-AUTO-01** Each control in the catalog records its verification mode: `continuous-automated`,
  `event-driven-automated`, `periodic-automated`, `periodic-manual` or `external-attestation` (the
  due diligence `evidence_mechanism` vocabulary).
- **CA-AUTO-02** Automated verification fails closed: a check that cannot run reports
  `indeterminate`, never `pass`.

### Evidence provenance
- **CA-PROV-01** Every evidence record names producer system id (registry), producer version and
  digest, input commit or artifact digest, time, and actor (human, agent or automation).
- **CA-PROV-02** Provenance claims distinguish *self-reported* from *platform-authenticated* identity
  (for example GitHub OIDC or App identity). Self-reported provenance is labelled as such.
- **CA-PROV-03** Evidence logs are append-only and hash-chained, with periodic external anchoring
  (gap; evidence architecture §5).

### Freshness, expiration and revocation
- **CA-FRESH-01** Each control defines a freshness interval; evidence older than it no longer supports
  a `verified` status.
- **CA-EXP-01** Evidence carries an explicit expiry; expired evidence is retained for history but
  excluded from current status.
- **CA-REV-01** Evidence can be revoked (for example the producing tool version is found defective,
  or the verifier key is compromised); revocation propagates to every status and claim that cited it.

### Reproducibility
- **CA-REPRO-01** Automated evidence records the exact command, tool version and inputs so that an
  independent party can re-run it (deterministic verification where feasible).

### Disclosure classification
- **CA-DISC-01** Every evidence record carries `classification` (STD-ORG-CLS-001). Public outputs cite
  evidence by id, class, status and freshness; they never embed Confidential or Restricted evidence.
- **CA-DISC-02** Restricted evidence (vulnerability details, personal data, incident evidence) never
  enters Git (STD-ORG-CLS-001 §5).

### Control-to-evidence traceability
- **CA-TRACE-01** Each control links to the requirements it satisfies, the evidence types that support
  it and the current evidence ids; each due diligence answer links to controls (BND-06).

### Independent verification
- **CA-IND-01** Status records `assurance_independence` (first-party, second-party,
  independent-third-party, regulatory-examination).
- **CA-IND-02** Continuous internal assurance is never described as certification, attestation or
  audit (POL-ORG-TRN-001 §6; due diligence DDT-04).
- **CA-IND-03** The verifier of a control is not the producer of the evidence where separation is
  feasible (Dokimos verifying Praxis completion is the pattern).

### Exceptions and residual risk
- **CA-EXC-01** Exceptions are explicit, scoped, time-limited, approved by a role holder who is not the
  requester, and carry compensating controls (generalise Tutela's exception model).
- **CA-RISK-01** Each control records residual risk in plain language; residual risks are published
  at design level unless exploit-relevant.

## 4. First increments

1. Enable Praxis verified/attested completion in one pilot repository (ECH1 finding 2).
2. Register Dokimos completion verification and Praxis attested completion as capabilities
   (registry GAP-04).
3. Generalise Tutela exception governance to one non-security control.

## 5. Visibility note

`SECURITY-AND-CONFIDENTIALITY.md` on the research branch records that the owner directed this
repository to be private and that it is public. The organizational transparency policy (proposed)
makes assurance research public by default. That conflict is an owner decision (OD-GOV-002); until it
is made, this repository continues to follow public-repository rules.
