# Security by construction

Date: 2026-11-25
Status: Owner-approved initiative; detailed requirements proposed for review. No implementation or effectiveness claim.

## Normative requirements

All application repositories SHALL apply the following constraints, with scope, owner, enforcement point, negative tests, evidence, residual risk and exception recorded in the unified control catalog. Existing control IDs must be mapped rather than duplicated. F# is the application implementation standard; managed infrastructure and explicitly approved runtime boundaries are distinguished from application code.

| ID | Constraint | Enforcement | Independent verification and evidence | Residual risk |
|---|---|---|---|---|
| SBC-001 | No dynamically constructed executable SQL; parameterize values; identifiers selected only from fixed approved mappings | Strata typed operations, restricted DB credentials, prohibit raw command escape paths; reviewed static migrations | AST and dependency checks; injection payload tests against real test DB; signed test receipts and DB privilege snapshot | Authorization, stored procedures, unsafe provider internals |
| SBC-002 | No untrusted raw HTML input/rendering; encode output by context | Forma/Limen safe rendering APIs; prohibit raw markup sinks; validate URLs; CSP defense in depth | Enumerate sinks; adversarial browser tests for markup, URL and DOM contexts; CSP/config evidence | Trusted component bugs, browser defects, allowed URL abuse |
| SBC-003 | No application-authored JS outside controlled Limen boundary | Build/source allowlist, dependency and generated artifact inventory; isolate and review runtime interop | Attempt forbidden source and interop paths; inspect shipped bundle and dependency provenance | Runtime JS, browser, third-party supply chain |
| SBC-004 | Memory-safe bounds-checked application code | F# managed/WASM runtime; prohibit unsafe/native escape paths unless explicitly reviewed | Compiler/build configuration, AST checks, malformed input and boundary property tests; native dependency inventory | Native runtime bugs, resource exhaustion, logic defects |
| SBC-005 | Mandatory Strata database boundary | Only approved adapter can obtain DB access; network and identity restrictions; no direct provider access from business code | Attempt direct connections and forbidden imports; verify deployment privileges and transaction tests | Adapter defects, admin access, migrations |
| SBC-006 | Deny-by-default server-side authorization | Central policy enforcement on every command, query and background action; no UI-only authorization | Enumerate endpoints and handlers; missing/invalid/expired identity and role escalation tests; authorization matrix | Incorrect policies, compromised privileged identities |
| SBC-007 | Explicit tenant isolation | Trusted tenant context derived from authenticated authorization; scope reads/writes, jobs, caches, exports and storage; DB isolation defense where appropriate | Multi-tenant property tests, cross-tenant ID substitution, worker and export tests; configuration snapshots | Misattribution, privileged operations, shared infrastructure |
| SBC-008 | Authenticated human, agent and service identities | Distinct least-privilege principals, short-lived credentials where supported, signed provenance, protected verifier identity | Reject forged/replayed/expired credentials; compare authenticated principal with provenance; access inventory | Credential theft, identity provider compromise |
| SBC-009 | Durable idempotent financial operations | Persist accepted requests before acknowledgement; scoped idempotency keys, transactional state/outbox, explicit ordering scope, reconciliation, controlled retries and dead-letter handling | Crash/replay/concurrency and duplicate tests; ledger invariants; external reconciliation; deployment interruption tests | External ambiguous outcomes, poison requests, business errors |
| SBC-010 | Verified completion before release | Praxis/Dokimos gates enforced by platform; bind receipts to exact artifact/commit; separate verifier credentials | Attempt merge with missing, stale, forged or mismatched receipts; inspect actual rulesets; release provenance | Test gaps, compromised gate/admin |
| SBC-011 | Controlled dependency admission | Pin versions and lockfiles; inventory transitive dependencies; provenance/license/vulnerability checks; documented exceptions | Inject unapproved dependency, tamper with lockfile/artifact; reproduce builds where feasible; SBOM and receipts | Unknown vulnerabilities, compromised upstream |
| SBC-012 | Secrets outside application code | Managed secret store, scoped runtime identities, no secrets in logs/build artifacts/client WASM; rotation and revocation | Seed synthetic secrets to test scanning/redaction; inspect client bundles and access policies; rotation tests | Runtime theft, excessive privilege, provider compromise |

## Additional baseline requirements

SBC-013: Validate all external input at trust boundaries with explicit schemas, length/range limits, canonicalization and resource budgets; reject ambiguous input. Test malformed, oversized and adversarial payloads.
SBC-014: Restrict outbound network access and URL fetching; validate destinations and redirects, block private/metadata endpoints where inappropriate; test SSRF and DNS rebinding defenses.
SBC-015: Protect state-changing browser requests against CSRF where ambient credentials are used; configure cookies, CORS and security headers explicitly. Test cross-origin requests and session expiry.
SBC-016: Use approved cryptographic libraries and managed keys; prohibit custom cryptography and insecure randomness. Test configuration, key rotation and invalid signatures.
SBC-017: Treat AI input/output as untrusted; enforce tool authorization, data disclosure boundaries and transaction approval outside the model. Test prompt injection and unauthorized tool invocation.
SBC-018: Apply rate limits, bounded queues, timeouts, backpressure and recovery controls. Test resource exhaustion and dependency failures.

## Validation and claims

For each requirement record applicability, threat model, exact enforcement scope, source/build/runtime/deployment checks, negative and positive tests, mutation or deliberate bypass tests where practical, authenticated verifier identity, artifact digest, configuration digest, timestamp, result, limitations and expiry/revalidation triggers. Tests must fail when the enforcement mechanism is deliberately disabled in a safe test environment. Use F# tooling and tests under Praxis governance. Never test destructive scenarios in production.

Distinguish designed, implemented, internally verified, independently assessed and externally attested. A passing scanner or test suite is not proof of universal absence of vulnerabilities. Technical verification is not organizational independence. A constraint may eliminate a specific attack mechanism only within an explicitly verified boundary. No claim of SOC compliance or replacement of external penetration testing.

## Integration and sequencing

Map existing controls and open questions to these requirements; inspect actual Strata, Limen, Forma, Praxis, Dokimos, Vigila, Arca and identity capabilities before assigning implementation status. Vigila records operational events; Arca supports evidence integrity; Folio renders sanitized reports. Verify actual capabilities rather than assuming them. Use Unbound as first reference; prioritize financial integrity, tenant isolation, identity, data access and release gates. Define requirements and test specifications before implementation; no other repository changes authorized by this record.

Public evidence: publish control objectives, test methodology, scoped results, versions and limitations. Restrict exploit details, production configuration, secrets, customer data and sensitive findings. Review disclosure before publication.

Reconcile the question register, control catalog, policies, integration matrix, roadmap and economics. Keep external auditor acceptance and legal applicability open. Challenge each requirement for bypass paths, cost, feasibility and unintended effects. Run existing register validation and required governance checks before merge.
