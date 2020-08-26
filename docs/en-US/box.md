---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.box
schema: 2.0.0
---

# box

## SYNOPSIS

Converts a value type to an object reference (type `O`).

## SYNTAX

```powershell
box
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format     | Assembly Format    |
| ---------- | ------------------ |
| 8C < `T` > | box `valTypeToken` |

 The stack transitional behavior, in sequential order, is:

1.  A value type is pushed onto the stack.

2.  The value type is popped from the stack; the `box` operation is performed.

3.  An object reference to the resulting "boxed" value type is pushed onto the stack.

 A value type has two separate representations within the Common Language Infrastructure (CLI):

-   A 'raw' form used when a value type is embedded within another object or on the stack.

-   A 'boxed' form, where the data in the value type is wrapped (boxed) into an object so it can exist as an independent entity.

 The `box` instruction converts the 'raw' (unboxed) value type into an object reference (type `O`). This is accomplished by creating a new object and copying the data from the value type into the newly allocated object. `valTypeToken` is a metadata token indicating the type of the value type on the stack.

 `System.OutOfMemoryException` is thrown if there is insufficient memory to satisfy the request.

 `System.TypeLoadException` is thrown if the class cannot be found. This is typically detected when Microsoft Intermediate Language (MSIL) is converted to native code, rather than at runtime.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
