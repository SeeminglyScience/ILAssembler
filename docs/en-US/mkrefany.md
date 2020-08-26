---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.mkrefany
schema: 2.0.0
---

# mkrefany

## SYNOPSIS

Pushes a typed reference to an instance of a specific type onto the evaluation stack.

## SYNTAX

```powershell
mkrefany
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format     | Assembly Format  |
| ---------- | ---------------- |
| C6 < `T` > | mkrefany `class` |

 The stack transitional behavior, in sequential order, is:

1.  A pointer to piece of data is pushed onto the stack.

2.  The pointer is popped and converted to a typed reference of type `class`.

3.  The typed reference is pushed onto the stack.

 The `mkrefany` instruction supports the passing of dynamically typed references. The pointer must be of type `&`, `*`, or `native int`, and hold the valid address of a piece of data. `Class` is the class token describing the type of the data referenced by the pointer. `Mkrefany` pushes a typed reference on the stack, providing an opaque descriptor of the pointer and the type `class`.

 The only valid operation permitted upon a typed reference is to pass it to a method that requires a typed reference as a parameter. The callee can then use the `refanytype` and `refanyval` instructions to retrieve the type (class) and the address respectively.

 `System.TypeLoadException` is thrown if `class` cannot be found. This is typically detected when Microsoft Intermediate Language (MSIL) instructions are converted to native code rather than at runtime.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
