# Enable -Verbose option
[CmdletBinding()]

param
(
    [Parameter(Mandatory=$true)]
    [int]$BuildNumber = $null,

    [Parameter(Mandatory=$false)]
    [string]$RootPath = $null
)

###
# Set paths
###
if (-not $RootPath)
{
    $RootPath = Join-Path $PSScriptRoot "\..\..\";
}

$msbuild = "C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe";
$srcDir = Join-Path $RootPath "src";
$toolsDir =  Join-Path $RootPath "tools";
$outDir = Join-Path $RootPath "build"
$packagesDir = Join-Path $srcDir "packages";

if (-not ($outDir.EndsWith("\"))) { 
    $outDir += '\' #MSBuild requires OutDir end with a trailing slash #awesome 
} 
  
if ($outDir.Contains(" ")) { 
    $outDir="$($outDir)\" #read comment from Johannes Rudolph here: http://www.markhneedham.com/blog/2008/08/14/msbuild-use-outputpath-instead-of-outdir/ 
}

###
# Build version
###
$majorVersion = 1;
$minorVersion = 0;
$buildDate = Get-Date;
$buildVersion = [string]::Format("{0}.{1}.{2}.{3}", $majorVersion, $minorVersion, $buildDate.ToString("yy") + $buildDate.DayOfYear, $BuildNumber);

$iexUpdateAssemblyVersion = "& '$toolsDir\Scripts\update-assembly-versions.ps1' -NewVersion $buildVersion -SourceDirectory '$srcDir'"

&iex $iexUpdateAssemblyVersion

###
# Restore NuGet packages
# Quiet verbosity because the restore gives a warning for the maintenance project which causes Jenkins to
# mark the build as unstable
###

&$toolsDir\NuGet\nuget.exe restore $srcDir\EA.Iws.sln -Verbosity quiet

###
# Transform config files
###
write-host "Transform config files"

&$toolsDir\WebConfigTransform\wct.exe $srcDir\EA.Iws.Web\Web.config $srcDir\EA.Iws.Web\Web.Release.config $srcDir\EA.Iws.Web\Web.config

&$toolsDir\WebConfigTransform\wct.exe $srcDir\EA.Iws.Api\Web.config $srcDir\EA.Iws.Api\Web.Release.config $srcDir\EA.Iws.Api\Web.config

write-host "Beginning build"

$buildTarget = $srcDir + "\EA.Iws.sln" 
###
# Build the solution
# Because all paths may contain spaces they have to be enclosed with the ' character and the string for iex-ing must start with &
###
$iexBuild = "& '$msbuild' '$buildTarget' /p:Configuration=Release /p:Platform=x64 /p:OutDir='$outDir'"
&iex "$iexBuild"

if ($lastExitCode -ne 0) { 
    write-error "Error while running MSBuild. Details:`n$msbuildErrorOutput" 
    exit 1 
}

write-host "Finish build"

###
# Copy database scripts
###
New-Item $outDir\Database\ -ItemType Directory -Force | Out-Null
Copy-Item $srcDir\EA.Iws.Database\scripts\* $outDir\Database\ -Force -Recurse -Container

write-host "Copied database scripts"

###
# Run unit tests
###
$xunit = dir $packagesDir -recurse | where { $_.PSIsContainer -eq $false -and $_.Name -eq "xunit.console.exe" } | foreach { $_.FullName } | sort -descending
$testDlls = dir $outDir | where { $_.Name -like "*.Tests.Unit.dll" } | foreach { "`"" + $_.FullName +"`"" }

write-host "Found xunit test dlls"

# As with build the paths may contain spaces and must be enclosed by ' for the iex to work
$testDllString = ([string]::Join(" ", $testDlls))
$testOutDir = $outDir + "\xunit-test-results.xml"

$testConsole = $xunit

if($xunit -is [system.array])
{
    $testConsole = $xunit[0]
}

$iexTest = "& '$testConsole' '$testDllString' -parallel none -nunit '$testOutDir'"
&iex $iexTest