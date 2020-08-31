---
external help file: ILAssembler-help.xml
online version: https://github.com/SeeminglyScience/ILAssembler/blob/master/docs/en-US/ldloca.auto.md
schema: 2.0.0
---

# ldloca.auto

## SYNOPSIS

Loads the address of the specified local variable onto the evaluation stack.

## SYNTAX

```powershell
ldloca.auto <psvariable>
```

## DESCRIPTION

The `ldloca.auto` instruction is a meta instruction specific to the `ILAssembler` PowerShell module. By passing a variable expression (e.g. `$varName`) that corresponds with a local defined in the `.locals` directive, this instruction will be replaced with the most appropriate `ldloca*` instruction.

 The stack transitional behavior, in sequential order, is:

1.  The address stored in the specified local variable is pushed onto the stack.

 The `ldloca.auto` instruction pushes the address of the local variable number at the passed index onto the stack, where local variables are numbered 0 onwards. The value pushed on the stack is already aligned correctly for use with instructions like `ldind.i` and `stind.i`. The result is a transient pointer (type `*`).

## PARAMETERS

### -variable

Specifies the variable to load.

```yaml
Type: psvariable
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

## INPUTS

### None

This function cannot be used with the pipeline.

## OUTPUTS

### None

This function cannot be used with the pipeline.

## NOTES

## RELATED LINKS
