---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_ovf_u2_un
schema: 2.0.0
---

# conv.ovf.u2.un

## SYNOPSIS

Converts the unsigned value on top of the evaluation stack to `unsigned int16` and extends it to `int32`, throwing `System.OverflowException` on overflow.

## SYNTAX

```powershell
conv.ovf.u2.un
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| 87     | conv.ovf.u2.un  |

 The stack transitional behavior, in sequential order, is:

1.  `value` is pushed onto the stack.

2.  `value` is popped from the stack and the conversion operation is attempted. If overflow occurs, an exception is thrown.

3.  If the conversion is successful, the resulting value is pushed onto the stack.

 The `conv.ovf.u2.un` opcode converts the `value` on top of the stack to the type specified in the opcode, and places that converted value on the top of the stack. If the value is too large or too small to be represented by the target type, an exception is thrown.

 Conversions from floating-point numbers to integer values truncate the number toward zero. Note that integer values of less than 4 bytes are extended to `int32` when they are loaded onto the evaluation stack (unless `conv.ovf.i` or `conv.ovf.u` are used, in which case the result is also `native int`).

 `System.OverflowException` is thrown if the result can not be represented in the result type.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
