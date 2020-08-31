---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.cpblk
schema: 2.0.0
---

# cpblk

## SYNOPSIS

Copies a specified number bytes from a source address to a destination address.

## SYNTAX

```powershell
cpblk
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| FE 17  | cpblk           |

 The stack transitional behavior, in sequential order, is:

1.  The destination address is pushed onto the stack.

2.  The source address is pushed onto the stack.

3.  The number of bytes to copy is pushed onto the stack.

4.  The number of bytes, the source address, and the destination address are popped from the stack; the specified number of bytes are copied from the source address to the destination address.

 The `cpblk` instruction copies a number (type `unsigned int32`) of bytes from a source address (of type `*`, `native int`, or `&`) to a destination address (of type `*`, `native int`, or `&`). The behavior of `cpblk` is unspecified if the source and destination areas overlap.

 `cpblk` assumes that both the source and destination addressed are aligned to the natural size of the machine. The `cpblk` instruction can be immediately preceded by the `unaligned.<prefix>` instruction to indicate that either the source or the destination is unaligned.

 The operation of the `cpblk` instruction can be altered by an immediately preceding `unaligned.` prefix instruction.

 `System.NullReferenceException` may be thrown if an invalid address is detected.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
