---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.sizeof
schema: 2.0.0
---

# sizeof

## SYNOPSIS

Pushes the size, in bytes, of a supplied value type onto the evaluation stack.

## SYNTAX

```powershell
sizeof <signature>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format        | Assembly Format  |
| ------------- | ---------------- |
| FE 1C < `T` > | sizeof `valType` |

 The stack transitional behavior, in sequential order, is:

1.  The size (in bytes) of the supplied value type (`valType`) is pushed onto the stack.

 `valType` must be a metadata token (a `typeref` or `typedef`) that specifies a value type, reference type, or generic type parameter.

 For a reference type, the size returned is the size of a reference value of the corresponding type (4 bytes on 32-bit systems), not the size of the data stored in objects referred to by the reference value. A generic type parameter can be used only in the body of the type or method that defines it. When that type or method is instantiated, the generic type parameter is replaced by a value type or reference type.

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
