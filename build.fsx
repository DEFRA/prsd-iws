// include Fake lib
#r "tools/FAKE/tools/FakeLib.dll"

open System
open Fake
open Fake.AssemblyInfoFile
open Fake.Testing.XUnit2

// Properties
let buildDir = "./build/"
let major = "1"
let minor = "0"

MSBuildDefaults <- { 
    MSBuildDefaults with 
        ToolsVersion = Some "14.0"
        Verbosity = Some MSBuildVerbosity.Minimal }

// Targets
Target "Clean" (fun _ ->
    CleanDir buildDir
)

Target "RestorePackages" (fun _ -> 
     "src/EA.Iws.sln"
     |> RestoreMSSolutionPackages (fun p ->
         { p with
             OutputPath = "src/packages"
             ToolPath = "tools/NuGet/nuget.exe"
             Retries = 4 })
)

let getBuildVersion = (fun _ -> 
    let buildDate = DateTime.Today.ToString("yy").ToString() + DateTime.Today.DayOfYear.ToString()
    let build = environVarOrDefault "BUILD_NUMBER" "0"
    String.Format("{0}.{1}.{2}.{3}", major, minor, buildDate, build)
)

Target "UpdateAssemblyVersions" (fun _ ->
    let version = getBuildVersion()

    BulkReplaceAssemblyInfoVersions "src/" (fun f -> 
    { f with
        AssemblyVersion = version
        AssemblyFileVersion = version})
)

Target "BuildApp" (fun _ ->
    !! "src/EA.Iws.sln"
      |> MSBuildRelease buildDir "Build"
      |> Log "AppBuild-Output: "
)

let transform = (fun args ->
    let result = ExecProcess (fun info -> 
        info.FileName <- "./tools/WebConfigTransform/wct.exe" 
        info.Arguments <- args) (TimeSpan.FromMinutes 5.0)

    if result <> 0 then failwithf "wct.exe returned with a non-zero exit code")

Target "TransformConfigFiles" (fun _ ->
    transform "src/EA.Iws.Web/Web.config src/EA.Iws.Web/Web.Release.config src/EA.Iws.Web/Web.config"
    transform "src/EA.Iws.Api/Web.config src/EA.Iws.Api/Web.Release.config src/EA.Iws.Api/Web.config"
)

Target "CopyDatabaseScripts" (fun _ ->
    CopyDir (buildDir @@ "Database") "src/EA.Iws.Database/scripts" allFiles
)

Target "Test" (fun _ ->
    !! (buildDir @@ "*.Tests.Unit.dll")
    |> xUnit2 (fun p -> 
        { p with 
            NUnitXmlOutputPath = Some (buildDir @@ "xunit-test-results.xml")
            ToolPath = "tools/xunit.runner.console/tools/xunit.console.exe" })
)

Target "Default" DoNothing

Target "Package" (fun _ ->
    CreateDir (buildDir @@ "Packages")

    NuGet (fun p -> 
        {p with
            Authors = [ "SFW Ltd" ]
            Project = "EA.Iws.Web"
            Description = "IWS Web"
            OutputPath = (buildDir @@ "Packages")
            Summary = "IWS Web"
            WorkingDir = "./"
            Version = getBuildVersion()
            Publish = false
            Files = [
                (@"build\_PublishedWebsites\EA.Iws.Web\**\*.*", Some "website", None)
                (@"tools\FAKE\tools\FakeLib.dll", Some "tools", None)
                (@"tools\FAKE\tools\Fake.IIS.dll", Some "tools", None)
                (@"tools\FAKE\tools\Microsoft.Web.Administration.dll", Some "tools", None)
            ]
        }) 
        "src/EA.Iws.nuspec"

    NuGet (fun p -> 
        {p with
            Authors = [ "SFW Ltd" ]
            Project = "EA.Iws.Api"
            Description = "IWS API"
            OutputPath = (buildDir @@ "Packages")
            Summary = "IWS API"
            WorkingDir = "./"
            Version = getBuildVersion()
            Publish = false
            Files = [
                (@"build\_PublishedWebsites\EA.Iws.Api\**\*.*", Some "website", None)
                (@"tools\FAKE\tools\FakeLib.dll", Some "tools", None)
                (@"tools\FAKE\tools\Fake.IIS.dll", Some "tools", None)
                (@"tools\FAKE\tools\Microsoft.Web.Administration.dll", Some "tools", None)
            ]
        }) 
        "src/EA.Iws.nuspec"

    NuGet (fun p -> 
        {p with
            Authors = [ "SFW Ltd" ]
            Project = "EA.Iws.Database"
            Description = "IWS Database"
            OutputPath = (buildDir @@ "Packages")
            Summary = "IWS Database"
            WorkingDir = "./"
            Version = getBuildVersion()
            Publish = false
            Files = [
                (@"tools\FAKE\tools\FakeLib.dll", Some "tools", None)
                (@"build\Database\**\*.*", Some "scripts", None)
            ]
        }) 
        "src/EA.Iws.nuspec"
)

// Dependencies
"Clean"
  ==> "RestorePackages"
  ==> "UpdateAssemblyVersions"
  ==> "TransformConfigFiles"
  ==> "BuildApp"
  ==> "CopyDatabaseScripts"
  ==> "Test"
  ==> "Default"
  ==> "Package"

// start build
RunTargetOrDefault "Default"