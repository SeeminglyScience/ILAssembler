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

    # Seems like PSDepend is failing to install from the gallery in the github
    # actions ubuntu image. As a band-aid I'm just gonna repeat the logic here
    # until I figure out a proper fix.
    if ($IsLinux) {
        $reqs = Import-PowerShellDataFile $PSScriptRoot/requirements.psd1
        foreach ($req in $reqs.GetEnumerator()) {
            if ($req.Key -match 'Dotnet') {
                continue
            }

            $importModuleSplat = @{
                MinimumVersion = $req.Value
                Force = $true
                ErrorAction = 'Ignore'
                PassThru = $true
                Name = $req.Key
            }

            if (-not (Import-Module @importModuleSplat)) {
                $installModuleSplat = @{
                    MinimumVersion = $req.Value
                    Scope = 'CurrentUser'
                    AllowClobber = $true
                    AllowPrerelease = $true
                    SkipPublisherCheck = $true
                    Force = $true
                    Name = $req.Key
                }

                Install-Module @installModuleSplat -ErrorAction Stop
                $importModuleSplat['ErrorAction'] = 'Stop'
                $null = Import-Module @importModuleSplat
            }

            # This also doesn't seem to work right.
            $env:PATH += ([System.IO.Path]::PathSeparator) + '/home/runner/.dotnet'
            sudo apt-get install -y zlib1g
        }
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
