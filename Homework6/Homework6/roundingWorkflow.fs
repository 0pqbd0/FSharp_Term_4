module roundingWorkflow

type RoundingBuilder(precision: int) =
    member this.Bind(x: double, f) = f (System.Math.Round(x, precision))
    member this.Return(x: double ) = System.Math.Round(x, precision)

let rounding precision = RoundingBuilder(precision)