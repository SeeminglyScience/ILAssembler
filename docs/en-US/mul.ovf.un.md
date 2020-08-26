---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.mul_ovf_un
schema: 2.0.0
---

# mul.ovf.un

## SYNOPSIS

Multiplies two unsigned integer values, performs an overflow check, and pushes the result onto the evaluation stack.

## SYNTAX

```powershell
mul.ovf.un
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| D9     | mul.ovf.un      |

 The stack transitional behavior, in sequential order, is:

1.  `value1` is pushed onto the stack.

2.  `value2` is pushed onto the stack.

3.  `value2` and `value1` are popped from the stack; `value1` is multiplied by `value2`, with an overflow check.

4.  The result is pushed onto the stack.

 The `mul.ovf.un` instruction multiplies unsigned integer `value1` by unsigned integer `value2` and pushes the result on the stack. An exception is thrown if the result will not fit in the result type.

 `System.OverflowException` is thrown if the result can not be represented in the result type.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
