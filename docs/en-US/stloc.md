---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stloc
schema: 2.0.0
---

# stloc

## SYNOPSIS

Pops the current value from the top of the evaluation stack and stores it in a the local variable list at a specified index.

## SYNTAX

```powershell
stloc
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format                     | Assembly Format |
| -------------------------- | --------------- |
| FE 0E < `unsigned int16` > | stloc `index`   |

 The stack transitional behavior, in sequential order, is:

1.  A value is popped off of the stack and placed in local variable `index`.

 The `stloc` instruction pops the top value off the evaluation stack and moves it into local variable number `index`, where local variables are numbered 0 onwards. The type of the value must match the type of the local variable as specified in the current method's local signature.

 Storing into locals that hold an integer value smaller than 4 bytes long truncates the value as it moves from the stack to the local variable. Floating-point values are rounded from their native size (type `F`) to the size associated with the argument.

 Correct Microsoft Intermediate Language (MSIL) instructions require that `index` be a valid local index. For the `stloc` instruction, `index` must lie in the range 0 to 65534 inclusive (specifically, 65535 is not valid). The reason for excluding 65535 is pragmatic: likely implementations will use a 2-byte integer to track both a local's index, as well as the total number of locals for a given method. If an index of 65535 had been made valid, it would require a wider integer to track the number of locals in such a method.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
