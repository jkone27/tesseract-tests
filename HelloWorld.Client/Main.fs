module HelloWorld.Client.Main

open System
open Elmish
open Bolero
open Bolero.Html
open Bolero.Json
open Bolero.Remoting
open Bolero.Remoting.Client
open Bolero.Templating.Client

/// Routing endpoints definition.
type Page =
    | [<EndPoint "/">] Home

/// The Elmish application's model.
type Model =
    {
        page: Page;
        error: string option;
        response : string;
    }

let initModel =
    {
        page = Home;
        error = None;
        response = String.Empty;
    }

/// The Elmish application's update messages.
type Message =
    | SetPage of Page
    | Error of exn
    | ClearError
    | FormSent of string

let Update message model =
    match message with
    | SetPage page ->
        { model with page = page }
    | FormSent str ->
        { model with page = Home; response = str }
    | Error exn ->
        { model with error = Some exn.Message }
    | ClearError ->
        { model with error = None }

/// Connects the routing system to the Elmish application.
let router = Router.infer SetPage (fun model -> model.page)

type Main = Template<"wwwroot/main.html">

//need to post as ... enctype="multipart/form-data" and read the response
// https://msdn.microsoft.com/en-us/magazine/mt833274.aspx

let View (model : Model) dispatch =
    Main()
        .Result(model.response)
        .Action("https://locahost:5001/ocr")
        .Submit(model.response, fun n -> dispatch (FormSent n))
        .Elt()

type MyApp() =
    inherit ProgramComponent<Model, Message>()

    override this.Program =
        Program.mkSimple (fun _ -> initModel) Update View
        |> Program.withRouter router
