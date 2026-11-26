module Echelon.Assurance.Tests.CsvTests

open Xunit
open FsCheck
open FsCheck.Xunit
open Echelon.Assurance.Registers

[<Fact>]
let ``parses quoted fields with commas, quotes and newlines`` () =
    let text = "a,b,c\n1,\"x, y\",\"he said \"\"hi\"\"\"\n2,\"line1\nline2\",\n"
    let rows = Csv.parse text

    Assert.Equal<Csv.Row list>(
        [
            [ "a"; "b"; "c" ]
            [ "1"; "x, y"; "he said \"hi\"" ]
            [ "2"; "line1\nline2"; "" ]
        ],
        rows
    )

[<Fact>]
let ``final line without terminator is a row and trailing newline adds none`` () =
    Assert.Equal<Csv.Row list>([ [ "a"; "b" ]; [ "1"; "2" ] ], Csv.parse "a,b\n1,2")
    Assert.Equal<Csv.Row list>([ [ "a"; "b" ]; [ "1"; "2" ] ], Csv.parse "a,b\n1,2\n")

[<Fact>]
let ``CRLF line endings are accepted`` () =
    Assert.Equal<Csv.Row list>([ [ "a"; "b" ]; [ "1"; "2" ] ], Csv.parse "a,b\r\n1,2\r\n")

[<Fact>]
let ``render quotes only when needed`` () =
    Assert.Equal("a,\"b,c\",\"d\"\"e\"\n", Csv.render [ [ "a"; "b,c"; "d\"e" ] ])

[<Fact>]
let ``table drops blank rows`` () =
    let header, rows = Csv.table "h1,h2\n1,2\n,\n3,4\n"
    Assert.Equal<Csv.Row>([ "h1"; "h2" ], header)
    Assert.Equal(2, List.length rows)

let private safeField =
    Arb.Default.String().Generator
    |> Gen.map (fun s -> if isNull s then "" else s.Replace("\r", ""))

[<Property>]
let ``render then parse round-trips rows with at least two fields`` () =
    let row = Gen.listOfLength 3 safeField
    let rows = Gen.nonEmptyListOf row

    Prop.forAll (Arb.fromGen rows) (fun rows -> Csv.parse (Csv.render rows) = rows)
