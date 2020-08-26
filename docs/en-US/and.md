---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.and
schema: 2.0.0
---

# and

## SYNOPSIS

Computes the bitwise AND of two values and pushes the result onto the evaluation stack.

## SYNTAX

```powershell
and
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Instruction |
| ------ | ----------- |
| 5F     | and         |

 The stack transitional behavior, in sequential order, is:

1.  `value1` is pushed onto the stack.

2.  `value2` is pushed onto the stack.

3.  `value1` and `value2` are popped from the stack; the bitwise AND of the two values is computed.

4.  The result is pushed onto the stack.

 The `and` instruction computes the bitwise AND of the top two values on the stack and leaves the result on the stack.

 `and` is an integer-specific operation.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
