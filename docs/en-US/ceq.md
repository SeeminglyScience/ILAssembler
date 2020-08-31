---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ceq
schema: 2.0.0
---

# ceq

## SYNOPSIS

Compares two values. If they are equal, the integer value `1` (`int32`) is pushed onto the evaluation stack; otherwise `0` (`int32`) is pushed onto the evaluation stack.

## SYNTAX

```powershell
ceq
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| FE 01  | ceq             |

 The stack transitional behavior, in sequential order, is:

1.  `value1` is pushed onto the stack.

2.  `value2` is pushed onto the stack.

3.  `value2` and `value1` are popped from the stack; `value1` is compared to `value2`.

4.  If `value1` is equal to `value2`, 1 is pushed onto the stack; otherwise 0 is pushed onto the stack.

 The `ceq` instruction compares `value1` and `value2`. If `value1` is equal to `value2`, then `1` (of type `int32`) is pushed on the stack. Otherwise `0` (of type `int32`) is pushed on the stack.

 For floating-point number, `ceq` will return `0` if the numbers are unordered (either or both are `NaN`). The infinite values are equal to themselves.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
