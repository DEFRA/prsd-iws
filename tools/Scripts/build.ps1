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
    $outDir="""$($outDir)\""" #read comment from Johannes Rudolph here: http://www.markhneedham.com/blog/2008/08/14/msbuild-use-outputpath-instead-of-outdir/ 
}

###
# Build version
###
$majorVersion = 1;
$minorVersion = 0;
$buildDate = Get-Date;
$buildVersion = [string]::Format("{0}.{1}.{2}.{3}", $majorVersion, $minorVersion, $buildDate.ToString("yy") + $buildDate.DayOfYear, $BuildNumber);

.\update-assembly-versions.ps1 -NewVersion $buildVersion -SourceDirectory $srcDir

###
# Restore NuGet packages
###
&$toolsDir\NuGet\nuget.exe restore $srcDir\EA.Iws.sln

###
# Transform config files
###
&$toolsDir\WebConfigTransform\wct.exe $srcDir\EA.Iws.Web\Web.config $srcDir\EA.Iws.Web\Web.Release.config $srcDir\EA.Iws.Web\Web.config

&$toolsDir\WebConfigTransform\wct.exe $srcDir\EA.Iws.Api\Web.config $srcDir\EA.Iws.Api\Web.Release.config $srcDir\EA.Iws.Api\Web.config

###
# Build the solution
###
&$msbuild $srcDir\EA.Iws.sln /p:Configuration=Release /p:Platform=x64 /p:OutDir="$outDir" 2>$msbuildErrOutput

if ($lastExitCode -ne 0) { 
    write-error "Error while running MSBuild. Details:`n$msbuildErrorOutput" 
    exit 1 
}

###
# Copy database scripts
###
New-Item $outDir\Database\ -ItemType Directory -Force | Out-Null
Copy-Item $srcDir\EA.Iws.Database\scripts\* $outDir\Database\ -Force -Recurse -Container

###
# Run unit tests
###
$xunit = dir $packagesDir -recurse | where { $_.PSIsContainer -eq $false -and $_.Name -eq "xunit.console.exe" } | foreach { $_.FullName } | sort -descending
$testDlls = dir $outDir | where { $_.Name -like "*.Tests.Unit.dll" } | foreach { "`"" + $_.FullName +"`"" }
&$xunit ([string]::Join(" ", $testDlls)) -parallel none -nunit $outDir\xunit-test-results.xml