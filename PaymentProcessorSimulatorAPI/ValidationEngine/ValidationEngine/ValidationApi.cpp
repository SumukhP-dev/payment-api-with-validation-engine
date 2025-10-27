#include "pch.h"   // ✅ If PCH enabled

#include "Payment.h"
#include "ValidationEngine.h"
#include "ValidationResult.h"
#include "AmountPositiveRule.h"
#include "CustomerNameRule.h"
#include "CurrencyRule.h"

#include <memory>
#include <string>

extern "C" {
#ifdef _WIN32
	__declspec(dllexport)
#else
	__attribute__((visibility("default")))
#endif
		bool ValidatePayment(const char* name, double amount, const char* currency,
			char* errorBuffer, int bufferSize)
	{
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
			}

#ifdef _MSC_VER // if compiling on MSVC
			strncpy_s(errorBuffer, bufferSize, errors.c_str(), _TRUNCATE);
#else
			std::strncpy(errorBuffer, errors.c_str(), bufferSize - 1);
			errorBuffer[bufferSize - 1] = '\0';
#endif

			errorBuffer[bufferSize - 1] = '\0';
		}

		return result.isValid;
	}
}