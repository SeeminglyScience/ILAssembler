Describe 'Delegate signatures' {
    BeforeAll {
        . "$($PSCommandPath | Split-Path)/Common.ps1"
    }

    It 'Array expression for no parameters' {
        $d = il { [int]@() } {
            ldc.i4.0
            ret
        }

        $d | Should -BeOfType ([Func[int]])
    }

    It 'Parameters in paren expression' {
        $d = il { [int]([int], [string]) } {
            ldc.i4.0
            ret
        }

        $d | Should -BeOfType ([Func[int, string, int]])
    }

    It 'Parameters in array expression' {
        $d = il { [int]@([object], [string]) } {
            ldc.i4.0
            ret
        }

        $d | Should -BeOfType ([Func[object, string, int]])
    }

    It 'Named parameters' {
        $d = il { [int]([object] $instance, [string] $name) } {
            ldc.i4.0
            ret
        }

        $d | Should -BeOfType ([Func[object, string, int]])
    }

    It 'Invoke member expression syntax' {
        $d = il { [int] $_._([object] $instance, [string] $name) } {
            ldc.i4.0
            ret
        }

        $d | Should -BeOfType ([Func[object, string, int]])
    }

    It 'Explicit delegate type' {
        $d = il ([Action[int]]) {
            ret
        }

        $d | Should -BeOfType ([Action[int]])
    }

    It 'Explicit non-standard delegate type' {
        $d = New-IlDelegate ([System.ResolveEventHandler]) {
            ret
        }

        $d | Should -BeOfType ([System.ResolveEventHandler])
        $method = $d.GetType().GetMethod('Invoke')
        $method.ReturnType | Should -Be ([System.Reflection.Assembly])
        $method.GetParameters().ParameterType | Should -Be ([object], [System.ResolveEventArgs])
    }

    It 'Generates delegate type when parameters cannot be generics' {
        $d = il { [bool]([int], [ref] [string], [int+]) } {
            ldc.i4.0
            ret
        }

        $method = $d.GetType().GetMethod('Invoke')
        $method.ReturnType | Should -Be ([bool])
        $method.GetParameters().ParameterType | Should -Be (
            [int],
            [string].MakeByRefType(),
            [int].MakePointerType())
    }
}
