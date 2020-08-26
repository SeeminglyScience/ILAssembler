---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldc_r4
schema: 2.0.0
---

# ldc.r4

## SYNOPSIS

Pushes a supplied value of type `float32` onto the evaluation stack as type `F` (float).

## SYNTAX

```powershell
ldc.r4
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format           | Assembly Format |
| ---------------- | --------------- |
| 22 < `float32` > | ldc.r4 `num`    |

 The stack transitional behavior, in sequential order, is:

1.  The value `num` is pushed onto the stack.

 This encoding pushes a `float32` value onto the stack.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
