---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.sub_ovf_un
schema: 2.0.0
---

# sub.ovf.un

## SYNOPSIS

Subtracts one unsigned integer value from another, performs an overflow check, and pushes the result onto the evaluation stack.

## SYNTAX

```powershell
sub.ovf.un
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| DB     | sub.ovf.un      |

 The stack transitional behavior, in sequential order, is:

1.  `value1` is pushed onto the stack.

2.  `value2` is pushed onto the stack.

3.  `value2` and `value1` are popped from the stack; `value2` is subtracted from `value1` with a check for overflow.

4.  The result is pushed onto the stack.

 `System.OverflowException` is thrown if the result can not be represented in the result type.

 This operation is performed on signed integers; for floating-point values, use `sub`.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
