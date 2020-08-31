---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.initblk
schema: 2.0.0
---

# initblk

## SYNOPSIS

Initializes a specified block of memory at a specific address to a given size and initial value.

## SYNTAX

```powershell
initblk
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| FE 18  | initblk         |

 The stack transitional behavior, in sequential order, is:

1.  A starting address is pushed onto the stack.

2.  An initialization value is pushed onto the stack.

3.  The number of bytes to initialize is pushed onto the stack.

4.  The number of bytes, the initialization value, and the starting address are popped from the stack, and the initialization is performed as per their values.

 The `initblk` instruction sets the number (`unsigned int32`) of bytes starting at the specified address (of type `native int`, `&`, or `*`) to the initialization value (of type `unsigned int8`). `initblk` assumes that the starting address is aligned to the natural size of the machine.

 The operation of the `initblk` instructions can be altered by an immediately preceding `volatile.` or `unaligned.` prefix instruction.

 `System.NullReferenceException` may be thrown if an invalid address is detected.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
