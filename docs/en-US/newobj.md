---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.newobj
schema: 2.0.0
---

# newobj

## SYNOPSIS

Creates a new object or a new instance of a value type, pushing an object reference (type `O`) onto the evaluation stack.

## SYNTAX

```powershell
newobj <signature>
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format     | Assembly Format |
| ---------- | --------------- |
| 73 < `T` > | newobj `ctor`   |

 The stack transitional behavior, in sequential order, is:

1.  Arguments `arg1` through `argn` are pushed on the stack in sequence.

2.  Arguments `argn` through `arg1` are popped from the stack and passed to `ctor` for object creation.

3.  A reference to the new object is pushed onto the stack.

 The `newobj` instruction creates a new object or a new instance of a value type. `Ctor` is a metadata token (a `methodref` or `methoddef` that must be marked as a constructor) that indicates the name, class and signature of the constructor to call.

 The `newobj` instruction allocates a new instance of the class associated with `ctor` and initializes all the fields in the new instance to 0 (of the proper type) or null references as appropriate. It then calls the constructor `ctor` with the given arguments along with the newly created instance. After the constructor has been called, the now initialized object reference (type `O`) is pushed on the stack.

 From the constructor's point of view, the uninitialized object is argument 0 and the other arguments passed to newobj follow in order.

 All zero-based, one-dimensional arrays are created using `newarr`, not `newobj`. On the other hand, all other arrays (more than one dimension, or one-dimensional but not zero-based) are created using `newobj`.

 Value types are not usually created using `newobj`. They are usually allocated either as arguments or local variables, using `newarr` (for zero-based, one-dimensional arrays), or as fields of objects. Once allocated, they are initialized using `initobj`. However, the `newobj` instruction can be used to create a new instance of a value type on the stack, that can then be passed as an argument, stored in a local, and so on.

 `System.OutOfMemoryException` is thrown if there is insufficient memory to satisfy the request.

 `System.MissingMethodException` is thrown if a constructor method `ctor` with the indicated name, class and signature could not be found. This is typically detected when Microsoft Intermediate Language (MSIL) instructions are converted to native code, rather than at runtime.

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
