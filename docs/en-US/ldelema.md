---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldelema
schema: 2.0.0
---

# ldelema

## SYNOPSIS

Loads the address of the array element at a specified array index onto the top of the evaluation stack as type `&` (managed pointer).

## SYNTAX

```powershell
ldelema
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format     | Assembly Format |
| ---------- | --------------- |
| 8F < `T` > | ldelema `class` |

 The stack transitional behavior, in sequential order, is:

1.  An object reference `array` is pushed onto the stack.

2.  An index value `index` is pushed onto the stack.

3.  `index` and `array` are popped from the stack; the address stored at position `index` in `array` is looked up.

4.  The address is pushed onto the stack.

 The `ldelema` is used to retrieve the address of an object at a particular index in an array of objects (of type `class`). The `ldelema` instruction loads the address of the value at index `index` (type `native int`) in the zero-based one-dimensional array `array` and places it on the top of the stack. Arrays are objects and hence represented by a value of type `O`. The value must be of type `class` passed with the instruction.

 The return value for `ldelema` is a managed pointer (type `&`).

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
