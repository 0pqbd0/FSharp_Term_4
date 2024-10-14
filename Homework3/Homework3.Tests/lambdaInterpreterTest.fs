module Homework3.Tests

open NUnit.Framework
open FsUnit
open lambdaInterpreter

let additionalTestCases () =
    [
        Var "x", Var "x"
        Application(Var "x", Var "y"), Application(Var "x", Var "y")
        Application(Abstraction("z", Var "z"), Var "x"), Var "x"
        Application(Abstraction("x", Var "y"), Application(Abstraction("x", Abstraction("x", Var "x")), Abstraction("x", Application(Var "x", Var "x")))), Var "y"
        Application(Abstraction("x", Application(Var"x", Var"y")), Abstraction("z", Var"z")), Application (Abstraction ("z", Var "z"), Var "y")
    ]|> List.map (fun (input, result) -> TestCaseData(input, result))

[<TestCaseSource("additionalTestCases")>]
let reduceTests data expected  =  
     lambdaInterpreter.reduce data |> should equal expected