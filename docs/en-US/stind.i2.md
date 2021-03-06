---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stind_i2
schema: 2.0.0
---

# stind.i2

## SYNOPSIS

Stores a value of type `int16` at a supplied address.

## SYNTAX

```powershell
stind.i2
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| 53     | stind.i2        |

 The stack transitional behavior, in sequential order, is:

1.  An address is pushed onto the stack.

2.  A value is pushed onto the stack.

3.  The value and the address are popped from the stack; the value is stored at the address.

 The `stind.i2` instruction stores an `int16` value at the supplied address (type `native int`, `*`, or `&`).

 Type safe operation requires that the `stind.2i` instruction be used in a manner consistent with the type of the pointer. The operation of the `stind.i2` instruction can be altered by an immediately preceding `volatile.` or `unaligned.` prefix instruction.

 `System.NullReferenceException` is thrown if `addr` is not naturally aligned for the argument type implied by the instruction suffix.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
