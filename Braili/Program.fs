// Learn more about F# at http://fsharp.org

open System
open System.IO
open System.Drawing
open Ocr

let workingDirectory = Environment.CurrentDirectory
let projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName

[<EntryPoint>]
let internal main _ =
    let imagePath = Path.Combine(projectDirectory, "tesseract-test.png")
    let img = Image.FromFile(imagePath)
    use ms = new MemoryStream()
    img.Save(ms, System.Drawing.Imaging.ImageFormat.Tiff);
    let bytes =  ms.ToArray();
    let txt = readTextfromImage bytes
    printfn "%s" txt
    
    0 // return an integer exit code
