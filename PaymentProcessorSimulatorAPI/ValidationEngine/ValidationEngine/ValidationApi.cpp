#include "pch.h"   // ✅ If PCH enabled

#include "Payment.h"
#include "ValidationEngine.h"
#include "ValidationResult.h"
#include "AmountPositiveRule.h"
#include "CustomerNameRule.h"
#include "CurrencyRule.h"

#include <memory>
#include <string>
#include <iostream>
#include <fstream>
#include <ctime>

// Debug includes for memory leak detection
#ifdef _DEBUG
#include <crtdbg.h>
#include <windows.h>
#endif

// Global debug flag
static bool g_debugEnabled = false;
static std::ofstream g_debugLog;

extern "C" {
#ifdef _WIN32
	__declspec(dllexport)
#else
	__attribute__((visibility("default")))
#endif
		bool ValidatePayment(const char* name, double amount, const char* currency,
			char* errorBuffer, int bufferSize)
	{
		// Debug logging
		if (g_debugEnabled) {
			g_debugLog << "[DEBUG] ValidatePayment called with: name=" << (name ? name : "null") 
					   << ", amount=" << amount << ", currency=" << (currency ? currency : "null") << std::endl;
		}

		Payment p{ name ? name : "", amount, currency ? currency : "" };

		ValidationEngine<Payment> engine;
		engine.addRule(std::make_unique<AmountPositiveRule>());
		engine.addRule(std::make_unique<CustomerNameRule>());
		engine.addRule(std::make_unique<CurrencyRule>());

		ValidationResult result = engine.validate(p);

		if (!result.isValid) {
			std::string errors;
			for (auto& e : result.errors) {
				errors += e + "\n";
				if (g_debugEnabled) {
					g_debugLog << "[DEBUG] Validation error: " << e << std::endl;
				}
			}

#ifdef _MSC_VER // if compiling on MSVC
			strncpy_s(errorBuffer, bufferSize, errors.c_str(), _TRUNCATE);
#else
			std::strncpy(errorBuffer, errors.c_str(), bufferSize - 1);
			errorBuffer[bufferSize - 1] = '\0';
#endif

			errorBuffer[bufferSize - 1] = '\0';
		}

		if (g_debugEnabled) {
			g_debugLog << "[DEBUG] Validation result: " << (result.isValid ? "VALID" : "INVALID") << std::endl;
			g_debugLog.flush();
		}

		return result.isValid;
	}

	// Debug functions
#ifdef _WIN32
	__declspec(dllexport)
#else
	__attribute__((visibility("default")))
#endif
	void EnableDebugMode(const char* logFilePath = "validation_debug.log")
	{
		g_debugEnabled = true;
		g_debugLog.open(logFilePath, std::ios::app);
		g_debugLog << "[DEBUG] Debug mode enabled at " << std::time(nullptr) << std::endl;
		
#ifdef _DEBUG
		// Enable memory leak detection
		_CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF);
		g_debugLog << "[DEBUG] Memory leak detection enabled" << std::endl;
#endif
	}

#ifdef _WIN32
	__declspec(dllexport)
#else
	__attribute__((visibility("default")))
#endif
	void DisableDebugMode()
	{
		if (g_debugEnabled) {
			g_debugLog << "[DEBUG] Debug mode disabled at " << std::time(nullptr) << std::endl;
			g_debugLog.close();
			g_debugEnabled = false;
		}
	}

#ifdef _WIN32
	__declspec(dllexport)
#else
	__attribute__((visibility("default")))
#endif
	void DumpMemoryLeaks()
	{
#ifdef _DEBUG
		if (g_debugEnabled) {
			g_debugLog << "[DEBUG] Dumping memory leaks..." << std::endl;
			_CrtDumpMemoryLeaks();
		}
#endif
	}

#ifdef _WIN32
	__declspec(dllexport)
#else
	__attribute__((visibility("default")))
#endif
	bool IsDebugModeEnabled()
	{
		return g_debugEnabled;
	}
}