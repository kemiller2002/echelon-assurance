---
id: JR-ASR-2026-B001
title: Research batches B0-B7 (2026-11-25)
status: complete
created: 2026-11-25
---

# Journal: research batches B0–B7

| Batch | Scope | Executed by | Outputs | Verification performed |
|---|---|---|---|---|
| B0 | Repository establishment | coordinator | AGENTS.md, confidentiality rules, identifiers, methodology, schemas, validator, merge tool, CI | `python3 -I tools/validate_registers.py`; CI green on PR #1 |
| B1 | Echelon systems (ECH1 governance systems; ECH2 runtime systems + Unbound inventory) | two research agents, read-only clones | `integration/fragments/*`, `financial/unbound-existing-assurance-inventory.md`, integration matrix, proposed changes, DF-ASR-2026-3A1E | File-level reading of docs, src and tests; **no test suites executed**; deployment state not observed |
| B2 | SOC landscape | research agent | `soc/*`, REQ-TSC/SOC1/SOCDC | AT-C 105/205/320, AU-C 402/9402 read in full; TSC and DC 200 behind login (summary-only, capped at auditor-interpretation) |
| B3 | Regulatory crosswalk and applicability | research agent | `regulatory/*` | Primary texts where reachable; eCFR blocked (Cornell LII mirror used); FFIEC 403; Nacha/PCI/ISO paywalled |
| B4 | Pen testing, compliance vendors | research agent | `pentest/*`, `vendors/*`, cost evidence | PCI DSS and FFIEC unreadable (403); all prices vendor claims |
| B5 | Policy as code, evidence architecture | research agent | `policy-as-code/*`, `architecture/evidence-*` | JSON Schema validated against 2020-12 metaschema with a synthetic envelope |
| B6 | AI governance, validation, resilience | research agent | `ai-governance/*`, `validation/*`, VAL-001..032, RSK-001..042 | Several sources summary-only (EU AI Act OJ, ISO 42001 text, OWASP agentic doc) |
| B7 | Synthesis | coordinator | policy inventory + 13 drafts, automation matrix, control catalog (104), risk links, financial controls, economics, roadmap, auditor and trust-center specs, challenge document, Clarity/EDF application | Validator OK after every commit; traceability check: every non-N/A requirement mapped except 4 documented exemptions |

## Invalidated assumptions

- "Strata provides append-only integrity" (Unbound) — false; schema CLI.
- "Folio produces evidence packages" (Unbound) — not observed.
- "Echelon governance gates block merges" — not evidenced; depends on rulesets.
- "Building internal assurance software is cheaper" — not supported by the TCO model.

## Process notes

- A secret-pattern false positive (`sk-` inside `risk-`) briefly failed CI; pattern fixed (commit 487997d).
- Unbound reuses the `VAL-` prefix; cross-repository references now prefixed (`UNB:`).
