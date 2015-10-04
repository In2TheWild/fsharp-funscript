#I "../packages/FunScript/lib/net40/"
#I "../packages/FunScript.TypeScript.Binding.lib/lib/net40"
#r "FunScript.dll"
#r "FunScript.TypeScript.binding.lib.dll"

open FunScript
open FunScript.TypeScript
open FunScript.Compiler
open System

[<ReflectedDefinition>]
module Jw =
    let document = Globals.document
    let main() =
        let div = document.getElementById("content")
        div.innerHTML <- "<span>Hello</span>"
        div.style.top <- "100px"
        
    let compile() =
        Compiler.Compile(
                <@ main @>,
                noReturn = true
            )

[<EntryPoint>]
let go args =
    try
        let rs = Jw.compile()
        Console.WriteLine rs
    with ex ->
        Console.WriteLine ex.Message
    0
