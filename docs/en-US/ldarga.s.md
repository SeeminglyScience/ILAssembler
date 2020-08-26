---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldarga_s
schema: 2.0.0
---

# ldarga.s

## SYNOPSIS

Load an argument address, in short form, onto the evaluation stack.

## SYNTAX

```powershell
ldarga.s
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format                 | Assembly Format  |
| ---------------------- | ---------------- |
| 0F < `unsigned int8` > | ldarga.s `index` |

 The stack transitional behavior, in sequential order, is:

1.  The address `addr` of the argument indexed by `index` is pushed onto the stack.

 `ldarga.s` (the short form of `ldarga`) should be used for argument numbers 0 through 255, and is a more efficient encoding.

 The `ldarga.s` instruction fetches the address (of type`*`) of the argument indexed by `index`, where arguments are indexed from 0 onwards. The address `addr` is always aligned to a natural boundary on the target machine.

 For procedures that take a variable-length argument list, the `ldarga.s` instruction can be used only for the initial fixed arguments, not those in the variable part of the signature.

 `ldarga.s` is used for by-ref parameter passing. For other cases, `ldarg.s` and `starg.s` should be used.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
