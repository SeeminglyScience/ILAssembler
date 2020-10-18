$projectBase = $PSCommandPath | Split-Path | Split-Path
$psd1File = Get-ChildItem $projectBase/Release/ILAssembler |
    Get-ChildItem |
    Get-ChildItem |
    Where-Object Name -Like *.psd1 |
    Select-Object -ExpandProperty FullName

if (-not (Get-Module ILAssembler -ea Ignore)) {
    Import-Module $psd1File -Global
}

function GetBodyAsShouldOperator {
    param(
        [Parameter(ValueFromPipeline)]
        [System.Delegate] $Delegate
    )
    process {
        $bytes = $Delegate | GetResolver | GetResolverField m_code
        '| Should -HaveBody {0}' -f (
            $(
                if ($bytes.Length -gt 1) { '('}
                $bytes.ForEach{ '0x{0:X2}' -f $PSItem } -join ', '
                if ($bytes.Length -gt 1) { ')'}
            ) -join '')
    }
}

function GetResolver {
    param(
        [Parameter(ValueFromPipeline)]
        [Delegate] $Delegate
    )
    process {
        $dynamicMethod = $Delegate.Method.GetType().
            GetField('m_owner', 60).
            GetValue($Delegate.Method)

        $dynamicMethod.GetType().
            GetField('m_resolver', 60).
            GetValue($dynamicMethod)
    }
}

function GetResolverField {
    param(
        [string] $FieldName,

        [Parameter(ValueFromPipeline)]
        [object] $Resolver
    )
    process {
        ,$Resolver.GetType().
            GetField($FieldName, 60).
            GetValue($Resolver)
    }
}

function ShouldHaveBody {
    param(
        $ActualValue,
        $ExpectedValue
    )
    end {

        function GetFailureMessage {
            param([string] $Reason)
            $failureMessage = "Expected method body to match, but $Reason." +
                [System.Environment]::NewLine + [System.Environment]::NewLine +
                'Expected: {0}' + [System.Environment]::NewLine +
                'Actual:   {1}'

            return $failureMessage -f (
                ($ExpectedValue.ForEach{ '0x{0:X2}' -f $PSItem } -join ', '),
                ($code.ForEach{ '0x{0:X2}' -f $PSItem } -join ', '))
        }

        $ExpectedValue = [byte[]]$ExpectedValue
        $code = [byte[]]($ActualValue | GetResolver | GetResolverField m_code)

        if ($ExpectedValue.Length -ne $code.Length) {
            return [PSCustomObject]@{
                Succeeded = $false
                FailureMessage = GetFailureMessage 'the byte count did not match'
            }
        }

        for ($i = 0; $i -lt $code.Length; $i++) {
            if ($ExpectedValue[$i] -ne $code[$i]) {
                return [PSCustomObject]@{
                    Succeeded = $false
                    FailureMessage = GetFailureMessage "the byte at offset $i did not match"
                }
            }
        }

        return [PSCustomObject]@{
            Succeeded = $true
            FailureMessage = [string]::Empty
        }
    }
}

$pesterModule = Get-Module Pester -ErrorAction Ignore
if (-not $pesterModule) {
    $pesterModule = Import-Module Pester -ErrorAction Stop -PassThru -Global
}

if (-not (& $pesterModule { $AssertionOperators.ContainsKey('HaveBody') })) {
    Add-ShouldOperator -Name HaveBody -Test $function:ShouldHaveBody
}
