/// Register validation: header, identifiers, enumerations, cross-register
/// references and a small set of high-confidence secret patterns. Pure.
module Echelon.Assurance.Registers.Validation

open System.Text.RegularExpressions
open Echelon.Assurance.Registers.Schema

/// A loaded register: None when the file does not exist.
type Loaded = Map<string, (Csv.Row * Csv.Row list) option>

let listSeparator = ';'

let splitList (value: string) =
    value.Split listSeparator
    |> Array.map (fun v -> v.Trim())
    |> Array.filter (fun v -> v <> "")
    |> List.ofArray

let secretPatterns =
    [
        Regex @"AKIA[0-9A-Z]{16}"
        Regex @"-----BEGIN (RSA |EC |OPENSSH )?PRIVATE KEY-----"
        Regex @"ghp_[A-Za-z0-9]{36}"
        Regex @"github_pat_[A-Za-z0-9_]{60,}"
        Regex @"xox[baprs]-[A-Za-z0-9-]{10,}"
        Regex @"(?<![A-Za-z0-9])sk-(ant-)?[A-Za-z0-9_-]{32,}"
    ]

let scannedSuffixes =
    [
        ".md"
        ".csv"
        ".json"
        ".py"
        ".fs"
        ".fsx"
        ".fsproj"
        ".yml"
        ".yaml"
        ".txt"
        ".sh"
        ".ps1"
        ".cmd"
        ".jsonl"
        ".props"
        ".targets"
        ".sln"
        ".fsproj"
    ]

let private record (spec: RegisterSpec) (row: Csv.Row) = List.zip spec.Columns row |> Map.ofList

let headerErrors (spec: RegisterSpec) (header: Csv.Row) =
    if header = spec.Columns then
        []
    else
        [
            sprintf "%s: header mismatch; expected %s" spec.Path (String.concat "," spec.Columns)
        ]

let rowErrors (spec: RegisterSpec) (rows: Csv.Row list) =
    let pattern = Regex spec.IdPattern
    let width = List.length spec.Columns

    let one (line, (row: Csv.Row)) =
        let where = sprintf "%s:%d" spec.Path line

        if List.length row <> width then
            [ sprintf "%s: %d fields, expected %d" where (List.length row) width ]
        else
            let r = record spec row
            let id = r[spec.IdColumn]

            let idErr =
                if pattern.IsMatch id then
                    []
                else
                    [ sprintf "%s: bad id '%s'" where id ]

            let enumErrs =
                spec.Enums
                |> Map.toList
                |> List.choose (fun (col, allowed) ->
                    let v = r[col]

                    if v <> "" && not (allowed.Contains v) then
                        Some(sprintf "%s: %s='%s' not in allowed values" where col v)
                    else
                        None)

            let listEnumErrs =
                listEnums
                |> Map.tryFind spec.Name
                |> Option.defaultValue Map.empty
                |> Map.toList
                |> List.collect (fun (col, allowed) ->
                    splitList r[col]
                    |> List.filter (allowed.Contains >> not)
                    |> List.map (sprintf "%s: %s contains '%s' not in allowed values" where col))

            idErr @ enumErrs @ listEnumErrs

    let idIndex = spec.Columns |> List.findIndex ((=) spec.IdColumn)

    let duplicates =
        rows
        |> List.choose (fun r -> List.tryItem idIndex r)
        |> List.countBy id
        |> List.filter (fun (_, n) -> n > 1)
        |> List.map fst
        |> List.sort
        |> List.map (sprintf "%s: duplicate id %s" spec.Path)

    (rows |> List.mapi (fun i r -> i + 2, r) |> List.collect one) @ duplicates

let idIndex (loaded: Loaded) : Map<string, Set<string>> =
    loaded
    |> Map.toList
    |> List.choose (fun (name, content) ->
        content
        |> Option.map (fun (_, rows) ->
            let spec = byName name
            let i = spec.Columns |> List.findIndex ((=) spec.IdColumn)
            name, rows |> List.choose (List.tryItem i) |> Set.ofList))
    |> Map.ofList

let referenceErrors (spec: RegisterSpec) (rows: Csv.Row list) (ids: Map<string, Set<string>>) =
    let width = List.length spec.Columns

    rows
    |> List.filter (fun r -> List.length r = width)
    |> List.collect (fun row ->
        let r = record spec row

        spec.Refs
        |> Map.toList
        |> List.collect (fun (col, target) ->
            match Map.tryFind target ids with
            | None -> []
            | Some known ->
                splitList r[col]
                |> List.filter (known.Contains >> not)
                |> List.map (fun missing ->
                    sprintf "%s: %s -> %s references unknown %s" spec.Path r[spec.IdColumn] col missing)))

/// Secret findings for (relative path, text) pairs.
let secretErrors (files: (string * string) list) =
    files
    |> List.collect (fun (path, text) ->
        secretPatterns
        |> List.filter (fun p -> p.IsMatch text)
        |> List.map (fun p -> sprintf "%s: possible secret matching %s" path (string p)))

/// All errors for a set of loaded registers and scanned files.
let validate (loaded: Loaded) (files: (string * string) list) =
    let missing =
        registers
        |> List.filter (fun s -> Map.tryFind s.Name loaded |> Option.flatten |> Option.isNone)
        |> List.map (fun s -> sprintf "%s: missing" s.Path)

    let ids = idIndex loaded

    let perRegister =
        loaded
        |> Map.toList
        |> List.collect (fun (name, content) ->
            match content with
            | None -> []
            | Some(header, rows) ->
                let spec = byName name
                headerErrors spec header @ rowErrors spec rows @ referenceErrors spec rows ids)

    missing @ perRegister @ secretErrors files
