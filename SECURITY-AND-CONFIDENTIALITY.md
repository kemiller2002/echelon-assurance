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

The owner directed that this repository be **private**. On 2026-11-25 the
GitHub repository listing reported `kemiller2002/echelon-assurance` as
**public**. Until visibility is confirmed private, treat every commit as
publicly readable and apply the rules below strictly. Visibility is an owner
action (GitHub → Settings → General → Danger Zone → Change visibility); no
agent in this program changes it.

Even once private, these rules still apply: a private repository is shared
with every collaborator, CI integration and AI provider granted access, and
assurance material is a high-value target.

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
restricted-use report content. The validator in `tools/` checks for a small
set of high-confidence secret patterns; it is not a substitute for judgment.
