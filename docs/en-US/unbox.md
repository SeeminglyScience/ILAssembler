---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.unbox
schema: 2.0.0
---

# unbox

## SYNOPSIS

Converts the boxed representation of a value type to its unboxed form.

## SYNTAX

```powershell
unbox
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format     | Assembly Format |
| ---------- | --------------- |
| 79 < `T` > | unbox `valType` |

 The stack transitional behavior, in sequential order, is:

1.  An object reference is pushed onto the stack.

2.  The object reference is popped from the stack and unboxed to a value type pointer.

3.  The value type pointer is pushed onto the stack.

 A value type has two separate representations within the Common Language Infrastructure (CLI):

-   A 'raw' form used when a value type is embedded within another object.

-   A 'boxed' form, where the data in the value type is wrapped (boxed) into an object so it can exist as an independent entity.

 The `unbox` instruction converts the object reference (type `O`), the boxed representation of a value type, to a value type pointer (a managed pointer, type `&`), its unboxed form. The supplied value type (`valType`) is a metadata token indicating the type of value type contained within the boxed object.

 Unlike `box`, which is required to make a copy of a value type for use in the object, `unbox` is not required to copy the value type from the object. Typically it simply computes the address of the value type that is already present inside of the boxed object.

 `System.InvalidCastException` is thrown if the object is not boxed as `valType`.

 `System.NullReferenceException` is thrown if the object reference is a null reference.

 `System.TypeLoadException` is thrown if the value type `valType` cannot be found. This is typically detected when Microsoft Intermediate Language (MSIL) instructions are converted to native code, rather than at runtime.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
