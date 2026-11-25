#!/usr/bin/env python3
"""Merge per-agent CSV fragments into the authoritative registers.

Fragments (written by research batches) live beside each register:
  sources/fragments/*.csv                     -> sources/source-register.csv
  regulatory/fragments/*.csv                  -> regulatory/requirements.csv
  registers/fragments/*-open-questions.csv    -> registers/open-questions.csv
  registers/fragments/*-evidence-types.csv    -> registers/evidence-types.csv
  registers/fragments/*-validation-procedures.csv -> registers/validation-procedures.csv
  registers/fragments/*-risks.csv             -> registers/risk-register.csv
  integration/fragments/*-system-register.csv -> integration/system-register.csv

Existing register rows win over fragment rows with the same ID (registers are
curated after merge). Rows whose header does not match the schema are reported
and skipped, never silently coerced. Pure transformation; I/O only in main.

Usage: python3 -I tools/merge_fragments.py [--check]
"""

import csv
import sys
from itertools import chain
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parent))
from schema import REGISTERS  # noqa: E402

ROOT = Path(__file__).resolve().parent.parent

FRAGMENT_GLOBS = {
    "sources": "sources/fragments/*.csv",
    "requirements": "regulatory/fragments/*.csv",
    "open_questions": "registers/fragments/*-open-questions.csv",
    "evidence_types": "registers/fragments/*-evidence-types.csv",
    "validations": "registers/fragments/*-validation-procedures.csv",
    "risks": "registers/fragments/*-risks.csv",
    "systems": "integration/fragments/*-system-register.csv",
}


def read_csv(path: Path) -> tuple:
    with path.open(newline="", encoding="utf-8") as handle:
        rows = tuple(csv.reader(handle))
    return (tuple(rows[0]) if rows else ()), tuple(r for r in rows[1:] if any(c.strip() for c in r))


def fragment_rows(name: str, paths) -> tuple:
    """Return (rows, problems) for all fragments of one register."""
    columns = tuple(REGISTERS[name]["columns"])

    def load(path):
        header, rows = read_csv(path)
        if header != columns:
            return (), (f"{path.relative_to(ROOT)}: header mismatch, skipped",)
        bad = tuple(f"{path.relative_to(ROOT)}: row with {len(r)} fields skipped" for r in rows if len(r) != len(columns))
        return tuple(r for r in rows if len(r) == len(columns)), bad

    loaded = tuple(map(load, paths))
    return (
        tuple(chain.from_iterable(r for r, _ in loaded)),
        tuple(chain.from_iterable(p for _, p in loaded)),
    )


def merge(existing: tuple, incoming: tuple, id_index: int) -> tuple:
    seen = frozenset(r[id_index] for r in existing)
    first_wins = {}
    for row in incoming:  # deterministic first occurrence per id
        first_wins.setdefault(row[id_index], row)
    added = tuple(r for k, r in first_wins.items() if k not in seen)
    return tuple(sorted(existing + added, key=lambda r: r[id_index]))


def plan(name: str) -> tuple:
    spec = REGISTERS[name]
    target = ROOT / spec["path"]
    header, existing = read_csv(target) if target.exists() else (tuple(spec["columns"]), ())
    rows, problems = fragment_rows(name, sorted(ROOT.glob(FRAGMENT_GLOBS[name])))
    merged = merge(existing, rows, spec["columns"].index(spec["id"]))
    return target, tuple(spec["columns"]), merged, problems, len(merged) - len(existing)


def write(target: Path, header: tuple, rows: tuple) -> None:
    with target.open("w", newline="", encoding="utf-8") as handle:
        writer = csv.writer(handle, lineterminator="\n")
        writer.writerow(header)
        writer.writerows(rows)


def main(argv) -> int:
    check_only = "--check" in argv
    plans = tuple(plan(n) for n in FRAGMENT_GLOBS)
    for target, header, rows, problems, added in plans:
        print(f"{target.relative_to(ROOT)}: +{added} rows ({len(rows)} total)")
        print("\n".join(f"  WARN {p}" for p in problems)) if problems else None
        None if check_only else write(target, header, rows)
    return 0


if __name__ == "__main__":
    sys.exit(main(sys.argv))
