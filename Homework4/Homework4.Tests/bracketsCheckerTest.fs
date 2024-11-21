module checkBracketsTest

open NUnit.Framework
open bracketsChecker

let validBracketSequences () = 
    [
        "()";
        "[]"; 
        "{}"; 
        "([])"; 
        "{()}"; 
        "[{}]";
        "({[]})";
        "([]{})";
        "(((())))";
        "[]{}()";
        "({(([[[{}]]]))}(){}[][[]])"
    ] |> List.map (fun (input) -> TestCaseData(input))


let invalidBracketSequences () = 
    [
        "{";
        "[";
        "(";
        "}";
        "]";
        ")";
        "(]";
        "{]";
        "(}";
        "{(([])})}"
    ] |> List.map (fun (input) -> TestCaseData(input))



[<TestCaseSource("validBracketSequences")>]
let validBracketSequencesTest(sequence: string) =
    Assert.That(checkBrackets sequence, Is.EqualTo true)

[<TestCaseSource("invalidBracketSequences")>]
let invalidBracketSequencesTest(sequence: string) =
    Assert.That(checkBrackets sequence, Is.EqualTo false)   