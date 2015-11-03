& $env:TF_BUILD_SOURCESDIRECTORY\tools\NuGet\nuget.exe restore $env:TF_BUILD_SOURCESDIRECTORY\src\EA.Iws.sln

& C:\SonarCube\SonarQube.MSBuild.Runner-1.0.1\MSBuild.SonarQube.Runner.exe begin /key:EA.Iws /name:EA.Iws /version:version1.0