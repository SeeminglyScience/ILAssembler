[CmdletBinding()]
param(
    [Parameter()]
    [ValidateSet('Debug', 'Release')]
    [string] $Configuration = 'Release',

    [Parameter()]
    [switch] $Force,

    [Parameter()]
    [switch] $Publish
)
end {
    $IsUnix = $PSEdition -eq 'Core' -and -not $IsWindows
    $requirements = Import-PowerShellDataFile $PSScriptRoot\requirements.psd1
    foreach ($requirement in $requirements.GetEnumerator()) {
        if ($requirement.Key -match 'Dotnet') {
            $global:dotnet = & "$PSScriptRoot/tools/GetDotNet.ps1" -Version $requirement.Value -Unix:$IsUnix
            continue
        }

        $importModuleSplat = @{
            MinimumVersion = $requirement.Value
            Force = $true
            ErrorAction = 'Ignore'
            PassThru = $true
            Name = $requirement.Key
        }

        if (-not (Import-Module @importModuleSplat)) {
            $installModuleSplat = @{
                MinimumVersion = $requirement.Value
                Scope = 'CurrentUser'
                AllowClobber = $true
                AllowPrerelease = $true
                SkipPublisherCheck = $true
                Force = $true
                Name = $requirement.Key
            }

            Install-Module @installModuleSplat -ErrorAction Stop
            $importModuleSplat['ErrorAction'] = 'Stop'
            $null = Import-Module @importModuleSplat
        }
    }

    if ($Publish) {
        $ibTask = 'Publish'
    } else {
        $ibTask = 'Build'
    }

    $invokeBuildSplat = @{
        Task = $ibTask
        File = "$PSScriptRoot/ILAssembler.build.ps1"
        Force = $Force.IsPresent
        Configuration = $Configuration
    }

    Invoke-Build @invokeBuildSplat
}
