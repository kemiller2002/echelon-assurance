/// `assurance` CLI: the only code with effects (file system and console).
///   assurance validate [root]
///   assurance merge [--check] [root]
///   assurance roadmap [root]
///   assurance trace [root]
///   assurance econ [root]
///   assurance governed [--check] [root]
module Echelon.Assurance.Cli.Program

open System.IO
open Echelon.Assurance.Registers

let private excludedDirectories = set [ ".git"; "bin"; "obj"; "node_modules" ]

let private readRegister (root: string) (spec: Schema.RegisterSpec) =
    let path = Path.Combine(root, spec.Path)

    if File.Exists path then
        Some(Csv.table (File.ReadAllText path))
    else
        None

let private loadAll root : Validation.Loaded =
    Schema.registers
    |> List.map (fun s -> s.Name, readRegister root s)
    |> Map.ofList

let rec private files (dir: string) : string seq =
    seq {
        yield! Directory.EnumerateFiles dir

        for sub in Directory.EnumerateDirectories dir do
            let name =
                match Path.GetFileName sub with
                | null -> ""
                | n -> n

            if not (excludedDirectories.Contains name) then
                yield! files sub
    }

let private scannedFiles root =
    files root
    |> Seq.filter (fun p -> Validation.scannedSuffixes |> List.exists (fun s -> p.EndsWith s))
    |> Seq.map (fun p -> Path.GetRelativePath(root, p), File.ReadAllText p)
    |> List.ofSeq

let private summary (loaded: Validation.Loaded) =
    Schema.registers
    |> List.map (fun s ->
        let n =
            Map.tryFind s.Name loaded
            |> Option.flatten
            |> Option.map (snd >> List.length)
            |> Option.defaultValue 0

        sprintf "  %s: %d rows" s.Path n)

let private validate root =
    let loaded = loadAll root

    let errors =
        Validation.validate loaded (scannedFiles root) @ Traceability.errors loaded

    (if List.isEmpty errors then
         [ "OK: all registers valid" ]
     else
         errors)
    @ summary loaded
    |> List.iter (printfn "%s")

    if List.isEmpty errors then 0 else 1

let private fragmentsFor root (dir: string) (suffix: string) =
    let full = Path.Combine(root, dir)

    if Directory.Exists full then
        Directory.EnumerateFiles full
        |> Seq.filter (fun p -> p.EndsWith suffix)
        |> Seq.sortWith (fun a b -> System.String.CompareOrdinal(a, b))
        |> Seq.map (fun p ->
            let header, rows = Csv.table (File.ReadAllText p)
            Merge.fragment (Path.GetRelativePath(root, p)) header rows)
        |> List.ofSeq
    else
        []

let private merge root check =
    let plans =
        Schema.fragmentSources
        |> List.map (fun (name, dir, suffix) ->
            let spec = Schema.byName name
            let existing = readRegister root spec |> Option.map snd |> Option.defaultValue []
            Merge.plan spec existing (fragmentsFor root dir suffix))

    plans
    |> List.iter (fun p ->
        printfn "%s: +%d rows (%d total)" p.Register.Path p.Added (List.length p.Merged)
        p.Problems |> List.iter (printfn "  WARN %s")

        if not check then
            File.WriteAllText(Path.Combine(root, p.Register.Path), Csv.render (p.Register.Columns :: p.Merged)))

    // With --check, fail when a merge would add rows or reports problems.
    if
        check
        && plans |> List.exists (fun p -> p.Added > 0 || not (List.isEmpty p.Problems))
    then
        1
    else
        0

let private roadmap root =
    let header, rows =
        Csv.table (File.ReadAllText(Path.Combine(root, "controls/control-catalog.csv")))

    let controls = rows |> List.map (fun r -> List.zip header r |> Map.ofList)
    let out = Path.Combine(root, "roadmap/generated")
    Directory.CreateDirectory out |> ignore
    File.WriteAllText(Path.Combine(out, "controls-by-phase.md"), Roadmap.render controls)

    let backlogPath = Path.Combine(root, "roadmap/backlog.csv")

    if File.Exists backlogPath then
        let bh, brows = Csv.table (File.ReadAllText backlogPath)
        let items = brows |> List.map (fun r -> List.zip bh r |> Map.ofList)
        File.WriteAllText(Path.Combine(out, "backlog-by-phase.md"), Roadmap.renderBacklog items)

    printfn "rendered %d controls" (List.length controls)
    0

let private trace root =
    let out = Path.Combine(root, "requirements/generated")
    Directory.CreateDirectory out |> ignore
    File.WriteAllText(Path.Combine(out, "traceability-matrix.md"), Traceability.render (loadAll root))
    printfn "rendered requirements/generated/traceability-matrix.md"
    0

let private governedPath root =
    Path.Combine(root, "tests/Echelon.Assurance.Tests/governed-record.csv")

/// Writes the governed-record snapshot; with --check, reports differences.
let private repositoryCsvFiles (root: string) =
    Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories)
    |> Seq.map (fun p -> Path.GetRelativePath(root, p).Replace('\\', '/'), p)
    |> Seq.filter (fst >> Governed.coversFile)
    |> Seq.map (fun (rel, p) -> rel, File.ReadAllText p)
    |> List.ofSeq

let private governed root check =
    let loaded = loadAll root
    let files = repositoryCsvFiles root
    let path = governedPath root

    if check then
        let committed =
            if File.Exists path then
                Csv.table (File.ReadAllText path)
                |> snd
                |> List.choose (function
                    | [ r; i; d ] -> Some(r, i, d)
                    | _ -> None)
            else
                []

        match Governed.differencesWith committed loaded files with
        | [] ->
            printfn "governed record current"
            0
        | diffs ->
            diffs |> List.iter (printfn "governed field changed: %s")
            1
    else
        File.WriteAllText(path, Governed.render loaded files)
        printfn "wrote %s" (Path.GetRelativePath(root, path))
        0

let private econ root =
    let load name f =
        let header, rows =
            Csv.table (File.ReadAllText(Path.Combine(root, "economics/v2", name)))

        rows |> List.map (fun r -> List.zip header r |> Map.ofList |> f)

    let activities = load "activities.csv" Economics.activity
    let scenarios = load "scenarios.csv" Economics.scenario
    let alternatives = load "alternatives.csv" (fun r -> r["alternative_id"])
    let out = Path.Combine(root, "economics/v2/generated")
    Directory.CreateDirectory out |> ignore
    File.WriteAllText(Path.Combine(out, "model.md"), Economics.renderMarkdown activities scenarios alternatives)

    File.WriteAllText(
        Path.Combine(out, "results.csv"),
        Csv.render (Economics.renderCsv activities scenarios alternatives)
    )

    printfn
        "rendered economics/v2/generated (%d activities, %d scenarios)"
        (List.length activities)
        (List.length scenarios)

    0

let private tco root (rate: float) =
    let header, rows =
        Csv.table (File.ReadAllText(Path.Combine(root, "economics/parameters.csv")))

    let lines = rows |> List.map (fun r -> List.zip header r |> Map.ofList |> Tco.line)
    let totals = Tco.summarise rate lines
    let out = Path.Combine(root, "economics/generated")
    Directory.CreateDirectory out |> ignore
    let markdown = Tco.renderMarkdown rate totals
    File.WriteAllText(Path.Combine(out, "tco-summary.md"), markdown)
    File.WriteAllText(Path.Combine(out, "tco-summary.csv"), Csv.render (Tco.renderCsv totals))
    printf "%s" markdown
    0

/// Split arguments into options (with values) and positional arguments.
let private parseArgs (args: string list) =
    let valued = set [ "--rate" ]

    let rec go opts pos rest =
        match rest with
        | [] -> opts, List.rev pos
        | name :: value :: tail when valued.Contains name -> go (Map.add name value opts) pos tail
        | flag :: tail when flag.StartsWith "--" -> go (Map.add flag "" opts) pos tail
        | arg :: tail -> go opts (arg :: pos) tail

    go Map.empty [] args

[<EntryPoint>]
let main argv =
    let options, positional = parseArgs (List.ofArray argv)

    let rootOf rest =
        rest |> List.tryHead |> Option.defaultValue (Directory.GetCurrentDirectory())

    match positional with
    | "validate" :: rest -> validate (rootOf rest)
    | "merge" :: rest -> merge (rootOf rest) (options.ContainsKey "--check")
    | "roadmap" :: rest -> roadmap (rootOf rest)
    | "trace" :: rest -> trace (rootOf rest)
    | "econ" :: rest -> econ (rootOf rest)
    | "governed" :: rest -> governed (rootOf rest) (options.ContainsKey "--check")
    | "tco" :: rest ->
        let rate =
            options
            |> Map.tryFind "--rate"
            |> Option.map (fun v -> System.Double.Parse(v, System.Globalization.CultureInfo.InvariantCulture))
            |> Option.defaultValue 0.0

        tco (rootOf rest) rate
    | _ ->
        eprintfn "usage: assurance (validate | merge [--check] | roadmap | trace | econ | tco [--rate r]) [root]"
        2
