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
    $psDependVersion = '0.3.8'
    $importModuleSplat = @{
        MinimumVersion = $psDependVersion
        Force = $true
        ErrorAction = 'Ignore'
        PassThru = $true
        Name = 'PSDepend'
    }

    if (-not (Import-Module @importModuleSplat)) {
        $installModuleSplat = @{
            MinimumVersion = $psDependVersion
            Scope = 'CurrentUser'
            AllowClobber = $true
            AllowPrerelease = $true
            SkipPublisherCheck = $true
            Force = $true
            Name = 'PSDepend'
        }

        Install-Module @installModuleSplat -ErrorAction Stop
        $importModuleSplat['ErrorAction'] = 'Stop'
        $null = Import-Module @importModuleSplat
    }

    Invoke-PSDepend -Path $PSScriptRoot/requirements.psd1 -Import -Install -Force:$Force -ErrorAction Stop

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
