module HttpHandler

    open Microsoft.AspNetCore.Http
    open Giraffe
    open FSharp.Control.Tasks.ContextInsensitive
    open Ocr
    open System.Drawing
    open System.IO
    open Microsoft.Extensions.Logging

    let handleFormRequest handler =
        fun (next : HttpFunc) (ctx : HttpContext) ->
        task {
            let logger = ctx.GetLogger<HttpHandler>()
            match ctx.Request.HasFormContentType with
            |false ->
                return! RequestErrors.BAD_REQUEST "unsupported mime" next ctx 
            |true ->
                logger.LogInformation(sprintf "output: %A" (Directory.GetCurrentDirectory()))
                let result = handler ctx.Request.Form.Files.[0]
                logger.LogInformation(sprintf "output: %A" result)
                return! text result next ctx
        }

    let readTextFromFormFileImage (imageFile :IFormFile) =
        use imageFileStream = imageFile.OpenReadStream()
        use tiffStream = new MemoryStream()
        let image = Image.FromStream(imageFileStream)
        image.Save(tiffStream, Imaging.ImageFormat.Tiff)
        let bytes : byte[] = tiffStream.ToArray()
        readTextfromImage bytes
        
            
