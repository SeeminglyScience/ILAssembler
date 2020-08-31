---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stelem_r4
schema: 2.0.0
---

# stelem.r4

## SYNOPSIS

Replaces the array element at a given index with the `float32` value on the evaluation stack.

## SYNTAX

```powershell
stelem.r4
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| A0     | stelem.r4       |

 The stack transitional behavior, in sequential order, is:

1.  An object reference to an array, `array`, is pushed onto the stack.

2.  A valid index to an element in `array` is pushed onto the stack.

3.  A value is pushed onto the stack.

4.  The value, the index, and the array reference are popped from the stack; the value is put into the array element at the given index.

 The `stelem.r4` instruction replaces the value of the element `index` in the one-dimensional array `array` with the `float32` value pushed onto the stack.

 Arrays are objects and hence represented by a value of type `O`. The index is type `native int`.

 `System.NullReferenceException` is thrown if `array` is a null reference.

 `System.IndexOutOfRangeException` is thrown if `index` is negative, or larger than the bound of `array`.

 `System.ArrayTypeMismatchException` is thrown if `array` does not hold elements of the required type.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
