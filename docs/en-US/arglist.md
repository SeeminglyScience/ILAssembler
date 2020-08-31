---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.arglist
schema: 2.0.0
---

# arglist

## SYNOPSIS

Returns an unmanaged pointer to the argument list of the current method.

## SYNTAX

```powershell
arglist
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| FE 00  | arglist         |

 No evaluation stack behaviors are performed by this operation.

 The `arglist` instruction returns an opaque handle (an unmanaged pointer, of type `native int`) that represents the argument list of the current method. This handle is valid only during the lifetime of the current method. You can, however, pass the handle to other methods as long as the current method is on the thread of control. You can only execute the `arglist` instruction within a method that takes a variable number of arguments.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
