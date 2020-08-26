---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.add_ovf_un
schema: 2.0.0
---

# add.ovf.un

## SYNOPSIS

Adds two unsigned integer values, performs an overflow check, and pushes the result onto the evaluation stack.

## SYNTAX

```powershell
add.ovf.un
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| D7     | add.ovf.un      |

 The stack transitional behavior, in sequential order, is:

1.  `value1` is pushed onto the stack.

2.  `value2` is pushed onto the stack.

3.  `value2` and `value1` are popped from the stack; `value1` is added to `value2` with a check for overflow.

4.  The result is pushed onto the stack.

 `System.OverflowException` is thrown if the result is not represented in the result type.

 You can perform this operation on signed integers. For floating-point values, use `add`.

 The acceptable operand types and their corresponding result data type are listed in the table below. If there is no entry for a particular type combination (for example, `int32` and `float`; `int32` and `int64`), it is an invalid Microsoft Intermediate Language (MSIL) instruction and generates an error.

| operand | value1 type  | value2 type  | result type  |
| ------- | ------------ | ------------ | ------------ |
| add     | `int32`      | `int32`      | `int32`      |
| add     | `int32`      | `native int` | `native int` |
| add     | `int32`      | `&`          | `&`          |
| add     | `int32`      | `*`          | `*`          |
| add     | `int64`      | `int64`      | `int64`      |
| add     | `native int` | `int32`      | `native int` |
| add     | `native int` | `native int` | `native int` |
| add     | `native int` | `&`          | `&`          |
| add     | `native int` | `*`          | `*`          |
| add     | `F`          | `F`          | `F`          |
| add     | `&`          | `int32`      | `&`          |
| add     | `&`          | `native int` | `&`          |
| add     | `*`          | `int32`      | `*`          |
| add     | `*`          | `native int` | `*`          |

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
