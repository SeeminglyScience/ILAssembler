---
external help file: ILAssembler-help.xml
online version: https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.tailcall
schema: 2.0.0
---

# tailcall

## SYNOPSIS

Performs a postfixed method call instruction such that the current method's stack frame is removed before the actual call instruction is executed.

## SYNTAX

```powershell
tailcall
```

## DESCRIPTION

The following table lists the instruction's hexadecimal and Microsoft Intermediate Language (MSIL) assembly format, along with a brief reference summary:

| Format | Assembly Format |
| ------ | --------------- |
| FE 14  | tail.           |

 There is no stack transition behavior defined for this instruction.

 The `tail` prefix instruction must immediately precede a `call`, `calli`, or `callvirt` instruction. It indicates that the current method's stack frame should be removed before the call instruction is executed. It also implies that the value returned from the following call is also the value returned by the current method, and the call can therefore be converted into a cross-method jump.

 The stack must be empty except for the arguments being transferred by the following call. The instruction following the call instruction must be a ret. Thus the only valid code sequence is `tail. call` (or `calli` or `callvirt`). Correct Microsoft Intermediate Language (MSIL) instructions must not branch to the `call` instruction, but they may branch to the subsequent `ret`.

 The current frame cannot be discarded when control is transferred from untrusted code to trusted code, since this would jeopardize code identity security. The .NET Framework security checks can therefore cause the `tail` to be ignored, leaving a standard `call` instruction. Similarly, in order to allow the exit of a synchronized region to occur after the call returns, the `tail` prefix is ignored when used to exit a method that is marked synchronized.

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
