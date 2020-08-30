---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.bgt_un
schema: 2.0.0
---

# bgt.un

## SYNOPSIS

Transfers control to a target instruction if the first value is greater than the second value, when comparing unsigned integer values or unordered float values.

## SYNTAX

```powershell
bgt.un <branch_name>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format         | Assembly Format |
| -------------- | --------------- |
| 42 < `int32` > | bgt.un `target` |

 The stack transitional behavior, in sequential order, is:

1.  `value1` is pushed onto the stack.

2.  `value2` is pushed onto the stack.

3.  `value2` and `value1` are popped from the stack; if `value1` is greater than `value2`, the branch operation is performed.

 The `bgt.un` instruction transfers control to the specified target instruction if `value1` is greater than `value2`, when compared using unsigned integer or unordered float values. The effect is identical to performing a `cgt.un` instruction followed by a `brtrue` branch to the specific target instruction. The target instruction is represented as a 4-byte signed offset from the beginning of the instruction following the current instruction.

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
