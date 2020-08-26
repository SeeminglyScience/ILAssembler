---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.constrained
schema: 2.0.0
---

# constrained.

## SYNOPSIS

Constrains the type on which a virtual method call is made.

## SYNTAX

```powershell
constrained.
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft intermediate language (MSIL) assembly format, along with a brief reference summary:

| Format        | Assembly Format         |
| ------------- | ----------------------- |
| FE 16 < `T` > | constrained. `thisType` |

 The `constrained.` prefix is permitted only on a `callvirt` instruction.

 The state of the MSIL stack at this point must be as follows:

1.  A managed pointer, `ptr`, is pushed onto the stack. The type of `ptr` must be a managed pointer (`&`) to `thisType`. Note that this is different from the case of an unprefixed `callvirt` instruction, which expects a reference of `thisType`.

2.  Method arguments `arg1` through `argN` are pushed onto the stack, just as with an unprefixed `callvirt` instruction.

 The `constrained.` prefix is designed to allow `callvirt` instructions to be made in a uniform way independent of whether `thisType` is a value type or a reference type.

 When a `callvirt` `method` instruction has been prefixed by `constrained.` `thisType`, the instruction is executed as follows:

-   If `thisType` is a reference type (as opposed to a value type) then `ptr` is dereferenced and passed as the 'this' pointer to the `callvirt` of `method`.

-   If `thisType` is a value type and `thisType` implements `method` then `ptr` is passed unmodified as the 'this' pointer to a `call` `method` instruction, for the implementation of `method` by `thisType`.

-   If `thisType` is a value type and `thisType` does not implement `method` then `ptr` is dereferenced, boxed, and passed as the 'this' pointer to the `callvirt` `method` instruction.

 This last case can occur only when `method` was defined on `stem.Object`, `System.ValueType`, or `System.Enum` and not overridden by `thisType`. In this case, the boxing causes a copy of the original object to be made. However, because none of the methods of `System.Object`, `System.ValueType`, and `System.Enum` modify the state of the object, this fact cannot be detected.

 The `constrained.` prefix supports IL generators that create generic code. Normally the `callvirt` instruction is not valid on value types. Instead it is required that IL compilers effectively perform the 'this' transformation outlined above at compile time, depending on the type of `ptr` and the method being called. However, when `ptr` is a generic type that is unknown at compile time, it is not possible to make this transformation at compile time.

 The `constrained.` opcode allows IL compilers to make a call to a virtual function in a uniform way independent of whether `ptr` is a value type or a reference type. Although it is intended for the case where `thisType` is a generic type variable, the `constrained.` prefix also works for nongeneric types and can reduce the complexity of generating virtual calls in languages that hide the distinction between value types and reference types.

 Using the `constrained.` prefix also avoids potential versioning problems with value types. If the `constrained.` prefix is not used, different IL must be emitted depending on whether or not a value type overrides a method of `System.Object`. For example, if a value type `V` overrides the `Object.ToString()` method, a `call` `V.ToString()` instruction is emitted; if it does not, a `box` instruction and a `callvirt` `Object.ToString()` instruction are emitted. A versioning problem can arise in the former case if the override is later removed, and in the latter case if an override is later added.

 The `constrained.` prefix can also be used for invocation of interface methods on value types, because the value type method implementing the interface method can be changed using a `MethodImpl`. If the `constrained.` prefix is not used, the compiler is forced to choose which of the value type's methods to bind to at compile time. Using the `constrained.` prefix allows the MSIL to bind to the method that implements the interface method at run time, rather than at compile time.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
