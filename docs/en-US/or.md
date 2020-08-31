---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.or
schema: 2.0.0
---

# or

## SYNOPSIS

Compute the bitwise complement of the two integer values on top of the stack and pushes the result onto the evaluation stack.

## SYNTAX

```powershell
or
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| 60     | or              |

 The stack transitional behavior, in sequential order, is:

1.  `value1` is pushed onto the stack.

2.  `value2` is pushed onto the stack.

3.  `value2` and `value1` are popped from the stack and their bitwise OR computed.

4.  The result is pushed onto the stack.

 The `or` instruction computes the bitwise OR of two values atop the stack, pushing the result onto the stack.

 `or` is an integer-specific operation.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
