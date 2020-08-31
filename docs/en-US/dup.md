---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.dup
schema: 2.0.0
---

# dup

## SYNOPSIS

Copies the current topmost value on the evaluation stack, and then pushes the copy onto the evaluation stack.

## SYNTAX

```powershell
dup
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| 25     | dup             |

 The stack transitional behavior, in sequential order, is:

1.  `value` is pushed onto the stack.

2.  `value` is popped off of the stack for duplication.

3.  `value` is pushed back onto the stack.

4.  A duplicate value is pushed onto the stack.

 The `dup` instruction duplicates the top element of the stack, and leaves two identical values atop it.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
