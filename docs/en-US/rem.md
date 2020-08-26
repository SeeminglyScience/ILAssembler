---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.rem
schema: 2.0.0
---

# rem

## SYNOPSIS

Divides two values and pushes the remainder onto the evaluation stack.

## SYNTAX

```powershell
rem
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| 5D     | rem             |


 The stack transitional behavior, in sequential order, is:

1.  A `value1` is pushed onto the stack.

2.  `value2` is pushed onto the stack.

3.  `value2` and `value1` are popped from the stack and the remainder of `value1` `div` `value2` computed.

4.  The result is pushed onto the stack.

 `result` = `value1` `rem` `value2` satisfies the following conditions:

 `result` = `value1` - `value2` `×` (`value1` `div` `value2`), and:

 0 = | `result` | < | `value2` |, sign(`result`) = sign(`value1`), where `div` is the division instruction that truncates towards zero.

 If `value2` is zero or `value1` is infinity the result is NaN. If `value2` is infinity, the result is `value1` (negated for `-infinity`).

 Integral operations throw `System.DivideByZeroException` if `value2` is zero.

 Note that on the Intel-based platforms an `System.OverflowException` is thrown when computing (minint `rem` -1).

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
