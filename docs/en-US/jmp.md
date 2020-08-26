---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.jmp
schema: 2.0.0
---

# jmp

## SYNOPSIS

Exits current method and jumps to specified method.

## SYNTAX

```powershell
jmp
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format     | Assembly Format |
| ---------- | --------------- |
| 27 < `T` > | jmp `method`    |

 There are no stack transition behaviors for this instruction.

 The `jmp` (jump) instruction transfers control to the method specified by `method`, which is a metadata token for a method reference. The current arguments are transferred to the destination method.

 The evaluation stack must be empty when this instruction is executed. The calling convention, number and type of arguments at the destination address must match that of the current method.

 The `jmp` instruction cannot be used to transferred control out of a `try`, `filter`, `catch`, or `finally` block.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
