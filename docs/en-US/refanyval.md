---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.refanyval
schema: 2.0.0
---

# refanyval

## SYNOPSIS

Retrieves the address (type `&`) embedded in a typed reference.

## SYNTAX

```powershell
refanyval
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format     | Assembly Format  |
| ---------- | ---------------- |
| C2 < `T` > | refanyval `type` |

 The stack transitional behavior, in sequential order, is:

1.  A value type reference is pushed onto the stack.

2.  The typed reference is popped from the stack and the corresponding address retrieved.

3.  The address is pushed onto the stack.

 A typed reference contains a type token and an address to an object instance.

 The `refanyval` instruction retrieves the address embedded in the a typed reference. The type embedded in the typed reference supplied on the stack must match the type specified by `type` (a metadata token, either a `typedef` or a `typeref`). See the `mkrefany` instruction for related content.

 `System.InvalidCastException` is thrown if `type` is not identical to the type stored in the type reference (in this case, `type` is the class supplied to the `mkrefany` instruction that constructed said typed reference).

 `System.TypeLoadException` is thrown if `type` cannot be found.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
