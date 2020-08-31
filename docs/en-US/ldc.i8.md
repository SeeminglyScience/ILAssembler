---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldc_i8
schema: 2.0.0
---

# ldc.i8

## SYNOPSIS

Pushes a supplied value of type `int64` onto the evaluation stack as an `int64`.

## SYNTAX

```powershell
ldc.i8 <int64>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format         | Assembly Format |
| -------------- | --------------- |
| 21 < `int64` > | ldc.i8 `num`    |

 The stack transitional behavior, in sequential order, is:

1.  The value `num` is pushed onto the stack.

 This encoding pushes an `int64` value onto the stack.

## PARAMETERS

### -value

Specifies the constant to push onto the stack.

```yaml
Type: int64
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
