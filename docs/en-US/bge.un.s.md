---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.bge_un_s
schema: 2.0.0
---

# bge.un.s

## SYNOPSIS

Transfers control to a target instruction (short form) if the first value is greater than the second value, when comparing unsigned integer values or unordered float values.

## SYNTAX

```powershell
bge.un.s
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format        | Assembly Format   |
| ------------- | ----------------- |
| 34 < `int8` > | bge.un.s `target` |

 The stack transitional behavior, in sequential order, is:

1.  `value1` is pushed onto the stack.

2.  `value2` is pushed onto the stack.

3.  `value2` and `value1` are popped from the stack; if `value1` is greater than or equal to `value2`, the branch operation is performed.

 The `bge.un.s` instruction transfers control to the specified target instruction if `value1` is greater than or equal to `value2`, when compared using unsigned integer or unordered float values. The effect is identical to performing a `clt.un` instruction (`clt` for floats) followed by a `brfalse` branch to the specific target instruction. The target instruction is represented as a 1-byte signed offset from the beginning of the instruction following the current instruction.

 If the target instruction has one or more prefix codes, control can only be transferred to the first of these prefixes. Control transfers into and out of `try`, `catch`, `filter`, and `finally` blocks cannot be performed by this instruction.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
