---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.brtrue_s
schema: 2.0.0
---

# brtrue.s

## SYNOPSIS

Transfers control to a target instruction (short form) if `value` is `true`, not `null`, or non-zero.

## SYNTAX

```powershell
brtrue.s
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format        | Assembly Format   |
| ------------- | ----------------- |
| 2D < `int8` > | brtrue.s `target` |

 The stack transitional behavior, in sequential order, is:

1.  `value` is pushed onto the stack by a previous operation.

2.  `value` is popped from the stack; if `value` is `true`, branch to `target`.

 The `brtrue.s` instruction transfers control to the specified target instruction if `value` (type `native int`) is nonzero (`true`). If `value` is zero (`false`) execution continues at the next instruction.

 If `value` is an object reference (type `O`) then `brinst` (an alias for `brtrue`) transfers control if it represents an instance of an object (for example, if it is not the null object reference; see `ldnull`).

 The target instruction is represented as a 1-byte signed offset from the beginning of the instruction following the current instruction.

 If the target instruction has one or more prefix codes, control can only be transferred to the first of these prefixes. Control transfers into and out of `try`, `catch`, `filter`, and `finally` blocks cannot be performed by this instruction.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
