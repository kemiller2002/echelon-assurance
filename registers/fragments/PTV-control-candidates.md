# PTV control candidates (penetration testing, security testing, compliance platforms)

These are proposals only. Every candidate has status `Proposed`, and no CTL IDs
are assigned; the coordinator assigns them. Most candidates sit in the `VUL`
domain, with `VEN`, `ASR` and `GOV` where noted.

- Sources: `sources/fragments/PTV.csv`.
- Requirements: `regulatory/fragments/PTV.csv` (REQ-PENTEST-nnn).
- Open questions: `registers/fragments/PTV-open-questions.csv` (OQ-300..313).
- Narrative: `pentest/penetration-testing-research.md`,
  `pentest/unbound-minimum-cost-testing-strategy.md`,
  `vendors/compliance-platform-analysis.md`.

## PT-1 Penetration testing program and rules of engagement (VUL/GOV)

**Objective.** Independent penetration testing is planned, scoped by risk,
authorized in writing and repeatable.

**Risk.** Without a program:
- tests are ad hoc or mis-scoped;
- testing breaches cloud-provider rules or harms data;
- results cannot be compared year over year.

**Mechanism.** A testing standard defines:
- scope selection from the threat model;
- frequency: annual plus significant-change triggers;
- independence criteria;
- tester qualification criteria;
- a rules-of-engagement template, including the cloud-provider policy
  (OQ-306) and a synthetic-data-only rule;
- report handling.

Each engagement record links to the threat-model version and to the
configuration hash of the environment that was tested.

**Requirements.** REQ-PENTEST-001, -002, -007, -011, -013, -019.

**Automation target: L1.** Engagements are scheduled and the significant-change
triggers create tasks automatically. Scoping and authorization stay human
decisions.

**Failure mode: detective-only.** A missed test is detected through a schedule
alert.

**Evidence.**
- Signed rules of engagement.
- Scope document.
- Tester credential record.

**Independence.** The owner approves. The work is performed externally.

## PT-2 Independent external penetration test before launch and periodically (VUL)

**Objective.** A qualified external tester checks whether authentication,
authorization, tenant isolation and financial business logic resist attack. The
test runs before real money or partner data, and then annually or after
significant change.

**Risk.** Exploitable flaws that internal testing missed. A key cause is
self-review blindness: the people and agents who built the system are not
independent.

**Mechanism.** A manual grey-box test of a production-equivalent environment,
scoped as described in the Unbound strategy section 5.

**Requirements.** REQ-PENTEST-002, -007, -011, -014, -017, -019.

**Automation target: L0.** The test is human professional work. Only scheduling
and tracking are automated, under PT-1 and PT-4.

**Failure mode: fail-closed for launch.** Proposed launch gate: real funds and
partner data are not onboarded until the test and the retest of
critical/high findings are complete, or the owner signs a documented risk
acceptance.

**Evidence.**
- Report, kept in the restricted store.
- Attestation letter.
- Retest report.

**Independence.** `external-penetration`. Automated or agent output never
substitutes for it.

## PT-3 Tester qualification and independence verification (VUL/VEN)

**Objective.** The tester is competent and free from conflicts.

**Risk.** A scan sold as a pen test, or an unqualified or conflicted tester.

**Mechanism.** A pre-engagement checklist confirms:
- the firm's accreditation (for example CREST) or equivalent QA, insurance and
  methodology evidence;
- the named lead tester holds a hands-on application credential (CCT-APP,
  OSWE, GWAPT or equivalent) and has relevant experience, following the PCI SSC
  questions (SRC-PTV-006);
- tester-days and a sample report;
- no involvement in building, operating or auditing Unbound.

**Requirements.** REQ-PENTEST-006, -014.

**Automation target: L0.** This is a judgment.

**Failure mode: fail-closed.** No engagement starts without a completed
checklist.

**Evidence.** The completed checklist.

**Independence.** Owner review.

## PT-4 Finding remediation, exception and retest tracking (VUL)

**Objective.** Every pen-test and scanner finding is triaged, fixed within its
SLA or formally excepted, and verified.

**Risk.** Findings stay open. Retests are paid for but fixes are not ready.

**Mechanism.** Findings sit in a restricted tracker and are referenced in this
repository only by opaque ID and severity. The tracker enforces:
- SLAs by severity;
- exceptions that need an owner decision and an expiry date;
- retests by the external tester for critical/high findings.

**Requirements.** REQ-PENTEST-003, -008. Unbound SOC2-016.

**Automation target: L3.** Overdue findings and expired exceptions alert
automatically. A release-gate integration (L4) is possible for open criticals.

**Failure mode: fail-closed** for open critical findings at release, once the
L4 gate exists. **Detective-only** until then.

**Evidence.**
- Tracker export.
- Retest report.
- Exception records.

**Independence.** The retest is external. Tracking is internal.

## PT-5 Continuous internal security testing in CI (VUL/SDL)

**Objective.** Known-pattern vulnerabilities, vulnerable dependencies, secrets
and IaC misconfigurations are blocked before deployment.

**Risk.** Regression into known-bad patterns. The pen-test budget gets spent on
issues a scanner could have found.

**Mechanism.**
- Dependency advisory scanning.
- Secret scanning.
- IaC policy checks.
- SAST where tools cover F#; coverage is verified per tool.
- Authenticated DAST against staging each release.

All of this is internal developer testing in the sense of NIST SA-11.

**Requirements.** REQ-PENTEST-008, -011, -016.

**Automation target: L4.** The merge or deploy gate blocks new
critical/high findings. L5 auto-remediation applies to dependency bumps only,
through bounded automated PRs.

**Failure mode: fail-closed**, with a justified, time-limited override
recorded as an exception under PT-4.

**Evidence.**
- CI check runs with scanner versions.
- Scan reports.

**Independence.** `automated`. Never described as a pen test.

## PT-6 Authorization-matrix and tenant-isolation adversarial suite (VUL/TEN)

**Objective.** Every combination of role, endpoint, tenant and resource
behaves as specified, in all tenancy modes.

**Risk.** Cross-tenant data exposure and privilege escalation. These are the
highest-impact flaws for a multi-partner DAF platform.

**Mechanism.** Executable tests are generated from the authorization
specification. They include cross-schema and cross-database probes and
identifier tampering, and they run on every build against seeded synthetic
tenants.

**Requirements.** REQ-PENTEST-016. This is also input to PT-2 scoping.

**Automation target: L4.** The suite is a fail-closed merge gate.

**Failure mode: fail-closed.**

**Evidence.**
- Test results per build, with suite coverage counts. This gives population
  coverage of the matrix.

**Independence.** `automated`. The suite's design is shown to the external
tester for challenge.

## PT-7 Vulnerability disclosure policy and security.txt (VUL/IR)

**Objective.** External researchers have a safe, documented channel, and
reports are triaged.

**Risk.** Vulnerabilities go unreported, or researchers face legal ambiguity.

**Mechanism.**
- `/.well-known/security.txt` (RFC 9116), with Contact and Expires and
  signed.
- A VDP page adapted from the CISA/DOJ-aligned template, with counsel-reviewed
  safe-harbor language (OQ-313).
- Triage feeds PT-4.

**Requirements.** REQ-PENTEST-015, -021, -022.

**Automation target: L3.** An automated check alerts before the security.txt
Expires date passes and checks that the file is reachable. Triage is human.

**Failure mode: detective-only.**

**Evidence.**
- security.txt history.
- Report log with acknowledgment times.

**Independence.** Reports are external and unsolicited.

## PT-8 Pen-test report confidentiality and customer disclosure (VUL/CUS)

**Objective.** Reports are protected, and customer statements match the actual
scope and date.

**Risk.**
- A leaked report gives attackers a roadmap.
- Statements overstate assurance, for example calling scans pen tests.

**Mechanism.**
- Reports go only to the restricted store.
- Customers receive a summary or attestation letter under NDA.
- A claim-review step checks every external statement against the
  engagement record (Unbound SOC2-020).

**Requirements.** REQ-PENTEST-019.

**Automation target: L1.**

**Failure mode: fail-closed.** No external statement goes out without
review.

**Evidence.**
- Disclosure log.
- Claim-review records.

**Independence.** Owner review.

## PT-9 Compliance-tooling access and independence safeguards (VEN/ASR)

This candidate applies only if a compliance platform is adopted.

**Objective.** A compliance platform or collector does not become an excessive
read path, and it does not compromise auditor independence.

**Risk.**
- A vendor compromise exposes the whole environment map.
- Evidence is templated or pre-populated.
- The auditor's independence is impaired through a bundled or affiliated
  arrangement (SRC-PTV-042, allegations).

**Mechanism.**
- Least-privilege read-only integration identities, one per integration,
  logged.
- No write scopes.
- The vendor is listed as a subservice organization in vendor risk.
- The CPA firm is selected independently of the software vendor.
- The auditor gets direct read access to source evidence.
- Export of the full evidence history is tested before signing.

**Requirements.** None. This candidate rests on a risk-based recommendation.

**Automation target: L3.** A drift check on the integration role's permissions
is feasible.

**Failure mode: detective-only.**

**Evidence.**
- Role policy exports.
- Access logs.
- Auditor-selection record.

**Independence.** `internal-independent` where possible; otherwise owner
review.
