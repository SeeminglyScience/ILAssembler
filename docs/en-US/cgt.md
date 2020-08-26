---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.cgt
schema: 2.0.0
---

# cgt

## SYNOPSIS

Compares two values. If the first value is greater than the second, the integer value `1` (`int32`) is pushed onto the evaluation stack; otherwise `0` (`int32`) is pushed onto the evaluation stack.

## SYNTAX

```powershell
cgt
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| FE 02  | cgt             |

 The stack transitional behavior, in sequential order, is:

1.  `value1` is pushed onto the stack.

2.  `value2` is pushed onto the stack.

3.  `value2` and `value1` are popped from the stack; `cgt` tests if `value1` is greater than `value2`.

4.  If `value1` is greater than `value2`, 1 is pushed onto the stack; otherwise 0 is pushed onto the stack.

 The `cgt` instruction compares `value1` and `value2`. If `value1` is strictly greater than `value2`, then an `int32` value of 1 is pushed on the stack. Otherwise, an `int32` value of 0 is pushed on the stack.

-   For floating-point numbers, `cgt` returns 0 if the numbers are unordered (that is, if one or both of the arguments are NaN).

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
