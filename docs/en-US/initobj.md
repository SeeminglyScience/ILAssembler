---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.initobj
schema: 2.0.0
---

# initobj

## SYNOPSIS

Initializes each field of the value type at a specified address to a null reference or a 0 of the appropriate primitive type.

## SYNTAX

```powershell
initobj
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format        | Assembly Format     |
| ------------- | ------------------- |
| FE 15 < `T` > | `initobj` `typeTok` |

 The stack transitional behavior, in sequential order, is:

1.  The address of the value type to initialize is pushed onto the stack.

2.  The address is popped from the stack; the value type at the specified address is initialized as type `typeTok`.

 The `initobj` instruction initializes each field of the value type specified by the pushed address (of type `native int`, `&`, or `*`) to a null reference or a 0 of the appropriate primitive type. After this method is called, the instance is ready for a constructor method to be called. If `typeTok` is a reference type, this instruction has the same effect as `ldnull` followed by `stind.ref`.

 Unlike `newobj`, `initobj` does not call the constructor method. `initobj` is intended for initializing value types, while `newobj` is used to allocate and initialize objects.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
