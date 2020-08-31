---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.volatile
schema: 2.0.0
---

# volatile.

## SYNOPSIS

Specifies that an address currently atop the evaluation stack might be volatile, and the results of reading that location cannot be cached or that multiple stores to that location cannot be suppressed.

## SYNTAX

```powershell
volatile.
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| FE 13  | volatile.       |

 The stack transitional behavior, in sequential order, is:

1.  An address is pushed onto the stack.

 `volatile.`. specifies that the address is a volatile address (that is, it can be referenced externally to the current thread of execution) and the results of reading that location cannot be cached or that multiple stores to that location cannot be suppressed. Marking an access as `volatile.` affects only that single access; other accesses to the same location must be marked separately. Access to volatile locations need not be performed atomically.

 The `unaligned.` and `volatile.` prefixes can be combined in either order. They must immediately precede a `ldind`, `stind`, `ldfld`, `stfld`, `ldobj`, `stobj`, `initblk`, or `cpblk` instruction. Only the `volatile.` prefix is allowed for the `ldsfld` and `stsfld` instructions.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
