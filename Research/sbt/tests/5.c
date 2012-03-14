#include <stdio.h> // printf
#define __USE_XOPEN_EXTENDED
#include <stdlib.h> // random
#include <time.h> // time

#include "sbt.h"

int main() {

	// пятый пример
	for (int i = 0; i < 10; i++)
	    SBT_AddUniq(i*2);
	for (int i = 0; i < 10; i++)
	    SBT_AddUniq(i*2 + 1);
	for (int i = 0; i < 10; i++)
	    SBT_AddUniq(i*2 + 1); // fail

	SBT_PrintAllNodes();
	SBT_CheckAllNodes();
	
	TNumber n = SBT_FindFirstNode(2); // = обычный Find для уникального ключа (number)
	printf("%lld\n", (long long int)n);
	
	for (int i = 0; i < 100; i++)
	    SBT_Delete(i);

	SBT_CheckAllNodes();
	SBT_PrintAllNodes();
	SBT_DumpAllNodes();

	return 0;
}