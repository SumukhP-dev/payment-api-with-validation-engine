#include <Windows.h>
#include <iostream>

typedef void(__stdcall* EnableLeakDetectionFunc)();

int main()
{
	HMODULE hMod = LoadLibrary(L"MyDebugDll.dll");
	if (!hMod) {
		std::cerr << "Could not load DLL. Error: " << GetLastError() << "\n";
		return 1;
	}

	auto enableLeaks = (EnableLeakDetectionFunc)GetProcAddress(hMod, "EnableLeakDetection");
	if (!enableLeaks) {
		std::cerr << "Could not find function.\n";
		FreeLibrary(hMod);
		return 1;
	}

	enableLeaks();

	// Simulate some heap allocations to test memory leak detection
	char* leak = new char[100];
	std::cout << "Leaking 100 bytes (to test _CrtDumpMemoryLeaks)\n";

	std::cout << "Press Enter to exit and trigger DLL unload...\n";
	std::cin.get();

	FreeLibrary(hMod);  // triggers DLL_PROCESS_DETACH ¨ _CrtDumpMemoryLeaks()
	return 0;
}