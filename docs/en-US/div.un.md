---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.div_un
schema: 2.0.0
---

# div.un

## SYNOPSIS

Divides two unsigned integer values and pushes the result (`int32`) onto the evaluation stack.

## SYNTAX

```powershell
div.un
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| 5C     | div.un          |

 The stack transitional behavior, in sequential order, is:

1.  `value1` is pushed onto the stack.

2.  `value2` is pushed onto the stack.

3.  `value2` and `value1` are popped from the stack; `value1` is divided by `value2`.

4.  The result is pushed onto the stack.

 The `div.un` instruction computes `value1` divided by `value2`, both taken as unsigned integers, and pushes the `result` on the stack.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
