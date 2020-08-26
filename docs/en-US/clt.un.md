---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.clt_un
schema: 2.0.0
---

# clt.un

## SYNOPSIS

Compares the unsigned or unordered values `value1` and `value2`. If `value1` is less than `value2`, then the integer value 1 (`int32`) is pushed onto the evaluation stack; otherwise 0 (`int32`) is pushed onto the evaluation stack.

## SYNTAX

```powershell
clt.un
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| FE 05  | clt.un          |

 The stack transitional behavior, in sequential order, is:

1.  `value1` is pushed onto the stack.

2.  `value2` is pushed onto the stack.

3.  `value2` and `value1` are popped from the stack; `clt.un` tests if `value1` is less than `value2`.

4.  If `value1` is less than `value2`, 1 is pushed onto the stack; otherwise 0 is pushed onto the stack.

 The `clt.un` instruction compares `value1` and `value2`. An `int32` value of 1 is pushed on the stack if any of the following is true:

-   `value1` is strictly less than `value2` (as for `clt`).

-   For floating-point numbers, `value1` is not ordered with respect to `value2`.

-   For integer values, `value1` is strictly less than `value2` when considered as unsigned numbers.

 Otherwise, an `int32` value of 0 is pushed on the stack.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
