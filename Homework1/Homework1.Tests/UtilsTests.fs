module Homework1.Test

open NUnit.Framework
open Utils

[<TestCase(0, 1)>]
[<TestCase(1, 1)>]
[<TestCase(2, 2)>]
[<TestCase(3, 6)>]
[<TestCase(4, 24)>]
[<TestCase(5, 120)>]
[<TestCase(6, 720)>]
[<TestCase(10, 3628800)>]
let FactorialWithCorrectArgumentTests (number, expected) =
    Assert.That((factorial number).Value, Is.EqualTo expected)

[<TestCase(-1)>]
[<TestCase(-10)>]
[<TestCase(-235)>]
let FactorialWithIncorrectArgumentTests number =
    Assert.That(factorial number, Is.EqualTo None)
    
[<TestCase(1, 0)>]
[<TestCase(2, 1)>]
[<TestCase(3, 1)>]
[<TestCase(11, 55)>]
[<TestCase(31, 832040)>]
let FibonacciWithCorrectArgumentTests (number, expected) =
    Assert.That((fibonacci number).Value, Is.EqualTo expected)

[<TestCase(0)>]
[<TestCase(-1)>]
[<TestCase(-100)>]
[<TestCase(-357)>]
let FibonacciWithIncorrectArgumentTests number =
    Assert.That(fibonacci number, Is.EqualTo None)

let reverseListTests =
    [ ([1; 2; 3], [3; 2; 1])        
      ([], [])              
      ([1], [1])          
      ([42; 99; 17], [17; 99; 42])   
      ([1; 2; 3; 2; 1], [1; 2; 3; 2; 1])
      ([100; 200; 300], [300; 200; 100])
    ]
    |> List.map (fun (input, expected) -> TestCaseData(input, expected))

[<TestCaseSource("reverseListTests")>]
let ReverseListTests (list, expected) =
    Assert.AreEqual(expected, reverseList list)

let correctPowerListTests =
    [(0, 0, [1])
     (1, 2, [2; 4; 8])
     (0, 3, [1; 2; 4; 8])
     (3, 3, [8; 16; 32; 64;])
     (0, 10, [1; 2; 4; 8; 16; 32; 64; 128; 256; 512; 1024])]
    |> List.map (fun (n, m, ls) -> TestCaseData(n, m, ls))

[<TestCaseSource("correctPowerListTests")>]
let PowerListWithCorrectArgumentsTests (n, m, expected) =
    Assert.That((powerList n m).Value, Is.EqualTo expected)

let incorrectPowerListTests =
    [ (-1, 0)
      (1, -1)
      (-3, -1) ]
    |> List.map (fun (n, m) -> TestCaseData(n, m))

[<TestCaseSource("incorrectPowerListTests")>]
let PowerListWithIncorrectArgumentsTests (n, m) =
    Assert.That(powerList n m, Is.EqualTo None)

let correctFindListTests =
    [ ([1; 2; 3; 4], 3, Some 2)
      ([1; 2; 3; 4], 1, Some 0) ]
    |> List.map (fun (list, elem, expected) -> TestCaseData(list, elem, expected))

[<TestCaseSource("correctFindListTests")>]
let FindListWithCorrectArguments (list, elem, expected) =
    Assert.AreEqual(expected, findList list elem)

let incorrectFindListTests =
    [ ([], 1)
      ([ -1; 7; 12 ], 11) ]
    |> List.map (fun (ls, n) -> TestCaseData(ls, n))

[<TestCaseSource("incorrectFindListTests")>]
let FindListWithIncorrectArguments (list, elem) =
    Assert.AreEqual(None, findList list elem)

