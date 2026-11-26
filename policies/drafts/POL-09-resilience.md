---
policy_id: POL-09
title: Resilience Policy
version: 0.1.0-draft
status: draft — not approved, not in force
owner: Engineering lead
approval_authority: Board or sole owner
review: annual
scope: production systems, assurance systems (evidence store, policy model) and critical vendors
authoritative_references: [TSC A1.1-A1.3, CC9.1, FFIEC Business Continuity Management booklet (via bank partners), NIST SP 800-34]
---

# POL-09 Resilience Policy

## 01 Backup and recovery
1. Production data stores and the evidence store are backed up automatically to a separate account with immutability (write-once retention) enabled.
2. Restoration is tested at least quarterly by restoring to an isolated environment and verifying integrity with automated checks (row counts, ledger balance invariants, hash verification). The restore test produces evidence; a backup that has not been restored is not counted as recoverable.

## 02 Business continuity and disaster recovery
1. Each service has a recovery time objective and recovery point objective, set from customer commitments and business impact analysis.
2. A disaster recovery plan covers loss of: a cloud region, a cloud account (including compromise), the identity provider, source control, key personnel, and AI providers.
3. Plans are exercised at least annually; results and gaps are recorded.
4. The assurance program itself must keep operating when AI providers are unavailable: all controls that block or approve actions are deterministic and do not depend on an AI provider.

## 03 Service availability
1. Availability commitments to customers are recorded as requirements and monitored with synthetic checks.
2. Capacity and dependency health are monitored; degradation triggers incident response per POL-08.

## Change history
| Version | Date | Change | Approved by |
|---|---|---|---|
| 0.1.0-draft | 2026-11-25 | Initial draft | Not approved |
