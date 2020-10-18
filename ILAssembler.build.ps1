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
$CoreFramework = 'netcoreapp3.1'

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

task AssertDotNet -If { -not $global:dotnet } {
    $requirements = Import-PowerShellDataFile "$PSScriptRoot/requirements.psd1"
    $global:dotnet = & "$PSScriptRoot/tools/GetDotNet.ps1" -Version $requirements['DotnetSdk::release'] -Unix:$IsUnix
}

task BuildManaged -Jobs AssertDotNet, {
    if (-not $IsUnix) {
        & $dotnet publish --framework $DesktopFramework --configuration $Configuration --verbosity q -nologo
    }

    & $dotnet publish --framework $CoreFramework --configuration $Configuration --verbosity q -nologo
}

task BuildMaml {
    if ($PSVersionTable.PSVersion.Major -lt 7) {
        Write-Warning 'Building MAML help is only supported in PSv7+.'
        return
    }

    & $ToolsPath/MarkdownToMaml.ps1 -DocsPath $PSScriptRoot/docs/en-US -DestinationFile $ReleasePath/en-US/ILAssembler-help.xml
}

task CopyToRelease {
    "$ModuleName.psm1", "$ModuleName.psd1" | ForEach-Object {
        Join-Path $PowerShellPath -ChildPath $PSItem | Copy-Item -Destination $ReleasePath -Recurse @FailOnError
    }

    if (-not $IsUnix) {
        if (-not (Test-Path $ReleasePath/Desktop)) {
            $null = New-Item -ItemType Directory $ReleasePath/Desktop @FailOnError
        }

        Get-ChildItem "$CSharpPath/$ModuleName/bin/$Configuration/$DesktopFramework/publish" | ForEach-Object {
            Copy-Item $PSItem.FullName -Destination $ReleasePath/Desktop @FailOnError
        }
    }

    if (-not (Test-Path $ReleasePath/Core)) {
        $null = New-Item -ItemType Directory $ReleasePath/Core
    }

    Get-ChildItem "$CSharpPath/$ModuleName/bin/$Configuration/$CoreFramework/publish" | ForEach-Object {
        Copy-Item $PSItem.FullName -Destination $ReleasePath/Core @FailOnError
    }
}

task DoTest {
    $testResultsFolder = "$PSScriptRoot/TestResults"
    $testResultsFile = "$testResultsFolder/Pester.xml"
    if (-not (Test-Path $testResultsFolder)) {
        $null = New-Item $testResultsFolder -ItemType Directory -ErrorAction Stop
    }

    if (Test-Path $testResultsFile) {
        Remove-Item $testResultsFile -ErrorAction Stop
    }

    $pwsh = [Environment]::GetCommandLineArgs()[0] -replace '\.dll$', ''

    $arguments = @(
        '-NoProfile'
        '-NonInteractive'
        if (-not $IsUnix) {
            '-ExecutionPolicy', 'Bypass'
        })

    # I know this parameter set is deprecated, but it was taking too much
    # fiddling to use the new stuff.
    $command = {
        Import-Module Pester -RequiredVersion 5.0.4
        $results = Invoke-Pester -OutputFormat NUnitXml -OutputFile '{0}' -WarningAction Ignore -PassThru
        if ($results.Failed) {{
            [Environment]::Exit(1)
        }}
    } -f $testResultsFile

    $encodedCommand = [convert]::ToBase64String(
        [Text.Encoding]::Unicode.GetBytes($command))

    & $pwsh @arguments -OutputFormat Text -EncodedCommand $encodedCommand

    if ($LASTEXITCODE -ne 0) {
        throw 'Pester test failed!'
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
    if ($env:GALLERY_API_KEY) {
        $apiKey = $env:GALLERY_API_KEY
    } else {
        $userProfile = [Environment]::GetFolderPath([Environment+SpecialFolder]::UserProfile)
        if (Test-Path $userProfile/.PSGallery/apikey.xml) {
            $apiKey = (Import-Clixml $userProfile/.PSGallery/apikey.xml).GetNetworkCredential().Password
        }
    }

    if (-not $apiKey) {
        throw 'Could not find PSGallery API key!'
    }

    Publish-Module -Name $ReleasePath -NuGetApiKey $apiKey -AllowPrerelease -Force:$Force.IsPresent
}

task Build -Jobs Clean, BuildManaged, CopyToRelease, BuildMaml

task Test -Jobs Build, DoTest

task Install -Jobs Build, DoTest, DoInstall

task Publish -Jobs Build, DoTest, DoPublish

task . Build
