# SOC control candidates (research key SOC)

These are proposed controls for the coordinator's unified catalog. No CTL IDs
are assigned. Status for all: **Proposed**. None is implemented or evidenced.

Notation:

- *Req* lists the requirement rows each control serves, from
  `regulatory/fragments/SOC.csv`.
- Automation levels L0–L5 follow the program scale. L0 is manual. L5 is fully
  autonomous, with humans only for exceptions.
- **Internal ≠ independent:** every control below is a management control.
  None substitutes for the service auditor's procedures.

---

## SOC-C1 Assurance scope and system-boundary register

| Attribute | Proposal |
|---|---|
| Domain | GOV / ASR |
| Objective | Keep one authoritative, versioned definition of each report's scope: services, legal entity, components, in-scope TSC categories, SOC 1 control objectives, period and boundary. |
| Risk | The description omits or distorts the system (AT-C 320.15c; DC3). Report scope drifts from what sales and contracts claim. Assertion is made without a reasonable basis. |
| Req | REQ-SOC1-002, REQ-SOC1-003, REQ-SOCDC-001..003 |
| Mechanism | Machine-readable scope manifest under change control. Every component, data store, vendor and agent identity is tagged in or out of scope with a rationale. A CI check fails if a deployed component has no scope tag. |
| Automation target | **L3.** Detection of untagged components is automated. Scope decisions remain human management judgment, because the standards make scope management's responsibility. |
| Failure mode | **Fail-closed** for deployments: an untagged component blocks release to the in-scope environment. |
| Evidence | Manifest versions, deployment-to-manifest diff reports, approval records. |
| Independence | Management control. Reviewed by the service auditor during scoping. |

## SOC-C2 Management description generated from canonical registers

| Attribute | Proposal |
|---|---|
| Domain | ASR |
| Objective | Produce the SOC 1 and SOC 2 system descriptions from the control catalog, scope manifest, CUEC/CSOC registers, incident register and change log, so the description matches operation. |
| Risk | The description is stale or inaccurate, leading to a modified opinion (AT-C 320.42a). It may also disclose exploitable detail, against the 2026 description-criteria emphasis (SRC-SOC-010). |
| Req | REQ-SOC1-003, REQ-SOC1-004, REQ-SOC1-012, REQ-SOCDC-001..009 |
| Mechanism | Template rendering from registers. A human editorial and legal review gate checks fairness, completeness and vulnerability-disclosure risk. Diffs between periods are kept. |
| Automation target | **L2.** Drafting is automated, but management signs the assertion. An agent cannot hold management's responsibility. |
| Failure mode | **Fail-closed.** No description is released without a recorded management approval. |
| Evidence | Generated draft, review comments, approved version hash, signed assertion stored in the restricted store. |
| Independence | Management. |

## SOC-C3 CUEC register and user-entity communication

| Attribute | Proposal |
|---|---|
| Domain | CUS / FIN |
| Objective | Maintain the controls that sponsors and partners must operate, such as authorising access requests, reviewing statements and approving grants above thresholds. Communicate them contractually and in onboarding. |
| Risk | User entities do not perform the assumed controls, so control objectives fail. User auditors cannot map CUECs (AU-C 402.14c/.17b; AU-C 9402 notes that SOC 1 and SOC 2 CUECs differ). |
| Req | REQ-SOC1-008, REQ-SOCDC-006, REQ-SOC1-022 |
| Mechanism | A register keyed to the existing Unbound `CUEC-CNV-FIN-*` IDs. Each CUEC is tagged SOC 1, SOC 2 or both. Contract schedule. In-product acknowledgment at tenant onboarding and on change. |
| Automation target | **L3.** Distribution and acknowledgment tracking are automated. Wording is human-approved. |
| Failure mode | **Fail-closed for tenant activation.** A tenant cannot activate without a CUEC acknowledgment. Otherwise detective. |
| Evidence | Register versions, acknowledgment records per tenant. |
| Independence | Management. |

## SOC-C4 Subservice organization (CSOC) monitoring

| Attribute | Proposal |
|---|---|
| Domain | VEN |
| Objective | For each carved-out subservice organization (cloud, custodians, processors, AI providers), do the following:<br>• obtain its SOC report each period;<br>• check that the report period overlaps ours;<br>• review its opinion and exceptions;<br>• map its CUECs to our controls and our CSOCs to its controls;<br>• track bridge letters for gaps. |
| Risk | A carved-out dependency fails unnoticed. CSOCs are unsupported. The user auditor's AU-C 402.18 burden goes unmet. A CC9.2 failure follows. |
| Req | REQ-TSC-033, REQ-SOC1-007, REQ-SOCDC-007 |
| Mechanism | A vendor register with report metadata. An automated coverage-gap calculator compares report periods with our period. A human review records the exception analysis. |
| Automation target | **L3.** Collection and coverage computation are automated. Exception evaluation is human, because it needs judgment about effect on our objectives. |
| Failure mode | **Detective-only**, with escalation. A missing or qualified subservice report raises a risk record. Compensating controls must be documented before period end. |
| Evidence | Retained reports (restricted store), review records, coverage reports. |
| Independence | Management. |

## SOC-C5 IPE completeness and accuracy for evidence

| Attribute | Proposal |
|---|---|
| Domain | ASR |
| Objective | Make every population and report given to an auditor reproducible and provably complete. |
| Risk | The auditor cannot rely on system-generated evidence (AT-C 205.36; AT-C 320.30, .A52), which drives up testing or leads to a scope limitation. |
| Req | REQ-SOC1-010, REQ-SOC1-011 |
| Mechanism | For each population: <br>• the source query is versioned in the repository;<br>• record counts are reconciled to independent control totals, such as ledger totals and bank statements;<br>• the output is hashed and the hash anchored;<br>• the extraction is logged with the actor;<br>• the auditor can run the query directly under read-only credentials. |
| Automation target | **L4.** Population extraction and reconciliation run automatically. The auditor's own extraction path is preserved, so automation never replaces independent extraction. |
| Failure mode | **Fail-closed** for evidence release. A population that fails reconciliation is not delivered and opens an exception. |
| Evidence | Query version, row counts, reconciliation result, hash, access log. |
| Independence | Management. The service auditor still tests IPE. |

## SOC-C6 Restricted-use report distribution

| Attribute | Proposal |
|---|---|
| Domain | DAT / GOV |
| Objective | Release SOC 1 and SOC 2 reports only to permitted specified parties: user entities, their auditors, and knowledgeable prospects under NDA. Never place them in public repositories. |
| Risk | The restricted-use alert is breached (AT-C 320.40n). Description detail leaks. Confidentiality is breached under this repository's rules. |
| Req | REQ-SOC1-014 |
| Mechanism | Trust-portal release with an NDA gate, watermarking and an access log. A secret-and-content scanner in CI blocks report-like PDFs and text patterns from this repository. |
| Automation target | **L4.** The gate and logging are automated. Approval of a recipient category is human. |
| Failure mode | **Fail-closed.** No NDA means no access. |
| Evidence | Access logs, NDA records. |
| Independence | Management. |

## SOC-C7 Service auditor qualification and independence check

| Attribute | Proposal |
|---|---|
| Domain | GOV |
| Objective | Before engagement and annually, verify the following and record the result:<br>• the service auditor is a CPA firm licensed in the relevant states;<br>• peer-review results have been reviewed;<br>• the firm did not design or operate the controls it examines;<br>• the firm has no fee or referral dependency on a compliance-platform vendor;<br>• its timeline is plausible. |
| Risk | The report is rejected by user auditors (AU-C 402.13a). A "quick-turn" or unlicensed provider is used, the risk flagged by the AICPA notice and the 2026 peer-review alert (SRC-SOC-008, -013, -014). |
| Req | REQ-SOC1-015, REQ-SOC1-016 |
| Mechanism | Checklist plus public-register lookups (state board license, peer-review public file). Independence representation from the firm. |
| Automation target | **L2.** Lookups can be assisted. The decision needs human accountability. |
| Failure mode | **Fail-closed.** No engagement letter is signed without a completed check. |
| Evidence | Completed checklist, lookup snapshots, firm representation. |
| Independence | Owner or board approval, separate from the team that operates the controls. |

## SOC-C8 Significant-change and period-continuity tracking

| Attribute | Proposal |
|---|---|
| Domain | SDL / ASR |
| Objective | Flag system and control changes during a report period that users would consider significant. Feed them to the description (DC9; AT-C 320.29). Keep superseded-control evidence. Monitor period continuity so there are no unplanned gaps between report periods. Control any bridge letter as a management statement reviewed against the actual change log. |
| Risk | Undisclosed changes, untestable superseded controls, and bridge letters asserting "no significant changes" that the change log contradicts. |
| Req | REQ-SOC1-004, REQ-SOC1-012, REQ-SOCDC-009 |
| Mechanism | Change records carry a `soc_significance` classification at approval. A period dashboard tracks open periods. Bridge-letter drafts are generated from the change log, then approved by management. |
| Automation target | **L3.** Classification is suggested automatically. A human confirms significance. |
| Failure mode | **Fail-closed** for bridge letters: none is issued without a reconciled change-log review. Detective for classification. |
| Evidence | Change records, significance decisions, retained superseded-control evidence, bridge-letter approvals. |
| Independence | Management. A bridge letter carries no auditor assurance and must say so. |

## SOC-C9 Deviation and exception management with management response

| Attribute | Proposal |
|---|---|
| Domain | GOV |
| Objective | Find control deviations internally before the auditor does:<br>• classify them;<br>• identify the cause, including possible fraud (AT-C 320.33);<br>• remediate;<br>• prepare management's response for the report. |
| Risk | Unexplained deviations lead to a qualified opinion. Fraud-related deviations go unescalated. |
| Req | REQ-SOC1-018, REQ-SOC1-009, REQ-TSC-014 |
| Mechanism | Every control's evidence stream includes failure events, which the Unbound directive SOC2-005 requires. Deviation records link to the control and cause. There is an SLA for root-cause analysis. Board-level reporting covers deviations over a threshold (CC4.2). |
| Automation target | **L4** for detection. **L1–L2** for root-cause judgment and the management response. |
| Failure mode | **Detective-only**, with a mandatory escalation timer. |
| Evidence | Deviation register, root-cause analysis records, remediation evidence, board minutes (restricted). |
| Independence | Management. Internal review by a party outside the control's operating chain. |

## SOC-C10 Written-representation readiness log

| Attribute | Proposal |
|---|---|
| Domain | GOV |
| Objective | Keep a running log so management can truthfully make the representations AT-C 320.36 requires. It covers:<br>• noncompliance with laws;<br>• uncorrected misstatements affecting user entities;<br>• known, suspected or alleged fraud;<br>• subsequent events (320.35);<br>• communications to affected user entities (320.45). |
| Risk | Management cannot represent, which is a scope limitation that may produce a disclaimer (320.38). Affected users are not told. |
| Req | REQ-SOC1-009, REQ-SOC1-017, REQ-SOC1-021 |
| Mechanism | Restricted register fed from incident, legal and fraud case systems. Quarterly management attestation. |
| Automation target | **L2.** Aggregation is automated. Content judgment is legal or management. |
| Failure mode | **Detective-only**, with escalation to counsel. |
| Evidence | Register (restricted), quarterly sign-offs. |
| Independence | Management and counsel. |

## SOC-C11 Assurance-claims governance

| Attribute | Proposal |
|---|---|
| Domain | CUS |
| Objective | Reconcile every external claim to an actual report's scope, period and opinion. This covers marketing, RFP answers, contracts and trust-portal text. Forbid words such as "certified", "SOC compliant" and "SOC 2 certification". |
| Risk | Misleading claims of the kind seen in the market (SRC-SOC-026 `vendor-claim`). Liability for false claims. Inconsistency with the Unbound directive SOC2-020. |
| Req | REQ-SOC1-014 (indirect); Unbound SOC2-020 |
| Mechanism | A claims library with an approved-phrasing linter applied to the website, documents and RFP tooling. Each claim cites a report ID. |
| Automation target | **L4** for linting. Human approval of new claims. |
| Failure mode | **Fail-closed** for the website and RFP template pipeline. |
| Evidence | Linter results, approved-claims register. |
| Independence | Management, reviewed by counsel. |

## SOC-C12 Automated application controls with ITGC dependency mapping

| Attribute | Proposal |
|---|---|
| Domain | FIN / SDL |
| Objective | Implement key accounting controls as automated, deterministic application controls. Examples include balanced journals, grant-eligibility checks, fee calculation, duplicate-payment prevention and segregation-of-duties enforcement. Record each control's dependency on IT general controls (change management, access) so the auditor can apply AT-C 320.A47. |
| Risk | Heavy manual-control sampling cost. Automated controls cannot be relied on because their ITGC dependencies are unmapped (320.31b). |
| Req | REQ-SOC1-011, REQ-SOC1-006, REQ-TSC-031, REQ-TSC-020 |
| Mechanism | Control-as-code with version pinning. A dependency map from each application control to its ITGCs. Configuration baselines for detecting changes to control parameters. |
| Automation target | **L5** for transaction-level enforcement, with human approval of configuration changes. Monitoring is **L4**. |
| Failure mode | **Fail-closed.** Transactions that fail a control do not post. |
| Evidence | Control configuration history, change approvals, test results for each change, rejected-transaction logs. |
| Independence | Management. The service auditor tests design, implementation and ITGC. |
