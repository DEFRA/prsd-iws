@echo off

"tools\nuget\nuget.exe" "install" "xunit.runner.console" "-OutputDirectory" "tools" "-ExcludeVersion"
"tools\nuget\nuget.exe" "install" "FAKE" "-OutputDirectory" "tools" "-ExcludeVersion"

:Build
cls

"tools\FAKE\tools\Fake.exe" "build.fsx"