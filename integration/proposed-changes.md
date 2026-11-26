# Proposed changes to other Echelon repositories

Proposals only. No other repository was modified (AGENTS.md rule 6). Each
needs the owning repository's own governance process.

| ID | Repository | Proposal | Reason | Priority |
|---|---|---|---|---|
| PC-01 | unbound | Correct `UNB-TEC-STRATA-001..003`, `UNB-ACC-026` and CCM "Integrity and security" so append-only integrity is not assigned to Strata; assign it to the evidence ledger / application persistence design | Strata is a schema CLI (ECH2 F3) | P0 before design freeze |
| PC-02 | unbound | Correct `SOC2-009`, `MON-015`: Folio renders; packaging/manifest/chain of custody belongs to the auditor export capability | Folio capability not observed (ECH2 F3) | P1 |
| PC-03 | praxis + consumers | Enable `requireVerifiedCompletion` / attested evidence in assurance-relevant repositories after upgrading pinned Praxis | Implemented but disabled (ECH1 #2) | P0 |
| PC-04 | praxis | Bind recorded actors to authenticated workload identities (OIDC tokens from CI / agent runtime) instead of self-reported fields | Provenance self-reported (ECH1 #4; OQ-615) | P1 |
| PC-05 | all governed repos | Define GitHub rulesets as code requiring Dokimos/Praxis checks and non-author review; disable admin bypass; record ruleset state snapshots as evidence | Gates advisory without rulesets (ECH1 #3; OQ-700) | P0 |
| PC-06 | ordo | Publish role authority matrix in a machine-readable form consumable by the policy model | SoD model exists only in Ordo types | P2 |
| PC-07 | tutela | Generalize exception model (verified, non-self, expiring) as the organization-wide exception engine | Strongest existing exception control (ECH2 F4) | P1 |
| PC-08 | conditor / release workflows | Apply signed attestation and digest pinning to all production-bound repos; retain attestations centrally | Only one tagged release today | P1 |
| PC-09 | aegis | Package redaction for reuse by every evidence emitter | Prevent secrets/PII in evidence | P2 |
| PC-10 | vigila | Add expected-source heartbeat and gap detection, or decide to use managed cloud logging and narrow Vigila's scope | SOC2-027 has no implementer | P1 |
| PC-11 | fides | Clarify deployment state; document that MFA enforcement evidence comes from the IdP; scope lifecycle/access review to the managed IdP | Contradictory status; F5 | P1 |
| PC-12 | arca | *(Superseded by DF-ASR-2026-C002: hash chaining and external anchoring deferred until a requirement is recorded.)* Managed retention-locked evidence storage interface only | Decision 10 (DF-ASR-2026-0D0A) and C002; proposal 3A1E superseded | P3 (design now) |
| PC-13 | percepta | Align envelope with `architecture/evidence-envelope.schema.json` | One evidence contract | P2 |
| PC-14 | unbound | Reuse Summa's ledger invariant, close and reconciliation patterns in the FIN kernel design | Avoid re-designing | P2 |
| PC-15 | echelon-registry, project-administration | Expose system inventory for control applicability | Asset inventory (CC6.1) | P2 |
| PC-16 | specula | Evaluate as a read-only observer for completeness reconciliation, independent of the collector code path | Candidate separate observer; Echelon-run, so `automated`, not independent and not an external witness (AGENTS rule 4) | P3 |
| PC-17 | praxis, clarity, EEF | Converge on one signed-approval record pattern | Three approval designs (ECH1 overlaps) | P2 |
| PC-18 | all repos | Context-status vs code consistency check (stale CURRENT-STATE contradicting code in ≥4 repos) | Management-assertion hygiene (ECH2 §3.6) | P2 |
| PC-19 | AI runtime (no owner yet) | Signed agent action receipts; AI egress gateway enforcing provider-by-data-class; control-weakening change classifier | Gaps identified by AIG batch; owner to decide whether these extend Praxis/Aegis or need a new component | P1 |
| PC-20 | unbound | Re-examine requirements that cite GLBA as their legal basis (marked "unverified general knowledge" in Unbound); GLBA applicability is contested and not assumed (OQ-200..203); the owner-approved internal design adopts the substantive GLBA safeguards regardless of classification and does not rely on the nonprofit exemption (DF-ASR-2026-0D11). Keep the safeguards as design baseline but cite them as framework-selected/contractual until a recorded legal resolution (internal research; external advice only on a recorded DF-ASR-2026-0D0F trigger) | REG batch finding | P1 |

## Owner decisions of 2026-11-25

Per `docs/decisions/owner-approved-foundations-2026-11-25.md`: PC-01 (Strata) and PC-02 (Folio) now follow
decisions 12 and 11. PC-10 is revised: Vigila becomes the logging and
observability initiative integrating managed infrastructure, with an
independently operated heartbeat (decision 13). PC-12 follows decision 10:
define evidence interfaces and use managed durable immutable storage before
any custom ledger. PC-03..05 follow decisions 7–8. The owner record does not
authorize changes to other repositories; these remain proposals.
