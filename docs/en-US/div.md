---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.div
schema: 2.0.0
---

# div

## SYNOPSIS

Divides two values and pushes the result as a floating-point (type `F`) or quotient (type `int32`) onto the evaluation stack.

## SYNTAX

```powershell
div
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| 5B     | div             |

 The stack transitional behavior, in sequential order, is:

1.  `value1` is pushed onto the stack.

2.  `value2` is pushed onto the stack.

3.  `value2` and `value1` are popped from the stack; `value1` is divided by `value2`.

4.  The result is pushed onto the stack.

 `result` = `value1` div value2 satisfies the following conditions:

 | `result` | = | `value1` | / | `value2` |, and:

 sign(`result`) = +, if sign(`value1`) = sign(`value2`), or -, if sign(`value1`) ~= sign(`value2`)

 The `div` instruction computes the result and pushes it on the stack.

 Integer division truncates towards zero.

 Division of a finite number by zero produces the correctly signed infinite value.

 Dividing zero by zero or infinity by infinity produces the NaN (Not-A-Number) value. Any number divided by infinity will produce a zero value.

 Integral operations throw `System.ArithmeticException` if the result cannot be represented in the result type. This can happen if `value1` is the maximum negative value, and `value2` is -1.

 Integral operations throw `System.DivideByZeroException` if `value2` is zero.

 Note that on Intel-based platforms an `System.OverflowException` is thrown when computing (minint div -1). Floating-point operations never throw an exception (they produce NaNs or infinities instead).

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
