#!/usr/bin/env python3
"""Validate every machine-readable register.

Checks: file exists, exact header, ID format and uniqueness, enum values,
cross-register references, and a small set of high-confidence secret
patterns across the repository. Pure functions produce error lists; only
`main` performs I/O and sets the exit status.

Usage: python3 -I tools/validate_registers.py [repo_root]
"""

import csv
import re
import sys
from functools import reduce
from itertools import chain
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parent))
from schema import REGISTERS  # noqa: E402

LIST_SEP = ";"
SECRET_PATTERNS = (
    re.compile(r"AKIA[0-9A-Z]{16}"),
    re.compile(r"-----BEGIN (RSA |EC |OPENSSH )?PRIVATE KEY-----"),
    re.compile(r"ghp_[A-Za-z0-9]{36}"),
    re.compile(r"github_pat_[A-Za-z0-9_]{60,}"),
    re.compile(r"xox[baprs]-[A-Za-z0-9-]{10,}"),
    re.compile(r"sk-(ant-)?[A-Za-z0-9_-]{32,}"),
)
SCANNED_SUFFIXES = (".md", ".csv", ".json", ".py", ".fs", ".fsx", ".yml", ".yaml", ".txt")


def split_list(value: str) -> tuple:
    return tuple(v.strip() for v in value.split(LIST_SEP) if v.strip())


def read_register(root: Path, spec) -> tuple:
    path = root / spec["path"]
    if not path.exists():
        return None, ()
    with path.open(newline="", encoding="utf-8") as handle:
        reader = csv.reader(handle)
        rows = tuple(reader)
    return (rows[0] if rows else ()), tuple(rows[1:])


def header_errors(name, spec, header) -> tuple:
    expected = tuple(spec["columns"])
    return () if tuple(header) == expected else (
        f"{spec['path']}: header mismatch; expected {','.join(expected)}",
    )


def row_errors(name, spec, rows) -> tuple:
    cols = spec["columns"]
    id_col = cols.index(spec["id"])
    pattern = re.compile(spec["id_pattern"])

    def one(indexed):
        line, row = indexed
        where = f"{spec['path']}:{line}"
        if len(row) != len(cols):
            return (f"{where}: {len(row)} fields, expected {len(cols)}",)
        record = dict(zip(cols, row))
        id_err = () if pattern.match(record[spec["id"]]) else (
            f"{where}: bad id {record[spec['id']]!r}",
        )
        enum_errs = tuple(
            f"{where}: {col}={record[col]!r} not in {allowed}"
            for col, allowed in spec["enums"].items()
            if record[col] and record[col] not in allowed
        )
        return id_err + enum_errs

    ids = tuple(r[id_col] for r in rows if len(r) > id_col)
    dupes = tuple(sorted({i for i in ids if ids.count(i) > 1}))
    dupe_errs = tuple(f"{spec['path']}: duplicate id {d}" for d in dupes)
    return tuple(chain.from_iterable(map(one, enumerate(rows, start=2)))) + dupe_errs


def reference_errors(name, spec, rows, id_index) -> tuple:
    cols = spec["columns"]
    records = (dict(zip(cols, r)) for r in rows if len(r) == len(cols))

    def check(record):
        return tuple(
            f"{spec['path']}: {record[spec['id']]} -> {col} references unknown {ref}"
            for col, target in spec["refs"].items()
            for ref in split_list(record[col])
            if target in id_index and ref not in id_index[target]
        )

    return tuple(chain.from_iterable(map(check, records)))


def secret_errors(root: Path) -> tuple:
    files = (
        p for p in root.rglob("*")
        if p.is_file() and p.suffix in SCANNED_SUFFIXES and ".git" not in p.parts
    )

    def scan(path):
        text = path.read_text(encoding="utf-8", errors="ignore")
        return tuple(
            f"{path.relative_to(root)}: possible secret matching {pat.pattern}"
            for pat in SECRET_PATTERNS if pat.search(text)
        )

    return tuple(chain.from_iterable(map(scan, files)))


def validate(root: Path) -> tuple:
    loaded = {name: read_register(root, spec) for name, spec in REGISTERS.items()}
    missing = tuple(
        f"{REGISTERS[n]['path']}: missing" for n, (h, _) in loaded.items() if h is None
    )
    present = {n: v for n, v in loaded.items() if v[0] is not None}
    id_index = {
        n: frozenset(
            r[REGISTERS[n]["columns"].index(REGISTERS[n]["id"])]
            for r in rows if r
        )
        for n, (_, rows) in present.items()
    }
    per_register = (
        header_errors(n, REGISTERS[n], h)
        + row_errors(n, REGISTERS[n], rows)
        + reference_errors(n, REGISTERS[n], rows, id_index)
        for n, (h, rows) in present.items()
    )
    return missing + reduce(lambda a, b: a + b, per_register, ()) + secret_errors(root)


def summary(root: Path) -> str:
    counts = (
        (REGISTERS[n]["path"], len(read_register(root, REGISTERS[n])[1]))
        for n in REGISTERS
    )
    return "\n".join(f"  {path}: {count} rows" for path, count in counts)


def main(argv) -> int:
    root = Path(argv[1]) if len(argv) > 1 else Path(__file__).resolve().parent.parent
    errors = validate(root)
    print("\n".join(errors) if errors else "OK: all registers valid")
    print(summary(root))
    return 1 if errors else 0


if __name__ == "__main__":
    sys.exit(main(sys.argv))
