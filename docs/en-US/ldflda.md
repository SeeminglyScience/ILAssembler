---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldflda
schema: 2.0.0
---

# ldflda

## SYNOPSIS

Finds the address of a field in the object whose reference is currently on the evaluation stack.

## SYNTAX

```powershell
ldflda <signature>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format     | Assembly Format |
| ---------- | --------------- |
| 7C < `T` > | ldflda `field`  |

 The stack transitional behavior, in sequential order, is:

1.  An object reference (or pointer) is pushed onto the stack.

2.  The object reference (or pointer) is popped from the stack; the address of the specified field in the object is found.

3.  The address of the specified field is pushed onto the stack.

 The `ldflda` instruction pushes the address of a field located in an object onto the stack. The object must be on the stack as an object reference (type `O`), a managed pointer (type `&`), an unmanaged pointer (type `native int`), a transient pointer (type `*`), or an instance of a value type. The use of an unmanaged pointer is not permitted in verifiable code. The object's field is specified by a metadata token that must refer to a field member.

 The value returned by `ldflda` is a managed pointer (type `&`) unless the object is pushed onto the stack as an unmanaged pointer, in which case the return address is also an unmanaged pointer (type `native int`).

 The `ldflda` instruction can be preceded by either or both of the `unaligned.` and `volatile.` prefixes.

 `System.InvalidOperationException` is thrown if the object is not within the application domain from which it is being accessed. The address of a field that is not inside the accessing application domain cannot be loaded.

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
