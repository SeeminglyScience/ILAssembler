---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.isinst
schema: 2.0.0
---

# isinst

## SYNOPSIS

Tests whether an object reference (type `O`) is an instance of a particular class.

## SYNTAX

```powershell
isinst
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format     | Assembly Format |
| ---------- | --------------- |
| 75 < `T` > | isinst `class`  |

 The stack transitional behavior, in sequential order, is:

1.  An object reference is pushed onto the stack.

2.  The object reference is popped from the stack and tested to see if it is an instance of the class passed in `class`.

3.  The result (either an object reference or a null reference) is pushed onto the stack.

 `class` is a metadata token indicating the desired class. If the class of the object on the top of the stack implements `class` (if `class` is an interface) or is a derived class of `class` (if `class` is a regular class) then it is cast to type `class` and the result is pushed on the stack, exactly as though `castclass` had been called. Otherwise, a null reference is pushed on the stack. If the object reference itself is a null reference, then `isinst` likewise returns a null reference.

 `System.TypeLoadException` is thrown if class cannot be found. This is typically detected when the Microsoft Intermediate Language (MSIL) instructions are converted to native code rather than at runtime.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
