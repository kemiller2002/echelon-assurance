# Continuous security testing and external assurance
Date: 2026-11-25
Status: Proposed requirements under owner direction to automate continuous security testing wherever feasible. No implementation, certification, legal conclusion or external acceptance claim.

## Tenet
Security validation is continuous engineering, not an annual compliance event. Build reusable automated capabilities across Echelon, reuse mature tools and managed services where economical, and reserve independent external work for independent challenge, judgment and mandatory obligations. Never describe internal automated testing as an independent penetration test.

## Requirements
| ID | Requirement | Trigger / verification |
|---|---|---|
| CST-001 | Inventory authorized assets, APIs, trust boundaries and test coverage | Changes; reconcile inventory and coverage |
| CST-002 | Enforce security-by-construction constraints with F# source/build rules | Relevant changes; negative fixtures |
| CST-003 | Static analysis and secret detection, including generated artifacts | Changes; synthetic seeded findings |
| CST-004 | Dependency inventory, SBOM, provenance, vulnerability and license evaluation | Changes and advisory updates; known vulnerable fixture |
| CST-005 | Authorization and tenant isolation regression across APIs, workers, exports and storage | Relevant changes; synthetic multi-tenant fixtures |
| CST-006 | Schema-driven API and browser security tests using authorized isolated targets | Release candidates; positive/negative coverage |
| CST-007 | Input fuzzing and property tests with bounded resources | Relevant changes and scheduled runs |
| CST-008 | Financial abuse and integrity tests: duplicate requests, unauthorized release, concurrency, reconciliation | Relevant changes; synthetic ledger |
| CST-009 | AI containment tests: untrusted content, permissions, disclosure and tool boundaries | Model/tool/policy changes |
| CST-010 | Infrastructure, identity, encryption and network configuration checks | Deployment and drift; controlled misconfiguration fixtures |
| CST-011 | Authorized external attack surface inventory and safe non-destructive checks | Scheduled; reconcile known assets |
| CST-012 | Production-equivalent isolated test environments with synthetic data, no real payment rails, explicit scope and budgets | Before active testing; verify isolation and configuration differences |
| CST-013 | Test orchestration selects suites by risk and change impact; full periodic regression catches missed dependencies | Every change and scheduled; selection mutation tests |
| CST-014 | Independent protected verifier binds results to artifact/configuration/test versions and scope | Each required run; reject stale or forged receipts |
| CST-015 | Validate scanners and test harnesses against known positive and negative fixtures; measure false negatives and coverage gaps | Tool changes and scheduled |
| CST-016 | Normalize findings, deduplicate, prioritize by exploitability, impact and exposure; track remediation and retest | Every finding; workflow tests |
| CST-017 | Enforced release gates for material findings and missing required evidence; bounded approved exceptions with expiry | Release; bypass tests |
| CST-018 | Continuous advisory monitoring and re-evaluation of affected deployed artifacts | Advisory updates; simulate new advisory |
| CST-019 | Independent heartbeat detects skipped or failed security jobs | Continuous; stop collector test |
| CST-020 | Publish scoped sanitized internal security assessment reports with methodology, coverage, dates, findings disposition and limitations | Material releases and scheduled; disclosure review |
| CST-021 | Maintain external engagement readiness package: architecture, inventory, threat model, test results, known gaps, scope and rules of engagement | Before external testing |
| CST-022 | Independent external test findings become regression fixtures where safe; verify remediation independently | Each engagement |
| CST-023 | Maintain regulatory, contractual, insurance and auditor testing obligation matrix with deadlines and scope | Onboarding and changes |
| CST-024 | Require explicit authorization, target allowlist, rate limits, stop conditions and provider rules for active testing; no unauthorized third-party testing or destructive production tests | Each run; deny out-of-scope target |
| CST-025 | Record testing costs, runtime, coverage, finding yield and escaped defects; optimize incremental cost without weakening mandatory controls | Periodic review |
| CST-026 | Protect testing infrastructure, credentials and sensitive findings; no public exploit details or customer data | Continuous; access and disclosure tests |

## Build versus reuse
Build F# orchestration, policy evaluation, evidence normalization, domain-specific tests and reusable fixtures. Evaluate existing SAST/SCA/DAST, secret scanners, browser automation and cloud configuration tools before recreating engines. External tools are controlled dependencies, not application-language exceptions by default. No new standalone service unless gap demonstrated. Praxis governs execution; Dokimos validates; Vigila observes; Arca supports evidence integrity; Folio renders reports. Verify actual capabilities. Unbound first reference. Define interfaces and fixtures before implementation.

## Question dispositions
- OQ-300,301: adopt continuous monitoring and testing baseline; legal applicability and exemptions remain open. FTC Safeguards Rule continuous monitoring alternative must be evaluated against exact applicable rule and actual effectiveness; do not assume automation satisfies it.
- OQ-302,303,308,309: automated internal assessments supplement independent testing; obtain actual auditor/customer acceptance, contractual scope and frequency. Production-equivalent testing requires documented differences and authorization.
- OQ-304,305: hosted payment architecture proposed; PCI applicability and exact requirements remain open; verify primary text if applicable.
- OQ-306: verify provider-specific testing rules before active tests.
- OQ-307: obtain insurer requirements during ordinary underwriting.
- OQ-310: prefer independently selected qualified CPA; evaluate independence and peer review, not vendor affiliation alone.
- OQ-311: obtain fixed-scope quotes when needed; compare relevant tester experience, methodology, credentials, independence, reporting and retest. CREST accreditation and OSCP/OSWE/GPEN/GWAPT are possible signals, not universal requirements or proof of quality. No paid engagement authorized.
- OQ-312: demonstrate reproducible synthetic evidence package; auditor acceptance remains open.
- OQ-313: draft coordinated vulnerability disclosure policy with explicit scope and authorization; legal review before publication.

  > *Consolidation note (2026-11-26, not owner text):* under owner direction DF-ASR-2026-0D0F (ODP-12) no legal engagement is authorized now; this review is performed as primary-source research with multiple-agent review, and external legal review is obtained only on a recorded trigger (BR-VUL-013).
- OQ-604,612: automated validation and independent technical review are distinct; external reliance remains open.

## External strategy
Maintain internal assessment report, independent penetration test report and CPA attestation as distinct artifacts. Seek risk-based external scope covering business logic, tenant isolation, authorization, financial workflows, infrastructure and weaknesses in internal testing. Do not exclude required scope solely because internal tests pass. Compare quotes and actual obligations; no guaranteed savings. SOC 2 does not itself prescribe a universal annual penetration test; verify applicable criteria, commitments, laws and contracts before deciding frequency. Do not claim internal tools are accredited or replace mandatory independent work.

## Follow-up
Map to existing controls and open questions; reconcile roadmap, policies, economics and risk register. Research current primary requirements before legal conclusions. No implementation, external testing, purchases or other repository changes authorized by this record. Run governance and register validation before merge.
