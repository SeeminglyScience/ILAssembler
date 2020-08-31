---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.clt
schema: 2.0.0
---

# clt

## SYNOPSIS

Compares two values. If the first value is less than the second, the integer value 1 (`int32`) is pushed onto the evaluation stack; otherwise 0 (`int32`) is pushed onto the evaluation stack.

## SYNTAX

```powershell
clt
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| FE 04  | clt             |

 The stack transitional behavior, in sequential order, is:

1.  `value1` is pushed onto the stack.

2.  `value2` is pushed onto the stack.

3.  `value2` and `value1` are popped from the stack; `clt` tests if `value1` is less than `value2`.

4.  If `value1` is less than `value2`, 1 is pushed onto the stack; otherwise 0 is pushed onto the stack.

 The `clt` instruction compares `value1` and `value2`. If `value1` is strictly less than `value2`, then an `int32` value of 1 is pushed on the stack. Otherwise, an `int32` value of 0 is pushed on the stack.

-   For floating-point numbers, `clt` returns 0 if the numbers are unordered (that is, if one or both of the arguments are NaN).

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
