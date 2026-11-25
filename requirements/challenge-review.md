---
id: ASR-REQ-CHAL-001
title: Challenge review of security-by-construction and financial-integrity-by-construction requirements
status: draft for owner review
created: 2026-11-25
inputs: [requirements/security-by-construction.md, requirements/financial-integrity-by-construction.md, integration/echelon-integration-matrix.md]
---

# Challenge review

Each owner requirement is registered (`REQ-SBC-*`, `REQ-FINBC-*`, owner IDs
kept in `framework_ref`) and mapped to controls. New controls were created
only where no existing control covered it (CTL-APP-001..010,
CTL-FIN-015..017). This review challenges each requirement for bypass paths,
cost, feasibility and unintended effects. Capability claims about Echelon
systems come from the read-only assessments: **none was verified as
implemented to the degree these requirements assume.**

## Security by construction

| ID | Strongest bypass path | Feasibility with current Echelon systems | Cost / unintended effects | Recommendation |
|---|---|---|---|---|
| SBC-001 / 005 SQL and Strata boundary | Raw ADO.NET/Npgsql calls in non-adapter code; stored procedures building dynamic SQL; admin credentials in jobs | Strata today is a schema-management CLI with grants/RLS; a typed runtime data-access adapter is **not observed** (ECH2). Decision 0D0C assigns it that role, so the adapter is new work | Medium build; may push complex reporting queries into awkward shapes | Keep; enforce with import/AST rule + per-adapter DB credentials (the credential rule is the real guarantee) |
| SBC-002 raw HTML | Trusted component that internally uses a raw sink; URL attributes (`javascript:`); markdown renderers | Forma assessed secondhand only; Limen is a TypeScript browser-kernel library | Low–medium; CSP may break third-party embeds | Keep; maintain a sink inventory and test each allowed sink |
| SBC-003 JS boundary | Third-party npm packages are JS by definition; generated WASM glue | Limen exists; bundle inventory tooling not observed | Medium; could block useful libraries | Keep, scoped to *application-authored* JS; dependencies go through SBC-011 |
| SBC-004 memory safety | P/Invoke, `NativePtr`, unsafe libraries, native DB drivers | F# is memory-safe by default; checks are cheap | Low | Keep; inventory native dependencies rather than forbid |
| SBC-006 deny-by-default authz | Background jobs and webhooks registered outside the router; admin tools | Unbound design specifies central authorization; not implemented | Medium; every handler needs a policy | Keep; the enumeration test (handler without policy fails) is the key validation |
| SBC-007 tenant isolation | Caches, exports, search indexes, logs and analytics copies keyed without tenant | Designed in Unbound; not implemented | Medium; complicates cross-tenant platform operations (which then need privileged access) | Keep; include derived stores in scope explicitly |
| SBC-008 identities | Shared CI tokens; agents running under a human's credentials | Praxis provenance is self-reported today (ECH1) | Medium; requires OIDC/workload identity everywhere | Keep; fix agents-under-human-credentials first (highest leverage) |
| SBC-009 durable idempotency | External side effect before persistence; key scoped too broadly | Designed (daf-landscape spec) | Medium–high; the hardest engineering in the kernel | Keep; ordering per aggregate only, as stated |
| SBC-010 verified completion | GitHub org owner disables rulesets; verifier key accessible to implementer | Implemented in Praxis/Dokimos, enabled nowhere (OQ-704) | Low to enable; slows merges | Keep; key custody and ruleset snapshots are the real controls |
| SBC-011 dependency admission | Transitive updates via lockfile regeneration; build-time downloads | Conditor pins digests; org-wide adoption not observed | Low–medium; friction on upgrades | Keep with an expiry-based exception path |
| SBC-012 secrets | Secrets embedded in WASM client config; CI logs | Aegis redaction exists; vault and scanning are managed services | Low | Keep; add "inspect shipped client bundle" test explicitly |
| SBC-013 input validation | Internal services trusted implicitly; file uploads | Not observed as shared library | Low–medium | Keep; treat queues and webhooks as trust boundaries too |
| SBC-014 outbound control | Allowed destinations with open redirects; DNS rebinding | Not observed | Low (network egress allowlist is a managed setting) | Keep; network-layer egress is the strong control, code checks defense in depth |
| SBC-015 browser protections | New endpoints without middleware | Not observed | Low | Keep |
| SBC-016 cryptography | Dependency bringing its own crypto; insecure random in tests promoted to prod | Not observed | Low | Keep |
| SBC-017 AI boundaries | Agent with both untrusted input and consequential tools in one execution | Designed (CTL-AI-003); not implemented | Medium | Keep; mapped to existing AI controls |
| SBC-018 resource limits | Unbounded batch jobs; recursive retries | Not observed | Low–medium | Keep |

**Cross-cutting challenge.** The validation clause ("tests must fail when
the enforcement mechanism is deliberately disabled") is the most valuable
part of the record and is the most expensive. Recommend applying it to the
L4 controls that protect money, tenant data and release (CTL-APP-001, 005,
CTL-TEN-001, CTL-SDL-001/002, CTL-FIN-001/002), and sampling elsewhere
(CTL-ASR-005).

## Financial integrity by construction

| IDs | Strongest bypass / weakness | Feasibility | Cost / unintended effects | Recommendation |
|---|---|---|---|---|
| 001–003, 011 journal integrity, corrections, periods | Direct DB writes by migration or admin role; backdated writes through a second code path | Summa shows the patterns (not deployed); Unbound designed | Medium | Keep; DB role without UPDATE/DELETE is the real guarantee; period lock in the posting boundary, not per function |
| 004 precision | Floating point in reporting or exports; FX rounding policy undefined | Straightforward in F# | Low | Keep; rounding policy needs sponsor approval (new control CTL-FIN-015) |
| 005, 014, 020 idempotency, ambiguous outcomes, populations | Provider callbacks without idempotency; manual re-submission by operators | Designed | Medium–high | Keep; operator re-submission must use the same command path |
| 006, 008 balance states, transitions | Reservation consumed without cash movement (a known prototype defect class, daf-landscape spec) | Designed | Medium | Keep; property and crash tests mandatory |
| 007, 013 ownership, unauthorized movement | Platform operator acting with sponsor authority | Designed | Medium | Keep; operator actions require recorded sponsor authorization (links OQ-212 fund-manager question) |
| 010 reconciliation | Reconciliation data obtained through a user-editable channel | Designed | Medium | Keep; source files/APIs fetched by a reconciliation identity separate from operators |
| 012 reproducible reports | Rule changes applied retroactively | Designed | Low–medium | Keep; effective-dated rules |
| 015–017 fees, contributions, grants | Sponsor configuration errors; stale charity data | Designed | Medium | Keep; independent reference calculations for fees are cheap and high value |
| 018 scheduled revalidation | Schedules created under old rules executing later | Not designed explicitly before | Low | Keep (new CTL-FIN-016) |
| 019 destination integrity | Social-engineered bank-detail change | Designed | Low–medium | Keep; out-of-band verification + cooling-off |
| 021 recoverability and replay | Replay triggering external side effects | Not designed explicitly before | Medium | Keep (new CTL-FIN-017); replay must never re-emit external effects |
| 022 sponsor configuration | Tenant override of mandatory safeguards | Designed (TEN-002) | Low | Keep; mandatory safeguards non-configurable by type |
| 023 exception management | Silent write-off via adjustment | Partly (FIN-004, FIN-011) | Low | Keep |
| 024 retention | Conflicting holds across sponsors and jurisdictions | Designed (DAT-002) | Medium | Keep; hold precedence rule needs counsel input (OQ-502) |

## What these requirements do not do

They do not establish legal applicability, auditor acceptance, operating
effectiveness or the absence of vulnerabilities, and they do not replace
independent penetration testing; both owner records say so, and the catalog
keeps every new control at `Designed`.
