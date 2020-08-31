---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.starg_s
schema: 2.0.0
---

# starg.s

## SYNOPSIS

Stores the value on top of the evaluation stack in the argument slot at a specified index, short form.

## SYNTAX

```powershell
starg.s <byte>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format                 | Assembly Format |
| ---------------------- | --------------- |
| 10 < `unsigned int8` > | starg.s `num`   |

 The stack transitional behavior, in sequential order, is:

1.  The value currently on top of the stack is popped and placed in argument slot `num`.

 The `starg.s` instruction pops a value from the stack and places it in argument slot `num`. The type of the value must match the type of the argument, as specified in the current method's signature.

 The `starg.s` instruction provides an efficient encoding for use with the first 256 arguments.

 For procedures that take a variable argument list, the `starg.s` instruction can be used only for the initial fixed arguments, not those in the variable part of the signature.

 Performing a store into arguments that hold an integer value smaller than 4 bytes long truncates the value as it moves from the stack to the argument. Floating-point values are rounded from their native size (type `F`) to the size associated with the argument.

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
