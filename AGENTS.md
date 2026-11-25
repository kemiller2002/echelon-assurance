---
id: GV-ASR-START-001
title: Agent contract — Echelon Assurance
status: canonical
version: 0.1.0
owners: [assurance-governance]
created: 2026-11-25
updated: 2026-11-25
review_cycle: quarterly
---

# Agent contract: Echelon Assurance

## Purpose

`echelon-assurance` is Echelon Foundry's authoritative repository for
**organizational assurance**: research, requirements, policy development,
control design, evidence architecture and validation methodology. SOC 1 and
SOC 2 are the initial research priorities; the model must absorb additional
regulatory, contractual, security, operational and financial assurance
requirements without redesign.

This repository supersedes every reference to a "SOC repository" in earlier
Echelon instructions.

## Authority

Implement only behavior supported by requirements or accepted decisions. Do
not invent exceptions, precedence rules, or business policies. Record
unsupported interpretations as unresolved questions. Do not treat your own
proposed decisions as authorized.

(Quoted verbatim from the Praxis authority contract, `GV-START-001`.)

Precedence: explicit owner instruction → law, safety and platform constraints
→ this contract and `SECURITY-AND-CONFIDENTIALITY.md` → accepted decision
records (`docs/decisions/`) → research conclusions → local convention.

## Rules

1. **Confidentiality first.** Read [`SECURITY-AND-CONFIDENTIALITY.md`](SECURITY-AND-CONFIDENTIALITY.md)
   before writing anything. Never commit credentials, production evidence,
   security findings against a live system, specific internal control
   weaknesses, customer data or confidential organizational information.
2. **Evidence or nothing.** Every significant conclusion carries an evidence
   class (see [`methodology/research-methodology.md`](methodology/research-methodology.md)):
   `authoritative`, `auditor-interpretation`, `industry-convention`,
   `customer-expectation`, `vendor-claim`, `recommendation`, `assumption`,
   `open-question`. Never fabricate sources, requirements, prices, approvals,
   test results or audit outcomes.
3. **No status inflation.** A policy is never "operational", a control never
   "effective", and an audit requirement never "satisfied" without cited
   observed evidence. Status vocabulary is fixed in
   [`controls/README.md`](controls/README.md).
4. **Internal ≠ independent.** Agents and automation controlled by Echelon
   Foundry are never described as independent auditors, penetration testers
   or attestation.
5. **Reuse before build.** Do not propose a new Echelon system until existing
   systems have been evaluated in [`integration/`](integration/) and a genuine
   gap is recorded with a decision record.
6. **Read-only dependencies.** Do not modify other Echelon repositories.
   Proposed changes to them are recorded in
   [`integration/proposed-changes.md`](integration/proposed-changes.md).
7. **Stable identifiers.** IDs follow [`methodology/identifiers.md`](methodology/identifiers.md).
   IDs are never reused or renumbered.
8. **Machine-readable registers are authoritative.** CSV registers under
   `registers/`, `controls/`, `regulatory/`, `sources/` and `policies/` are the
   source of truth; Markdown narratives cite them. Run
   `python3 -I tools/validate_registers.py` before committing; it must pass.
9. **Preserve contradictions.** Record disagreeing sources and interpretations;
   do not resolve them silently.
10. **Branches.** `main` is protected. Work on a branch, open a pull request,
    never force-push.
11. **Durable state.** Update [`context/CURRENT-STATE.md`](context/CURRENT-STATE.md)
    and add a journal entry under `research/journals/` for each research batch,
    including what was verified and how.

## Praxis conventions

This repository follows Praxis (Echelon Foundry repository operating system)
conventions: `context/` shared state, canonical research records under
`research/` with `<PREFIX>-<AREA>-<YYYY>-<TOKEN>` identifiers (area `ASR`),
decision records (`DF-ASR-2026-xxxx`) under `docs/decisions/`, the Praxis
confidence vocabulary (`very-low` … `very-high`), and front matter on canonical
records. The `praxis` CLI has **not yet been installed** in this repository
(see `HANDOFF.md`); until it is, the registers and validator here are the
enforcement mechanism, and Praxis-generated registries are absent rather than
hand-written.

## Verify before you push

```bash
python3 -I tools/validate_registers.py
```
