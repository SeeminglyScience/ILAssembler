---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.localloc
schema: 2.0.0
---

# localloc

## SYNOPSIS

Allocates a certain number of bytes from the local dynamic memory pool and pushes the address (a transient pointer, type `*`) of the first allocated byte onto the evaluation stack.

## SYNTAX

```powershell
localloc
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| FE 0F  | localloc        |

 The stack transitional behavior, in sequential order, is:

1.  The number of bytes to be allocated is pushed onto the stack.

2.  The number of bytes is popped from the stack; an amount of memory corresponding to the size is allocated from the local heap.

3.  A pointer to the first byte of the allocated memory is pushed onto the stack.

 The `localloc` instruction allocates `size` (type `natural unsigned int`) bytes from the local dynamic memory pool and returns the address (a transient pointer, type `*`) of the first allocated byte. The block of memory returned is initialized to 0 only if the initialize flag on the method is `true`. When the current method executes a `ret`, the local memory pool is made available for reuse.

 The resulting address is aligned so that any primitive data type can be stored there using the `stind` instructions (such as `stind.i4`) and loaded using the `ldind` instructions (such as `ldind.i4`).

 The `localloc` instruction cannot occur within a `filter`, `catch`, `finally`, or `fault` block.

 `System.StackOverflowException` is thrown if there is insufficient memory to service the request.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
