---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldloc
schema: 2.0.0
---

# ldloc

## SYNOPSIS

Loads the local variable at a specific index onto the evaluation stack.

## SYNTAX

```powershell
ldloc <int32>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format                     | Assembly Format |
| -------------------------- | --------------- |
| FE 0C < `unsigned int16` > | ldloc `index`   |

 The stack transitional behavior, in sequential order, is:

1.  The local variable value at the specified index is pushed onto the stack.

 The `ldloc` instruction pushes the contents of the local variable number at the passed index onto the evaluation stack, where the local variables are numbered 0 onwards. Local variables are initialized to 0 before entering the method only if the initialize flag on the method is true. There are 65,535 (2^16-1) local variables possible (0-65,534). Index 65,535 is not valid since likely implementations will use a 2-byte integer to track both a local's index, along with the total number of locals for a given method. If an index of 65535 had been made valid, it would require a wider integer to track the number of locals in such a method.

 The `ldloc.0`, `ldloc.1`, `ldloc.2`, and `ldloc.3` instructions provide an efficient encoding for accessing the first four local variables.

 The type of the value is the same as the type of the local variable, which is specified in the method header. See Partition I. Local variables that are smaller than 4 bytes long are expanded to type `int32` when they are loaded onto the stack. Floating-point values are expanded to their native size (type `F`).

## PARAMETERS

### -index

Specifies the index to load.

```yaml
Type: int32
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
