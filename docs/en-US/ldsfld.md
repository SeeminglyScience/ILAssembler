---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldsfld
schema: 2.0.0
---

# ldsfld

## SYNOPSIS

Pushes the value of a static field onto the evaluation stack.

## SYNTAX

```powershell
ldsfld <signature>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format     | Assembly Format |
| ---------- | --------------- |
| 7E < `T` > | ldsfld `field`  |

 The stack transitional behavior, in sequential order, is:

1.  The value of the specific field is pushed onto the stack.

 The `ldsfld` instruction pushes the value of a static (shared among all instances of a class) field on the stack. The return type is that associated with the passed metadata token `field`.

 The `ldsfld` instruction can have a `volatile.` prefix.

## PARAMETERS

### -signature

Specifies the target signature.

```yaml
Type: signature
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
