module stringCalculator

type StringCalculator() =
    member this.Bind(x: string, f) =
        match System.Int32.TryParse x with
        | true, value -> f value
        | false, _ -> None

    member this.Return x = Some(x)

let calculate = StringCalculator()