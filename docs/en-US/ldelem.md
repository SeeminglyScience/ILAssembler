---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldelem
schema: 2.0.0
---

# ldelem

## SYNOPSIS

Loads the element at a specified array index onto the top of the evaluation stack as the type specified in the instruction.

## SYNTAX

```powershell
ldelem <signature>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft intermediate language (MSIL) assembly format, along with a brief reference summary:

| Format     | Assembly Format  |
| ---------- | ---------------- |
| A3 < `T` > | ldelem `typeTok` |

 The stack transitional behavior, in sequential order, is:

1.  An object reference `array` is pushed onto the stack.

2.  An index value `index` is pushed onto the stack.

3.  `index` and `array` are popped from the stack; the value stored at position `index` in `array` is looked up.

4.  The value is pushed onto the stack.

 The `ldelem` instruction loads the value of the element with index `index` (type `native int`) in the zero-based one-dimensional array `array` and places it on the top of the stack. Arrays are objects, and hence represented by a value of type `O`.

 The type of the return value is specified by the token `typeTok` in the instruction.

 `System.NullReferenceException` is thrown if `array` is a null reference.

 `System.IndexOutOfRangeException` is thrown if `index` is negative, or larger than the upper bound of `array`.

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
