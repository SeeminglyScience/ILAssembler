---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.break
schema: 2.0.0
---

# break

## SYNOPSIS

Signals the Common Language Infrastructure (CLI) to inform the debugger that a break point has been tripped.

## SYNTAX

```powershell
break
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| 01     | break           |

 No evaluation stack behaviors are performed by this operation.

 The `break` instruction is for debugging support. It signals the CLI to inform the debugger that a break point has been tripped. It has no other effect on the interpreter state.

 The `break` instruction has the smallest possible instruction size enabling code patching with a break point and generating minimal disturbance to the surrounding code.

 The `break` instruction can trap to a debugger, do nothing, or raise a security exception. The exact behavior is implementation-defined.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
