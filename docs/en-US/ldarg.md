---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldarg
schema: 2.0.0
---

# ldarg

## SYNOPSIS

Loads an argument (referenced by a specified index value) onto the stack.

## SYNTAX

```powershell
ldarg
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format                     | Assembly Format |
| -------------------------- | --------------- |
| FE 09 < `unsigned int16` > | ldarg `index`   |

 The stack transitional behavior, in sequential order, is:

1.  The argument value at `index` is pushed onto the stack.

 The `ldarg` instruction pushes the argument indexed at `index`, where arguments are indexed from 0 onwards, onto the evaluation stack. The `ldarg` instruction can be used to load a value type or a primitive value onto the stack by copying it from an incoming argument. The type of the argument value is the same as the type of the argument, as specified by the current method's signature.

 For procedures that take a variable-length argument list, the `ldarg` instruction can be used only for the initial fixed arguments, not those in the variable part of the signature (see the `arglist` instruction for more details).

 Arguments that hold an integer value smaller than 4 bytes long are expanded to type `int32` when they are loaded onto the stack. Floating-point values are expanded to their native size (type `F`).

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
