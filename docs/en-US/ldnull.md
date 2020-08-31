---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldnull
schema: 2.0.0
---

# ldnull

## SYNOPSIS

Pushes a null reference (type `O`) onto the evaluation stack.

## SYNTAX

```powershell
ldnull
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| 14     | ldnull          |

 The stack transitional behavior, in sequential order, is:

1.  A null object reference is pushed onto the stack.

 `ldnull` pushes a null reference (type `O`) on the stack. This is used to initialize locations before they are populated with data, or when they become deprecated.

 `ldnull` provides a null reference that is size-independent.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
