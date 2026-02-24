#pragma once
#include "IValidationRule.h"
#include <memory>
#include <vector>

template <typename T>
class ValidationEngine {
    std::vector<std::unique_ptr<IValidationRule<T>>> rules;
public:
    void addRule(std::unique_ptr<IValidationRule<T>> rule) {
        rules.push_back(std::move(rule));
    }
    ValidationResult validate(const T& obj) const {
        ValidationResult result;
        for (auto& rule : rules)
            rule->validate(obj, result);
        return result;
    }
};