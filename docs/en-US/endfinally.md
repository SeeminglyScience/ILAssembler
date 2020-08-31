---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.endfinally
schema: 2.0.0
---

# endfinally

## SYNOPSIS

Transfers control from the `fault` or `finally` clause of an exception block back to the Common Language Infrastructure (CLI) exception handler.

## SYNTAX

```powershell
endfinally
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format       |
| ------ | --------------------- |
| DC     | endfinally / endfault |

 There are no stack transition behaviors for this instruction.

 `Endfinally` and `endfault` signal the end of the `finally` or `fault` clause so that stack unwinding can continue until the exception handler is invoked. The `endfinally` or `endfault` instruction transfers control back to the CLI exception mechanism. The mechanism then searches for the next `finally` clause in the chain if the protected block was exited with a leave instruction. If the protected block was exited with an exception, the CLI will search for the next `finally` or `fault`, or enter the exception handler chosen during the first pass of exception handling.

 An `endfinally` instruction might only appear lexically within a `finally` block. Unlike the `endfilter` instruction, there is no requirement that the block end with an `endfinally` instruction, and there can be as many `endfinally` instructions within the block as required. These same restrictions apply to the `endfault` instruction and the `fault` block.

 Control cannot be transferred into a `finally` (or `fault`) block except through the exception mechanism. Control cannot be transferred out of a `finally` (or `fault`) block except through the use of a `throw` instruction or executing the `endfinally` (or `endfault`) instruction. In particular, you cannot "fall out" of a `finally` (or `fault`) block or to execute a `ret` or `leave` instruction within a `finally` (or `fault`) block.

 Note that the `endfault` and `endfinally` instructions are aliases - they correspond to the same opcode.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
