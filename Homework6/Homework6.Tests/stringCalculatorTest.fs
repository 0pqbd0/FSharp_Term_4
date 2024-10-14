module stringCalculatorTest

open FsUnit
open NUnit.Framework
open stringCalculator

[<Test>]
let stringCalculatorWithTwoCorrectNumbers () =
    let result = 
        calculate {
        let! x = "15"
        let! y = "79"
        return x + y
    }
    result |> should equal (Some 94)

[<Test>]
let stringCalculatorWithThreeCorrectNumbers () =
    let calculate = StringCalculator()

    let result = calculate {
        let! x = "1500"
        let! y = "350"
        let! z = "70"
        let w = x + y + z
        return w
    }
    result |> should equal (Some 1920) 

[<Test>]
let stringCalculatorWithIncorrectNumbers () =
    let result = calculate {
        let! x = "1"
        let! y = "two"
        return x + y
    }
    result |> should equal None