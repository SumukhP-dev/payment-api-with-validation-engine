#pragma once

#ifdef MYDEBUGDLL_EXPORTS
#define MYDEBUG_API __declspec(dllexport)
#else
#define MYDEBUG_API __declspec(dllimport)
#endif

extern "C"
{
	// Enables the CRT debug leak detection features
	MYDEBUG_API void EnableLeakDetection();

	// Optionally you can export other utility functions here...
	// e.g. MYDEBUG_API void TestMemoryLeak();
}