---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldfld
schema: 2.0.0
---

# ldfld

## SYNOPSIS

Finds the value of a field in the object whose reference is currently on the evaluation stack.

## SYNTAX

```powershell
ldfld <signature>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format     | Assembly Format |
| ---------- | --------------- |
| 7B < `T` > | ldfld `field`   |

 The stack transitional behavior, in sequential order, is:

1.  An object reference (or pointer) is pushed onto the stack.

2.  The object reference (or pointer) is popped from the stack; the value of the specified field in the object is found.

3.  The value stored in the field is pushed onto the stack.

 The `ldfld` instruction pushes the value of a field located in an object onto the stack. The object must be on the stack as an object reference (type `O`), a managed pointer (type `&`), an unmanaged pointer (type `native int`), a transient pointer (type `*`), or an instance of a value type. The use of an unmanaged pointer is not permitted in verifiable code. The object's field is specified by a metadata token that must refer to a field member. The return type is the same as the one associated with the field. The field may be either an instance field (in which case the object must not be a null reference) or a static field.

 The `ldfld` instruction can be preceded by either or both of the `unaligned.` and `volatile.` prefixes.

 `System.NullReferenceException` is thrown if the object is null and the field is not static.

 `System.MissingFieldException` is thrown if the specified field is not found in the metadata. This is typically checked when Microsoft Intermediate Language (MSIL) instructions are converted to native code, not at run time.

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
