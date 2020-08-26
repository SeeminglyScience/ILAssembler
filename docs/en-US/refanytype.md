---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.refanytype
schema: 2.0.0
---

# refanytype

## SYNOPSIS

Retrieves the type token embedded in a typed reference.

## SYNTAX

```powershell
refanytype
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| FE 1D  | refanytype      |

 The stack transitional behavior, in sequential order, is:

1.  A value type reference is pushed onto the stack.

2.  The typed reference is popped from the stack and its corresponding type token retrieved.

3.  The type token is pushed onto the stack.

 A typed reference contains a type token and an address to an object instance.

 The `refanytype` instruction retrieves the type token embedded in the typed reference. See the `mkrefany` instruction for information on creating typed references.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
