#pragma once
#include "IValidationRule.h"
#include "Payment.h"

class CustomerNameRule : public IValidationRule<Payment> {
public:
	void validate(const Payment& p, ValidationResult& result) const override {
		if (p.customerName.empty()) {
			result.addError("Customer name cannot be empty.");
		}
	}
};