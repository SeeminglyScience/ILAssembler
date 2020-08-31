---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.rem_un
schema: 2.0.0
---

# rem.un

## SYNOPSIS

Divides two unsigned values and pushes the remainder onto the evaluation stack.

## SYNTAX

```powershell
rem.un
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| 5E     | rem.un          |

 The stack transitional behavior, in sequential order, is:

1.  `value1` is pushed onto the stack.

2.  `value2` is pushed onto the stack.

3.  `value2` and `value1` are popped from the stack and the remainder of `value1` `div` `value2` computed.

4.  The result is pushed onto the stack.

 `result` = `value1` `rem.un` `value2` satisfies the following conditions:

 `result` = `value1` - `value2` x(`value1` `div.un` `value2`), and:

 0 = `result` < `value2`, where `div.un` is the unsigned division instruction.

 The `rem.un` instruction computes `result` and pushes it on the stack. `Rem.un` treats its arguments as unsigned integers, while `rem` treats them as signed integers.

 `rem.un` is unspecified for floating-point numbers.

 Integral operations throw `System.DivideByZeroException` if `value2` is zero.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
