
#I "packages/FAKE/tools"
#r "FakeLib.dll"

open System
open System.IO
open Fake
open Fake.FscHelper
open Fake.ProcessHelper

let getFullName dir name  =
    let src = DirectoryInfo(dir).GetFiles(name, SearchOption.AllDirectories) |> Seq.head
    src.FullName

let getSource name = getFullName "src" name
let getOuput name = getFullName "out" name
let getDll name = getFullName "packages" name

Target "jw.exe" (fun _ ->
        ["jw.fsx"] |> List.map getSource
        |> Fsc(fun p -> { p with Output = "out/jw.exe" })
    )

Target "locate" (fun _ ->
        let move src =
            let info = FileInfo(src)
            let target = sprintf "%s/%s" "out" info.Name
            if not (File.Exists target) then File.Copy(info.FullName, target, false)
        [
            "FunScript.dll"
            "FunScript.Interop.dll"
            "FunScript.TypeScript.Binding.lib.dll"
        ] |>  Seq.iter (getDll >> move)
    )

Target "execute" (fun _ ->
        let command = "jw.exe" |> getOuput
        Shell.Exec(command) |> ignore
    )

Target "watch" (fun _ ->
        use watcher = !! "src/*.fsx" |> WatchChanges(fun change ->
                    tracefn "%A" change
                    Run "execute"
                )
        Console.ReadLine() |> ignore
        watcher.Dispose()
    )

"jw.exe"
    ==> "locate"
    ==> "execute"

RunTargetOrDefault "watch"



