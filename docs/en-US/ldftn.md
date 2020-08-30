---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldftn
schema: 2.0.0
---

# ldftn

## SYNOPSIS

Pushes an unmanaged pointer (type `native int`) to the native code implementing a specific method onto the evaluation stack.

## SYNTAX

```powershell
ldftn <signature>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format        | Assembly Format |
| ------------- | --------------- |
| FE 06 < `T` > | ldftn `method`  |

 The stack transitional behavior, in sequential order, is:

1.  The unmanaged pointer to a specific method is pushed onto the stack.

 The specific method (`method`) can be called using the `calli` instruction if it references a managed method (or a stub that transitions from managed to unmanaged code).

 The value returned points to native code using the CLR calling convention. This method pointer should not be passed to unmanaged native code as a callback routine.

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
