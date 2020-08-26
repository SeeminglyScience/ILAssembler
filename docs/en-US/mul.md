---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.mul
schema: 2.0.0
---

# mul

## SYNOPSIS

Multiplies two values and pushes the result on the evaluation stack.

## SYNTAX

```powershell
mul
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| 5A     | mul             |

 The stack transitional behavior, in sequential order, is:

1.  `value1` is pushed onto the stack.

2.  `value2` is pushed onto the stack.

3.  `value2` and `value1` are popped from the stack; `value1` is multiplied by `value2`.

4.  The result is pushed onto the stack.

 The `mul` instruction multiplies `value1` by `value2` and pushes the result on the stack. Integer operations silently truncate the upper bits on overflow.

 See `mul.ovf` for an integer-specific multiply operation with overflow handling.

 For floating-point types, 0 * infinity = NaN.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
