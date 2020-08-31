---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.newarr
schema: 2.0.0
---

# newarr

## SYNOPSIS

Pushes an object reference to a new zero-based, one-dimensional array whose elements are of a specific type onto the evaluation stack.

## SYNTAX

```powershell
newarr <signature>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format     | Assembly Format |
| ---------- | --------------- |
| 8D < `T` > | newarr `etype`  |

 The stack transitional behavior, in sequential order, is:

1.  The number of elements in the array is pushed onto the stack.

2.  The number of elements is popped from the stack and the array is created.

3.  An object reference to the new array is pushed onto the stack.

 The `newarr` instruction pushes an object reference (type `O`) to a new zero-based, one-dimensional array whose elements are of type `etype` (a metadata token describing the type). The number of elements in the new array should be specified as a `native int`. Valid array indexes range from zero to the maximum number of elements minus one.

 The elements of an array can be any type, including value types.

 Zero-based, one-dimensional arrays of numbers are created using a metadata token referencing the appropriate value type (`System.Int32`, and so on). Elements of the array are initialized to 0 of the appropriate type.

 Nonzero-based one-dimensional arrays and multidimensional arrays are created using `newobj` rather than `newarr`. More commonly, they are created using the methods of the `System.Array` class in the .NET Framework.

 `System.OutOfMemoryException` is thrown if there is insufficient memory to satisfy the request.

 `System.OverflowException` is thrown if `numElems` is less than 0.

## PARAMETERS

### -signature

Specifies the target signature.

```yaml
Type: signature
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
