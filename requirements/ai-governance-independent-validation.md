# AI governance and independent validation
Date: 2026-11-25
Status: Proposed detailed requirements under approved foundations; no implementation, risk acceptance or external assurance claim.

## Agent controls
| ID | Requirement | Verification |
|---|---|---|
| AIG-BC-001 | Authenticated distinct agent execution identity | Reject forged and replayed credentials |
| AIG-BC-002 | Identifiable authorizing principal and signed delegation | Reject missing or invalid authorization |
| AIG-BC-003 | Scope, duration, resource and action budgets enforced externally | Attempt out-of-scope, expired and over-budget actions |
| AIG-BC-004 | No self-escalation or modification of governing permissions | Attempt privilege and policy changes |
| AIG-BC-005 | No self-approval of material exceptions or policy changes | Test delegation-chain and conflict checks |
| AIG-BC-006 | Deterministic external security and financial gates | Adversarial model output cannot bypass gate |
| AIG-BC-007 | Least privilege and data-class/provider allowlists | Attempt unauthorized reads and disclosures |
| AIG-BC-008 | External content untrusted; isolate research from privileged execution | Indirect prompt injection fixtures; tool and egress denial |
| AIG-BC-009 | Attributable actions with authenticated provenance | Compare source-system logs and signed receipts |
| AIG-BC-010 | No agent alteration of authoritative activity evidence | Attempt overwrite, deletion and collector bypass |
| AIG-BC-011 | Independent suspension and revocation | Revoke active run; deny new actions and reconcile in-flight effects |
| AIG-BC-012 | Critical controls and business operations function without AI | Provider outage and fallback tests |
| AIG-BC-013 | Inventory models, versions, providers, intended use, limitations and changes | Inventory reconciliation and change-triggered evaluation |
| AIG-BC-014 | Evaluate task-specific correctness, uncertainty, abstention, drift and adversarial behavior before deployment and on material change | Versioned held-out fixtures and regression thresholds |
| AIG-BC-015 | Minimize prompts/traces, redact secrets, enforce retention and provider terms | Synthetic sensitive-data leakage and retention tests |
| AIG-BC-016 | Disclose material AI involvement where appropriate; provide correction and escalation paths | User workflow and disclosure tests |
| AIG-BC-017 | No unsupported financial, legal, audit or compliance assertions; distinguish source facts and model interpretations | Evidence linkage and adversarial fabrication tests |
| AIG-BC-018 | Incident response for agent misuse, disclosure, unauthorized actions and provider compromise | Tabletop and containment tests |

## Authority model
A0 read public or specifically authorized data; A1 propose drafts and decisions; A2 execute bounded reversible or otherwise explicitly authorized low-risk operations; A3 execute material operations only through independently enforced standing or transaction-specific human authorization and deterministic gates; A4 reserved human authority for policy weakening, material exceptions, risk acceptance, privileged access, management assertions and other nondelegable decisions. A3 is a proposed clarification of prior A0-A2 agent model: no agent independently authorizes movement of funds or material exceptions. Sponsor retains financial authority. Owner is accountable for Echelon policy and management assertions; actual named roles, legal signatories and independent reviewer availability must be confirmed. No self-approved material exception; if independent approval is required and unavailable, block action or redesign. Routine low-risk changes may use standing authorization and independent technical gates under approved foundations. Do not represent technical separation as human organizational independence.

## Validation requirements
| ID | Requirement |
|---|---|
| VAL-BC-001 | Separate implementer, test executor, verifier and approval roles by authenticated permissions and trust boundaries |
| VAL-BC-002 | Bind test receipts to exact commit, artifact, configuration, test suite version, environment and verifier identity |
| VAL-BC-003 | Verify positive, negative, boundary, property, concurrency, fault, recovery and adversarial cases according to risk |
| VAL-BC-004 | Mutation/bypass tests demonstrate that controls and validation detect deliberate failure in safe environments |
| VAL-BC-005 | Independent reference calculations and source populations for material financial controls |
| VAL-BC-006 | Prevent implementer from modifying verifier keys, required checks or authoritative results; protect test changes |
| VAL-BC-007 | Separate primary source evidence, deterministic derived evidence and AI interpretations; AI summaries link sources and cannot replace them |
| VAL-BC-008 | Independent technical review uses separate credentials, execution and criteria; different model alone is not independence |
| VAL-BC-009 | Risk-tier validation: routine deterministic checks; material controls independent technical verification and accountable review; external human review where required or justified |
| VAL-BC-010 | Distinguish internally tested, technically independently verified, organizationally independently assessed and externally attested; no automatic promotion |
| VAL-BC-011 | Revalidate on material code, policy, dependency, model, infrastructure or threat change; stale evidence is unknown |
| VAL-BC-012 | Validate evidence machinery itself, including completeness, tampering, silence, recovery and administrator bypass |
| VAL-BC-013 | Formal methods considered for critical state machines only where incremental benefit justifies cost; model proof is not executable-code proof |
| VAL-BC-014 | Record residual risks, coverage gaps, failed tests, exceptions and reviewer limitations; never suppress failures |

## Question dispositions
- OQ-600: US-focused initial design; research territorial scope and AI Act duties before EU deployment or relevant output use; legal applicability open.
- OQ-601,604,612,510,765: use primary/derived/AI-interpreted evidence classes, reproducible independent verification; auditor reliance and external reviewer quotes open.
- OQ-602,603: inventory providers and dependency roles, classify data, restrict sensitive evidence to approved contractual terms; no assumption of no training/retention or carve-out acceptance.
- OQ-605,613,107,113: monitor authoritative final criteria and NIST changes; no reliance on unissued drafts as mandatory rules.
- OQ-606,609,610: defer AI-specific certification purchases; collect customer/bank/insurer requirements during ordinary diligence.
- OQ-607,608,701,763,800,803: apply authority and risk-tier validation model above; named staffing and external acceptance remain open. Reconcile conflicting policies and budget actual independent review where required.
- OQ-611: specify critical state machines and property tests first; propose time-boxed formal modeling pilot only after requirements and cost-benefit review; no implementation authorized.
- OQ-614: minimize and classify traces, retain only justified evidence under record-class schedules and holds; legal periods open.
- OQ-615,702,706: authenticated execution receipts mandatory target; verify current Praxis capabilities and bind platform identity externally if needed.
- OQ-616: restrict untrusted-content research agents to least privilege, no privileged write or sensitive egress by default; residual prompt-injection risk remains explicitly unaccepted pending owner review.

## Integration
Praxis authorization and governance, Dokimos validation, Vigila monitoring, Arca evidence integrity, Fides application identity, Tutela exceptions and Folio rendering are proposed responsibility mappings; verify actual capabilities. Use F# and managed infrastructure, avoid new services by default. Map to existing controls and reconcile question register, policies, risk register, roadmap and economics. No implementation, purchases, external consultations or other repo changes authorized. Run governance and register validation before merge.
