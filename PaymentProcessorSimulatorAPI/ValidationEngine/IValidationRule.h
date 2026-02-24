#pragma once
#include "ValidationResult.h"

template <typename T>
class IValidationRule {
public:
    virtual ~IValidationRule() = default;
    virtual void validate(const T& obj, ValidationResult& result) const = 0;
};