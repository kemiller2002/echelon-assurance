---
id: ASR-POL-INV-001
title: Policy inventory and policy architecture
status: draft
version: 0.1.0
created: 2026-11-25
---

# Policy inventory

Register: [`policy-inventory.csv`](policy-inventory.csv) (13 policies, 71 sections; POL-11.02 also covers section 02a).

## Design principle

Compliance platforms usually ship 20–35 separate policy templates (see
`vendors/compliance-platform-analysis.md`). No framework we reviewed requires
a particular number of documents. The requirement is that policies exist,
are approved, are communicated, and cover the required subject matter
(e.g. TSC CC5.3 "deploys control activities through policies"; FTC Safeguards
Rule 16 CFR 314.4 requires a *written* information security program, not a
document set). Evidence class: `authoritative` for the requirement that
written policies exist; `industry-convention` for the document count.

We therefore keep **13 policies**, each addressed to one accountable owner and
one approval authority, with numbered sections. Each section is addressable
(`POL-06.02`) so controls, requirements and agent instructions can cite it
precisely. Generating policy text from a structured policy model in
`policy-as-code/` is planned (BR-GOV-001, BR-GOV-002) and not implemented;
the Markdown drafts in `policies/drafts/` are currently hand-written.

| Policy | Absorbs (conventional standalone documents) |
|---|---|
| POL-01 Assurance Governance | Information security policy, risk management, compliance management, internal control, exception management, delegation of authority, segregation of duties, document & records management, management review |
| POL-02 Code of Conduct and Acceptable Use | Ethics, conflicts of interest, acceptable use, personnel AI use |
| POL-03 People Security | Onboarding, screening, training, offboarding |
| POL-04 Identity and Access | IAM, privileged access, MFA, password, secrets, access review |
| POL-05 Data Protection | Data classification, encryption & key management, retention & deletion, privacy, tenant data isolation, financial record retention |
| POL-06 Secure Engineering and Change | SDLC, change, code review, release, emergency change, dependency, supply chain, signing & provenance, testing, environment segregation, production access, architecture governance, IaC, third-party software approval |
| POL-07 Operations Security | Configuration, network, cloud, endpoint, vulnerability, penetration testing, logging & monitoring, physical security responsibilities |
| POL-08 Incident Response and Notification | Incident response, AI incident response, breach/customer/regulator notification |
| POL-09 Resilience | Backup, business continuity, disaster recovery, availability |
| POL-10 Third-Party and Vendor Risk | Vendor management, TPRM, subservice organizations, AI provider risk, bank partner obligations |
| POL-11 AI and Autonomous Systems | Agent identity, authorization, credentials, execution boundaries, human approval, AI-generated code, prompt injection, AI data handling, AI evidence validation, autonomous remediation, accountability, model evaluation |
| POL-12 Financial Operations Integrity | Transaction authorization, accounting integrity, reconciliation, fraud, reversals/corrections/manual adjustments |
| POL-13 Customer Operations | Customer onboarding/termination, configuration governance, complaints, notification |

## Documents deliberately not created

| Conventional document | Disposition | Reasoning |
|---|---|---|
| Standalone password policy | Section POL-04.04 | With SSO and phishing-resistant MFA enforced by the identity provider, password rules are an IdP configuration. NIST SP 800-63B rejects composition rules and periodic rotation; a separate document mostly restates outdated convention. (`recommendation`) |
| Standalone physical security policy | Section POL-07.07 | No offices or datacenters; physical controls are inherited from cloud providers and evidenced by their SOC reports under the carve-out method. (`recommendation`; revisit if premises are acquired) |
| Separate encryption policy | Section POL-05.02 | Encryption requirements are data-class driven. |
| Separate logging and monitoring policies | Merged POL-07.06 | Same pipeline, same owner. |
| Separate BCP and DRP | Merged POL-09.02 | For a small cloud-hosted organization the plans share people, dependencies and tests. Customers may still request a "BCP document"; it is rendered from POL-09 plus runbooks. |
| Separate prompt injection policy | Section POL-11.04, primarily technical objective | Prompt injection is mitigated by architecture (tool permission isolation, untrusted-content handling), not by personnel behavior. |
| Clean desk, media disposal | Not needed now | No paper records or removable media in scope; endpoint encryption covers device loss. (`assumption`: confirm with owner) |

## Policies whose manual procedures disappear but whose objectives remain

Charter §21 asks whether technical impossibility removes the need for a
policy. Our conclusion: technical enforcement removes **procedures** (and
the evidence burden of proving people followed them), never the **policy
objective and accountability**. Auditors test that the objective is defined
and that the mechanism enforces it (`auditor-interpretation`; see
`soc/soc-reporting-landscape.md`). Examples:

| Behavior | Enforcement making it impossible | What still must exist |
|---|---|---|
| Merging unreviewed code to `main` | Branch protection/rulesets with required reviews and status checks, admin bypass disabled | Policy objective, ruleset-as-code, evidence of ruleset state over the period, bypass audit |
| Self-approval of a financial transaction | Domain authorization rejects approver = initiator | Authority matrix, negative tests, rejection logs |
| Using static cloud keys | Organization policy forbids key creation; workload identity federation | Policy objective, org-policy evidence, exception process |
| Storing secrets in repositories | Push protection + scanning | Objective, scanner evidence, incident path for bypass |
| Agent modifying historical evidence | Append-only store with no delete permission for agent identities; compliance-mode retention lock (anchoring deferred, DF-ASR-2026-C002) | Objective, permission evidence, integrity verification results |

Section-level owners, approval authorities and links to controls are in the
register; `requirement_ids` is empty in the inventory, and traceability runs
from the baseline rows (`requirements/baseline.csv`, `policy_ids`) to the
policy sections.
