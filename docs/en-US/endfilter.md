---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.endfilter
schema: 2.0.0
---

# endfilter

## SYNOPSIS

Transfers control from the `filter` clause of an exception back to the Common Language Infrastructure (CLI) exception handler.

## SYNTAX

```powershell
endfilter
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| FE 11  | endfilter       |

 The stack transitional behavior, in sequential order, is:

1.  `value` is pushed onto the stack.

2.  `value` is popped from the stack; `endfilter` is executed and control is transferred to the exception handler.

 `Value` (which must be of type `int32` and is one of a specific set of values) is returned from the filter clause. It should be one of:

-   `exception_continue_search` (`value` = 0) to continue searching for an exception handler

-   `exception_execute_handler` (`value` = 1) to start the second phase of exception handling where finally blocks are run until the handler associated with this filter clause is located. Upon discovery, the handler is executed.

 Other integer values will produce unspecified results.

 The entry point of a filter, as shown in the method's exception table, must be the first instruction in the filter's code block. The `endfilter` instruction must be the last instruction in the filter's code block (hence there can only be one `endfilter` for any single filter block). After executing the `endfilter` instruction, control logically flows back to the CLI exception handling mechanism.

 Control cannot be transferred into a filter block except through the exception mechanism. Control cannot be transferred out of a filter block except through the use of a `throw` instruction or by executing the final `endfilter` instruction. You cannot embed a `try` block within a `filter` block. If an exception is thrown inside the `filter` block, it is intercepted and a value of 0 (`exception_continue_search`) is returned.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
