---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.throw
schema: 2.0.0
---

# throw

## SYNOPSIS

Throws the exception object currently on the evaluation stack.

## SYNTAX

```powershell
throw
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| 7A     | throw           |

 The stack transitional behavior, in sequential order, is:

1.  An object reference (to an exception) is pushed onto the stack.

2.  The object reference is popped from the stack and the exception thrown.

 The `throw` instruction throws the exception object (type `O`) currently on the stack.

 `System.NullReferenceException` is thrown if the object reference is a null reference.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
