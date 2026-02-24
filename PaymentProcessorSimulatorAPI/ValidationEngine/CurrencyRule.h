#pragma once
#include "IValidationRule.h"
#include "Payment.h"
#include <set>

class CurrencyRule : public IValidationRule<Payment> {
public:
    void validate(const Payment& p, ValidationResult& result) const override {
        static const std::set<std::string> validCurrencies = { "USD", "EUR", "GBP" };

        if (validCurrencies.find(p.currency) == validCurrencies.end()) {
            result.addError("Currency is not valid. Must be USD/EUR/GBP.");
        }
    }
};