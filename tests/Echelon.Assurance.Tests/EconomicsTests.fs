module Echelon.Assurance.Tests.EconomicsTests

open Xunit
open Echelon.Assurance.Registers.Economics

let private scenario =
    {
        Id = "T"
        Description = "test"
        MaintenanceMultiplier = 1.0
        AuditorReliance = true
        AdditionalProductStartYear = 1
        DiscountRate = 0.0
        OpportunityMultiplier = 0.5
        LaborRate = 100.0, 200.0
    }

let private act alt cls perProduct annual start hours cash scaling condition =
    {
        Id = "ACT-X"
        Alternative = alt
        Component = "c"
        CostClass = cls
        PerProduct = perProduct
        Annual = annual
        StartYear = start
        Hours = hours
        Cash = cash
        Scaling = scaling
        Condition = condition
    }

[<Fact>]
let ``annual shared cash line costs five years`` () =
    let a = act "A" "subscription" false true 1 (0.0, 0.0) (10.0, 20.0) 0.0 "always"
    Assert.Equal((50.0, 100.0), lifecycle scenario 1 a)

[<Fact>]
let ``one-time line costs once in its start year`` () =
    let a = act "A" "shared-build" false false 2 (10.0, 10.0) (0.0, 0.0) 0.0 "always"
    Assert.Equal((1000.0, 2000.0), lifecycle scenario 3 a)

[<Fact>]
let ``labor converts hours at the low and high rate`` () =
    let a = act "A" "human-admin" false false 1 (1.0, 2.0) (5.0, 5.0) 0.0 "always"
    Assert.Equal((105.0, 405.0), unitCost scenario a)

[<Fact>]
let ``per-product line scales additional products by the scaling factor`` () =
    let a =
        act "A" "external-examination" true true 1 (0.0, 0.0) (100.0, 100.0) 0.5 "always"
    // 1 + (3 - 1) * 0.5 = 2 units per year, 5 years
    Assert.Equal((1000.0, 1000.0), lifecycle scenario 3 a)

[<Fact>]
let ``additional products start no earlier than the adoption year`` () =
    let slow =
        { scenario with
            AdditionalProductStartYear = 3
        }

    let a =
        act "A" "external-examination" true true 1 (0.0, 0.0) (100.0, 100.0) 1.0 "always"
    // first product 5 years, second product years 3..5
    Assert.Equal((800.0, 800.0), lifecycle slow 2 a)

[<Fact>]
let ``maintenance multiplier applies only to maintenance classes`` () =
    let heavy =
        { scenario with
            MaintenanceMultiplier = 2.0
        }

    let m =
        act "A" "shared-maintenance" false true 1 (0.0, 0.0) (10.0, 10.0) 0.0 "always"

    let s = act "A" "subscription" false true 1 (0.0, 0.0) (10.0, 10.0) 0.0 "always"
    Assert.Equal((100.0, 100.0), lifecycle heavy 1 m)
    Assert.Equal((50.0, 50.0), lifecycle heavy 1 s)

[<Fact>]
let ``nonreliance lines apply only when the auditor does not rely`` () =
    let a = act "A" "human-admin" false true 1 (0.0, 0.0) (10.0, 10.0) 0.0 "nonreliance"
    Assert.Equal((0.0, 0.0), lifecycle scenario 1 a)

    Assert.Equal(
        (50.0, 50.0),
        lifecycle
            { scenario with
                AuditorReliance = false
            }
            1
            a
    )

[<Fact>]
let ``discounting reduces later years`` () =
    let d = { scenario with DiscountRate = 0.1 }
    let a = act "A" "subscription" false true 1 (0.0, 0.0) (100.0, 100.0) 0.0 "always"
    let lo, _ = lifecycle d 1 a
    Assert.True(lo < 500.0 && lo > 400.0)

[<Fact>]
let ``baseline lines are excluded from incremental cost and common lines included`` () =
    let lines =
        [
            act "BASELINE" "baseline-engineering" false true 1 (0.0, 0.0) (1.0, 1.0) 0.0 "always"
            act "ALL" "external-pentest" false true 1 (0.0, 0.0) (2.0, 2.0) 0.0 "always"
            act "A" "subscription" false true 1 (0.0, 0.0) (3.0, 3.0) 0.0 "always"
            act "B" "subscription" false true 1 (0.0, 0.0) (7.0, 7.0) 0.0 "always"
        ]

    let r = evaluate lines scenario "A" 1
    Assert.Equal((25.0, 25.0), r.Incremental)
    Assert.Equal((5.0, 5.0), r.Baseline)

[<Fact>]
let ``opportunity cost counts build hours only`` () =
    let lines =
        [
            act "A" "shared-build" false false 1 (10.0, 10.0) (500.0, 500.0) 0.0 "always"
            act "A" "human-admin" false true 1 (10.0, 10.0) (0.0, 0.0) 0.0 "always"
        ]

    Assert.Equal((500.0, 1000.0), (evaluate lines scenario "A" 1).Opportunity)

[<Fact>]
let ``break-even finds first product count where candidate is no more expensive`` () =
    let lines =
        [
            act "BUILD" "shared-build" false false 1 (0.0, 0.0) (100.0, 100.0) 0.0 "always"
            act "BUILD" "external-examination" true true 1 (0.0, 0.0) (1.0, 1.0) 1.0 "always"
            act "BUY" "subscription" true true 1 (0.0, 0.0) (10.0, 10.0) 1.0 "always"
        ]

    // build: 100 + 5N ; buy: 50N  -> 100 + 5N <= 50N  -> N >= 2.22
    Assert.Equal(Some 3, breakEven lines scenario "BUILD" "BUY")
    Assert.Equal(Some 1, breakEven lines scenario "BUY" "BUILD")

[<Fact>]
let ``marginal cost of later products is constant for linear scaling`` () =
    let lines =
        [
            act "A" "external-examination" true true 1 (0.0, 0.0) (10.0, 10.0) 0.5 "always"
        ]

    Assert.Equal((25.0, 25.0), marginal lines scenario "A" 4)
