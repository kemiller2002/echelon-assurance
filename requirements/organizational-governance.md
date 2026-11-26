# Organizational governance and AI-native operations
Date: 2026-11-25
Status: Proposed requirements; actual appointments, staffing, legal structure and deployed controls unverified.

## Tenet
Responsibilities are mandatory; departments and headcount are driven by workload, risk and independence obligations. Automation executes delegated work but cannot assume legal accountability.

## Requirements
| ID | Requirement |
|---|---|
| GOV-BC-001 | Maintain responsibility register with accountable human role, delegated executor, authority, backup, evidence and escalation for each obligation |
| GOV-BC-002 | Distinguish owner, management, sponsor, workforce, agent, service and independent reviewer authority; no invented appointments |
| GOV-BC-003 | Standing delegation for routine bounded operations, versioned scope, expiry, limits and revocation |
| GOV-BC-004 | Reserve material risk acceptance, policy weakening, management assertions and legal commitments to authorized humans |
| GOV-BC-005 | Separate technical verification from organizational independence; no self-approved material exceptions; block or redesign when required independent approval unavailable |
| GOV-BC-006 | Maintain risk register with accountable owner, treatment, review trigger, residual risk and evidence |
| GOV-BC-007 | Maintain obligation register by entity, jurisdiction, customer, contract, framework and effective date |
| GOV-BC-008 | Maintain service and system inventory, boundaries, dependencies, data flows, owners and criticality; include supporting engineering systems where relevant |
| GOV-BC-009 | Maintain vendor/subservice register, due diligence, contractual obligations, complementary controls and exit plans |
| GOV-BC-010 | Workforce lifecycle controls for employees, contractors and privileged agents; onboarding, role changes, termination and access review |
| GOV-BC-011 | Workforce identity with MFA, least privilege, distinct service identities and centralized revocation; application identity is not workforce identity |
| GOV-BC-012 | Protect root/admin/key custody; separate privileges where feasible, controlled emergency access, independent alerts and recovery; document unavoidable concentration |
| GOV-BC-013 | Enforce repository governance, verified completion, protected branches and release provenance; verify actual settings and plan capabilities |
| GOV-BC-014 | Maintain signed release provenance target; interim pinned artifact digests and verified source, explicitly record limitations |
| GOV-BC-015 | Maintain privacy inventory including website inquiry workers, retention, subprocessors and actual deployed data paths |
| GOV-BC-016 | Maintain incident response, continuity and succession plans including owner unavailability; test notification, recovery and delegated emergency authority |
| GOV-BC-017 | Maintain role-based training and competency evidence, including AI literacy, security and financial responsibilities |
| GOV-BC-018 | Generate periodic management review packages from authoritative evidence; human review of material risks, exceptions, changes and resource decisions |
| GOV-BC-019 | Maintain versioned customer assurance answer bank with source, scope, evidence freshness, disclosure class and approval; unknown is not yes |
| GOV-BC-020 | Public standards and sanitized evidence by default; restricted customer data, vulnerabilities, credentials, confidential reports and operational details |
| GOV-BC-021 | Maintain regulatory change monitoring with source verification and accountable approval; no autonomous legal interpretation activation |
| GOV-BC-022 | Track operational workload, exception volume, review capacity and independence gaps; hire or contract only when justified or required |
| GOV-BC-023 | Maintain customer commitments and SLA register; verify service performance and notify according to applicable obligations |
| GOV-BC-024 | Establish complaints, whistleblowing and conflict-of-interest reporting with protected escalation and no retaliation |
| GOV-BC-025 | Maintain policy and control review cadence proportional to risk; trigger review on material changes and incidents |
| GOV-BC-026 | Ensure essential operations and governance remain functional without AI; maintain human-accessible procedures and recovery authority |

## Question dispositions
- OQ-109: classify continuous monitoring as management monitoring, not automatically internal audit; external reliance remains open.
- OQ-110: align observation period with sponsor fiscal year and auditor/customer needs; no arbitrary period.
- OQ-114,766: shared standards across all products; assess actual external services and financial reporting relevance before defining SOC boundaries; commercial intent and scope remain open.
- OQ-707,750: include relevant supporting systems in boundary analysis; determine GitHub/subservice treatment with auditor. Vigila operational monitoring is a target, not verified capability.
- OQ-709: prioritize signed releases before production reliance; interim pinned digests and provenance, no claim signing exists.
- OQ-710,752: verify website inquiry data flow and actual Fides deployment through authorized configuration/owner evidence; unknown until verified.
- OQ-764: prefer managed workforce identity with MFA and lifecycle controls; compare existing provider capabilities before purchase; no provider selected.
- OQ-607,608,701,800,804: role-based authority matrix; actual people, backup and administrative custody require owner confirmation; no fabricated independence.
- OQ-700,704,761: mandatory target controls; actual platform settings must be inspected and verified; plan upgrades not authorized.
- OQ-100,214: obtain authoritative licensed criteria through authorized access; do not reproduce restricted text or claim exact mapping verified.
- OQ-806: inspect Vigila current capability; assign runtime logging/heartbeat as proposed responsibility only if fit; otherwise use managed monitoring and revise mapping.

## Operating cadence (proposed)
Continuous automated monitoring and event-triggered escalation; weekly exception/workload triage when active operations justify it; monthly management review of material risks and evidence gaps; quarterly obligation/vendor review; annual or material-change policy and continuity review. Adjust by risk and actual commitments. These are proposed defaults, not statutory frequencies.

## Validation
Test delegation expiry, unauthorized approval, access revocation, emergency access, owner absence, evidence freshness, missed obligations, vendor failure, complaint escalation and AI outage using synthetic scenarios. Verify actual staffing, contracts, configurations and review evidence before operating claims. Map to existing controls and reconcile registers, policies, roadmap and economics. No implementation, appointments, purchases, external consultations or other repo changes authorized. Run governance and register validation before merge.
