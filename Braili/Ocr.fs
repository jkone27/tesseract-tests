module Ocr
    
    open System
    open System.IO
    open Tesseract

    let private workingDirectory = Environment.CurrentDirectory
    let private projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName

    let private initializeTesseractEngine (_ : unit) =
        let tessDataPath = Path.Combine(projectDirectory, "tessdata")
        new TesseractEngine(tessDataPath, "eng", EngineMode.Default)

    let private tesseractEngine = initializeTesseractEngine()

    let readTextfromImage imgData =
        let img = Pix.LoadTiffFromMemory(imgData)
        //https://docs.opencv.org/master/d7/d4d/tutorial_py_thresholding.html (in case optimization is needed)
        //https://www.codeproject.com/Articles/38319/Famous-Otsu-Thresholding-in-C
        let page = tesseractEngine.Process(img)
        page.GetText()