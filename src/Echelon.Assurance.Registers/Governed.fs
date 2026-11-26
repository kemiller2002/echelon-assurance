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
        | Some(header, rows) ->
            // Raw rows, including malformed ones, so a row that fails to parse
            // still changes the snapshot (round 13).
            let idIndex =
                header |> List.tryFindIndex ((=) spec.IdColumn) |> Option.defaultValue 0

            rows
            |> List.map (fun row ->
                let named =
                    row
                    |> List.mapi (fun i v ->
                        let c =
                            if i < List.length header then
                                header[i]
                            else
                                sprintf "extra%d" i

                        c, v)
                    |> List.filter (fun (c, _) -> not (excluded.Contains(spec.Name, c)))
                    |> List.map (fun (c, v) -> c + "=" + v)
                    |> String.concat "\u001f"

                let id = if idIndex < List.length row then row[idIndex] else ""
                spec.Name, id, digest named))
    |> List.sort

/// Is a repository-relative CSV path an input outside the registers that the
/// governed record must also cover (economics inputs, mappings)? Generated
/// views, fragments, tests and Praxis registries are excluded.
let coversFile (relative: string) =
    let p = relative.Replace('\\', '/')
    let lower = p.ToLowerInvariant()
    let registerPaths = Schema.registers |> List.map (fun s -> s.Path) |> set
    let name = lower.Substring(lower.LastIndexOf '/' + 1)

    let buildOrOutput =
        [ "/bin/"; "/obj/" ] |> List.exists lower.Contains
        || [ "bin/"; "obj/" ] |> List.exists lower.StartsWith

    let excludedRoot =
        [ "tests/"; "registries/"; ".git/"; "node_modules/" ]
        |> List.exists lower.StartsWith

    let buildInput =
        lower.EndsWith ".fsproj"
        || lower.EndsWith ".sln"
        || name.StartsWith "directory.build."
        || name.StartsWith "directory.packages."
        || name = "global.json"
        || name = "nuget.config"
        || lower = ".config/dotnet-tools.json"
        || lower = ".editorconfig"

    not buildOrOutput
    && not (excludedRoot && not (lower.EndsWith ".fsproj"))
    && (buildInput
        || lower.StartsWith "src/"
        || (lower.StartsWith "docs/decisions/" && lower.EndsWith ".md")
        || (lower.StartsWith "docs/charter/" && lower.EndsWith ".md")
        || (lower.StartsWith "policies/" && lower.EndsWith ".md")
        || (lower.StartsWith "findings/"
            && lower.EndsWith ".md"
            && not (lower.StartsWith "findings/challenge-passes/"))
        || (lower.EndsWith ".md"
            && not (lower.Contains "/generated/" || lower.Contains "/fragments/")
            && ([
                    "controls/"
                    "governance/"
                    "ai-governance/"
                    "validation/"
                    "architecture/"
                    "roadmap/"
                    "context/"
                    "customer-assurance/"
                    "economics/"
                    "regulatory/"
                    "financial/"
                    "soc/"
                    "pentest/"
                    "methodology/"
                ]
                |> List.exists lower.StartsWith
                || lower = "readme.md"
                || lower = "handoff.md"))
        || lower = "project-charter.md"
        || lower = "agents.md"
        || lower = "security-and-confidentiality.md"
        || (lower.StartsWith "requirements/"
            && lower.EndsWith ".md"
            && not (lower.StartsWith "requirements/generated/"))
        || (lower.StartsWith ".github/workflows/"
            && (lower.EndsWith ".yml" || lower.EndsWith ".yaml"))
        || (lower.EndsWith ".csv"
            && not (registerPaths.Contains p)
            && not ([ "/generated/"; "/fragments/" ] |> List.exists lower.Contains)
            && not excludedRoot))

/// Digest rows for covered input files (whole file, line endings normalized).
let fileSnapshot (files: (string * string) list) =
    files
    |> List.filter (fst >> coversFile)
    |> List.map (fun (path, content) -> "file", path.Replace('\\', '/'), digest (content.Replace("\r\n", "\n")))

/// Combined snapshot: register records and covered input files.
let full (loaded: Loaded) (files: (string * string) list) =
    snapshot loaded @ fileSnapshot files |> List.sort

/// Snapshot as CSV text.
let render (loaded: Loaded) (files: (string * string) list) =
    "register,id,digest\n"
    + (full loaded files
       |> List.map (fun (r, i, d) -> sprintf "%s,%s,%s" r i d)
       |> String.concat "\n")
    + "\n"

/// Differences between a committed snapshot and the current registers.
let differencesWith (committed: (string * string * string) list) (loaded: Loaded) (files: (string * string) list) =
    let current = full loaded files |> set
    let previous = set committed

    let key (r, i, _) = r, i

    let changed =
        Set.union (Set.difference current previous) (Set.difference previous current)
        |> Set.map key
        |> Set.toList

    changed |> List.map (fun (r, i) -> sprintf "%s %s" r i)

/// Differences for register records only (files not supplied).
let differences (committed: (string * string * string) list) (loaded: Loaded) =
    differencesWith (committed |> List.filter (fun (r, _, _) -> r <> "file")) loaded []
