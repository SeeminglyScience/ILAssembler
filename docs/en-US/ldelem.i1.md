---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldelem_i1
schema: 2.0.0
---

# ldelem.i1

## SYNOPSIS

Loads the element with type `int8` at a specified array index onto the top of the evaluation stack as an `int32`.

## SYNTAX

```powershell
ldelem.i1
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| 90     | ldelem.i1       |

 The stack transitional behavior, in sequential order, is:

1.  An object reference `array` is pushed onto the stack.

2.  An index value `index` is pushed onto the stack.

3.  `index` and `array` are popped from the stack; the value stored at position `index` in `array` is looked up.

4.  The value is pushed onto the stack.

 The `ldelem.i1` instruction loads the value of the element with index `index` (type `native int`) in the zero-based one-dimensional array `array` and places it on the top of the stack. Arrays are objects and hence represented by a value of type `O`.

 The return value for `ldelem.i1` is `int8`.

 Note that integer values of less than 4 bytes are extended to `int32` (not `native int`) when they are loaded onto the evaluation stack.

 `System.NullReferenceException` is thrown if `array` is a null reference.

 `System.ArrayTypeMismatchException` is thrown if `array` does not hold elements of the required type.

 `System.IndexOutOfRangeException` is thrown if `index` is negative, or larger than the bound of `array`.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
