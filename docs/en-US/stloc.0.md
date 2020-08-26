---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stloc_0
schema: 2.0.0
---

# stloc.0

## SYNOPSIS

Pops the current value from the top of the evaluation stack and stores it in a the local variable list at index 0.

## SYNTAX

```powershell
stloc.0
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| 0A     | stloc.0         |

 The stack transitional behavior, in sequential order, is:

1.  A value is popped off of the stack and placed in the local variable indexed by 0.

 The `stloc.0` instruction pops the top value off the evaluation stack and moves it into the local variable indexed by 0. The type of the value must match the type of the local variable as specified in the current method's local signature.

 `stloc.0` is an especially efficient encoding for storing values in local variable 0.

 Storing into locals that hold an integer value smaller than 4 bytes long truncates the value as it moves from the stack to the local variable. Floating-point values are rounded from their native size (type `F`) to the size associated with the argument.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
