# ABOUT BAD IMAGE FORMAT

## SHORT DESCRIPTION

Describes some possible causes of a `BadImageFormatException` or
`InvalidProgramException` when using the `New-ILDelegate` command.

## LONG DESCRIPTION

The `BadImageFormatException` or `InvalidProgramException` is thrown by the
Common Language Runtime (CLR) whenever it detects invalid IL. This topic will
go over some of the potential reasons this can occur. This is far from an
exhaustive list and mostly goes over the common cases.

### Invalid evaluation stack state

When returning from a method the evaluation stack must either be empty (in the
case of a `void` return method) or contain exactly one item (in the case of a
return).

The following example shows an evaluation stack with too many items.

```powershell
il { [int]@() } {
    # Load `0` as an int32 onto the eval stack.
    ldc.i4.0

    # Load a second `0` as an int32 onto the eval stack.
    ldc.i4.0

    # The CLR expects only a single item on the eval stack, but sees two.
    ret
}
```

The following example shows an evaluation stack with too few items.

```powershell
il { [int]@() } {
    # Load `0` as an int32 onto the eval stack.
    ldc.i4.0

    # Pull an int32 off the eval stack and pass to Console.WriteLine as an
    # argument.
    call { [void] [Console]::WriteLine([int]) }

    # Eval stack is empty, but the method is declared to return an int32.
    ret
}
```

### .maxstack is too low

Methods must declare to the CLR the maximum amount of items that they will put
on the evaluation stack. The default for this module (when not specified with
`.maxstack`) is `8`.

```powershell
il { [void]@() } {
    ldc.i4.0
    ldc.i4.0
    ldc.i4.0
    ldc.i4.0
    ldc.i4.0
    ldc.i4.0
    ldc.i4.0
    ldc.i4.0

    # Since `.maxstack` has not been specified it has defaulted to 8. This
    # nineth item will result in invalid IL. Adding `.maxstack 9` would make
    # this valid.
    ldc.i4.0
    pop; pop; pop; pop; pop; pop; pop; pop; pop
    ret
}
```

```powershell
il { [void]@() } {
    .maxstack 1
    ldc.i4.0

    # Since `.maxstack` was declared as 1, this second item will result in
    # invalid IL. Changing `.maxstack` to 2 would make this valid.
    ldc.i4.0
    pop; pop
    ret
}
```

### Leaving a structured exception handling region incorrectly

Exiting a structured exception handling (SEH) region requires a specific
instruction for each type of SEH region.

The following table describes which instruction to use when exiting each SEH
region type.

| SEH Type  | Instruction        |
| --------- | ------------------ |
| `catch`   | `leave.s`, `leave` |
| `finally` | `endfinally`       |
| `fault`   | `endfault`         |
| `filter`  | `endfilter`        |

Incorrect:

```powershell
il { [void]@() } {
    .try {
        newobj { [void] [Exception].new() }
        throw
    }
    catch {
        # Invalid, cannot `ret` from a catch block.
        ret
    }
}
```

Correct:

```powershell
il { [void]@() } {
    .try {
        newobj { [void] [Exception].new() }
        throw
    }
    catch {
        # Pop the exception off the eval stack.
        pop
        leave.s exitSEH
    }

    exitSEH: ret
}
```

### Missing `ret`

A method must use the `ret` instruction to exit, regardless of if it has a
non-void return.

Incorrect:

```powershell
il { [void]@() } {
    ldstr 'Does not return, but still must exit with ret'
    call { [void] [Console]::WriteLine([string]) }
}
```

Correct:

```powershell
il { [void]@() } {
    ldstr 'Does not return, but still must exit with ret'
    call { [void] [Console]::WriteLine([string]) }
    ret
}
```
