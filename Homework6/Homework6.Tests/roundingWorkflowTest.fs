    module Homework6.Tests

    open FsUnit
    open NUnit.Framework
    open roundingWorkflow

    [<Test>]
    let validPrecision() =
        let result = rounding 3 {
            let! a = 1.0
            let! b = 7.0
            return  a / b
        }
        result |> should equal 0.143

    [<Test>]
    let invalidPrecision() =
        (fun () ->  
            rounding -1 {
                let! a = 3.0
                let! b = 7.0
                return  a / b
            } 
            |> ignore)
        |> should throw typeof<System.ArgumentOutOfRangeException>