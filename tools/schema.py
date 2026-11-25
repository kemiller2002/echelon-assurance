"""Register schemas: the single definition of every machine-readable register.

Pure data. `validate_registers.py` interprets it; `registers/SCHEMA.md` documents it.
"""

from types import MappingProxyType as Frozen

EVIDENCE_CLASSES = (
    "authoritative", "auditor-interpretation", "industry-convention",
    "customer-expectation", "vendor-claim", "recommendation", "assumption",
    "open-question",
)
CONFIDENCE = ("very-low", "low", "medium", "high", "very-high")
APPLICABILITY = (
    "legal", "contractual", "framework-selected", "customer-expectation",
    "voluntary", "not-applicable", "undetermined",
)
CONTROL_STATUS = (
    "Proposed", "Designed", "Specified", "Implemented", "Operating", "Tested",
    "Deficient", "Remediating", "AuditorReviewed", "Retired",
)
AUTOMATION_LEVELS = ("L0", "L1", "L2", "L3", "L4", "L5", "unknown")
FAILURE_MODES = ("fail-closed", "fail-open-justified", "detective-only", "n/a")
COST_CLASSES = ("none", "low", "medium", "high", "very-high", "unknown")
POLICY_DISPOSITION = (
    "standalone", "section", "technical-objective", "not-needed", "deferred",
)
SOURCE_TYPES = (
    "statute", "regulation", "regulator-guidance", "attestation-standard",
    "primary-standard", "standard-catalog", "accreditation-body",
    "auditor-publication", "industry-body", "academic", "vendor",
    "news", "echelon-internal",
)
ACCESS = ("public", "paywalled", "summary-only", "internal")

# Each register: path, id column, id regex, columns, enum constraints,
# reference columns (column -> (target register, separator)).
REGISTERS = Frozen({
    "sources": Frozen({
        "path": "sources/source-register.csv",
        "id": "source_id",
        "id_pattern": r"^SRC-[A-Z0-9]+-\d{3}$",
        "columns": (
            "source_id", "title", "publisher", "url", "publication_date",
            "retrieved", "jurisdiction", "source_type", "access",
            "limitations", "used_in",
        ),
        "enums": Frozen({"source_type": SOURCE_TYPES, "access": ACCESS}),
        "refs": Frozen({}),
    }),
    "requirements": Frozen({
        "path": "regulatory/requirements.csv",
        "id": "req_id",
        "id_pattern": r"^REQ-[A-Z0-9]+-\d{3}$",
        "columns": (
            "req_id", "framework", "framework_ref", "requirement_summary",
            "source_ids", "applicability", "applicability_reasoning",
            "applies_to", "evidence_class", "confidence", "control_ids",
            "open_question_ids",
        ),
        "enums": Frozen({
            "applicability": APPLICABILITY,
            "evidence_class": EVIDENCE_CLASSES,
            "confidence": CONFIDENCE,
        }),
        "refs": Frozen({
            "source_ids": "sources", "control_ids": "controls",
            "open_question_ids": "open_questions",
        }),
    }),
    "controls": Frozen({
        "path": "controls/control-catalog.csv",
        "id": "control_id",
        "id_pattern": r"^CTL-[A-Z]{2,3}-\d{3}$",
        "columns": (
            "control_id", "title", "objective", "risk_addressed", "domain",
            "frameworks", "requirement_ids", "source_ids", "applicability",
            "policy_ids", "owner_role", "implementation_mechanism",
            "echelon_systems", "automation_target", "automation_current",
            "validation_ids", "validation_frequency", "evidence_type_ids",
            "independence_requirement", "failure_mode", "failure_response",
            "exception_handling", "impl_cost", "recurring_cost", "status",
            "known_limitations", "unbound_refs", "roadmap_phase",
        ),
        "enums": Frozen({
            "applicability": APPLICABILITY,
            "automation_target": AUTOMATION_LEVELS,
            "automation_current": AUTOMATION_LEVELS,
            "failure_mode": FAILURE_MODES,
            "impl_cost": COST_CLASSES,
            "recurring_cost": COST_CLASSES,
            "status": CONTROL_STATUS,
            "roadmap_phase": ("P0", "P1", "P2", "P3", "P4", "P5"),
        }),
        "refs": Frozen({
            "requirement_ids": "requirements", "source_ids": "sources",
            "policy_ids": "policies", "echelon_systems": "systems",
            "validation_ids": "validations", "evidence_type_ids": "evidence_types",
        }),
    }),
    "policies": Frozen({
        "path": "policies/policy-inventory.csv",
        "id": "policy_id",
        "id_pattern": r"^POL-\d{2}(\.\d{2})?$",
        "columns": (
            "policy_id", "title", "category", "disposition", "parent_policy",
            "rationale", "requirement_ids", "control_ids", "owner_role",
            "approval_authority", "review_cycle", "status", "draft_path",
        ),
        "enums": Frozen({
            "disposition": POLICY_DISPOSITION,
            "status": ("inventory", "drafted", "approved", "retired"),
        }),
        "refs": Frozen({
            "requirement_ids": "requirements", "control_ids": "controls",
            "parent_policy": "policies",
        }),
    }),
    "evidence_types": Frozen({
        "path": "registers/evidence-types.csv",
        "id": "evidence_type_id",
        "id_pattern": r"^EVT-\d{3}$",
        "columns": (
            "evidence_type_id", "name", "source_system", "producing_event",
            "collection_method", "population_completeness_method",
            "integrity_mechanism", "retention_class", "access_class",
            "control_ids",
        ),
        "enums": Frozen({}),
        "refs": Frozen({"control_ids": "controls"}),
    }),
    "validations": Frozen({
        "path": "registers/validation-procedures.csv",
        "id": "validation_id",
        "id_pattern": r"^VAL-\d{3}$",
        "columns": (
            "validation_id", "name", "method", "independence_level",
            "performed_by", "frequency", "description", "control_ids",
        ),
        "enums": Frozen({
            "independence_level": (
                "self-assessment", "automated", "internal-independent",
                "external-technical", "external-penetration",
                "external-attestation",
            ),
        }),
        "refs": Frozen({"control_ids": "controls"}),
    }),
    "risks": Frozen({
        "path": "registers/risk-register.csv",
        "id": "risk_id",
        "id_pattern": r"^RSK-\d{3}$",
        "columns": (
            "risk_id", "title", "category", "description", "likelihood",
            "impact", "control_ids", "treatment", "owner_role", "status",
        ),
        "enums": Frozen({
            "likelihood": ("low", "medium", "high"),
            "impact": ("low", "medium", "high", "severe"),
            "treatment": ("mitigate", "accept", "transfer", "avoid", "undecided"),
        }),
        "refs": Frozen({"control_ids": "controls"}),
    }),
    "open_questions": Frozen({
        "path": "registers/open-questions.csv",
        "id": "oq_id",
        "id_pattern": r"^OQ-\d{3}$",
        "columns": (
            "oq_id", "question", "why_unresolvable_internally",
            "lowest_cost_resolution", "resolver", "blocks", "priority",
            "status", "related",
        ),
        "enums": Frozen({
            "resolver": (
                "owner", "cpa-firm", "counsel", "bank-partner", "auditor",
                "qsa", "tax-advisor", "insurer", "customer", "regulator",
                "internal-research",
            ),
            "priority": ("P0", "P1", "P2", "P3"),
            "status": ("open", "answered", "withdrawn"),
        }),
        "refs": Frozen({}),
    }),
    "decisions": Frozen({
        "path": "registers/decision-register.csv",
        "id": "decision_id",
        "id_pattern": r"^DF-ASR-2026-[0-9A-F]{4}$",
        "columns": (
            "decision_id", "title", "status", "decided_by", "date",
            "summary", "record_path",
        ),
        "enums": Frozen({
            "status": ("proposed", "accepted", "rejected", "superseded"),
        }),
        "refs": Frozen({}),
    }),
    "systems": Frozen({
        "path": "integration/system-register.csv",
        "id": "system_id",
        "id_pattern": r"^SYS-[A-Z0-9-]+$",
        "columns": (
            "system_id", "name", "repository", "verified_purpose",
            "maturity_observed", "assurance_capabilities_verified",
            "assurance_gaps", "evidence_produced", "trust_boundary",
            "verification_basis",
        ),
        "enums": Frozen({}),
        "refs": Frozen({}),
    }),
})
