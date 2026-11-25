---
id: RS-ASR-2026-REG2
title: Framework crosswalk method - one unified control model
status: draft
version: 0.1.0
owners: [assurance-governance]
created: 2026-11-25
updated: 2026-11-25
review_cycle: semiannual
---

# Framework crosswalk method

Goal: one Echelon control catalog (`controls/control-catalog.csv`, `CTL-*`)
that satisfies SOC 1/SOC 2, the legal and contractual requirements in
`regulatory/regulatory-applicability-matrix.md`, and voluntary frameworks,
**without** maintaining parallel catalogs. Sources: `SRC-REG-*`.

## 1. Requirements of the method

1. **Requirement-first, not framework-first.** Every `CTL-` traces to `REQ-`
   rows that carry an applicability class; a control with only voluntary
   drivers is labelled so and can be deferred.
2. **One outcome taxonomy, one control vocabulary.** Outcomes answer "what
   must be true"; controls answer "what mechanism makes it true".
3. **Explicit relationship semantics.** "Maps to" is not enough; record
   whether the Echelon control is equal to, a subset of, a superset of, or
   intersects the external element (NIST IR 8477 set-theory style,
   SRC-REG-029; also used by SCF STRM, SRC-REG-054).
4. **Licence-safe.** The catalog may become public (repository visibility is
   currently public) and may be embedded in Echelon products; it must not
   republish restricted text.
5. **Machine-readable and diffable.** CSV/OSCAL, version-pinned to framework
   releases, so a framework revision produces a reviewable delta.

## 2. Anchor options compared

| Option | Strengths | Weaknesses | Licensing | Verdict |
|---|---|---|---|---|
| **NIST CSF 2.0 outcomes + SP 800-53r5 controls** | Public US-government works; CSF named by regulators as CAT successor (SRC-REG-019); 800-53 is the hub of NIST OLIR/CPRT mappings and available in OSCAL (SRC-REG-026, SRC-REG-030); AICPA and CSA publish mappings to both (SRC-REG-055) | 800-53 is large and federal-flavoured; needs tailoring; CSF outcomes are not testable alone | NIST material is generally reusable with attribution (NIST copyright page, not fully read; US-government works are not subject to US copyright — general knowledge) | **Recommended anchor** |
| Secure Controls Framework (SCF) | ~1,000+ controls pre-mapped to 200+ frameworks with STRM relationships; fast | Third-party judgment on every mapping; licence conflict (CC BY-ND 4.0 on production pages vs CC BY 4.0 on a staging page) — "no derivatives" conflicts with tailoring and republishing | CC BY-ND 4.0 likely; commercial licence exists | Use as **reference cross-check** only, cite by ID; OQ-214 |
| CSA CCM v4.1 | Cloud-native; CAIQ is a sellable artefact; CSA maps to ISO 27001/17/18 (in peer review) (SRC-REG-038) | Cloud-centric; v4.1 mappings still being rebuilt; licence terms not verified | Not verified | Secondary mapping target (customer-expectation) |
| ISO/IEC 27001 Annex A | Internationally recognised; SoA structure is a good applicability model | Text paywalled; Annex A is thin on application security | Paywalled; cannot republish | Mapping target; borrow SoA concept only |
| CIS Controls v8.1 | Prioritised (IG1–3), practical | CC BY-NC-ND 4.0; commercial use needs CIS approval (SRC-REG-032) | Restrictive | Reference by safeguard ID only |
| AICPA TSC 2017 (rev. PoF 2022) | It is what the SOC 2 auditor tests | Criteria, not controls; mapping spreadsheets gated (member/account) (SRC-REG-055) | AICPA copyright | Mandatory mapping target, never the anchor |

`recommendation`, confidence **medium** (licence facts partly secondary).

## 3. The model

```
External element (REQ-*)  --relationship-->  CTL-*  --implements-->  CSF 2.0 subcategory
   (GLBA 314.4(c)(5), TSC CC6.1,                (Echelon wording,      + SP 800-53r5 control id(s)
    Nacha fraud monitoring, ...)                 testable, owned)        (vocabulary, not copied text)
```

- **Layer 1 – Outcome:** CSF 2.0 Function → Category → Subcategory
  (`REQ-CSF-001..022` at category level). Used for governance reporting and
  bank-facing profiles.
- **Layer 2 – Control:** Echelon-authored `CTL-` statements, each tagged with
  one or more 800-53r5 identifiers (e.g., `IA-2(1)`), written in Echelon's own
  words so the catalog is free of third-party text.
- **Layer 3 – Requirements:** every external requirement (`REQ-`) linked to
  controls with a relationship type: `equal`, `subset-of` (Echelon control is
  narrower — gap remains), `superset-of`, `intersects`, `no-relationship`.
  Only `equal`/`superset-of` (or a set of controls whose union is
  documented to cover it) can mark a requirement as *designed-for*.
- **Applicability carried per requirement**, not per framework: e.g., GLBA
  314.4(c)(5) is `undetermined` legally but the control is still built because
  SOC 2 and bank contracts drive it.
- **Organisation-specific overlays** (sponsor, bank, Nacha role) are
  parameter sets on controls (thresholds, notice windows), not new controls.

## 4. Build procedure

1. **Freeze versions:** CSF 2.0 (2024-02-26), 800-53r5 Release 5.2.0
   (2025-08-27), SSDF 1.1 (+1.2 draft watch), TSC 2017/PoF 2022, PCI DSS v4.0.1,
   CCM v4.1, ASVS 5.0.0, SLSA v1.2. Record in the source register.
2. **Ingest public machine-readable catalogs:** 800-53 and CSF via NIST
   CPRT/OSCAL; OLIR informative references (SRC-REG-030). Do not ingest SCF,
   CIS or AICPA text into the repository.
3. **Draft controls from requirements with legal/contractual drivers first**
   (GLBA 314.4 baseline, bank App. B/TPRM, Nacha, state breach law, IRS,
   OFAC), then SOC 2 TSC, then voluntary frameworks.
4. **Map each `REQ-` to controls** with relationship type and a one-line
   rationale. Use published mappings (NIST OLIR, AICPA TSC→CSF/800-53, CSA
   CCM→ISO) only as **suggestions**; record which published mapping was
   consulted, because third-party mappings embed judgment and differ.
5. **Gap pass:** any legal/contractual `REQ-` without `equal`/`superset`
   coverage becomes a gap item; any `CTL-` without a non-voluntary driver is
   flagged for possible deferral (charter Section 21 challenge).
6. **Independent review of mappings:** mappings that a SOC auditor or bank
   will rely on are reviewed by someone other than their author; mappings
   produced by Echelon agents are never described as independent
   (AGENTS.md rule 4).
7. **Change control:** framework releases (e.g., SSDF 1.2 final, CCM v4.1
   mapping completion, 800-53 5.2.x) trigger a delta review using the pinned
   version diff.

## 5. Mapping quality rules

- One mapping row per (REQ, CTL) pair; no many-to-many cells.
- Never claim coverage from title similarity; read the external element.
- When a primary text was unavailable (ISO, PCI, Nacha rule text), mark the
  relationship `assumption` until read.
- Preserve conflicting published mappings (e.g., AICPA vs SCF) in the
  rationale rather than choosing silently (AGENTS.md rule 9).

## 6. Licensing constraints summary

| Material | Constraint found | Practical rule |
|---|---|---|
| NIST CSF, SP 800-53, SSDF, IR 8477, OLIR | Public; NIST requests attribution (SRC-REG-030; NIST copyright statement partly read) | May quote IDs and short text with attribution |
| CIS Controls v8.1 | CC BY-NC-ND 4.0; commercial use subject to CIS approval (SRC-REG-032) | IDs only; no derived text; ask CIS (OQ-214) |
| SCF | CC BY-ND 4.0 per production pages; conflicting CC BY 4.0 statement (SRC-REG-054) | Consult, cite IDs; no redistributed derivatives |
| AICPA TSC and mappings | AICPA copyright; downloads gated (SRC-REG-055) | Reference criteria IDs (CC6.1 etc.); do not republish spreadsheets |
| CSA CCM | Terms not verified (SRC-REG-038) | IDs only until verified |
| ISO/IEC 27001/27701 | Paywalled (SRC-REG-039/040) | Clause/control numbers only; buy the standards before claiming coverage |
| PCI DSS | Licence acceptance required (SRC-REG-023) | Requirement numbers only |
| OWASP (ASVS, SAMM, API Top 10) | Open licences (CC BY-SA for ASVS per general knowledge, unverified) | Share-alike: avoid copying text into the proprietary catalog; reference IDs |

## 7. Open points

- OQ-214 (licences), OQ-215 (which frameworks customers want), OQ-217 (bank
  self-assessment format post-CAT).
- Whether to publish the catalog in OSCAL is a later decision; the model is
  OSCAL-compatible because the anchor is.
