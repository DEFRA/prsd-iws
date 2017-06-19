#addin "Cake.Powershell&version=0.3.3"
#addin "Cake.XdtTransform&version=0.10.1"
#tool "nuget:?package=xunit.runner.console&version=2.2.0"

var target = Argument("target", "Default");
var version = Argument("buildversion", "9.9.9.9");
var configuration = Argument("configuration", "Release");

var buildDir = Directory("./build");

Task("Clean")
  .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("Restore-NuGet-Packages")
  .IsDependentOn("Clean")
  .Does(() =>
{
    NuGetRestore("./src/EA.Iws.sln");
});

Task("Build")
  .IsDependentOn("Restore-NuGet-Packages")
  .IsDependentOn("Update-AssemblyInfo")
  .Does(() =>
{
  MSBuild("./src/EA.Iws.sln", settings =>
    settings.SetConfiguration(configuration)
      .SetPlatformTarget(PlatformTarget.MSIL)
      .WithProperty("OutDir", MakeAbsolute(buildDir).FullPath + "\\")
      .SetVerbosity(Verbosity.Minimal));
});

Task("TransformWebConfig")
  .IsDependentOn("Build")
  .Does(() =>
{
  var sourceFile      = File("build/_PublishedWebsites/EA.Iws.Web/Web.config");
  var transformFile   = File("src/EA.Iws.Web/Web.Release.config");
  var targetFile      = File("build/_PublishedWebsites/EA.Iws.Web/Web.config");
  XdtTransformConfig(sourceFile, transformFile, targetFile);
});

Task("TransformApiConfig")
  .IsDependentOn("Build")
  .Does(() =>
{
  var sourceFile      = File("build/_PublishedWebsites/EA.Iws.Api/Web.config");
  var transformFile   = File("src/EA.Iws.Api/Web.Release.config");
  var targetFile      = File("build/_PublishedWebsites/EA.Iws.Api/Web.config");
  XdtTransformConfig(sourceFile, transformFile, targetFile);
});

Task("TransformConfigs")
  .IsDependentOn("TransformWebConfig")
  .IsDependentOn("TransformApiConfig");

Task("Run-Unit-Tests")
  .IsDependentOn("Build")
  .Does(() =>
{
  var testAssemblies = GetFiles("./build/*.Tests.Unit.dll");
  XUnit2(testAssemblies,
    new XUnit2Settings {
      Parallelism = ParallelismOption.All,
      HtmlReport = false,
      XmlReport = false,
      NUnitReport = true,
      NoAppDomain = true,
      ReportName = "xunit-test-results",
      OutputDirectory = "./build"
    });
});

Task("Copy-Database-Scripts")
  .Does(() => {
    CopyDirectory("src/EA.Iws.Database/scripts", "build/Database");
  });

Task("Default")
  .IsDependentOn("Run-Unit-Tests")
  .IsDependentOn("TransformConfigs")
  .IsDependentOn("Copy-Database-Scripts");

Task("Update-AssemblyInfo")
  .Description("Update all AssemblyInfo.cs files in the solution")
  .Does(() =>
{
  if (!BuildSystem.IsLocalBuild)
  {
    StartPowershellFile("./tools/Scripts/update-assembly-versions.ps1", args =>
    {
      args.Append("NewVersion", version)
        .Append("SourceDirectory", "./src");
    });
  }
  else
  {
    Information("Skipping Update-AssemblyInfo for local build");
  }
});

RunTarget(target);