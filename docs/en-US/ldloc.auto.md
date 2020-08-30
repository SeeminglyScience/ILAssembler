---
external help file: ILAssembler-help.xml
online version: https://github.com/SeeminglyScience/ILAssembler/blob/master/docs/en-US/ldloc.auto.md
schema: 2.0.0
---

# ldloc.auto

## SYNOPSIS

Loads the specified local variable evaluation stack.

## SYNTAX

```powershell
ldloc.auto <psvariable>
```

## DESCRIPTION

The `ldloc.auto` instruction is a meta instruction specific to the `ILAssembler` PowerShell module. By passing a variable expression (e.g. `$varName`) that corresponds with a local defined in the `.locals` directive, this instruction will be replaced with the most appropriate `ldloc*` instruction.

 The stack transitional behavior, in sequential order, is:

1.  The local variable value specified by the variable expression is pushed onto the stack.

 The type of the value is the same as the type of the local variable, which is specified in the method header. See Partition I. Local variables that are smaller than 4 bytes long are expanded to type `int32` when they are loaded onto the stack. Floating-point values are expanded to their native size (type `F`).

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
