---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.stsfld
schema: 2.0.0
---

# stsfld

## SYNOPSIS

Replaces the value of a static field with a value from the evaluation stack.

## SYNTAX

```powershell
stsfld <signature>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format     | Assembly Format |
| ---------- | --------------- |
| 80 < `T` > | stsfld `field`  |

 The stack transitional behavior, in sequential order, is:

1.  A value is pushed onto the stack.

2.  A value is popped from the stack and stored in `field`.

 The `stsfld` instruction replaces the value of a static field with a value from the stack. `field` is a metadata token that must refer to a static field member.

 The `stsfld` instruction may be prefixed by `volatile.`.

 `System.MissingFieldException` is thrown if field is not found in the metadata. This is typically checked when Microsoft Intermediate Language (MSIL) instructions are converted to native code, not at run time.

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
