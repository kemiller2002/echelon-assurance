/// Governed record: every field of every register (statuses, applicability,
/// independence, authority, gating, legal positions, decisions, tenets, answers). A committed snapshot of
/// their digests (tests/Echelon.Assurance.Tests/governed-record.csv) makes every
/// such change visible in review: changing one without regenerating the
/// snapshot fails the tests (adversarial rounds 3–5,
/// findings/reviews/RV-ASR-2026-11-26.md). Pure.
module Echelon.Assurance.Registers.Governed

open System.Security.Cryptography
open System.Text
open Echelon.Assurance.Registers.Validation

/// Columns excluded from the governed record (none today). Every other column
/// of every register is governed, so new columns are covered by default
/// (adversarial round 6).
let excluded: Set<string * string> = Set.empty

let private digest (text: string) =
    SHA256.HashData(Encoding.UTF8.GetBytes text)
    |> Array.take 8
    |> Array.map (fun b -> b.ToString "x2")
    |> String.concat ""

/// Snapshot rows: register, record id, digest of the record's governed fields
/// (every column of every register, less `excluded`).
let snapshot (loaded: Loaded) : (string * string * string) list =
    Schema.registers
    |> List.collect (fun spec ->
        match Map.tryFind spec.Name loaded |> Option.flatten with
        | None -> []
        | Some(header, _) ->
            let cols = header |> List.filter (fun c -> not (excluded.Contains(spec.Name, c)))

            Traceability.records loaded spec.Name
            |> List.map (fun r ->
                let value c =
                    Map.tryFind c r |> Option.defaultValue ""

                let fields = cols |> List.map (fun c -> c + "=" + value c) |> String.concat "\u001f"

                spec.Name, value spec.IdColumn, digest fields))
    |> List.sort

/// Snapshot as CSV text.
let render (loaded: Loaded) =
    "register,id,digest\n"
    + (snapshot loaded
       |> List.map (fun (r, i, d) -> sprintf "%s,%s,%s" r i d)
       |> String.concat "\n")
    + "\n"

/// Differences between a committed snapshot and the current registers.
let differences (committed: (string * string * string) list) (loaded: Loaded) =
    let current = snapshot loaded |> set
    let previous = set committed

    let key (r, i, _) = r, i

    let changed =
        Set.union (Set.difference current previous) (Set.difference previous current)
        |> Set.map key
        |> Set.toList

    changed |> List.map (fun (r, i) -> sprintf "%s %s" r i)
