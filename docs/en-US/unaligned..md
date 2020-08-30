---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.unaligned
schema: 2.0.0
---

# unaligned.

## SYNOPSIS

Indicates that an address currently atop the evaluation stack might not be aligned to the natural size of the immediately following `ldind`, `stind`, `ldfld`, `stfld`, `ldobj`, `stobj`, `initblk`, or `cpblk` instruction.

## SYNTAX

```powershell
unaligned. <1 | 2 | 4>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format                    | Assembly Format        |
| ------------------------- | ---------------------- |
| FE 12 < `unsigned int8` > | unaligned. `alignment` |

 The stack transitional behavior, in sequential order, is:

1.  An address is pushed onto the stack.

 `Unaligned` specifies that the address (an unmanaged pointer, `native int`) on the stack might not be aligned to the natural size of the immediately following `ldind`, `stind`, `ldfld`, `stfld`, `ldobj`, `stobj`, `initblk`, or `cpblk` instruction. That is, for a `ldind.i4` instruction the alignment of the address may not be to a 4-byte boundary. For `initblk` and `cpblk` the default alignment is architecture dependent (4-byte on 32-bit CPUs, 8-byte on 64-bit CPUs). Code generators that do not restrict their output to a 32-bit word size must use `unaligned` if the alignment is not known at compile time to be 8-byte.

 The value of alignment must be 1, 2, or 4 and means that the generated code should assume that the address is byte, double-byte, or quad-byte aligned, respectively. Note that transient pointers (type `*`) are always aligned.

 While the alignment for a `cpblk` instruction would logically require two numbers (one for the source and one for the destination), there is no noticeable impact on performance if only the lower number is specified.

 The `unaligned` and `volatile` prefixes can be combined in either order. They must immediately precede a `ldind`, `stind`, `ldfld`, `stfld`, `ldobj`, `stobj`, `initblk`, or `cpblk` instruction. Only the `volatile.` prefix is allowed for the `ldsfld` and `stsfld` instructions.

## PARAMETERS

### -alignment

Specifies that generated code should assume that the following address is byte, double-byte, or quad-byte aligned.

```yaml
Type: 1 | 2 | 4
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
