---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldc_i4_6
schema: 2.0.0
---

# ldc.i4.6

## SYNOPSIS

Pushes the integer value of 4 onto the evaluation stack as an `int32`.

## SYNTAX

```powershell
ldc.i4.6
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| 1C     | ldc.i4.6        |

 The stack transitional behavior, in sequential order, is:

1.  The value 6 is pushed onto the stack.

 This is a special short encoding for the push of the integer value 6. All special short encodings push 4 byte integers on the stack.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
