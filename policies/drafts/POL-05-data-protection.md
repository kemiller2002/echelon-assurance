---
policy_id: POL-05
title: Data Protection Policy
version: 0.1.0-draft
status: draft — not approved, not in force
owner: Owner
approval_authority: Board or sole owner
review: annual
scope: all data Echelon Foundry creates, receives, processes or stores, in every environment
authoritative_references: [TSC CC6.1, CC6.7, C1, P1-P8, 16 CFR 314.4(c)(2)-(3) and (c)(6), GLBA 15 USC 6802 (if applicable, as service provider; classification not assumed, OQ-202), state breach and privacy laws, IRS record retention guidance]
---

# POL-05 Data Protection Policy

## 01 Classification and handling
| Class | Examples | Minimum handling |
|---|---|---|
| Public | Published docs | None |
| Internal | Designs, non-sensitive research | Authenticated access |
| Confidential | Contracts, security architecture, vendor terms, audit reports | Need-to-know; encrypted; not to unapproved AI providers |
| Restricted | Donor/customer personal and financial data, credentials, evidence, security findings | Least privilege; encrypted with managed keys; access logged; redacted in general logs; approved AI providers only by explicit exception |

Data is labelled by schema or storage location, not by manual tagging, so that controls can act on the label.

## 02 Encryption and key management
1. Data in transit uses TLS 1.2 or later; data at rest uses provider-managed or customer-managed keys in a managed KMS.
2. Key administration and data access are separate roles. Key use is logged.
3. Encryption settings are verified continuously as configuration policy (L4/L5 target).

## 03 Retention, deletion and legal hold
1. Each data class has a retention schedule. Approved routine expiry is automated: before each deletion or crypto-shred the system checks legal hold, limits batch size and records deletion evidence. Mass deletion, key destruction, retention-period changes and any destruction of evidence require elevated human approval (owner decision DF-ASR-2026-0D09).
2. Append-only evidence stores retain hashes and references to personal data rather than the personal data itself, so that deletion obligations can be met without breaking integrity.
3. Retention periods are set from identified legal and contractual sources; where none is identified, the shortest period meeting the business purpose applies. No universal period is assumed.

## 04 Privacy
1. Personal information is processed only for the purposes stated to customers and in contracts.
2. Where Echelon acts as a service provider to a financial institution or nonprofit, it processes nonpublic personal information only as instructed by that customer and as permitted by applicable law.
3. Requests from individuals are routed to the customer (controller) unless Echelon is the controller.

## 05 Tenant data isolation
1. Every record is bound to a tenant at write time; queries are tenant-scoped by construction, not by convention.
2. Cross-tenant access is impossible through application paths; operator access across tenants is privileged access (POL-04.02).
3. Isolation is verified by automated negative tests on every change and by periodic adversarial testing.

## 06 Financial record retention
Ledger, contribution, grant, fee and tax-relevant records are retained for the longest of: applicable IRS requirements, customer contract terms and bank partner requirements, as recorded in the requirement register. Records are immutable for that period; corrections are new records (POL-12.05).

## Change history
| Version | Date | Change | Approved by |
|---|---|---|---|
| 0.1.0-draft | 2026-11-25 | Initial draft | Not approved |
