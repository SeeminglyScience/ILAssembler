---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldsflda
schema: 2.0.0
---

# ldsflda

## SYNOPSIS

Pushes the address of a static field onto the evaluation stack.

## SYNTAX

```powershell
ldsflda <signature>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format     | Assembly Format |
| ---------- | --------------- |
| 7F < `T` > | ldsflda `field` |

 The stack transitional behavior, in sequential order, is:

1.  The address of a specific field is pushed onto the stack.

 The `ldsflda` instruction pushes the address of a static (shared among all instances of a class) field on the stack. The address may be represented as a transient pointer (type `*`) if the metadata token `field` refers to a type whose memory is managed. Otherwise, it corresponds to an unmanaged pointer (type `native int`). Note that `field` may be a static global with an assigned relative virtual address (the offset of the field from the base address at which its containing PE file is loaded into memory) where the memory is unmanaged.

 The `ldsflda` instruction can have a `volatile.` prefix.

 `System.MissingFieldException` is thrown if field is not found in the metadata. This is typically checked when Microsoft Intermediate Language (MSIL) instructions are converted to native code, not at runtime.

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
