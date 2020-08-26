---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldind_i1
schema: 2.0.0
---

# ldind.i1

## SYNOPSIS

Loads a value of type `int8` as an `int32` onto the evaluation stack indirectly.

## SYNTAX

```powershell
ldind.i1
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| 46     | ldind.i1        |

 The stack transitional behavior, in sequential order, is:

1.  An address is pushed onto the stack.

2.  The address is popped from the stack; the value located at the address is fetched.

3.  The fetched value is pushed onto the stack.

 The `ldind.i1` instruction indirectly loads an `int8` value from the specified address (of type `native int`, `&`, or *) onto the stack as an `int32`.

 All of the `ldind` instructions are shortcuts for a `ldobj` instruction that specifies the corresponding built-in value class.

 Note that integer values of less than 4 bytes are extended to `int32` (not `native int`) when they are loaded onto the evaluation stack. Floating-point values are converted to `F` type when loaded onto the evaluation stack.

 Correctly-formed Microsoft Intermediate Language (MSIL) ensures that the `ldind` instructions are used in a manner consistent with the type of the pointer.

 The address initially pushed onto the stack must be aligned to the natural size of objects on the machine or a `System.NullReferenceException` can occur (see the `unaligned.` prefix instruction for preventative measures). The results of all MSIL instructions that return addresses (for example, `ldloca` and `ldarga`) are safely aligned. For datatypes larger than 1 byte, the byte ordering is dependent on the target CPU. Code that depends on byte ordering might not run on all platforms.

 `System.NullReferenceException` can be thrown if an invalid address is detected.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
