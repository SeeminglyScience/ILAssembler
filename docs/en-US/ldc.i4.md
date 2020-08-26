---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldc_i4
schema: 2.0.0
---

# ldc.i4

## SYNOPSIS

Pushes a supplied value of type `int32` onto the evaluation stack as an `int32`.

## SYNTAX

```powershell
ldc.i4
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format         | Assembly Format |
| -------------- | --------------- |
| 20 < `int32` > | ldc.i4 `num`    |

 The stack transitional behavior, in sequential order, is:

1.  The value `num` is pushed onto the stack.

 Note that there are special short (and hence more efficient) encodings for the integers -128 through 127, and especially short encodings for -1 through 8. All short encodings push 4 byte integers on the stack. Longer encodings are used for 8 byte integers and 4 and 8 byte floating-point numbers, as well as 4-byte values that do not fit in the short forms. There are three ways to push an 8 byte integer constant onto the stack

 1. Use the `ldc.i8` instruction for constants that must be expressed in more than 32 bits.

 2. Use the `ldc.i4` instruction followed by a `conv.i8` for constants that require 9 to 32 bits.

 3. Use a short form instruction followed by a `conv.i8` for constants that can be expressed in 8 or fewer bits.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
