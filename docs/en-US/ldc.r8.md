---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldc_r8
schema: 2.0.0
---

# ldc.r8

## SYNOPSIS

Pushes a supplied value of type `float64` onto the evaluation stack as type `F` (float).

## SYNTAX

```powershell
ldc.r8 <double>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format           | Assembly Format |
| ---------------- | --------------- |
| 23 < `float64` > | ldc.r8 `num`    |

 The stack transitional behavior, in sequential order, is:

1.  The value `num` is pushed onto the stack.

 This encoding pushes a `float64` value onto the stack.

## PARAMETERS

### -value

Specifies the constant to push onto the stack.

```yaml
Type: branch_name
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
