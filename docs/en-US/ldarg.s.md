---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldarg_s
schema: 2.0.0
---

# ldarg.s

## SYNOPSIS

Loads the argument (referenced by a specified short form index) onto the evaluation stack.

## SYNTAX

```powershell
ldarg.s <byte>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format                 | Assembly Format |
| ---------------------- | --------------- |
| 0E < `unsigned int8` > | ldarg.s `index` |

 The stack transitional behavior, in sequential order, is:

1.  The argument value at `index` is pushed onto the stack.

 The `ldarg.s` instruction is an efficient encoding for loading arguments indexed from 4 through 255.

 The `ldarg.s` instruction pushes the argument indexed at `index`, where arguments are indexed from 0 onwards, onto the evaluation stack. The `ldarg.s` instruction can be used to load a value type or a primitive value onto the stack by copying it from an incoming argument. The type of the argument value is the same as the type of the argument, as specified by the current method's signature.

 For procedures that take a variable-length argument list, the `ldarg.s` instruction can be used only for the initial fixed arguments, not those in the variable part of the signature (see the `arglist` instruction for more details).

 Arguments that hold an integer value smaller than 4 bytes long are expanded to type `int32` when they are loaded onto the stack. Floating-point values are expanded to their native size (type `F`).

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
