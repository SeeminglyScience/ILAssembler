---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_u8
schema: 2.0.0
---

# conv.u8

## SYNOPSIS

Converts the value on top of the evaluation stack to `unsigned int64`, and extends it to `int64`.

## SYNTAX

```powershell
conv.u8
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| 6E     | conv.u8         |

 The stack transitional behavior, in sequential order, is:

1.  `value` is pushed onto the stack.

2.  `value` is popped from the stack and the conversion operation is attempted.

3.  If the conversion is successful, the resulting value is pushed onto the stack.

 The `conv.u8` opcode converts the `value` on top of the stack to the type specified in the opcode, and leave that converted value on the top of the stack. Integer values of less than 4 bytes are extended to `int32` when they are loaded onto the evaluation stack (unless `conv.i` or `conv.u` is used, in which case the result is also `native int`). Floating-point values are converted to the `F` type.

 Conversion from floating-point numbers to integer values truncates the number toward zero. When converting from an `float64` to an `float32`, precision can be lost. If `value` is too large to fit in a `float32 (F)`, positive infinity (if `value` is positive) or negative infinity (if `value` is negative) is returned. If overflow occurs converting one integer type to another, the high order bits are truncated. If the result is smaller than an `int32`, the value is sign-extended to fill the slot.

 If overflow occurs converting a floating-point type to an integer the value returned is unspecified.

 No exceptions are ever thrown when using this field. See `conv.ovf.i8` and `conv.ovf.i8.un` for equivalent instructions that will throw an exception when the result type can not properly represent the result value.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
