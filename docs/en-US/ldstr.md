---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldstr
schema: 2.0.0
---

# ldstr

## SYNOPSIS

Pushes a new object reference to a string literal stored in the metadata.

## SYNTAX

```powershell
ldstr <string>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format     | Assembly Format |
| ---------- | --------------- |
| 72 < `T` > | ldstr `mdToken` |

 The stack transitional behavior, in sequential order, is:

1.  An object reference to a string is pushed onto the stack.

 The `ldstr` instruction pushes an object reference (type `O`) to a new string object representing the specific string literal stored in the metadata. The `ldstr` instruction allocates the requisite amount of memory and performs any format conversion required to convert the string literal from the form used in the file to the string format required at runtime.

 The Common Language Infrastructure (CLI) guarantees that the result of two `ldstr` instructions referring to two metadata tokens that have the same sequence of characters return precisely the same string object (a process known as "string interning").

## PARAMETERS

### -value

Specifies the constant to push onto the stack.

```yaml
Type: string
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
