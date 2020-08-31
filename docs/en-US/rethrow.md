---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.rethrow
schema: 2.0.0
---

# rethrow

## SYNOPSIS

Rethrows the current exception.

## SYNTAX

```powershell
rethrow
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| FE 1A  | rethrow         |

 No stack transition behavior is defined for this instruction.

 The `rethrow` instruction is only permitted within the body of a `catch` handler. It throws the same exception that was caught by this handler.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
