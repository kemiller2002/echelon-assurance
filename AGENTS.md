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
   `dotnet run --project src/Echelon.Assurance.Cli -- validate` before committing; it must pass.
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
records. Praxis 3.25.0 was installed in this repository on 2026-11-25 (`./praxis`,
`.echelon/ros.json`; mandatory per owner direction DF-ASR-2026-0D10). Run
`./praxis registry build` and `./praxis validate` alongside the assurance
validator. Praxis registries under `registries/` are generated, not hand-written.
Repository rulesets that make both checks required are an owner action (WI-ASR-03).

## Verify before you push

```bash
dotnet tool restore
dotnet fantomas --check src tests
dotnet test Echelon.Assurance.sln -c Release
dotnet run --project src/Echelon.Assurance.Cli -- validate
dotnet run --project src/Echelon.Assurance.Cli -- merge --check
dotnet run --project src/Echelon.Assurance.Cli -- governed --check
./praxis registry check && ./praxis validate
```

**Governed record.** A change to a governed field (status, applicability,
independence, authority, gating, legal position or customer answer; list in
`src/Echelon.Assurance.Registers/Governed.fs`) fails the tests until the
snapshot `tests/Echelon.Assurance.Tests/governed-record.csv` is regenerated
with `assurance governed`. Regenerate only after the change is justified in the
pull request, citing its authority; the snapshot diff is what reviewers check.

Register tooling is F# (owner decision DF-ASR-2026-0D06): `src/Echelon.Assurance.Registers`
(pure library: CSV, schema, validation, merge, roadmap, TCO) and
`src/Echelon.Assurance.Cli` (`assurance`, the only code with effects).
