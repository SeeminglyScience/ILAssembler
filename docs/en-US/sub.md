---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.sub
schema: 2.0.0
---

# sub

## SYNOPSIS

Subtracts one value from another and pushes the result onto the evaluation stack.

## SYNTAX

```powershell
sub
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| 59     | sub             |

 The stack transitional behavior, in sequential order, is:

1.  `value1` is pushed onto the stack.

2.  `value2` is pushed onto the stack.

3.  `value2` and `value1` are popped from the stack; `value2` is subtracted from `value1`.

4.  The result is pushed onto the stack.

 Overflow is not detected for integer operations (for proper overflow handling, see `sub.ovf`).

 Integer subtraction wraps, rather than saturates. For example: assuming 8-bit integers, where `value1` is set to 0 and `value2` is set to 1, the "wrapped" result will be 255.

 Floating-point overflow returns `+inf` (`PositiveInfinity`) or `-inf` (`NegativeInfinity`).

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
