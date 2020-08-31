---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.not
schema: 2.0.0
---

# not

## SYNOPSIS

Computes the bitwise complement of the integer value on top of the stack and pushes the result onto the evaluation stack as the same type.

## SYNTAX

```powershell
not
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| 66     | not             |

 The stack transitional behavior, in sequential order, is:

1.  `value` is pushed onto the stack.

2.  `value` is popped from the stack and its bitwise complement computed.

3.  The result is pushed onto the stack.

 The `not` instruction computes the bitwise complement of an integer value and pushes the result onto the stack. The return type is the same as the operand type.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
