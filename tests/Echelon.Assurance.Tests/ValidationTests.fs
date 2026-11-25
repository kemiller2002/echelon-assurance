module Echelon.Assurance.Tests.ValidationTests

open Xunit
open Echelon.Assurance.Registers
open Echelon.Assurance.Registers.Schema

let private decisions = byName "decisions"

let private decisionRow id status =
    [ id; "t"; status; "owner"; "2026-11-25"; "s"; "p.md" ]

[<Fact>]
let ``header mismatch is reported`` () =
    let errors = Validation.headerErrors decisions [ "decision_id"; "title" ]
    Assert.Single errors |> ignore

[<Fact>]
let ``correct header passes`` () =
    Assert.Empty(Validation.headerErrors decisions decisions.Columns)

[<Fact>]
let ``bad identifier, bad enum and wrong width are reported`` () =
    let rows =
        [
            decisionRow "DF-ASR-2026-ZZZZ" "accepted"
            decisionRow "DF-ASR-2026-0D01" "maybe"
            [ "DF-ASR-2026-0D02"; "too"; "short" ]
        ]

    let errors = Validation.rowErrors decisions rows
    Assert.Contains(errors, fun e -> e.Contains "bad id")
    Assert.Contains(errors, fun e -> e.Contains "status='maybe'")
    Assert.Contains(errors, fun e -> e.Contains "3 fields, expected 7")

[<Fact>]
let ``duplicate identifiers are reported`` () =
    let rows =
        [
            decisionRow "DF-ASR-2026-0D01" "accepted"
            decisionRow "DF-ASR-2026-0D01" "accepted"
        ]

    let errors = Validation.rowErrors decisions rows
    Assert.Contains(errors, fun e -> e.Contains "duplicate id DF-ASR-2026-0D01")

[<Fact>]
let ``dangling cross-register reference is reported`` () =
    let risks = byName "risks"

    let row =
        [
            "RSK-001"
            "t"
            "c"
            "d"
            "low"
            "low"
            "CTL-GOV-001;CTL-XXX-999"
            "mitigate"
            "o"
            "identified"
        ]

    let ids = Map.ofList [ "controls", set [ "CTL-GOV-001" ] ]
    let errors = Validation.referenceErrors risks [ row ] ids

    Assert.Equal<string list>(
        [
            "registers/risk-register.csv: RSK-001 -> control_ids references unknown CTL-XXX-999"
        ],
        errors
    )

[<Fact>]
let ``references to an absent register are not checked`` () =
    let risks = byName "risks"

    let row =
        [
            "RSK-001"
            "t"
            "c"
            "d"
            "low"
            "low"
            "CTL-XXX-999"
            "mitigate"
            "o"
            "identified"
        ]

    Assert.Empty(Validation.referenceErrors risks [ row ] Map.empty)

[<Fact>]
let ``list values are split on semicolons and trimmed`` () =
    Assert.Equal<string list>([ "a"; "b"; "c" ], Validation.splitList " a; b;;c ")

[<Fact>]
let ``secret patterns detect high-confidence secrets`` () =
    let fake = "ghp_" + String.replicate 36 "a"
    let errors = Validation.secretErrors [ "x.md", sprintf "token %s here" fake ]
    Assert.Single errors |> ignore

[<Fact>]
let ``sk- inside an ordinary word is not a secret`` () =
    let text =
        "https://example.org/risk-related-to-ai-systems-and-agents-and-other-things"

    Assert.Empty(Validation.secretErrors [ "x.csv", text ])

[<Fact>]
let ``missing registers are reported`` () =
    let loaded = registers |> List.map (fun s -> s.Name, None) |> Map.ofList
    let errors = Validation.validate loaded []
    Assert.Equal(List.length registers, List.length errors)

/// Guards the composition in `validate` (a mutation that dropped all
/// per-register checks previously survived the suite).
[<Fact>]
let ``validate reports header, row and reference errors from loaded registers`` () =
    let empty =
        registers |> List.map (fun s -> s.Name, Some(s.Columns, [])) |> Map.ofList

    let risks = byName "risks"

    let riskRow =
        [
            "RSK-001"
            "t"
            "c"
            "d"
            "low"
            "low"
            "CTL-XXX-999"
            "mitigate"
            "o"
            "identified"
        ]

    let dup = decisionRow "DF-ASR-2026-0D01" "accepted"

    let loaded =
        empty
        |> Map.add risks.Name (Some(risks.Columns, [ riskRow ]))
        |> Map.add decisions.Name (Some(decisions.Columns, [ dup; dup ]))
        |> Map.add "sources" (Some([ "wrong" ], []))

    let errors = Validation.validate loaded []
    Assert.Contains(errors, fun e -> e.Contains "references unknown CTL-XXX-999")
    Assert.Contains(errors, fun e -> e.Contains "duplicate id DF-ASR-2026-0D01")
    Assert.Contains(errors, fun e -> e.Contains "sources/source-register.csv: header mismatch")

[<Fact>]
let ``validate reports secrets in scanned files`` () =
    let loaded =
        registers |> List.map (fun s -> s.Name, Some(s.Columns, [])) |> Map.ofList

    let fake = "AKIA" + String.replicate 16 "A"
    Assert.NotEmpty(Validation.validate loaded [ "f.md", fake ])
