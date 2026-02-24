#include "pch.h"
#define _CRTDBG_MAP_ALLOC
#include <stdlib.h>
#include <crtdbg.h>

void EnableLeakDetection()
{
	int tmpFlag = _CrtSetDbgFlag(_CRTDBG_REPORT_FLAG);
	tmpFlag |= _CRTDBG_ALLOC_MEM_DF;        // Enable memory allocation tracking
	tmpFlag |= _CRTDBG_LEAK_CHECK_DF;       // Dump leaks automatically on exit
	_CrtSetDbgFlag(tmpFlag);
}

BOOL APIENTRY DllMain(HMODULE hModule,
	DWORD  ul_reason_for_call,
	LPVOID lpReserved)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		EnableLeakDetection();
		break;

	case DLL_PROCESS_DETACH:
		// Optional: you can call this manually if you want a mid-run dump.
		_CrtDumpMemoryLeaks();
		break;

	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
		break;
	}
	return TRUE;
}