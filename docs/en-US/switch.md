---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.switch
schema: 2.0.0
---

# switch

## SYNOPSIS

Implements a jump table.

## SYNTAX

```powershell
switch <branch_name[]>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format                                             | Assembly Format                  |
| -------------------------------------------------- | -------------------------------- |
| 45 < `unsigned int32` > < `int32` >... < `int32` > | switch (`N`, `t1`, `t2`... `tN`) |

 The stack transitional behavior, in sequential order, is:

1.  A value is pushed onto the stack.

2.  The value is popped off the stack and execution is transferred to the instruction at the offset indexed by the value, where the value is less than `N`.

 The `switch` instruction implements a jump table. The format of the instruction is an `unsigned int32` representing the number of targets `N`, followed by `N` int32 values specifying jump targets. These targets are represented as offsets (positive or negative) from the beginning of the instruction following this `switch` instruction.

 The `switch` instruction pops a value off the stack and compares it, as an unsigned integer, to `N`. If value is less than `N`, execution is transferred to the target indexed by value, where targets are numbered from 0 (for example, a value of 0 takes the first target, a value of 1 takes the second target, and so on). If the value is greater than or equal to `N`, execution continues at the next instruction (fall through).

 If the target instruction has one or more prefix codes, control can only be transferred to the first of these prefixes.

 Control transfers into and out of `try`, `catch`, `filter`, and `finally` blocks cannot be performed by this instruction. (Such transfers are severely restricted and must use the leave instruction instead).

## PARAMETERS

### -branchs

Specifies a list of labels to transfer execution to should their index match the value popped off the stack.

```yaml
Type: branch_name[]
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
