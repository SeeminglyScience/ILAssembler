<h1 align="center">ILAssembler</h1>

<p align="center">
    <sub>
        Use Common Intermediate Language (CIL/MSIL) in PowerShell to build invokable delegates using a domain specific
        language similar to ILAsm.
    </sub>
    <br />
    <a href="https://github.com/SeeminglyScience/ILAssembler/commits/master">
        <img src="https://github.com/SeeminglyScience/ILAssembler/workflows/build/badge.svg" alt="BuildStatus" />
    </a>
    <a href="https://www.powershellgallery.com/packages/ILAssembler">
        <img alt="PowerShell Gallery Version" src="https://img.shields.io/powershellgallery/v/ILAssembler" />
    </a>
</p>


## Features

- All opcodes are supported
- Help documentation for each instruction (including on hover in editors)
- Exception regions (`try`/`catch`/`finally`)
- `.locals` declaration including pinned types and `init` switch

## Getting Started

### Install

```powershell
# AllowClobber probably not necessary.
Install-Module ILAssembler -Scope CurrentUser -Force -AllowClobber
```

### `il` blocks

```powershell
$delegate = il { [int]([object[]], [int]) } {
    ldarg.0
    ldlen
    ldarg.1
    add
    ret
}

$delegate.GetType().ToString()
# returns: System.Func`3[System.Object[],System.Int32,System.Int32]

$delegate.Invoke(0..10, 1)
# returns: 12
```

Creates a delegate that takes `object[]` and `int` as parameters and returns `int`. The first `ScriptBlock` argument
declares the delegate's signature.

### Signatures

Various opcodes take a signature that is used to resolve a reference.

#### Anonymous Method Signature

The same signature used in the first argument of an `il` block, but also for `calli` instructions.
Think of it like a method invocation (e.g. `[Console]::WriteLine([string])`) but without the subject
or member name (e.g. `[Console]::WriteLine`), plus the return type.

```powershell
# Takes no parameters and has a void return.
{ [void]@() }

# Takes string and double as parameters, returns int
{ [int]([string], [double]) }

# Takes string and int& as parameters, returns bool
{ [bool]([string], [ref] [int]) }

# Same as above, subject, member name, and parameter names are ignored
{ [bool] $_::TryParse([string] $s, [ref] [int] $result) }

# Takes IntPtr, IntPtr, returns int
$callMethodPointer = il { [int]([IntPtr] $iunknownPointer, [IntPtr] $methodPointer) } {
    ldarg.0
    ldarg.1

    # Takes void*, returns int
    calli unmanaged stdcall { [int]([void+] $this) }
    ret
}
```

#### Resolvable Method Signature

These signatures *must* resolve to an existing method defined by a type loaded in the AppDomain at assemble time.

```powershell
# Resolves to the static method Console.WriteLine(string)
call { [void] [Console]::WriteLine([string]) }

# Resolves to Unsafe.AsRef<int>(void*)
# (use [g[arg1, arg2]] as the first argument to specify generic type arguments)
call { [ref] [int] [System.Runtime.CompilerServices.Unsafe]::AsRef([g[int]], [void+]) }

# Resolves to the instance property String.Length
callvirt { [int] [string].get_Length() }
```

#### Resolvable Type Signature

This is a normal type expression wrapped in a `ScriptBlock` with some extra shortcuts for resolving pointers and byref types.

```powershell
# System.String
newarr { [string] }

# System.Int64**
ldobj { [long++] }

# System.Int32&
stobj { [ref] [int] }
```

#### Resolvable Field Signature

Must resolve to an existing field defined by a type loaded in the AppDomain at assemble time.

```powershell
ldsfld { [string] [string]::Empty }

ldfld { [int] [ValueTuple[int]].Item1 }
```

### Meta Instructions

#### `.maxstack`

```powershell
.maxstack 8
```

Equivilent of `.maxstack` in ILAsm. Declares the maximum amount of items that will be on the
evaluation stack. Defaults to `8` if not specified.

**NOTE:** Must be before any opcodes.

#### `.locals`

```powershell
.locals { }
.locals init { }
```

Declares local variables, equivilent of `.locals` in ILAsm. Specifying `init` will initialize locals
to zero on the stack at the start of the method (this is the default behavior in C#).

**NOTE:** Must be before any opcodes.

```powershell
using namespace System.Runtime.CompilerServices

$copyArray = il { [void]([int[]] $source, [int[]] $destination) } {
    .locals init {
        [int+] $pSource,
        [int+] $pDest,
        [pinned] [int[]] $pinnedSource,
        [pinned] [int[]] $pinnedDest
    }

    ldarg.0
    stloc.auto $pinnedSource

    ldarg.1
    stloc.auto $pinnedDest

    ldloc.auto $pinnedSource
    ldc.i4.0
    ldelema { [int] }
    conv.i
    stloc.auto $pSource

    ldloc.auto $pinnedDest
    ldc.i4.0
    ldelema { [int] }
    conv.i
    stloc.auto $pDest

    ldloc.auto $pDest
    ldloc.auto $pSource
    ldloc.auto $pinnedSource
    ldlen
    sizeof { [int] }
    mul
    conv.u4
    call { [void] [Unsafe]::CopyBlock([void+], [void+], [uint]) }
    ret
}
```

### Branch Labels

Labels work the same as in ILAsm. They can prefix an instruction or be placed on their own line. When
marking a label, it must be suffixed with a colon (<kbd>:</kbd>) character.

```powershell
            idc.4.0
            brtrue.s was_true
            br.s invalid
was_true:   ldc.4.s 10
            ret

            invalid:
            newobj { [void] [Exception].new() }
            throw
```

#### Switch instruction

Since the `switch` opcode doesn't actually fit the syntax of a PowerShell `switch` statement, the syntax
cannot be used. Since it's a infrequently used opcode, you'll need to force it to be parsed as a command
with an invocation operator.

```powershell
il { [void]([int] $input) } {
    ldarg.0
    switch was_0, was_1, was_2, was_3
    ldstr 'input'
    newobj { [void] [ArgumentOutOfRangeException].new([string]) }
    throw

    was_0:      ldstr 'input was 0'; br.s writeline
    was_1:      ldstr 'input was 1'; br.s writeline
    was_2:      ldstr 'input was 2'; br.s writeline
    was_3:      ldstr 'input was 3'
    write_line: call { [void] [Console]::WriteLine([string]) }
                ret
}
```

### Exception Handling

All of the exception handling regions are supported (`try`, `catch`, `filter`, `fault`, and `finally`).

#### `.try`

[Protected blocks][protected] (aka `.try` blocks) work as they do in ILAsm. When encoding the exception regions,
the first enclosed instruction will be the start offset, directly after the last will be the end offset.

Protected blocks must only have *one* type of exception handler attached, though they can be nested.

#### `catch`

[Catch blocks][catch] must have a catch type declared unless preceeded by a `filter` block. For a "catch all", type
the catch as `object`.

```powershell
.try {
    newobj { [void] [InvalidOperationException].new() }
    throw
}
catch { [InvalidOperationException] } {
    pop
    leave.s afterTry
}
# catch all
catch { [object] } {
    rethrow
}

afterTry: ret
```

#### `filter`

**NOTE:** Because `filter` is an existing PowerShell keyword with conflicting syntax, it needs to be preceeded by an invocation operator such as <kbd>&</kbd> or <kbd>.</kbd>.

A [filter block][filter] determines whether the following catch block should handle the exception.  When leaving
the filter, a value is popped from the stack to determine if the catch should be executed.

1. [`exception_continue_search` (`0x0`)][endfilter] indicates the exception did not match
1. [`exception_execute_handler` (`0x1`)][endfilter] indicates that execution should be transfered to the following `catch` block

Below is an example of a filter in C# with it's rough translation into CIL.

```csharp
try
{
    throw new Exception() { HResult = 0x10 };
}
catch (Exception e) when (e.HResult is 0x10)
{
    // Do nothing.
}
catch
{
    throw;
}
```

```powershell
.try {
    newobj { [void] [Exception].new() }
    dup
    ldc.i4.s 0x10
    callvirt { [void] [Exception].set_HResult([int]) }
    throw
}
. filter {
    callvirt { [int] [Exception].get_HResult() }
    ldc.i4.s 0x10
    ceq
    endfilter
}
catch {
    pop
    leave.s exitSEH
}
catch { [object] } {
    rethrow
}

exitSEH: ret
```

#### `finally`

The [finally block][finally] will always run when the `try` completes, even if an exception is thrown.

```powershell
.try {
    .try {
        # The finally will run even if you change this to ArgumentNullException.
        newobj { [void] [InvalidOperationException].new() }
        throw
    }
    catch { [ArgumentNullException] } {
        pop
        leave.s exitSEH
    }
}
finally {
    ldstr 'This always runs'
    call { [void] [Console]::WriteLine([string]) }
    endfinally
}

exitSEH: ret
```

#### `fault`

The [fault block][fault] acts just like `finally` except it *does not* get executed when the try block completes normally. It is only executed when there was an uncaught exception. In this way it's like a "catch all" style `catch` block but it **does not** handle the exception.

```powershell
.try {
    .try {
        # The finally will run even if you change this to ArgumentNullException.
        newobj { [void] [InvalidOperationException].new() }
        throw
    }
    catch { [ArgumentNullException] } {
        pop
        leave.s exitSEH
    }
}
fault {
    ldstr 'This only runs if the exception is *not* caught above.'
    call { [void] [Console]::WriteLine([string]) }
    endfault
}

exitSEH: ret
```

[protected]: https://www.ecma-international.org/publications/files/ECMA-ST/ECMA-335.pdf#page=227&zoom=100,116,722 "EMCA §II.19.1"
[catch]: https://www.ecma-international.org/publications/files/ECMA-ST/ECMA-335.pdf#page=228&zoom=100,116,210 "EMCA §II.19.3"
[filter]: https://www.ecma-international.org/publications/files/ECMA-ST/ECMA-335.pdf#page=228&zoom=100,116,509 "EMCA §II.19.4"
[finally]: https://www.ecma-international.org/publications/files/ECMA-ST/ECMA-335.pdf#page=229&zoom=100,116,120 "EMCA §II.19.5"
[fault]: https://www.ecma-international.org/publications/files/ECMA-ST/ECMA-335.pdf#page=229&zoom=100,116,429 "EMCA §II.19.6"
[endfilter]: https://www.ecma-international.org/publications/files/ECMA-ST/ECMA-335.pdf#page=385&zoom=100,116,104 "EMCA §III.3.34"
