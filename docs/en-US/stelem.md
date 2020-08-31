---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stelem
schema: 2.0.0
---

# stelem

## SYNOPSIS

Replaces the array element at a given index with the value on the evaluation stack, whose type is specified in the instruction.

## SYNTAX

```powershell
stelem <signature>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft intermediate language (MSIL) assembly format, along with a brief reference summary:

| Format     | Assembly Format  |
| ---------- | ---------------- |
| A4 < `T` > | stelem `typeTok` |

 The stack transitional behavior, in sequential order, is:

1.  An object reference to an array, `array`, is pushed onto the stack.

2.  An index value, `index`, to an element in `array` is pushed onto the stack.

3.  A value of the type specified in the instruction is pushed onto the stack.

4.  The value, the index, and the array reference are popped from the stack; the value is put into the array element at the given index.

 The `stelem` instruction replaces the value of the element at the supplied zero-based index in the one-dimensional array `array` with the value. The value has the type specified by the token `typeTok` in the instruction.

 Arrays are objects, and hence represented by a value of type `O`. The index is type `native int`.

 `System.NullReferenceException` is thrown if `array` is a null reference.

 `System.IndexOutOfRangeException` is thrown if `index` is negative, or larger than the bound of `array`.

 `System.ArrayTypeMismatchException` is thrown if `array` does not hold elements of the required type.

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
