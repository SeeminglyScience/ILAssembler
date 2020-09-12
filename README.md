# ILAssembler

Use Common Intermediate Language (CIL/MSIL) in PowerShell to build invokable delegates using a domain specific
language similar to ILAsm.

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
