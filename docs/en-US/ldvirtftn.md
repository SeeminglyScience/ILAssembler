---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ldvirtftn
schema: 2.0.0
---

# ldvirtftn

## SYNOPSIS

Pushes an unmanaged pointer (type `native int`) to the native code implementing a particular virtual method associated with a specified object onto the evaluation stack.

## SYNTAX

```powershell
ldvirtftn <signature>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format        | Assembly Format    |
| ------------- | ------------------ |
| FE 07 < `T` > | ldvirtftn `method` |

 The stack transitional behavior, in sequential order, is:

1.  An object reference is pushed onto the stack.

2.  The object reference is popped from the stack and the address of the entry point to the method (as specified by the metadata token `method`) is looked up.

3.  The pointer to `method` is pushed onto the stack.

 The resulting unmanaged pointer pushed onto the stack by the `ldvirtftn` instruction can be called using the `calli` instruction if it references a managed method (or a stub that transitions from managed to unmanaged code).

 The unmanaged pointer points to native code using the CLR calling convention. This method pointer should not be passed to unmanaged native code as a callback routine.

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
