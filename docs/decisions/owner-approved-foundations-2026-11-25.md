# Owner-approved assurance foundations

Date: 2026-11-25
Status: Approved design direction by owner in conversation. Not evidence of implementation, legal applicability, or auditor acceptance.

| Decision | Related questions | Approved direction |
|---|---|---|
| 1 | OQ-753, OQ-756, OQ-102 | Echelon operates Unbound as commercial technology and servicing platform for sponsors and financial institutions; participants use it through those organizations. Support white-label without requiring Echelon to sponsor or custody assets. Legal structure remains to be confirmed. |
| 2 | OQ-101, OQ-103, OQ-112, OQ-754, OQ-755 | Design for SOC 1 Type II and SOC 2 Type II, all five trust categories, shared controls. Actual examination scope and engagement remain subject to customer and auditor confirmation. Type I or SOC 3 only with demonstrated commercial benefit. |
| 3 | OQ-200–203 | Adopt substantive GLBA safeguards as design baseline without asserting legal applicability. |
| 4 | OQ-701, OQ-800, OQ-763 | Human standing authorization, separate implementing agent, technical verifier and enforced release gate for routine low-risk work. Elevated approval for material policy, privilege, financial exceptions and verifier changes. Explore fractional human review when required; technical independence is not organizational independence. |
| 5 | OD-GOV-002, OQ-214 | Public research, general policies, control objectives, methodology and sanitized results; separately restricted production evidence, vulnerabilities, secrets, personnel/customer data and confidential reports. Respect source licensing. |
| 6 | OQ-708 | Adopt Praxis and F# standards; migrate Python validators with equivalent tests. |
| 7 | OQ-700, OQ-704, OQ-761 | Require verified completion, enforced checks, authenticated provenance, protected branches and controlled releases; emergency exceptions evidenced and reviewed. Verify actual settings separately. |
| 8 | OQ-702, OQ-706 | Separate authenticated agent identities and bind provenance to platform identities and signed execution records where practical. |
| 9 | OQ-801, OQ-804 | Automate approved routine retention expiration with legal hold checks, bounded impact and evidence. Elevated safeguards for mass deletion, key destruction, retention changes and evidence destruction. Reversibility is not universally required. |
| 10 | OQ-705, OQ-759, OQ-805 | Separate evidence generation and storage; use managed durable production storage with appropriate immutability and retention. Arca integrity capabilities subject to verification. GitHub is not sole production evidence store. Define interfaces before custom ledger. |
| 11 | OQ-751 | Folio renders authoritative evidence packages; not responsible for evidence collection, validity or authoritative storage. |
| 12 | OQ-760 | Strata is database interaction layer: controlled access, transactions, parameterization, schema governance, testability and instrumentation; not entire evidence system. |
| 13 | OQ-806 | Vigila is logging and observability initiative integrating managed infrastructure; independently operated heartbeat required. |
| 14 | OQ-802, OQ-803 | Optimize lifecycle cost and risk per control: eliminate risk, prevent, detect, collect evidence, bounded remediation, human judgment, manual administration in that order where appropriate. Risk-proportionate validation; external independence where necessary or materially beneficial. |

## Reconciliation instructions

Align policies, control catalog, roadmap, architecture, economics and question register with these decisions. Preserve question IDs and external-confirmation dependencies. Do not mark auditor acceptance, legal conclusions, actual configuration, staffing or operating effectiveness as resolved merely because a design is approved. Review existing public contents for sensitive material before merge. Apply Praxis governance and validation before merging. This record does not authorize changes to other repositories.

Guiding tenet: Every manual compliance activity must justify why it cannot be eliminated, enforced architecturally, automated or evidenced through normal operations.
