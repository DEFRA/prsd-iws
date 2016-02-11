# Enable -Verbose option
[CmdletBinding()]

param
(
    [Parameter(Mandatory=$true)]
    [uri]$url = $null,

    [Parameter(Mandatory=$true)]
    [string]$username = $null,

    [Parameter(Mandatory=$true)]
    [string]$password = $null,

    [Parameter(Mandatory=$true)]
    [string]$testEmail = $null
)

$failedTests = @();
$smokeTestUrl = New-Object System.Uri($url, "admin/smoke-test");
$loginUrl = New-Object System.Uri($url, "account/login");
$testEmailUrl = New-Object System.Uri($url, "admin/test-email");
$testEmailSuccessUrl = New-Object System.Uri($url, "admin/test-email/success");

### Smoke test ###
try 
{
    $smokeTestResult = Invoke-RestMethod $smokeTestUrl.ToString();
}
catch [System.Net.WebException]
{
    # Supress any exceptions
}

if ($smokeTestResult -ne "True")
{
    $failedTests += "Failed admin/smoke-test";
}

### Login ###
$loginResult = Invoke-WebRequest $loginUrl.ToString() -SessionVariable session;

$loginResult.Forms[0].Fields["Email"] = $username;
$loginResult.Forms[0].Fields["Password"] = $password;

$loginPostResult = Invoke-WebRequest $loginUrl.ToString() -Method Post -Body $loginResult.Forms[0].Fields -ContentType 'application/x-www-form-urlencoded' -WebSession $session;

if (!($loginPostResult.BaseResponse.ResponseUri -ne $loginUrl -and $loginPostResult.StatusCode -eq 200))
{
    $failedTests += "Failed login";
}

### Send test email ###
$testEmailResult = Invoke-WebRequest $testEmailUrl.ToString() -WebSession $session;

if ($testEmailResult.BaseResponse.ResponseUri -ne $testEmailUrl)
{
    $failedTests += "Access denied to send test email";
}

$testEmailResult.Forms[0].Fields["EmailTo"] = $testEmail;

$testEmailPostResult = Invoke-WebRequest $testEmailUrl.ToString() -Method Post -Body $testEmailResult.Forms[0].Fields -ContentType 'application/x-www-form-urlencoded' -WebSession $session;

if ($testEmailPostResult.BaseResponse.ResponseUri -ne $testEmailSuccessUrl -and $testEmailPostResult.StatusCode -ne 200)
{
    $failedTests += "Failed to send test email";
}

### Report success or failure ###
if ($failedTests.Length -gt 0)
{
    Write-Host "[FAILURE] : Smoke tests failed!";

    foreach ($failure in $failedTests)
    {
        Write-Host $failure;
    }
}
else
{
    Write-Host "[SUCCESS] : All smoke tests successful!";
}