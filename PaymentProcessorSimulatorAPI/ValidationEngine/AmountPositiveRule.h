#pragma once
#include "IValidationRule.h"
#include "Payment.h"

class AmountPositiveRule : public IValidationRule<Payment> {
public:
    void validate(const Payment& p, ValidationResult& result) const override {
        if (p.amount <= 0)
            result.addError("Payment amount must be greater than zero.");
    }
};