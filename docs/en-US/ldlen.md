---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldlen
schema: 2.0.0
---

# ldlen

## SYNOPSIS

Pushes the number of elements of a zero-based, one-dimensional array onto the evaluation stack.

## SYNTAX

```powershell
ldlen
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| 8E     | ldlen           |

 The stack transitional behavior, in sequential order, is:

1.  An object reference to an array is pushed onto the stack.

2.  The array reference is popped from the stack and the length is computed.

3.  The length is pushed onto the stack.

 Arrays are objects and hence represented by a value of type `O`. The length is returned as a `natural unsigned int`.

 `System.NullReferenceException` is thrown if the array reference is a null reference.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
