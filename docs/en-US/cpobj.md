---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.cpobj
schema: 2.0.0
---

# cpobj

## SYNOPSIS

Copies the value type located at the address of an object (type "`&`", `*` or `native int`) to the address of the destination object (type `&`, `*` or `native int`).

## SYNTAX

```powershell
cpobj <signature>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format     | Assembly Format  |
| ---------- | ---------------- |
| 70 < `T` > | cpobj `classTok` |

 The stack transitional behavior, in sequential order, is:

1.  The destination object reference is pushed onto the stack.

2.  The source object reference is pushed onto the stack.

3.  The two object references are popped from the stack; the value type at the address of the source object is copied to the address of the destination object.

 The behavior of `cpobj` is unspecified if the source and destination object references are not pointers to instances of the class represented by the class token `classTok` (a `typeref` or `typedef`), or if `classTok` does not represent a value type.

 `System.NullReferenceException` may be thrown if an invalid address is detected.

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
