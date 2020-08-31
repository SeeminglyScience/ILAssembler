---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldobj
schema: 2.0.0
---

# ldobj

## SYNOPSIS

Copies the value type object pointed to by an address to the top of the evaluation stack.

## SYNTAX

```powershell
ldobj <signature>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format     | Assembly Format |
| ---------- | --------------- |
| 71 < `T` > | ldobj `class`   |

 The stack transitional behavior, in sequential order, is:

1.  The address of a value type object is pushed onto the stack.

2.  The address is popped from the stack and the instance at that particular address is looked up.

3.  The value of the object stored at that address is pushed onto the stack.

 The `ldobj` instruction is used to pass a value type as a parameter.

 The `ldobj` instruction copies the value pointed to by `addrOfValObj` (of type `&`, `*`, or `native int`) to the top of the stack. The number of bytes copied depends on the size of the class (as specified by the `class` parameter). The `class` parameter is a metadata token representing the value type.

 The operation of the `ldobj` instruction can be altered by an immediately preceding `volatile.` or `unaligned.` prefix instruction.

 `System.TypeLoadException` is thrown if class cannot be found. This is typically detected when the Microsoft Intermediate Language (MSIL) instruction is converted to native code rather than at runtime.

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
