Describe 'OpCode signatures' {
    BeforeAll {
        . "$($PSCommandPath | Split-Path)/Common.ps1"
    }

    Context 'method signatures' {
        It 'call static' {
            il { [void]@() } {
                call { [void] [Console]::WriteLine() }
                ret
            } | Should -HaveBody (0x28, 0x02, 0x00, 0x00, 0x06, 0x2A)
        }

        It 'callvirt instance' {
            il { [string]([object]) } {
                ldarg.0
                callvirt { [string] [object].ToString() }
                ret
            } | Should -HaveBody (0x02, 0x6F, 0x02, 0x00, 0x00, 0x06, 0x2A)
        }

        It 'calli managed' {
            il { [void]@() } {
                ldtoken { [void] [Console]::WriteLine() }
                calli default { [void]@() }
                ret
            } | Should -HaveBody (0xD0, 0x02, 0x00, 0x00, 0x06, 0x29, 0x03, 0x00, 0x00, 0x11, 0x2A)
        }

        It 'calli unmanaged' {
            il { [int]@([IntPtr] $methodPointer, [IntPtr] $this, [int] $arg0) } {
                ldarg.1
                ldarg.2

                ldarg.0
                calli unmanaged stdcall { [int]([void+], [int]) }
                ret
            } | Should -HaveBody (0x03, 0x04, 0x02, 0x29, 0x02, 0x00, 0x00, 0x11, 0x2A)
        }
    }

    Context 'ldtoken' {
        It 'Type' {
            il { [type]@() } {
                ldtoken { [Text.StringBuilder] }
                call { [type] [type]::GetTypeFromHandle([RuntimeTypeHandle]) }
                ret
            } | Should -HaveBody (0xD0, 0x02, 0x00, 0x00, 0x02, 0x28, 0x03, 0x00, 0x00, 0x06, 0x2A)
        }

        It 'Field' {
            il { [Reflection.FieldInfo]@() } {
                ldtoken { [string] [string]::Empty }
                call { [Reflection.FieldInfo] [Reflection.FieldInfo]::GetFieldFromHandle([RuntimeFieldHandle]) }
                ret
            } | Should -HaveBody (0xD0, 0x02, 0x00, 0x00, 0x04, 0x28, 0x03, 0x00, 0x00, 0x06, 0x2A)
        }

        It 'Method' {
            il { [Reflection.MethodInfo]@() } {
                ldtoken { [int[]] [array]::Empty([g[int]]) }
                call { [Reflection.MethodBase] [Reflection.MethodBase]::GetMethodFromHandle([RuntimeMethodHandle]) }
                ret
            } | Should -HaveBody (0xD0, 0x02, 0x00, 0x00, 0x06, 0x28, 0x03, 0x00, 0x00, 0x06, 0x2A)
        }
    }

    It 'switch' {
        il { [string]([int] $value) } {
            ldarg.0
            . switch zero, one, two, three
            ldstr 'value'
            newobj { [void] [ArgumentOutOfRangeException].new([string]) }
            throw

            zero:  ldstr 'zero'; ret
            one:   ldstr 'one'; ret
            two:   ldstr 'two'; ret
            three: ldstr 'three'; ret
        } | Should -HaveBody (
            0x02, 0x45, 0x04, 0x00, 0x00, 0x00,
            0x0B, 0x00, 0x00, 0x00, 0x11, 0x00,
            0x00, 0x00, 0x17, 0x00, 0x00, 0x00,
            0x1D, 0x00, 0x00, 0x00, 0x72, 0x02,
            0x00, 0x00, 0x70, 0x73, 0x03, 0x00,
            0x00, 0x06, 0x7A, 0x72, 0x04, 0x00,
            0x00, 0x70, 0x2A, 0x72, 0x05, 0x00,
            0x00, 0x70, 0x2A, 0x72, 0x06, 0x00,
            0x00, 0x70, 0x2A, 0x72, 0x07, 0x00,
            0x00, 0x70, 0x2A)
    }
}
