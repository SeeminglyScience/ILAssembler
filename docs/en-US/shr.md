---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.shr
schema: 2.0.0
---

# shr

## SYNOPSIS

Shifts an integer value (in sign) to the right by a specified number of bits, pushing the result onto the evaluation stack.

## SYNTAX

```powershell
shr
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| 63     | shr             |

 The stack transitional behavior, in sequential order, is:

1.  A value is pushed onto the stack.

2.  The amount of bits to be shifted is pushed onto the stack.

3.  The number of bits to be shifted and the value are popped from the stack; the value is shifted right by the specified number of bits.

4.  The result is pushed onto the stack.

 The `shr.un` instruction shifts the value (type `int32`, `int64` or `native int`) right by the specified number of bits. The number of bits is a value of type `int32` or `native int`. The return value is unspecified if the number of bits to be shifted is greater than or equal to the width (in bits) of the supplied value.

 `shr` replicates the high order bit on each shift, preserving the sign of the original value in the `result`.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
