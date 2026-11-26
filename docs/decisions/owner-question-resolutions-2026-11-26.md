# Owner-approved internal design resolutions — 2026-11-26

Status: Owner approved the preceding proposed answers; implementation, source verification and external acceptance remain separate. This record supplements the original question and disposition registers; it does not assert legal conclusions or operational effectiveness.

## Approved answers (73)

| Area | Question | Internal design resolution |
|---|---|---|
| SOC | OQ-102 | Operator is Echelon-side service organization for planning; actual entity pending |
| SOC | OQ-103 | Design all five TSC categories; examination scope pending |
| SOC | OQ-104 | Configurable privacy roles; sponsor instructions default, own-purpose data separate |
| SOC | OQ-106 | Classify counterparties by contracts and control dependencies |
| SOC | OQ-108 | Source-authenticated reproducible evidence with completeness and accuracy checks |
| SOC | OQ-109 | Management monitoring, not independent internal audit |
| SOC | OQ-112 | No Type 1 or SOC 3 absent commercial justification |
| SOC | OQ-114 | Shared standards; scope actual external services |
| SOC | OQ-116 | Draft DAF control objectives from transaction flows and reporting risks |
| SOC | OQ-755 | Shared controls and evidence, distinct examination objectives |
| SOC | OQ-766 | Assess future services by financial reporting relevance |
| Regulatory | OQ-200 | Adopt substantive GLBA safeguards regardless of classification |
| Regulatory | OQ-201 | Do not rely on nonprofit exemption |
| Regulatory | OQ-202 | Meet substantive safeguards without assuming legal classification |
| Regulatory | OQ-203 | Configurable notices, disclosures, preferences and opt-outs |
| Regulatory | OQ-205 | Support direct bank relationships with activation gates |
| Regulatory | OQ-206 | Default narrower ACH service-provider model; track aggregate volumes and role changes |
| Regulatory | OQ-207 | Defer cards; sponsor merchant and hosted collection if introduced |
| Regulatory | OQ-209 | Configurable jurisdictional privacy policies |
| Regulatory | OQ-210 | Immediate internal alerts and configurable external deadlines |
| Regulatory | OQ-211 | Support regulated customer controls without assuming NYDFS coverage |
| Regulatory | OQ-212 | Sponsor-approved acknowledgments, DAF reporting and applicable disposition records |
| Regulatory | OQ-213 | Own sanctions-control framework plus sponsor responsibilities; revalidate before release |
| Regulatory | OQ-216 | No receipt, custody or unilateral movement of customer funds initially |
| Regulatory | OQ-217 | Framework-neutral catalog and crosswalks |
| Security | OQ-300 | Continuous monitoring and validation; no small-entity exemption assumption |
| Security | OQ-301 | No assumed NYDFS license; reassess operating-model changes |
| Security | OQ-302 | Comprehensive internal testing and reproducible evidence |
| Security | OQ-306 | Enforce provider-specific testing authorization boundaries |
| Security | OQ-308 | Production-equivalent testing plus production configuration delta verification |
| Security | OQ-309 | Continuous AI-assisted testing with deterministic reproduction |
| Security | OQ-310 | Select CPA on competence, independence and value, not affiliation alone |
| Security | OQ-312 | Evidence independent of commercial compliance platforms |
| Security | OQ-313 | Coordinated disclosure with explicit authorization; no immunity beyond authority |
| Evidence | OQ-500 | Managed immutable storage and independently verifiable integrity/completeness; no unjustified custom witness network |
| Evidence | OQ-501 | Versioned approved policy model authoritative; generate readable documents |
| Evidence | OQ-502 | Configurable retention by class, jurisdiction and contract |
| Evidence | OQ-503 | Crypto-deletion where appropriate, not presumed legal erasure |
| Evidence | OQ-504 | Managed immutable storage in isolated boundary; provider pending |
| Evidence | OQ-506 | Portable structured exports; OSCAL when justified |
| Evidence | OQ-507 | Risk-appropriate authenticated timestamps; qualified services only if required |
| Evidence | OQ-508 | Existing GitHub capabilities first; upgrade only if necessary and economical |
| Evidence | OQ-510 | Authenticated execution records distinct from agent interpretations |
| AI | OQ-600 | US-first; reassess EU applicability before relevant deployment |
| AI | OQ-601 | Original source remains authoritative despite AI collection or interpretation |
| AI | OQ-602 | Classify providers by role, access and dependency |
| AI | OQ-603 | Provider acceptance criteria for confidentiality, retention, training, access and contracts |
| AI | OQ-604 | Mutation and synthetic tests are management monitoring and internal validation |
| AI | OQ-606 | No AI certification without demonstrated need |
| AI | OQ-607 | Owner retains organizational reserved authority until formal delegation; actual legal signatories pending |
| AI | OQ-608 | Multi-agent review and protected deterministic verification; no retainer |
| AI | OQ-612 | Internal validation first; external review conditional |
| AI | OQ-614 | Minimize and redact traces; configurable retention |
| AI | OQ-615 | Authenticated provenance via Praxis or compatible external binding |
| Engineering | OQ-700 | Protected branches, mandatory checks, controlled releases and verified completion |
| Engineering | OQ-701 | Standing human authorization and deterministic verification for routine work; not organizational independence |
| Engineering | OQ-702 | Authenticated principals, not self-report |
| Engineering | OQ-703 | Managed key custody, separate identities, restricted privileges and recovery |
| Engineering | OQ-704 | Enable verified completion in scope |
| Engineering | OQ-705 | Git not sole production evidence store |
| Engineering | OQ-706 | Distinct authenticated agent and human identities |
| Engineering | OQ-707 | Scope by effect on operations, security, releases and financial processing |
| Engineering | OQ-708 | Praxis and F# tooling mandatory; verify actual installation |
| Engineering | OQ-709 | Prioritize authenticated release signing |
| Engineering | OQ-750 | Explicit GitHub dependency and control mappings |
| Engineering | OQ-756 | Separate sponsor and operator by design; legal entities pending |
| Engineering | OQ-758 | IPE register with sources, transformations, completeness, accuracy and reproducibility |
| Engineering | OQ-761 | Consistent repository governance |
| Engineering | OQ-762 | Configurable retention and legal holds |
| Engineering | OQ-763 | Deterministic gates not independent human approvers |
| Engineering | OQ-764 | Managed workforce identity, MFA and lifecycle controls |
| Engineering | OQ-765 | Reproducible automation evidence; auditor reliance separate |
| Engineering | OQ-804 | Separate admin identities, deletion delays, immutable storage and alerts; concentration risk remains |

## Register reconciliation
For every question preserve original wording, evidence references and history. Track separately: architectural decision, requirements definition, implementation verification, external acceptance (including legal/customer/auditor dependencies). Record approved internal direction for the rows above; do not automatically mark the other dimensions resolved. Use not-applicable only with justification. Consolidate duplicate dependencies by canonical reference without deleting original IDs. In particular group auditor reliance questions OQ-108/312/500/510/601/604/765 and preserve distinct evidence types.

Keep authoritative-source questions OQ-100/214/305, future standards OQ-107/113/605/613, customer-specific OQ-105/204/215/303/610/757, auditor-specific OQ-101/110/754, pricing OQ-111/311, insurance OQ-307/609 and deployment OQ-710/752 open until evidence exists. Preserve all other unresolved questions not explicitly answered here, including regulatory applicability and provider selection. No external spending, production activation or changes to other repositories authorized. Continue primary-source research and multi-agent review. Reconcile baseline, policies, controls, roadmap and question registers; validate exact final commit under Praxis before merge.
