# .ExternalHelp ILAssembler-help.xml

if ($PSVersionTable.PSVersion.Major -eq 5) {
    Microsoft.PowerShell.Core\Import-Module $PSScriptRoot\Desktop\ILAssembler.dll -ErrorAction Stop
} else {
    Microsoft.PowerShell.Core\Import-Module $PSScriptRoot\Core\ILAssembler.dll -ErrorAction Stop
}

$functionDrive = 'Microsoft.PowerShell.Core\Function'
$safeSetContent = Microsoft.PowerShell.Core\Get-Command 'Microsoft.PowerShell.Management\Set-Content' -CommandType Cmdlet

function ldc.i4 {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory, Position = 0)]
        [int] $value
    )
    end {
        $PSCmdlet.ThrowTerminatingError(
            [Management.Automation.ErrorRecord]::new(
                [Management.Automation.PSNotSupportedException]::new(
                    'This function can only be called inside a New-ILDelegate block.'),
                'OpCodeOutsideAssembler',
                [Management.Automation.ErrorCategory]::NotImplemented,
                $null));
    }
}

Microsoft.PowerShell.Core\Export-ModuleMember -Function ldc.i4

function ldc.i8 {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory, Position = 0)]
        [int64] $value
    )
    end {
        $PSCmdlet.ThrowTerminatingError(
            [Management.Automation.ErrorRecord]::new(
                [Management.Automation.PSNotSupportedException]::new(
                    'This function can only be called inside a New-ILDelegate block.'),
                'OpCodeOutsideAssembler',
                [Management.Automation.ErrorCategory]::NotImplemented,
                $null));
    }
}

Microsoft.PowerShell.Core\Export-ModuleMember -Function ldc.i8

function ldc.r8 {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory, Position = 0)]
        [double] $value
    )
    end {
        $PSCmdlet.ThrowTerminatingError(
            [Management.Automation.ErrorRecord]::new(
                [Management.Automation.PSNotSupportedException]::new(
                    'This function can only be called inside a New-ILDelegate block.'),
                'OpCodeOutsideAssembler',
                [Management.Automation.ErrorCategory]::NotImplemented,
                $null));
    }
}

Microsoft.PowerShell.Core\Export-ModuleMember -Function ldc.r8

function ldstr {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory, Position = 0)]
        [string] $value
    )
    end {
        $PSCmdlet.ThrowTerminatingError(
            [Management.Automation.ErrorRecord]::new(
                [Management.Automation.PSNotSupportedException]::new(
                    'This function can only be called inside a New-ILDelegate block.'),
                'OpCodeOutsideAssembler',
                [Management.Automation.ErrorCategory]::NotImplemented,
                $null));
    }
}

Microsoft.PowerShell.Core\Export-ModuleMember -Function ldstr

function ldc.r4 {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory, Position = 0)]
        [float] $value
    )
    end {
        $PSCmdlet.ThrowTerminatingError(
            [Management.Automation.ErrorRecord]::new(
                [Management.Automation.PSNotSupportedException]::new(
                    'This function can only be called inside a New-ILDelegate block.'),
                'OpCodeOutsideAssembler',
                [Management.Automation.ErrorCategory]::NotImplemented,
                $null));
    }
}

Microsoft.PowerShell.Core\Export-ModuleMember -Function ldc.r4

function unaligned. {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory, Position = 0)]
        [byte] $alignment
    )
    end {
        $PSCmdlet.ThrowTerminatingError(
            [Management.Automation.ErrorRecord]::new(
                [Management.Automation.PSNotSupportedException]::new(
                    'This function can only be called inside a New-ILDelegate block.'),
                'OpCodeOutsideAssembler',
                [Management.Automation.ErrorCategory]::NotImplemented,
                $null));
    }
}

Microsoft.PowerShell.Core\Export-ModuleMember -Function unaligned.

function switch {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory, Position = 0)]
        [string[]] $branches
    )
    end {
        $PSCmdlet.ThrowTerminatingError(
            [Management.Automation.ErrorRecord]::new(
                [Management.Automation.PSNotSupportedException]::new(
                    'This function can only be called inside a New-ILDelegate block.'),
                'OpCodeOutsideAssembler',
                [Management.Automation.ErrorCategory]::NotImplemented,
                $null));
    }
}

Microsoft.PowerShell.Core\Export-ModuleMember -Function switch

function ldc.i4.s {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory, Position = 0)]
        [byte] $value
    )
    end {
        $PSCmdlet.ThrowTerminatingError(
            [Management.Automation.ErrorRecord]::new(
                [Management.Automation.PSNotSupportedException]::new(
                    'This function can only be called inside a New-ILDelegate block.'),
                'OpCodeOutsideAssembler',
                [Management.Automation.ErrorCategory]::NotImplemented,
                $null));
    }
}

Microsoft.PowerShell.Core\Export-ModuleMember -Function ldc.i4.s

$noOperand = {
    [CmdletBinding()]
    param()
    end {
        $PSCmdlet.ThrowTerminatingError(
            [Management.Automation.ErrorRecord]::new(
                [Management.Automation.PSNotSupportedException]::new(
                    'This function can only be called inside a New-ILDelegate block.'),
                'OpCodeOutsideAssembler',
                [Management.Automation.ErrorCategory]::NotImplemented,
                $null));
    }
}

$branch = {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory, Position = 0)]
        [string] $branch
    )
    end {
        $PSCmdlet.ThrowTerminatingError(
            [Management.Automation.ErrorRecord]::new(
                [Management.Automation.PSNotSupportedException]::new(
                    'This function can only be called inside a New-ILDelegate block.'),
                'OpCodeOutsideAssembler',
                [Management.Automation.ErrorCategory]::NotImplemented,
                $null));
    }
}

$signature = {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory, Position = 0)]
        [scriptblock] $signature
    )
    end {
        $PSCmdlet.ThrowTerminatingError(
            [Management.Automation.ErrorRecord]::new(
                [Management.Automation.PSNotSupportedException]::new(
                    'This function can only be called inside a New-ILDelegate block.'),
                'OpCodeOutsideAssembler',
                [Management.Automation.ErrorCategory]::NotImplemented,
                $null));
    }
}

$variable = {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory, Position = 0)]
        [psvariable] $variable
    )
    end {
        $PSCmdlet.ThrowTerminatingError(
            [Management.Automation.ErrorRecord]::new(
                [Management.Automation.PSNotSupportedException]::new(
                    'This function can only be called inside a New-ILDelegate block.'),
                'OpCodeOutsideAssembler',
                [Management.Automation.ErrorCategory]::NotImplemented,
                $null));
    }
}

$index = {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory, Position = 0)]
        [int] $index
    )
    end {
        $PSCmdlet.ThrowTerminatingError(
            [Management.Automation.ErrorRecord]::new(
                [Management.Automation.PSNotSupportedException]::new(
                    'This function can only be called inside a New-ILDelegate block.'),
                'OpCodeOutsideAssembler',
                [Management.Automation.ErrorCategory]::NotImplemented,
                $null));
    }
}

$shortIndex = {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory, Position = 0)]
        [sbyte] $index
    )
    end {
        $PSCmdlet.ThrowTerminatingError(
            [Management.Automation.ErrorRecord]::new(
                [Management.Automation.PSNotSupportedException]::new(
                    'This function can only be called inside a New-ILDelegate block.'),
                'OpCodeOutsideAssembler',
                [Management.Automation.ErrorCategory]::NotImplemented,
                $null));
    }
}

$branchOpCodes =
    'beq', 'bge', 'bge.un', 'bgt', 'bgt.un', 'ble', 'ble.un', 'blt', 'blt.un',
    'bne.un', 'br', 'brfalse', 'brtrue', 'leave', 'beq.s', 'bge.s', 'bge.un.s',
    'bgt.s', 'bgt.un.s', 'ble.s', 'ble.un.s', 'blt.s', 'blt.un.s', 'bne.un.s',
    'brfalse.s', 'brtrue.s', 'br.s', 'leave.s'

foreach ($opCode in $branchOpCodes) {
    & $safeSetContent "$functionDrive::$opCode" -Value $branch -ErrorAction Stop
}

Microsoft.PowerShell.Core\Export-ModuleMember -Function $branchOpCodes

$signatureOpCodes =
    'ldfld', 'ldflda', 'ldsfld', 'ldsflda', 'stfld', 'stsfld', 'call',
    'callvirt', 'jmp', 'ldftn', 'ldvirtftn', 'newobj', 'calli', 'ldtoken',
    'box', 'castclass', 'constrained.', 'cpobj', 'initobj', 'isinst', 'ldelem',
    'ldelema', 'ldobj', 'mkrefany', 'newarr', 'refanyval', 'sizeof', 'stelem',
    'stobj', 'unbox', 'unbox.any'

foreach ($opCode in $signatureOpCodes) {
    & $safeSetContent "$functionDrive::$opCode" -Value $signature -ErrorAction Stop
}

Microsoft.PowerShell.Core\Export-ModuleMember -Function $signatureOpCodes

$noOperandOpCodes =
    'add', 'add.ovf', 'add.ovf.un', 'and', 'arglist', 'break', 'ceq', 'cgt',
    'cgt.un', 'ckfinite', 'clt', 'clt.un', 'conv.i', 'conv.i1', 'conv.i2',
    'conv.i4', 'conv.i8', 'conv.ovf.i', 'conv.ovf.i1', 'conv.ovf.i1.un',
    'conv.ovf.i2', 'conv.ovf.i2.un', 'conv.ovf.i4', 'conv.ovf.i4.un', 'conv.ovf.i8',
    'conv.ovf.i8.un', 'conv.ovf.i.un', 'conv.ovf.u', 'conv.ovf.u1', 'conv.ovf.u1.un',
    'conv.ovf.u2', 'conv.ovf.u2.un', 'conv.ovf.u4', 'conv.ovf.u4.un', 'conv.ovf.u8',
    'conv.ovf.u8.un', 'conv.ovf.u.un', 'conv.r4', 'conv.r8', 'conv.r.un', 'conv.u',
    'conv.u1', 'conv.u2', 'conv.u4', 'conv.u8', 'cpblk', 'div', 'div.un', 'dup',
    'endfilter', 'endfinally', 'initblk', 'ldarg.0', 'ldarg.1', 'ldarg.2', 'ldarg.3',
    'ldc.i4.0', 'ldc.i4.1', 'ldc.i4.2', 'ldc.i4.3', 'ldc.i4.4', 'ldc.i4.5', 'ldc.i4.6',
    'ldc.i4.7', 'ldc.i4.8', 'ldc.i4.m1', 'ldelem.i', 'ldelem.i1', 'ldelem.i2', 'ldelem.i4',
    'ldelem.i8', 'ldelem.r4', 'ldelem.r8', 'ldelem.ref', 'ldelem.u1', 'ldelem.u2', 'ldelem.u4',
    'ldind.i', 'ldind.i1', 'ldind.i2', 'ldind.i4', 'ldind.i8', 'ldind.r4', 'ldind.r8',
    'ldind.ref', 'ldind.u1', 'ldind.u2', 'ldind.u4', 'ldlen', 'ldloc.0', 'ldloc.1',
    'ldloc.2', 'ldloc.3', 'ldnull', 'localloc', 'mul', 'mul.ovf', 'mul.ovf.un', 'neg',
    'nop', 'not', 'or', 'pop', 'prefix1', 'prefix2', 'prefix3', 'prefix4', 'prefix5',
    'prefix6', 'prefix7', 'prefixref', 'readonly.', 'refanytype', 'rem', 'rem.un', 'ret',
    'rethrow', 'shl', 'shr', 'shr.un', 'stelem.i', 'stelem.i1', 'stelem.i2', 'stelem.i4',
    'stelem.i8', 'stelem.r4', 'stelem.r8', 'stelem.ref', 'stind.i', 'stind.i1', 'stind.i2',
    'stind.i4', 'stind.i8', 'stind.r4', 'stind.r8', 'stind.ref', 'stloc.0', 'stloc.1',
    'stloc.2', 'stloc.3', 'sub', 'sub.ovf', 'sub.ovf.un', 'tail.', 'throw', 'volatile.', 'xor'

foreach ($opCode in $noOperandOpCodes) {
    & $safeSetContent "$functionDrive::$opCode" -Value $noOperand -ErrorAction Stop
}

Microsoft.PowerShell.Core\Export-ModuleMember -Function $noOperandOpCodes

$indexOpCodes = 'ldarg', 'ldarga', 'ldloc', 'ldloca', 'starg', 'stloc'
foreach ($opCode in $indexOpCodes) {
    & $safeSetContent "$functionDrive::$opCode" -Value $index -ErrorAction Stop
}

Microsoft.PowerShell.Core\Export-ModuleMember -Function $indexOpCodes

$shortIndexOpCodes = 'ldarga.s', 'ldarg.s', 'ldloca.s', 'ldloc.s', 'starg.s', 'stloc.s'
foreach ($opCode in $shortIndexOpCodes) {
    & $safeSetContent "$functionDrive::$opCode" -Value $shortIndex -ErrorAction Stop
}

Microsoft.PowerShell.Core\Export-ModuleMember -Function $shortIndexOpCodes

$variableOpCodes = 'ldloc.auto', 'ldloca.auto', 'stloc.auto'
foreach ($opCode in $variableOpCodes) {
    & $safeSetContent "$functionDrive::$opCode" -Value $variable -ErrorAction Stop
}

Microsoft.PowerShell.Core\Export-ModuleMember -Function $variableOpCodes

Microsoft.PowerShell.Core\Export-ModuleMember -Alias il -Cmdlet New-ILDelegate
