---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stelem_ref
schema: 2.0.0
---

# stelem.ref

## SYNOPSIS

Replaces the array element at a given index with the object ref value (type `O`) on the evaluation stack.

## SYNTAX

```powershell
stelem.ref
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| A2     | stelem.ref      |

 The stack transitional behavior, in sequential order, is:

1.  An object reference to an array, `array`, is pushed onto the stack.

2.  A valid index to an element in `array` is pushed onto the stack.

3.  A value is pushed onto the stack.

4.  The value, the index, and the array reference are popped from the stack; the value is put into the array element at the given index.

 The `stelem.ref` instruction replaces the value of the element at the supplied index in the one-dimensional array `array` with the `ref` (type `O`) value pushed onto the stack.

 Arrays are objects and hence represented by a value of type `O`. The index is type `native int`.

 Note that `stelem.ref` implicitly casts the supplied value to the element type of `array` before assigning the value to the array element. This cast can fail, even for verified code. Thus the `stelem.ref` instruction can throw `System.InvalidCastException`. For one-dimensional arrays that aren't zero-based and for multidimensional arrays, the `System.Array` class provides a `System.Array.SetValue*` method.

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
