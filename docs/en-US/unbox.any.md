---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.unbox_any
schema: 2.0.0
---

# unbox.any

## SYNOPSIS

Converts the boxed representation of a type specified in the instruction to its unboxed form.

## SYNTAX

```powershell
unbox.any
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft intermediate language (MSIL) assembly format, along with a brief reference summary:

| Format     | Assembly Format     |
| ---------- | ------------------- |
| A5 < `T` > | unbox.any `typeTok` |

 The stack transitional behavior, in sequential order, is:

1.  An object reference `obj` is pushed onto the stack.

2.  The object reference is popped from the stack and unboxed to the type specified in the instruction.

3.  The resulting object reference or value type is pushed onto the stack.

 When applied to the boxed form of a value type, the `unbox.any` instruction extracts the value contained within `obj` (of type `O`), and is therefore equivalent to `unbox` followed by `ldobj`.

 When applied to a reference type, the `unbox.any` instruction has the same effect as `castclass` `typeTok`.

 If the operand `typeTok` is a generic type parameter, then the runtime behavior is determined by the type that is specified for that generic type parameter.

 `System.InvalidCastException` is thrown if `obj` is not a boxed type.

 `System.NullReferenceException` is thrown if `obj` is a null reference.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
