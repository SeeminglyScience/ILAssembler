---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.bne_un_s
schema: 2.0.0
---

# bne.un.s

## SYNOPSIS

Transfers control to a target instruction (short form) when two unsigned integer values or unordered float values are not equal.

## SYNTAX

```powershell
bne.un.s
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format        | Assembly Format   |
| ------------- | ----------------- |
| 33 < `int8` > | bne.un.s `target` |

 The stack transitional behavior, in sequential order, is:

1.  `value1` is pushed onto the stack.

2.  `value2` is pushed onto the stack.

3.  `value2` and `value1` are popped from the stack; if `value1` is not equal to `value2`, the branch operation is performed.

 The `bne.un` instruction transfers control to the specified target instruction if `value1` is not equal to `value2`, when compared using unsigned integer or unordered float values. The effect is identical to performing a `ceq` instruction followed by a `brfalse` branch to the specific target instruction. The target instruction is represented as a 4-byte signed offset from the beginning of the instruction following the current instruction.

 If the target instruction has one or more prefix codes, control can only be transferred to the first of these prefixes. Control transfers into and out of `try`, `catch`, `filter`, and `finally` blocks cannot be performed by this instruction.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
