/// Merge per-agent CSV fragments into authoritative registers. Pure.
/// Existing register rows win over fragment rows with the same identifier;
/// among fragments the first occurrence wins; output is sorted by identifier
/// (ordinal). Fragments with a wrong header or rows of the wrong width are
/// reported and skipped, never coerced.
module Echelon.Assurance.Registers.Merge

open Echelon.Assurance.Registers.Schema

type Fragment =
    {
        Path: string
        Header: Csv.Row
        Rows: Csv.Row list
    }

type Plan =
    {
        Register: RegisterSpec
        Merged: Csv.Row list
        Added: int
        Problems: string list
    }

let fragment path header rows : Fragment =
    {
        Path = path
        Header = header
        Rows = rows
    }

let private idOf (spec: RegisterSpec) =
    let i = spec.Columns |> List.findIndex ((=) spec.IdColumn)
    fun (row: Csv.Row) -> List.item i row

let fragmentRows (spec: RegisterSpec) (fragments: Fragment list) =
    let width = List.length spec.Columns

    let load (f: Fragment) =
        if f.Header <> spec.Columns then
            [], [ sprintf "%s: header mismatch, skipped" f.Path ]
        else
            let good, bad = f.Rows |> List.partition (fun r -> List.length r = width)

            good,
            bad
            |> List.map (fun r -> sprintf "%s: row with %d fields skipped" f.Path (List.length r))

    let loaded = fragments |> List.map load
    List.collect fst loaded, List.collect snd loaded

let merge (spec: RegisterSpec) (existing: Csv.Row list) (incoming: Csv.Row list) =
    let key = idOf spec
    let seen = existing |> List.map key |> Set.ofList

    let added =
        incoming
        |> List.distinctBy key
        |> List.filter (fun r -> not (seen.Contains(key r)))

    existing @ added
    |> List.sortWith (fun a b -> System.String.CompareOrdinal(key a, key b))

let plan (spec: RegisterSpec) (existing: Csv.Row list) (fragments: Fragment list) =
    let rows, problems = fragmentRows spec fragments
    let merged = merge spec existing rows

    {
        Register = spec
        Merged = merged
        Added = List.length merged - List.length existing
        Problems = problems
    }
