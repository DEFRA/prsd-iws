# Enable -Verbose option
[CmdletBinding()]

param
(
    [Parameter(Mandatory=$false)]
    [string]$RootPath = $null
)

function Compile-Project($project, $configuration, $platform, $outdir) {
  if (-not ($outdir.EndsWith("\"))) {
    $outdir += '\' #MSBuild requires OutDir end with a trailing slash #awesome
  }
 
  if ($outdir.Contains(" ")) {
    $outdir="""$($outdir)\""" #read comment from Johannes Rudolph here: http://www.markhneedham.com/blog/2008/08/14/msbuild-use-outputpath-instead-of-outdir/
  }

  If (Test-Path $outDir){
    Remove-Item $outDir -Recurse -Force
  }

  &$toolsDir\NuGet\nuget.exe restore $project -Verbosity quiet

  $iexBuild = "& '$msBuildPath' '$project' /m /verbosity:m /p:Configuration='$configuration' /p:Platform='$platform' /p:OutDir='$outdir'"
  &iex "$iexBuild" 2>$msbuildErrOutput
  if ($lastExitCode -ne 0) {
    write-error "Error while running MSBuild. Details:`n$msbuildErrorOutput"
    exit 1
  }
}

function Run-Xunit($dir) {
    &$toolsDir\NuGet\nuget.exe install "xunit.runner.console" "-OutputDirectory" $toolsDir "-ExcludeVersion" "-version" "2.1.0"
    $xunit = Join-Path $toolsDir "xunit.runner.console\tools\xunit.console.x86.exe"

    $testDlls = dir $dir | where { $_.Name -like "*.Tests.Unit.dll" } | foreach { "`"" + $_.FullName +"`"" }

    # As with build the paths may contain spaces and must be enclosed by ' for the iex to work
    $testDllString = ([string]::Join(" ", $testDlls))
    $testOutDir = $outDir + "\xunit-test-results.xml"

    $iexTest = "& '$xunit' '$testDllString' -parallel assemblies -noappdomain -nunit '$testOutDir'"
    &iex $iexTest
}

if (-not $RootPath)
{
    $RootPath = Join-Path $PSScriptRoot "\..\..\";
}

$msBuildPath = "C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe";

$srcDir = Join-Path $RootPath "src";
$toolsDir =  Join-Path $RootPath "tools";
$outDir = Join-Path $RootPath "build";
$buildTarget = Join-Path $srcDir "EA.Iws.sln";

Compile-Project $buildTarget "Release" "Any CPU" $outDir;

Run-Xunit $outDir