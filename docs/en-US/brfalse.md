---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.brfalse
schema: 2.0.0
---

# brfalse

## SYNOPSIS

Transfers control to a target instruction if `value` is `false`, a `null` reference (`Nothing` in Visual Basic), or zero.

## SYNTAX

```powershell
brfalse <branch_name>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format         | Assembly Format  |
| -------------- | ---------------- |
| 39 < `int32` > | brfalse `target` |

 The stack transitional behavior, in sequential order, is:

1.  `value` is pushed onto the stack by a previous operation.

2.  `value` is popped from the stack; if `value` is `false`, branch to `target`.

 The `brfalse` instruction (and its aliases `brnull` and `brzero`) transfers control to the specified target instruction if `value` (of type `int32`, `int64`, object reference `O`, managed pointer `&`, transient pointer `*`, `native int`) is zero (`false`). If `value` is non-zero (`true`) execution continues at the next instruction.

 The target instruction is represented as a 4-byte signed offset from the beginning of the instruction following the current instruction.

 If the target instruction has one or more prefix codes, control can only be transferred to the first of these prefixes. Control transfers into and out of `try`, `catch`, `filter`, and `finally` blocks cannot be performed by this instruction.

## PARAMETERS

### -branch

Specifies the label representing the target offset.

```yaml
Type: branch_name
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
