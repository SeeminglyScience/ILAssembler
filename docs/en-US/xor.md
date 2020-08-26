---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.xor
schema: 2.0.0
---

# xor

## SYNOPSIS

Computes the bitwise XOR of the top two values on the evaluation stack, pushing the result onto the evaluation stack.

## SYNTAX

```powershell
xor
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| 61     | xor             |

 The stack transitional behavior, in sequential order, is:

1.  `value1` is pushed onto the stack.

2.  `value2` is pushed onto the stack.

3.  `value2` and `value1` are popped from the stack and their bitwise XOR computed.

4.  The bitwise XOR of `value2` and `value1` is pushed onto the stack.

 The `xor` instruction computes the bitwise XOR of the top two values on the stack and leaves the result on the stack.

 `xor` is an integer-specific operation.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
