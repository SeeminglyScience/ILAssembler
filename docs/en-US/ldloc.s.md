---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldloc_s
schema: 2.0.0
---

# ldloc.s

## SYNOPSIS

Loads the local variable at a specific index onto the evaluation stack, short form.

## SYNTAX

```powershell
ldloc.s <byte>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format                 | Assembly Format |
| ---------------------- | --------------- |
| 11 < `unsigned int8` > | ldloc.s `index` |

 The stack transitional behavior, in sequential order, is:

1.  The local variable value at the specified index is pushed onto the stack.

 The `ldloc.s` instruction pushes the contents of the local variable number at the passed index onto the evaluation stack, where the local variables are numbered 0 onwards. Local variables are initialized to 0 before entering the method if the initialize flag on the method is true. There are 256 (2^8) local variables possible (0-255) in the short form, which is a more efficient encoding than `ldloc`.

 The type of the value is the same as the type of the local variable, which is specified in the method header. See Partition I. Local variables that are smaller than 4 bytes long are expanded to type `int32` when they are loaded onto the stack. Floating-point values are expanded to their native size (type `F`).

## PARAMETERS

### -index

Specifies the index to load.

```yaml
Type: byte
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
