---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ckfinite
schema: 2.0.0
---

# ckfinite

## SYNOPSIS

Throws `System.ArithmeticException` if value is not a finite number.

## SYNTAX

```powershell
ckfinite
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| C3     | ckfinite        |

 The stack transitional behavior, in sequential order, is:

1.  `value` is pushed onto the stack.

2.  `value` is popped from the stack and the `ckfinite` instruction is performed on it.

3.  `value` is pushed back onto the stack if no exception is thrown.

 The `ckfinite instruction` throws `System.ArithmeticException` if `value` (a floating-point number) is either a "not a number" value (NaN) or a `+-` infinity value. `Ckfinite` leaves the value on the stack if no exception is thrown. Execution is unspecified if `value` is not a floating-point number.

 `System.ArithmeticException` is thrown if `value` is not a 'normal' number.

 Note that a special exception or a derived class of `System.ArithmeticException` may be more appropriate, passing the incorrect value to the exception handler.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
