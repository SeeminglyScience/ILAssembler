using namespace System.ComponentModel
using namespace System.Diagnostics
using namespace System.Runtime.InteropServices

# Windows x64
# int(int* args, int count)
# {
#     int total = 0;
#     for (int i = 0; i < count; i++)
#     {
#         total += args[i];
#     }
#
#     return total;
# }
$assembly = (
    0x85, 0xD2,              # test edx,edx
    0x7E, 0x14,              # jle  18 <LBB0_1>
    0x41, 0x89, 0xD0,        # mov  r8d,edx
    0x31, 0xD2,              # xor  edx,edx
    0x31, 0xC0,              # xor  eax,eax
# LBB0_4:
    0x03, 0x04, 0x91,        # add  eax,DWORD PTR [rcx+rdx*4]
    0x48, 0x83, 0xC2, 0x01,  # add  rdx,0x1
    0x49, 0x39, 0xD0,        # cmp  r8,rdx
    0x75, 0xF4,              # jne  b <LBB0_4>
    0xC3,                    # ret
# LBB0_1
    0x31, 0xC0,              # xor  eax,eax
    0xC3                     # ret
)

$delegate = il { [int]([byte[]] $asm, [int[]] $args) } {
    .locals init {
        [void+] $virtualProtectEx,

        [void+] $getLastError,

        [uint] $flOldProtect,

        [pinned] [byte[]] $pinnedAsm,

        [pinned] [int[]] $pinnedArgs
    }

    # [DllImport("kernel32")]
    # static extern int VirtualProtectEx(void*, void*, int, int, int*)
    ldstr 'kernel32'
    call { [IntPtr] [NativeLibrary]::Load([string]) }
    dup
    ldstr 'VirtualProtectEx'
    call { [IntPtr] [NativeLibrary]::GetExport([IntPtr], [string]) }
    stloc.auto $virtualProtectEx

    # [DllImport("kernel32")]
    # static extern int GetLastError()
    ldstr 'GetLastError'
    call { [IntPtr] [NativeLibrary]::GetExport([IntPtr], [string]) }
    stloc.auto $getLastError

    # pinnedAsm = asm
    ldarg.0
    stloc.auto $pinnedAsm

    # Process.GetCurrentProcess().Handle
    call { [Process] [Process]::GetCurrentProcess() }
    callvirt { [IntPtr] [Process].get_Handle() }
    # &pinnedAsm[0]
    ldloc.auto $pinnedAsm
    ldc.i4.0
    ldelema { [byte] }
    # pinnedAsm.Length
    ldloc.auto $pinnedAsm
    ldlen
    # 0x40
    ldc.i4.s 0x40
    # &flOldProtect
    ldloca.auto $flOldProtect

    # VirtualProtectEx(Process.GetCurrentProcess().Handle, &pinnedAsm[0], pinnedAsm.Length, 0x40, &flOldProtect)
    ldloc.auto $virtualProtectEx
    calli unmanaged stdcall { [int]([void+], [void+], [int], [int], [int+]) }

    # if (vpResult != 0) goto success
    brtrue.s success

    # throw new Win32Exception(GetLastError())
    ldloc.auto $getLastError
    calli unmanaged stdcall { [int]@() }
    newobj { [void] [Win32Exception].new([int]) }
    throw

success:

    # pinnedArgs = args
    ldarg.1
    stloc.auto $pinnedArgs

    # &pinnedArgs[0]
    ldloc.auto $pinnedArgs
    ldc.i4.0
    ldelema { [int] }
    # pinnedArgs.Length
    ldloc.auto $pinnedArgs
    ldlen
    # &pinnedAsm[0]
    ldloc.auto $pinnedAsm
    ldc.i4.0
    ldelema { [byte] }

    # return ((delegate* unmanaged[Stdcall]<int*, int, int>)&pinnedAsm[0])(&pinnedArgs[0], pinnedArgs.Length)
    calli unmanaged stdcall { [int]([int+], [int]) }
    ret
}

$delegate.Invoke($assembly, @(20, 60, 324))
# returns: 404
