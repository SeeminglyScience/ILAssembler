---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldc_i4_s
schema: 2.0.0
---

# ldc.i4.s

## SYNOPSIS

Pushes the supplied `int8` value onto the evaluation stack as an `int32`, short form.

## SYNTAX

```powershell
ldc.i4.s <byte>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format        | Assembly Format |
| ------------- | --------------- |
| 1F < `int8` > | ldc.i4.s `num`  |

 The stack transitional behavior, in sequential order, is:

1.  The value `num` is pushed onto the stack.

 `ldc.i4.s` is a more efficient encoding for pushing the integers from -128 to 127 onto the evaluation stack.

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
