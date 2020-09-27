---
external help file: ILAssembler.dll-Help.xml
Module Name: ILAssembler
online version: https://github.com/SeeminglyScience/ILAssembler/blob/master/docs/en-US/New-ILDelegate.md
schema: 2.0.0
---

# New-IlDelegate

## SYNOPSIS

The New-IlDelegate command creates a invokable delegate using from the provided common intermediate language (CIL) instructions.

## SYNTAX

### ByAstSignature (Default)

```powershell
New-IlDelegate [-Signature] <ScriptBlock> [-Body] <ScriptBlock> [<CommonParameters>]
```

### ByDelegateSignature

```powershell
New-IlDelegate [-DelegateType] <Type> [-Body] <ScriptBlock> [<CommonParameters>]
```

## DESCRIPTION

Provides an entry point for a domain specific language (DSL) resembling that of ILAsm.exe. When invoked, the `ScriptBlock` provided by the `Body` parameter is analyzed for matching instructions. If analysis is successful, the CIL is encoded into a `DynamicMethod` and a delegate is generated.

During the creation of the delegate, no PowerShell code is actually invoked. Instead, the abstract syntax tree is traversed statically.

## EXAMPLES

### -------------------------- EXAMPLE 1 --------------------------

```powershell
il { [void]([int]) } {
    ldarg.0
    call { [void] [Console]::Write([int]) }
    ret
}
```

Defines a delegate with a void return that takes an int as a parameter.

### -------------------------- EXAMPLE 2 --------------------------

```powershell
il { [int]@() } {
    ldc.i4.0
    ret
}
```

Defines a delegate that returns int and does not have any parameters.

### -------------------------- EXAMPLE 3 --------------------------

```powershell
il System.AsyncCallback {
    ldarg.0
    callvirt { [object] [IAsyncResult].get_AsyncState() }
    call { [void] [Console]::Write([object]) }
    ret
}
```

Defines a delegate of type `System.AsyncCallback`.

### -------------------------- EXAMPLE 4 --------------------------

```powershell
il { [string]([ref] [int] $sum, [int+] $intsPointer, [int] $intsCount) } {
    .locals init {
        [int] $i,
    }

    # i = 0
    ldc.i4.0
    stloc.0

    br.s condition
loop_start:
    # sum (ld location to eval stack for the stind.i4 later)
    ldarg.0

    # *sum
    ldarg.0
    ldind.i4

    # intsPointer[i]
    ldarg.1
    ldloc.0
    conv.i
    sizeof { [int] }
    mul
    add
    ldind.i4

    # *sum + intsPointer[i]
    add

    # *sum = *sum + intsPointer[i]
    stind.i4

    # i++
    ldloc.0
    ldc.i4.1
    add
    stloc.0

condition:
    # if (i < intsCount) goto loop_start
    ldloc.0
    ldarg.2
    blt.s loop_start

    # return sum.ToString()
    ldarg.0
    call { [string] [int].ToString() }
    ret
}
```

Defines a delegate that returns string and takes int&, int*, and int as parameters.

## PARAMETERS

### -Body

Specifies the block of CIL instructions to assemble.

```yaml
Type: ScriptBlock
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DelegateType

Specifies the delegate type to return. Use this parameter only if you need a delegate of a specific kind to pass to an existing .NET API.

```yaml
Type: Type
Parameter Sets: ByDelegateSignature
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Signature

Specifies an anonymous method signature block used to determine parameters and return type. This block may include any parameter types that are valid in a method signature. Types that are not valid as generic parameters (such as `byref`, `byreflike` or pointer types) will result in the generation of a suitable delegate type.

```yaml
Type: ScriptBlock
Parameter Sets: ByAstSignature
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters

This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

This command does not accept input from the pipeline.

## OUTPUTS

### System.Delegate

This command emits an invokable subclass of `System.Delegate` to the pipeline.

## NOTES

## RELATED LINKS
