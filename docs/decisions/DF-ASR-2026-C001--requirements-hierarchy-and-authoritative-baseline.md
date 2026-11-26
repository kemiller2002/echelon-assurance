---
id: DF-ASR-2026-C001
title: Requirements hierarchy and authoritative baseline register
status: accepted
decided_by: owner — accepted in principle by owner (DF-ASR-2026-0D10, ODP-01), subject to traceability and validation
date: 2026-11-25
related_questions: [OQ-802]
---

# DF-ASR-2026-C001: Requirements hierarchy and authoritative baseline register

## Context
Nine requirement documents (security, financial integrity, executable policy and evidence, AI governance and validation, continuous security testing, economics, organizational governance, reconciliation, and the PR #2 continuous assurance document) stated 189 owner requirements with heavy overlap and no single place recording owner, enforcement, validation, evidence, residual risk and disposition per requirement. Owner-requirement IDs were partly registered in `regulatory/requirements.csv` (102 of 189) and partly not.

## Decision (proposed)
1. The hierarchy is: tenets (`registers/tenets.csv`) → policies (`policies/policy-inventory.csv`) → control objectives (`controls/control-catalog.csv`) → baseline requirements (`requirements/baseline.csv`) → enforcement, validation and evidence (columns of the baseline row, referencing `registers/validation-procedures.csv` and `registers/evidence-types.csv`).
2. `requirements/baseline.csv` is the authoritative register of Echelon-internal (owner-mandated) requirements. `regulatory/requirements.csv` remains authoritative for externally derived obligations; its existing owner rows (REQ-SBC, REQ-FINBC, REQ-PEC, REQ-AIGBC, REQ-VALBC) keep their IDs for stability and are dispositioned in `requirements/source-dispositions.csv`.
3. Every original owner requirement ID has exactly one disposition row naming its baseline requirement(s). The same applies to the 18 challenge findings in `reconciliation-and-roadmap.md` (REC-01..18) and the security-by-construction "validation and claims" paragraph (SBC-VC-01). `challenge-review.md` is rationale for the SBC and FIN-BC dispositions and adds no obligations. The nine Markdown documents are preserved as historical source records and are not edited further; where they conflict with the baseline, the baseline governs.
4. `assurance validate` enforces bidirectional traceability, missing owners/validation/evidence, unsupported controls, status inflation, tenet coverage, question disposition coverage and dependency-ordered backlog.

## Consequences
Reversible; removes the need to maintain nine overlapping documents. No mandatory obligation is removed: the only `remove` disposition is REC-04, an editorial identifier instruction. Scope narrowing is labelled `simplify`, with the narrowing stated. The baseline already reflects decisions C002, C003 and C006; if one is rejected, the affected rows revert.

Proposed by the assurance consolidation (journal `JR-ASR-2026-C001`). Not
authorized until the owner accepts it; see `docs/decisions/owner-decision-package-2026-11-25.md`.

*Accepted in principle with the baseline (owner record DF-ASR-2026-0D10, ODP-01), subject to traceability and validation; read with owner direction DF-ASR-2026-0D0F. The sentence above is historical (note added 2026-11-26).*
