module Echelon.Assurance.Tests.TraceabilityTests

open Xunit
open Echelon.Assurance.Registers.Traceability

let private rec' (pairs: (string * string) list) = Map.ofList pairs

let private br id sources controls status =
    rec' (
        [
            "br_id", id
            "source_refs", sources
            "control_ids", controls
            "implementation_status", status
            "disposition", "retain"
            "tenet_ids", "TNT-01"
        ]
        @ (requiredBaselineColumns
           |> List.filter (fun c -> c <> "tenet_ids" && c <> "control_ids")
           |> List.map (fun c -> c, "x"))
    )

let private disp source brs disposition =
    rec'
        [
            "source_req_id", source
            "baseline_ids", brs
            "disposition", disposition
            "rationale", ""
        ]

[<Fact>]
let ``complete baseline row has no missing fields`` () =
    Assert.Empty(missingFields [ br "BR-APP-001" "" "CTL-APP-001" "Designed" ])

[<Fact>]
let ``empty owner or validation is reported`` () =
    let b =
        (br "BR-APP-001" "" "CTL-APP-001" "Designed").Add("owner_role", "").Add("validation_method", " ")

    Assert.Equal(2, List.length (missingFields [ b ]))

[<Fact>]
let ``deferred requirements may be incomplete`` () =
    let b =
        (br "BR-APP-001" "" "CTL-APP-001" "Designed").Add("owner_role", "").Add("disposition", "defer")

    Assert.Empty(missingFields [ b ])

[<Fact>]
let ``bidirectional disposition links pass when consistent`` () =
    let baseline = [ br "BR-APP-001" "SBC-001;SBC-005" "" "Designed" ]

    let ds =
        [
            disp "SBC-001" "BR-APP-001" "consolidate"
            disp "SBC-005" "BR-APP-001" "consolidate"
        ]

    Assert.Empty(dispositionLinks baseline ds)

[<Fact>]
let ``citation without disposition and one-way disposition are both reported`` () =
    let baseline =
        [ br "BR-APP-001" "SBC-001" "" "Designed"; br "BR-APP-002" "" "" "Designed" ]

    let ds = [ disp "SBC-009" "BR-APP-002" "retain" ]
    let errors = dispositionLinks baseline ds
    Assert.Equal(2, List.length errors)

[<Fact>]
let ``removal requires rationale and needs no baseline row`` () =
    let ok = (disp "CST-099" "" "remove").Add("rationale", "duplicate of SBC-001")
    Assert.Empty(dispositionLinks [] [ ok ])
    Assert.Single(dispositionLinks [] [ disp "CST-098" "" "remove" ]) |> ignore
    Assert.Single(dispositionLinks [] [ disp "CST-097" "" "retain" ]) |> ignore

[<Fact>]
let ``unsupported control is reported`` () =
    let ctl id =
        rec'
            [
                "control_id", id
                "objective", "o"
                "owner_role", "Owner"
                "validation_ids", "VAL-001"
                "evidence_type_ids", "EVT-001"
                "status", "Designed"
            ]

    let errors =
        controlSupport [ ctl "CTL-APP-001"; ctl "CTL-APP-002" ] [ br "BR-APP-001" "" "CTL-APP-001" "Designed" ] []

    Assert.Equal<string list>(
        [
            "controls/control-catalog.csv: CTL-APP-002 is not supported by any requirement"
        ],
        errors
    )

[<Fact>]
let ``baseline status above its controls is inflation`` () =
    let ctl = rec' [ "control_id", "CTL-APP-001"; "status", "Designed" ]

    Assert.Single(statusInflation [ ctl ] [ br "BR-APP-001" "" "CTL-APP-001" "Operating" ])
    |> ignore

    Assert.Empty(statusInflation [ ctl ] [ br "BR-APP-001" "" "CTL-APP-001" "Designed" ])

[<Fact>]
let ``customer answer cannot claim operating over designed controls`` () =
    let ctl = rec' [ "control_id", "CTL-APP-001"; "status", "Designed" ]

    let answer =
        rec'
            [
                "question_id", "CAQ-001"
                "implementation_status", "operating"
                "related_control_ids", "CTL-APP-001"
            ]

    Assert.Single(answerInflation [ ctl ] [ answer ]) |> ignore

[<Fact>]
let ``tenet without baseline requirement is reported`` () =
    let tenets = [ rec' [ "tenet_id", "TNT-01" ]; rec' [ "tenet_id", "TNT-02" ] ]

    Assert.Single(tenetCoverage tenets [ br "BR-APP-001" "" "" "Designed" ])
    |> ignore

[<Fact>]
let ``question status must agree with its disposition`` () =
    let q = rec' [ "oq_id", "OQ-100"; "status", "open" ]

    let d =
        rec' [ "oq_id", "OQ-100"; "proposed_status", "answered"; "next_action", "x" ]

    Assert.Single(questionCoverage [ q ] [ d ]) |> ignore
    Assert.Single(questionCoverage [ q ] []) |> ignore
