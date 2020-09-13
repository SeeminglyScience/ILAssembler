---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.readonly
schema: 2.0.0
---

# readonly.

## SYNOPSIS

Specifies that the subsequent array address operation performs no type check at run time, and that it returns a managed pointer whose mutability is restricted.

## SYNTAX

```powershell
readonly
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft intermediate language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| FE 1E  | readonly.       |

 This prefix can only appear immediately preceding the `ldelema` instruction and calls to the special `Address` method on arrays. Its effect on the subsequent operation is twofold:

1.  At run time, no type check operation is performed. Note that there is normally an implicit type check for the `ldelema` and `stelem` instructions when used on reference type arrays. There is never a run-time type check for value classes, so `readonly` is a no-op in that case.

2.  The verifier treats the result of the address-of operation as a managed pointer with restricted mutability.

 The pointer is said to have restricted mutability because the defining type controls whether the value can be mutated. For value classes that expose no public fields or methods that update the value in place, the pointer is read-only (hence the name of the prefix). In particular, the classes representing primitive types (for example, System.Int32) do not expose mutators and thus are read-only.

 A managed pointer restricted in this fashion can be used only in the following ways:

-   As the `object` parameter for the `ldfld`, `ldflda`, `stfld`, `call`, or`constrained. callvirt` instructions.

-   As the `pointer` parameter to the `ldobj` instruction or to one of the `ldind` instructions.

-   As the `source` parameter to the `cpobj` instruction.

 All other operations disallowed, including the `stobj`, `initobj`, or `mkrefany` operations, or any of the `stind` instructions.

 The purpose of the `readonly.` prefix is to avoid a type check when fetching an element from an array in generic code. For example, the expression `arr[i].m()`, where the element type of the array `arr` is a generic type that has been constrained to have an interface with method `m`, might compile to the following MSIL.

```cil
ldloc arr
ldloc i
readonly.
ldelema !0    // Loads the pointer to the object.
…             // Load the arguments to the call.
constrained. !0
callvirt m
```

 Without the `readonly.` prefix, the `ldelema` instruction would perform a type check in the case where !0 was a reference type. Not only is this type check inefficient, but it is semantically incorrect. The type check for `ldelema` is an exact match, which is too strong. If the array held subclasses of type !0, the code above would fail the type check.

 The address of the array element is fetched, instead of the element itself, in order to have a handle for `arr[i]` that works for both value types and reference types, and thus can be passed to the `constrained callvirt` instruction.

 In general it would be unsafe to skip the run-time check if the array held elements of a reference type. To be safe, it is necessary to ensure that no modifications to the array are made through this pointer. The verifier rules ensure this. The restricted managed pointer can be passed as the object of instance method calls, so it is not strictly speaking read-only for value types, but there is no type safety problem for value types.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
