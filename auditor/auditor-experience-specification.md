---
id: ASR-AUD-SPEC-001
title: Auditor experience specification
status: draft
version: 0.1.0
created: 2026-11-25
---

# Auditor experience specification

## 1. What an auditor actually needs

A service auditor issuing a SOC 1 or SOC 2 report has to obtain sufficient
appropriate evidence that (a) management's description is fairly presented,
(b) controls were suitably designed, and (c) for Type II, they operated
effectively throughout the period. In practice that resolves to a small set
of recurring requests (`auditor-interpretation`; see `soc/soc-reporting-landscape.md`):

| Need | Why | Typical conventional artifact |
|---|---|---|
| System description and boundaries | Description criteria; scoping | Narrative document, diagrams |
| Control descriptions with owners | Design evaluation | Control matrix spreadsheet |
| Policy versions in force during the period | CC5.3; design | PDFs with approval dates |
| **Complete populations** of control-relevant events | Sampling requires a complete, accurate population (information produced by the entity, IPE) | System exports + screenshots of the query |
| Evidence for selected samples | Operating effectiveness | Screenshots, tickets |
| Exceptions and how they were handled | Deviation evaluation | Exception log |
| Remediation history | Deficiency evaluation | Tickets |
| Responsible personnel and changes in personnel | Description; CC1 | Org chart |
| Subservice organizations and CSOCs; CUECs | Carve-out/inclusive | Vendor list, SOC reports |

The single most expensive item in conventional audits is **population
completeness and IPE testing**: proving that the list the auditor samples from
is complete and was not edited. This is where an evidence architecture can
create the largest real saving (`recommendation`, confidence `medium`: depends
on the auditor accepting the reliance approach — see OQ register).

## 2. Design principles

1. **Read-only, scoped access.** Auditors get a time-boxed, read-only identity
   scoped to the examination period and in-scope systems. No access to
   production systems themselves; access to evidence and to the systems'
   *exports*.
2. **Reproducible queries.** Every population is produced by a versioned,
   published query against the evidence store, with the query text, its
   version, the execution timestamp and the result hash recorded. An auditor
   can re-run it and get the same result (IPE re-performance).
3. **Completeness proofs alongside populations.** Each population includes
   its completeness evidence: source-system counts for the same period, the
   evidence log checkpoint (Merkle root) covering the period, and the
   reconciliation result (`architecture/evidence-architecture.md`).
4. **Failures are first-class.** Populations include denials, exceptions,
   missed heartbeats and remediation — not only successes.
5. **No self-attestation dressing.** Internal automated validation results are
   labeled with their independence level (`registers/validation-procedures.csv`)
   and are never presented as auditor testing.
6. **Auditor chooses samples.** The interface supports auditor-driven sample
   selection (random seed recorded by the auditor) and returns sample evidence
   bundles; management does not pre-select.

## 3. Capabilities

| Capability | Specification | Implemented by (proposal; see `integration/`) |
|---|---|---|
| Control catalog view | Control ID, objective, owner, mechanism, policy section, frameworks, status as of a date | Rendered from `controls/control-catalog.csv` + policy model |
| Policy history | Every version of each policy section with approval record and effective dates | Git history of the policy model; approval events |
| Period-scoped populations | For each control, the population of events in [start, end] with completeness evidence | Evidence store query + checkpoint |
| Sample request | Auditor submits IDs or a seed; system returns evidence envelopes with source references | Evidence store |
| Exception and remediation history | Exceptions with approver, expiry, compensating control; deficiencies with remediation timeline | Exception register / work tracker |
| System boundary view | Components, data flows, subservice organizations, CUECs/CSOCs | System description generated from architecture records |
| Personnel view | Roles, role holders over time, training/acknowledgement evidence | HR/identity events |
| Evidence package export | Signed, hash-manifested archive (ZIP or OCI artifact) per request, including query text and checkpoints; optional OSCAL `assessment-results`-adjacent JSON | Export function (Folio is the candidate, pending verification) |
| Verification kit | Offline tool + instructions to verify manifest hashes, signatures and log inclusion proofs without trusting Echelon infrastructure | Small open verification script |
| Access log | Everything the auditor viewed/exported is itself logged | Evidence store |

## 4. Structured formats

OSCAL (NIST) provides models for catalogs, profiles, component definitions,
SSPs, assessment plans, assessment results and POA&Ms. It is mature in the
FedRAMP ecosystem; we found no evidence that SOC service auditors consume
OSCAL directly (`open-question`; see `policy-as-code/tool-evaluation.md`).
Recommendation: generate OSCAL as a secondary export from the authoritative
model (cheap if the model is structured) but deliver auditors the formats they
actually use (CSV/XLSX populations, PDF/HTML descriptions, evidence archives).

## 5. Sensitive information

Auditor packages contain restricted information. Packages are generated on
request, encrypted to the requesting firm, logged, and expire. Nothing from an
auditor package is committed to any repository.

## 6. Questions for the first auditor engagement

See `registers/open-questions.csv` (auditor resolver): acceptance of
system-generated populations with completeness proofs as IPE evidence;
acceptance of continuous automated validation results as a basis for reduced
sample sizes or as management's monitoring (CC4.1) only; preferred export
format; whether a read-only evidence portal changes fieldwork scope or price.
