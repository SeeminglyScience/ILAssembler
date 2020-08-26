---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.br_s
schema: 2.0.0
---

# br.s

## SYNOPSIS

Unconditionally transfers control to a target instruction (short form).

## SYNTAX

```powershell
br.s
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format        | Assembly Format |
| ------------- | --------------- |
| 2B < `int8` > | br.s `target`   |

 No evaluation stack behaviors are performed by this operation.

 The `br.s` instruction unconditionally transfers control to a target instruction. The target instruction is represented as a 1-byte signed offset from the beginning of the instruction following the current instruction.

 If the target instruction has one or more prefix codes, control can only be transferred to the first of these prefixes. Control transfers into and out of `try`, `catch`, `filter`, and `finally` blocks cannot be performed by this instruction.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
