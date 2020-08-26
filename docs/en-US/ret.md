---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.ret
schema: 2.0.0
---

# ret

## SYNOPSIS

Returns from the current method, pushing a return value (if present) from the callee's evaluation stack onto the caller's evaluation stack.

## SYNTAX

```powershell
ret
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| 2A     | ret             |

 The stack transitional behavior, in sequential order, is:

1.  The return value is popped from the callee evaluation stack.

2.  The return value obtained in step 1 is pushed onto the caller evaluation stack.

 If the return value is not present on the callee evaluation stack, no value is returned (no stack transition behaviors for either the callee or caller method).

 The type of the return value, if any, of the current method determines the type of value to be fetched from the top of the stack and copied onto the stack of the method that called the current method. The evaluation stack for the current method must be empty except for the value to be returned.

 The `ret` instruction cannot be used to transfer control out of a`try`, `filter`, `catch`, or `finally` block. From within a `try` or `catch`, use the `leave` instruction with a destination of a `ret` instruction that is outside all enclosing exception blocks. Because the `filter` and `finally` blocks are logically part of exception handling and not the method in which their code is embedded, correctly generated Microsoft Intermediate Language (MSIL) instructions do not perform a method return from within a `filter` or `finally`.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
