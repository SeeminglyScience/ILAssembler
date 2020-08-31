---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldtoken
schema: 2.0.0
---

# ldtoken

## SYNOPSIS

Converts a metadata token to its runtime representation, pushing it onto the evaluation stack.

## SYNTAX

```powershell
ldtoken <signature>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format     | Assembly Format |
| ---------- | --------------- |
| D0 < `T` > | ldtoken `token` |

 The stack transitional behavior, in sequential order, is:

1.  The passed token is converted to a `RuntimeHandle` and pushed onto the stack.

 The `ldtoken` instruction pushes a `RuntimeHandle` for the specified metadata token. A `RuntimeHandle` can be a `fieldref/fielddef`, a `methodref/methoddef`, or a `typeref/typedef`.

 The value pushed on the stack can be used in calls to `Reflection` methods in the system class library.

 For information on runtime handles, see the following classes: `System.RuntimeFieldHandle`, `System.RuntimeTypeHandle`, and `System.RuntimeMethodHandle`.

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
