#include "pch.h"
#include "IValidationRule.h"
#include "Payment.h"
#include "ValidationResult.h"
#include <iostream>
#include <fstream>

class CurrencyRule : public IValidationRule<Payment> {
public:
	void validate(const Payment& p, ValidationResult& result) const override {
		// Always output to console for debugging
		std::cout << "[CurrencyRule] Validating currency: '" << p.currency << "'" << std::endl;
		
		if (p.currency != "USD" && p.currency != "EUR") {
			std::cout << "[CurrencyRule] INVALID currency detected: '" << p.currency << "'" << std::endl;
			result.addError("Currency must be USD or EUR.");
		} else {
			std::cout << "[CurrencyRule] VALID currency: '" << p.currency << "'" << std::endl;
		}
	}
};