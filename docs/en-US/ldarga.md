---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldarga
schema: 2.0.0
---

# ldarga

## SYNOPSIS

Load an argument address onto the evaluation stack.

## SYNTAX

```powershell
ldarga <int32>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format                     | Assembly Format |
| -------------------------- | --------------- |
| FE 0A < `unsigned int16` > | ldarga `index`  |

 The stack transitional behavior, in sequential order, is:

1.  The address `addr` of the argument indexed by `index` is pushed onto the stack.

 The `ldarga` instruction fetches the address (of type `*`) of the argument indexed by `index`, where arguments are indexed from 0 onwards. The address `addr` is always aligned to a natural boundary on the target machine.

 For procedures that take a variable-length argument list, the `ldarga` instruction can be used only for the initial fixed arguments, not those in the variable part of the signature.

 `ldarga` is used for by-ref parameter passing. For other cases, `ldarg` and `starg` should be used.

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
