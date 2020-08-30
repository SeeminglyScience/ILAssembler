---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.leave_s
schema: 2.0.0
---

# leave.s

## SYNOPSIS

Exits a protected region of code, unconditionally transferring control to a target instruction (short form).

## SYNTAX

```powershell
leave.s <branch_name>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format        | Assembly Format  |
| ------------- | ---------------- |
| DE < `int8` > | leave.s `target` |

 There is no stack transition behavior specified for this instruction.

 The `leave.s` instruction unconditionally transfers control to the passed target instruction, represented as a 1-byte signed offset from the beginning of the instruction following the current instruction.

 The `leave.s` instruction is similar to the `br` instruction, but it can be used to exit a `try`, `filter`, or `catch` block whereas the ordinary branch instructions can only be used in such a block to transfer control within it. The `leave.s` instruction empties the evaluation stack and ensures that the appropriate surrounding `finally` blocks are executed.

 You cannot use a `leave.s` instruction to exit a `finally` block. To ease code generation for exception handlers it is valid from within a catch block to use a `leave.s` instruction to transfer control to any instruction within the associated `try` block.

 If an instruction has one or more prefix codes, control can only be transferred to the first of these prefixes.

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
