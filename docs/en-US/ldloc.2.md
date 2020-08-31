---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldloc_2
schema: 2.0.0
---

# ldloc.2

## SYNOPSIS

Loads the local variable at index 2 onto the evaluation stack.

## SYNTAX

```powershell
ldloc.2
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format | Description                                                    |
| ------ | --------------- | -------------------------------------------------------------- |
| 08     | ldloc.2         | Loads the local variable at index 2 onto the evaluation stack. |

 The stack transitional behavior, in sequential order, is:

1.  The local variable value at the index 2 is pushed onto the stack.

 `ldloc.2` is an especially efficient encoding for `ldloc`, allowing access to the local variable at index 2.

 The type of the value is the same as the type of the local variable, which is specified in the method header. Local variables that are smaller than 4 bytes long are expanded to type `int32` when they are loaded onto the stack. Floating-point values are expanded to their native size (type `F`).

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
