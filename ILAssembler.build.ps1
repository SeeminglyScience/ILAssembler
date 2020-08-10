#requires -Module InvokeBuild -Version 5.1
[CmdletBinding()]
param(
    [Parameter()]
    [ValidateSet('Debug', 'Release')]
    [string] $Configuration = 'Debug',

    [Parameter()]
    [switch] $Force
)

$ModuleName = 'ILAssembler'
$DesktopFramework = 'net471'
$CoreFramework = 'net5'

$FailOnError = @{
    ErrorAction = [System.Management.Automation.ActionPreference]::Stop
}

$Silent = @{
    ErrorAction = [System.Management.Automation.ActionPreference]::Ignore
    WarningAction = [System.Management.Automation.ActionPreference]::Ignore
}

$Manifest = Test-ModuleManifest -Path "$PSScriptRoot/module/$ModuleName.psd1" @Silent
$Version = $Manifest.Version
$PowerShellPath = "$PSScriptRoot/module"
$CSharpPath = "$PSScriptRoot/src"
$ReleasePath = "$PSScriptRoot/Release/$ModuleName/$Version"
$ToolsPath = "$PSScriptRoot/tools"
$IsUnix = $PSEdition -eq 'Core' -and -not $IsWindows

task Clean {
    if (Test-Path $ReleasePath) {
        Remove-Item $ReleasePath -Recurse
    }

    New-Item -ItemType Directory $ReleasePath | Out-Null
}

task AssertDependencies AssertDotNet

task AssertDotNet {
    $script:dotnet = & $ToolsPath/GetDotNet.ps1 -Unix:$IsUnix
}

task BuildManaged {
    & $dotnet publish --framework $DesktopFramework --configuration $Configuration --verbosity q -nologo
    & $dotnet publish --framework $CoreFramework --configuration $Configuration --verbosity q -nologo
}

task CopyToRelease {
    "$ModuleName.psm1", "$ModuleName.psd1" | ForEach-Object {
        Join-Path $PowerShellPath -ChildPath $PSItem | Copy-Item -Destination $ReleasePath -Recurse @FailOnError
    }

    if (-not (Test-Path $ReleasePath/Desktop)) {
        $null = New-Item -ItemType Directory $ReleasePath/Desktop @FailOnError
    }

    Get-ChildItem "$CSharpPath/$ModuleName/bin/$Configuration/$DesktopFramework/publish" | ForEach-Object {
        Copy-Item $PSItem.FullName -Destination $ReleasePath/Desktop @FailOnError
    }

    if (-not (Test-Path $ReleasePath/Core)) {
        $null = New-Item -ItemType Directory $ReleasePath/Core
    }

    Get-ChildItem "$CSharpPath/$ModuleName/bin/$Configuration/$CoreFramework/publish" | ForEach-Object {
        Copy-Item $PSItem.FullName -Destination $ReleasePath/Core @FailOnError
    }
}

task DoInstall {
    $installBase = $Home
    if ($profile) {
        $installBase = $profile | Split-Path
    }

    $installPath = "$installBase/Modules/$ModuleName/$Version"
    if (-not (Test-Path $installPath)) {
        $null = New-Item $installPath -ItemType Directory
    }

    Copy-Item -Path $ReleasePath/* -Destination $installPath -Force -Recurse
}

task DoPublish {
    if (-not (Test-Path $env:USERPROFILE/.PSGallery/apikey.xml)) {
        throw 'Could not find PSGallery API key!'
    }

    $apiKey = (Import-Clixml $env:USERPROFILE/.PSGallery/apikey.xml).GetNetworkCredential().Password
    Publish-Module -Name $ReleasePath -NuGetApiKey $apiKey -Confirm
}

task Build -Jobs Clean, AssertDependencies, BuildManaged, CopyToRelease

task Install -Jobs Build, DoInstall

task Publish -Jobs Build, DoPublish

task . Build
