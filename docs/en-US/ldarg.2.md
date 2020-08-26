---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldarg_2
schema: 2.0.0
---

# ldarg.2

## SYNOPSIS

Loads the argument at index 2 onto the evaluation stack.

## SYNTAX

```powershell
ldarg.2
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| 04     | ldarg.2         |

 The stack transitional behavior, in sequential order, is:

1.  The argument value at index 2 is pushed onto the stack.

 The `ldarg.2` instruction is an efficient encoding for loading the argument value at index 2.

 The `ldarg.2` instruction pushes the argument indexed at 2 onto the evaluation stack. The `ldarg.2` instruction can be used to load a value type or a primitive value onto the stack by copying it from an incoming argument. The type of the argument value is the same as the type of the argument, as specified by the current method's signature.

 Arguments that hold an integer value smaller than 4 bytes long are expanded to type `int32` when they are loaded onto the stack. Floating-point values are expanded to their native size (type `F`).

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
