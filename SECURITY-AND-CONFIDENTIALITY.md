---
id: GV-ASR-CONF-001
title: Repository confidentiality and disclosure rules
status: canonical
version: 0.1.0
owners: [assurance-governance]
created: 2026-11-25
updated: 2026-11-25
---

# Confidentiality and disclosure rules

## Repository visibility

Owner decision `DF-ASR-2026-0D05` (2026-11-25): this repository may hold
**public research, general policies, control objectives, methodology and
sanitized results**. Production evidence, vulnerabilities, secrets,
personnel and customer data, and confidential reports are kept in separately
restricted stores and never committed here. Source licensing is respected
(for example, CIS Controls and SIG content are not reproduced).

The repository was public on 2026-11-25. A sensitivity review of its full
contents was performed the same day before merge (journal
`JR-ASR-2026-0D00`): no secrets, personal data, production evidence or live
vulnerability details were found; design-level gaps of Echelon systems that
those systems' own public documentation already discloses are retained as
sanitized results.

## Never commit

| Category | Examples | Where it belongs instead |
|---|---|---|
| Credentials and secrets | API keys, tokens, private keys, connection strings, recovery codes | Secret manager; never in any repository |
| Production evidence | Real audit logs, access lists, screenshots of consoles, ticket exports, user lists | Evidence store with access controls (see `architecture/evidence-architecture.md`) |
| Live security findings | Vulnerability details, pen-test reports, exploitable misconfigurations of deployed systems | Restricted finding tracker; reference here only by opaque ID and severity class |
| Specific internal control weaknesses | "Control X is not operating in system Y" with exploit-relevant detail | Restricted remediation register; this repo may say "status: not evidenced" |
| Customer and personal data | Donor, charity, tenant, employee or partner data | Never |
| Confidential organizational information | Contract terms, negotiated prices, bank relationship details, legal advice | Restricted document store |
| Auditor workpapers and reports | SOC reports (restricted-use), management letters | Restricted document store; SOC 1/SOC 2 reports are restricted-use by design |

## Permitted

- Public-source research with citations.
- Requirements, policy drafts, control designs and architecture specifications.
- Design-level status (`Proposed`, `Designed`, `Specified`) and the honest
  statement that something is **not yet evidenced**.
- Synthetic examples clearly marked `synthetic`.
- Cost models built from public price evidence, with the evidence class marked.

## Pre-commit check

Before each commit, confirm: no secrets (scan with a secret scanner where
available), no production data, no exploit-relevant weakness detail, no
restricted-use report content. The validator (`assurance validate`) checks for a small
set of high-confidence secret patterns; it is not a substitute for judgment.
