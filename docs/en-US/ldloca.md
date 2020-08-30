---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldloca
schema: 2.0.0
---

# ldloca

## SYNOPSIS

Loads the address of the local variable at a specific index onto the evaluation stack.

## SYNTAX

```powershell
ldloca <int32>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format                     | Assembly Format |
| -------------------------- | --------------- |
| FE OD < `unsigned int16` > | ldloca `index`  |

 The stack transitional behavior, in sequential order, is:

1.  The address stored in the local variable at the specified index is pushed onto the stack.

 The `ldloca` instruction pushes the address of the local variable number at the passed index onto the stack, where local variables are numbered 0 onwards. The value pushed on the stack is already aligned correctly for use with instructions like `ldind.i` and `stind.i`. The result is a transient pointer (type `*`).

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
