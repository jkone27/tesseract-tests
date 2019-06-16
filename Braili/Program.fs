// Learn more about F# at http://fsharp.org

open System
open System.IO
open Tesseract
open System.Drawing

let workingDirectory = Environment.CurrentDirectory
let projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName

let initializeTesseractEngine (_ : unit) =
    let tessDataPath = Path.Combine(projectDirectory, "tessdata")
    new TesseractEngine(tessDataPath, "eng", EngineMode.Default)

let tesseractEngine = initializeTesseractEngine()

let readTextfromImage imgData =
    let img = Pix.LoadTiffFromMemory(imgData)
    //https://docs.opencv.org/master/d7/d4d/tutorial_py_thresholding.html (in case optimization is needed)
    //https://www.codeproject.com/Articles/38319/Famous-Otsu-Thresholding-in-C
    let page = tesseractEngine.Process(img)
    page.GetText()

[<EntryPoint>]
let main argv =
    let imagePath = Path.Combine(projectDirectory, "tesseract-test.png")
    let img = Image.FromFile(imagePath)
    use ms = new MemoryStream()
    img.Save(ms, System.Drawing.Imaging.ImageFormat.Tiff);
    let bytes =  ms.ToArray();
    let txt = readTextfromImage bytes
    printfn "%s" txt
    
    0 // return an integer exit code
