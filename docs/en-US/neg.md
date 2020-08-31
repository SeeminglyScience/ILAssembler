---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.neg
schema: 2.0.0
---

# neg

## SYNOPSIS

Negates a value and pushes the result onto the evaluation stack.

## SYNTAX

```powershell
neg
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| 65     | neg             |

 The stack transitional behavior, in sequential order, is:

1.  A value is pushed onto the stack.

2.  A value is popped from the stack and negated.

3.  The result is pushed onto the stack.

 The `neg` instruction negates value and pushes the result on top of the stack. The return type is the same as the operand type.

 Negation of integral values is standard two's complement negation. In particular, negating the most negative number (which does not have a positive counterpart) yields the most negative number. To detect this overflow use the `sub.ovf` instruction instead (that is, subtract from 0).

 Negating a floating-point number cannot overflow, and negating NaN returns NaN.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
