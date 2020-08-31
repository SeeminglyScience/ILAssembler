---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_r_un
schema: 2.0.0
---

# conv.r.un

## SYNOPSIS

Converts the unsigned integer value on top of the evaluation stack to `float32`.

## SYNTAX

```powershell
conv.r.un
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| 76     | conv.r.un       |

 The stack transitional behavior, in sequential order, is:

1.  `value` is pushed onto the stack.

2.  `value` is popped from the stack and the conversion operation is attempted.

3.  If the conversion is successful, the resulting value is pushed onto the stack.

 The `conv.r.un` opcode converts the `value` on top of the stack to the type specified in the opcode, and leave that converted value on the top of the stack. Integer values of less than 4 bytes are extended to `int32` when they are loaded onto the evaluation stack (unless `conv.i` or `conv.u` is used, in which case the result is also `native int`). Floating-point values are converted to the `F` type.

 Conversion from floating-point numbers to integer values truncates the number toward zero. When converting from a `float64` to a `float32`, precision can be lost. If `value` is too large to fit in a `float32 (F)`, positive infinity (if `value` is positive) or negative infinity (if `value` is negative) is returned. If overflow occurs converting one integer type to another, the high order bits are truncated. If the result is smaller than an `int32`, the value is sign-extended to fill the slot.

 If overflow occurs converting a floating-point type to an integer the `result` returned is unspecified. The `conv.r.un` operation takes an integer off the stack, interprets it as unsigned, and replaces it with a floating-point number to represent the integer: either a `float32`, if this is wide enough to represent the integer without loss of precision, or else a `float64`.

 No exceptions are ever thrown when using this field.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
