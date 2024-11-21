module pointFree

(*   Point-free   *)

let pointFreeFunc = List.map << (*) 

//Шаги
//      List.map (fun y -> y * x) l   ->
//      List.map (fun y -> y * x)   ->
//      List.map ((*) x)   ->
//      List.map << (*)
