# Enable -Verbose option
[CmdletBinding()]

param
(
    [Parameter(Mandatory=$true)]
    [string]$configPath = $null
)

# Open document as xml
[xml] $doc = get-content $configPath

$doc.SelectSingleNode("//customHeaders/add[@name=""Content-Security-Policy""]").Value = ""

$doc.Save($configPath);