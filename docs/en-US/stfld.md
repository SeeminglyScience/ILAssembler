---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stfld
schema: 2.0.0
---

# stfld

## SYNOPSIS

Replaces the value stored in the field of an object reference or pointer with a new value.

## SYNTAX

```powershell
stfld
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format     | Assembly Format |
| ---------- | --------------- |
| 7D < `T` > | stfld `field`   |

 The stack transitional behavior, in sequential order, is:

1.  An object reference or pointer is pushed onto the stack.

2.  A value is pushed onto the stack.

3.  The value and the object reference/pointer are popped from the stack; the value of `field` in the object is replaced with the supplied value.

 The `stfld` instruction replaces the value of a field of an object (type `O`) or via a pointer (type `native int`, `&`, or `*`) with a given value. `Field` is a metadata token that refers to a field member reference. The `stfld` instruction can have a prefix of either or both of `unaligned.` and `volatile.`.

 `System.NullReferenceException` is thrown if the object reference or pointer is a null reference and the field isn't static.

 `System.MissingFieldException` is thrown if `field` is not found in the metadata. This is typically checked when the Microsoft Intermediate Language (MSIL) instruction is converted to native code, not at runtime.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
