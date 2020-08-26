---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stobj
schema: 2.0.0
---

# stobj

## SYNOPSIS

Copies a value of a specified type from the evaluation stack into a supplied memory address.

## SYNTAX

```powershell
stobj
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format     | Assembly Format |
| ---------- | --------------- |
| 81 < `T` > | stobj `class`   |

 The stack transitional behavior, in sequential order, is:

1.  An address is pushed onto the stack.

2.  A value type object of type `class` is pushed onto the stack.

3.  The object and the address are popped from the stack; the value type object is stored at the address.

 The `stobj` instruction copies the value type object into the address specified by the address (a pointer of type `native int`, `*`, or `&`). The number of bytes copied depends on the size of the class represented by `class`, a metadata token representing a value type.

 The operation of the `stobj` instruction can be altered by an immediately preceding `volatile.` or `unaligned.` prefix instruction.

 `System.TypeLoadException` is thrown if class cannot be found. This is typically detected when Microsoft Intermediate Language (MSIL) instructions are converted to native code rather than at run time.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
