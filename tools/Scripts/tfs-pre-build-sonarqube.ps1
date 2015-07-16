& $env:TF_BUILD_SOURCESDIRECTORY\tools\NuGet\nuget.exe restore $env:TF_BUILD_SOURCESDIRECTORY\src\EA.Iws.sln

& C:\SonarCube\SonarQube.MSBuild.Runner-0.9\SonarQube.MSBuild.Runner.exe /key:EA.Iws /name:EA.Iws /version:version1.0