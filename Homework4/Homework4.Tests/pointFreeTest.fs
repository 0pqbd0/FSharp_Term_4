module pointFreeTest

open pointFree
open NUnit.Framework
open FsCheck

[<Test>]
let MultiplicationTest () =
    let isEqule x list =
        List.map (fun y -> y * x) list = pointFreeFunc x list

    Check.Quick isEqule