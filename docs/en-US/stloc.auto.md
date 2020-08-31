---
external help file: ILAssembler-help.xml
online version: https://github.com/SeeminglyScience/ILAssembler/blob/master/docs/en-US/stloc.auto.md
schema: 2.0.0
---

# stloc.auto

## SYNOPSIS

Pops the current value from the top of the evaluation stack and stores it in the local variable list at the index corresponding to the specified variable expression.

## SYNTAX

```powershell
stloc.auto <psvariable>
```

## DESCRIPTION

The `stloc.auto` instruction is a meta instruction specific to the `ILAssembler` PowerShell module. By passing a variable expression (e.g. `$varName`) that corresponds with a local defined in the `.locals` directive, this instruction will be replaced with the most appropriate `stloc*` instruction.

 The stack transitional behavior, in sequential order, is:

1.  A value is popped off of the stack and placed in specified local variable.

 The `stloc.auto` instruction pops the top value off the evaluation stack and moves it into the specified local variable. The type of the value must match the type of the local variable as specified in the current method's local signature.

 Storing into locals that hold an integer value smaller than 4 bytes long truncates the value as it moves from the stack to the local variable. Floating-point values are rounded from their native size (type `F`) to the size associated with the argument.

## PARAMETERS

### -variable

Specifies the target variable location.

```yaml
Type: int32
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
