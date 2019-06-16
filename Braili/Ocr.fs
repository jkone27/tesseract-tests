module Ocr
    
    open System
    open System.IO
    open Tesseract

    let private tesseractEngine =
        new TesseractEngine(@"./tessdata", "eng", EngineMode.Default)

    let readTextfromImage imgData =
        let img = Pix.LoadTiffFromMemory(imgData)
        //https://docs.opencv.org/master/d7/d4d/tutorial_py_thresholding.html (in case optimization is needed)
        //https://www.codeproject.com/Articles/38319/Famous-Otsu-Thresholding-in-C
        use page = tesseractEngine.Process(img)
        page.GetText()