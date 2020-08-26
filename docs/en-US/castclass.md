---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.castclass
schema: 2.0.0
---

# castclass

## SYNOPSIS

Attempts to cast an object passed by reference to the specified class.

## SYNTAX

```powershell
castclass
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format     | Assembly Format   |
| ---------- | ----------------- |
| 74 < `T` > | castclass `class` |

 The stack transitional behavior, in sequential order, is:

1.  An object reference is pushed onto the stack.

2.  The object reference is popped from the stack; the referenced object is cast as the specified `class`.

3.  If successful, a new object reference is pushed onto the stack.

 The `castclass` instruction attempts to cast the object reference (type `O`) atop the stack to a specified class. The new class is specified by a metadata token indicating the desired class. If the class of the object on the top of the stack does not implement the new class (assuming the new class is an interface) and is not a derived class of the new class then an `System.InvalidCastException` is thrown. If the object reference is a null reference, `castclass` succeeds and returns the new object as a null reference.

 `System.InvalidCastException` is thrown if obj cannot be cast to class.

 `System.TypeLoadException` is thrown if class cannot be found. This is typically detected when a Microsoft Intermediate Language (MSIL) instruction is converted to native code rather than at runtime.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
