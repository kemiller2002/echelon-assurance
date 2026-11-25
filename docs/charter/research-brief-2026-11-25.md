---
id: GV-ASR-BRIEF-001
title: Owner research brief — Echelon Foundry SOC Research and Continuous Assurance Initiative
status: canonical (owner instruction)
received: 2026-11-25
note: Condensed faithfully from the owner's instruction; section numbers preserved. Owner amendments of the same date are listed first.
---

# Owner research brief (2026-11-25)

## Owner amendments (take precedence)

- Use `kemiller2002/echelon-assurance` (already created); it supersedes every reference to "the SOC repository".
- Scope is **organizational assurance**, not only SOC. SOC 1 and SOC 2 are initial priorities; architecture must absorb further regulatory, contractual, security, operational and financial requirements without substantial redesign.
- Establish the repository with current Echelon Foundry repository governance standards and Praxis conventions.
- Keep the repository **private**; never publish sensitive findings, internal control weaknesses, production evidence, credentials or confidential information.
- Focus on research, requirements, policy, architecture specifications and validation methodology; **do not prematurely implement a new software platform**. Evaluate existing Echelon systems first.
- Execute the complete script below, committing findings incrementally.

## 1. Mission
Exhaustive research into SOC reporting, security governance, compliance engineering, automated control enforcement, continuous assurance and independent validation, toward an AI-native compliance operating model that: minimizes recurring cost and manual work; generates evidence through normal operations; enforces policy technically; independently validates effectiveness; reduces dependence on commercial platforms; supports credible external audits; scales across products without duplication; meets financial-institution and enterprise expectations; makes security and compliance measurable. Challenge whether conventional workflows should exist in their current form.

## 2. Repository and execution
Inspect the destination repository first. Review Unbound, DAF Landscape, Echelon Organization Administration, Praxis, Ordo, Dokimos, Aegis, Vigila, Fides, Arca, Strata and other relevant systems; verify capabilities from documentation and implementation, never from names. Unbound is the reference implementation; design for organization-wide reuse. Do not modify other repositories; record proposed changes here. Preserve existing research; commit batches through the repository's governance process.

## 3. Methodology
Apply Clarity and the Echelon Diagnostic Framework where appropriate. Distinguish authoritative requirements, auditor interpretations, industry conventions, customer expectations, vendor claims, evidence-supported recommendations, unverified assumptions and open questions. Prefer primary sources (AICPA, NIST, CISA, CIS, OWASP, regulators, accreditation bodies, auditing standards); record URLs, publication and retrieval dates, jurisdictions, limitations. Never fabricate requirements or treat convention as mandatory. Seek contradictory evidence.

## 4. SOC research
SOC 1 Type I/II; SOC 2 Type I/II; SOC 3; SOC for Cybersecurity; SOC for Supply Chain; TSC; description criteria; CUECs; CSOCs; inclusive vs carve-out; boundaries; materiality; design vs operating effectiveness; management assertions; evidence sufficiency; sampling; observation periods; exceptions and qualified opinions; auditor independence and qualifications; distribution restrictions; bridge letters; subservice dependencies. Determine relevance to Echelon and Unbound (financial administration, accounting, DAFs, payments, sensitive data, FI relationships), including whether Unbound affects customers' ICFR and creates SOC 1 demand. Do not assume SOC 2 alone suffices.

## 5. Regulatory and framework crosswalk
AICPA TSC; NIST CSF; SP 800-53; SSDF; AI RMF; ISO 27001/27002, 27701, 42001; CIS Controls; OWASP ASVS, SAMM, API Security, AI/agentic guidance; SLSA; OpenSSF; CSA CCM; PCI DSS where applicable; GLBA and FTC Safeguards where applicable; FI third-party risk guidance; FFIEC; state privacy and breach laws; GDPR where applicable; IRS requirements relevant to Unbound; payment network and banking partner requirements. Build one unified control model. Classify each requirement: legally applicable, contractually applicable, required for a selected framework, common customer expectation, voluntary, not applicable — with reasoning.

## 6. Policy inventory
Investigate (at minimum) organizational governance (information security, risk, compliance, internal control, delegation of authority, SoD, ethics/conflicts, acceptable use, exceptions, records, on/offboarding, training); technical security (IAM, privileged access, MFA, passwords/credentials, secrets, encryption/keys, network, cloud, endpoint, configuration, vulnerability, pen testing, monitoring, logging, incident response, classification, retention/deletion, backup, BC, DR, physical responsibilities); software development (SDLC, change, code review, release, dependencies, supply chain, signing/provenance, testing, environment segregation, production access, emergency changes, architecture governance, infrastructure configuration, third-party software); AI (acceptable use, agent identity/authorization, AI code, execution boundaries, human approval, model/provider risk, prompt injection, AI data, agent credentials, AI evidence validation, autonomous remediation, accountability, AI incidents, model evaluation/monitoring); financial and customer operations (transaction authorization, accounting integrity, reconciliation, fraud, customer data isolation, customer on/offboarding, vendor management, TPRM, complaints, privacy, financial record retention, reversals/corrections, customer configuration, availability, notification). Decide for each whether a separate document is needed; avoid documents created only because vendors recommend them.

## 7. Policy as code
F#-first, human- and machine-consumable representation with: id, version, owner, purpose, scope, systems, references, effective date, review, mandatory controls, enforcement, validation, evidence, exceptions, approval authority, remediation, change history. Assess whether one definition can generate documents, auditor docs, enforcement rules, CI checks, agent instructions, runtime config requirements, dashboards, evidence requirements, control tests, customer assurance docs. Evaluate OPA/Rego, Cedar, OSCAL, cloud-native policy; decide adopt/integrate/limited F# model; avoid duplicate sources of truth.

## 8. Control automation
Levels L0 manual documentation; L1 reminders; L2 evidence collection; L3 automated validation; L4 preventive enforcement; L5 continuous enforcement with bounded remediation. Do not assume L5 everywhere; evaluate risk, independence, economics, limits. Examples: unauthorized merges, required tests, unapproved production changes, access validation, dormant accounts, MFA coverage, exposed secrets, backup restoration, drift, encryption, dependency integrity, overdue vulnerabilities, incident deadlines, financial thresholds, SoD violations, tenant isolation, retention, agent permissions, unauthorized policy changes. Define failure behavior, escalation, recovery, evidence; distinguish fail-closed from justified fail-open.

## 9. Continuous evidence architecture
Evidence as a byproduct (commits, PRs, reviews, CI, tests, deployments, cloud audit logs, access events, scans, dependency inventories, signed artifacts, restore tests, incidents, reconciliations, policy evaluations, agent records, customer configuration changes, training). Envelope: id, control ids, source, actor, timestamp, policy version, collection method, integrity hash, provenance, validation status, retention, access classification, exceptions. Investigate tamper evidence, append-only, signing, independent timestamps, completeness verification and proof of non-omission. Integrity is not accuracy.

## 10. Independent validation
Distinguish self-assessment, automated validation, internal independent review, external technical validation, independent pen testing, external attestation. Layers: unit, integration, property-based, negative security, adversarial, control mutation, fault injection, configuration verification, synthetic transactions, independent reconciliation, recovery exercises, access testing, cross-tenant isolation, runtime invariants. Assess formal methods for high-risk invariants; separate validation agents, different models, isolated environments, independent evidence sources. Echelon-controlled agents are not independent auditors. Prevent one system implementing, validating and approving its own controls.

## 11. Penetration testing economics
When required, expected by auditors, contractually required, or voluntary; internal vs independent; certifications (CREST, OSCP, OSWE, GPEN, GWAPT, others; individual vs organizational); variation by framework and customer. Alternatives: automated testing, continuous assessment, threat modeling, AI-assisted adversarial testing, freelancers, narrow scope, architecture testing, shared-platform assessments, risk-based frequency, retests, bug bounty, CVD. Never present scanning as an independent pen test. Produce a defensible minimum-cost external testing strategy for Unbound.

## 12. Commercial compliance platforms
Vanta, Drata, Secureframe, Sprinto, Hyperproof, AuditBoard, Thoropass, others: functionality, evidence collection, integrations, policy generation, monitoring, auditor collaboration, pricing, complaints, lock-in, security implications, automated-evidence limits, remaining manual work. Identify what Echelon already has, what it can build economically, and the minimum capabilities to avoid or reduce subscriptions — without cloning every feature.

## 13. Echelon system integration
Responsibility matrix per system: existing and missing capabilities, overlaps, interfaces, evidence produced, controls enforced, validation responsibilities, trust boundaries, failure modes, proposed enhancements. Emphasize Praxis, Dokimos, Vigila, Aegis, Fides, Strata. No new system without a demonstrated gap. Preserve F#-first, minimal infrastructure, monoliths where appropriate, managed services, mandatory testing.

## 14. AI-native compliance operations
Agents may monitor requirement changes, identify affected policies, propose revisions, detect failures, investigate evidence gaps, generate remediation items, evaluate exceptions, prepare auditor responses and questionnaires, maintain mappings, prepare management review, identify risks. Agents must not approve their own exceptions, fabricate evidence, conceal failures, modify historical evidence, represent internal validation as attestation, silently weaken controls, change material policies without approval, or expose confidential evidence to unauthorized providers. Must function when AI providers are unavailable.

## 15. Auditor and customer experience
Auditor interface: control descriptions, policy versions, evidence history, population completeness, test results, exceptions, remediation, boundaries, personnel, documentation, read-only access, exportable packages. Investigate OSCAL and other formats. Customer trust center with disclosure controls; answer questionnaires from the control and evidence model; avoid exposing sensitive details.

## 16. Financial controls and Unbound
Ledger integrity, double entry, authorization, completeness, idempotency, reconciliation, cash and asset movement, grant disbursements, contributions, fees, investment accounting, financial reporting, donor restrictions, charity eligibility, fraud, manual adjustments, historical corrections, tenant-specific policies, FI integrations. Identify SOC 1-relevant controls; evaluate invariants and independent reconciliation versus manual sampling.

## 17. Organizational resilience
How assurance itself fails: compromised admins, agents or CI/CD; evidence manipulation; missing evidence; disabled monitoring; collusion; incorrect policies; outdated interpretations; shared infrastructure failure; bad automated remediation; single cloud or AI provider reliance; excessive complexity; compliance theater. Detect failures of the assurance infrastructure; independently verify evidence and validation systems.

## 18. Compliance economics
Five-year TCO for conventional manual, commercial platform, outsourced, Echelon automated, hybrid. Include audits, pen tests, consulting, subscriptions, engineering, administration, evidence preparation, remediation, infrastructure, maintenance, customer reviews, opportunity cost. Separate unavoidable from reducible costs; model marginal cost per product; assess economies of scale; do not assume building is cheaper.

## 19. Deliverables
Executive findings; methodology; source register; SOC landscape; applicability matrix; unified control catalog; policy inventory; draft policies; policy-as-code specification; automation matrix; evidence architecture; validation architecture; pen-test research; vendor analysis; integration matrix; AI governance model; Unbound financial controls; auditor experience spec; trust center spec; economics; risk register; decision register; open questions; roadmap. Machine-readable registers where practical; stable control IDs traceable to source, policy, mechanism, validation and evidence.

## 20. Control catalog fields
ID, objective, risk, frameworks, citations, applicability, policies, owner, mechanism, automation level, validation method and frequency, evidence requirements and source, independence, failure response, exception handling, implementation and recurring cost, status, limitations. Bidirectional traceability among requirements, policies, controls, evidence and validation.

## 21. Challenge conventional compliance
For each practice: real risk; effectiveness; legal/contractual/customary basis; architectural elimination; preventive replacing detective; continuous replacing periodic; evidence from the transaction; stronger mechanism than sampling; new risks; auditor acceptance and its evidence. Investigate policies made unnecessary by technical impossibility while retaining objectives and accountability.

## 22. Execution and completion
Proceed autonomously; do not stop at an initial report; checkpoint and commit batches. Review passes: completeness, source accuracy, regulatory applicability, internal consistency, automation feasibility, validation independence, economic viability, security architecture, Unbound-specific requirements, auditor acceptance uncertainty. Continue until diminishing returns with material questions resolved or documented. Never claim a policy operational, control effective or requirement satisfied without evidence. For questions needing professional confirmation: the precise question, why internal research cannot resolve it, and the lowest-cost credible confirmation method. Conclude with a roadmap: immediate organizational requirements; before production; before onboarding financial institutions; before an audit observation period; for external attestation; longer term.
