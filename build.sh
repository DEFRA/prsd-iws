#!/bin/bash
if test "$OS" = "Windows_NT"
then
  # use .Net

"./tools/nuget/nuget.exe" "install" "xunit.runner.console" "-OutputDirectory" "tools" "-ExcludeVersion" "-version" "2.1.0"
"./tools/nuget/nuget.exe" "install" "FAKE" "-OutputDirectory" "tools" "-ExcludeVersion" "-version" "4.8.0"

tools/FAKE/tools/FAKE.exe build.fsx 
else
echo "Not supported"
fi