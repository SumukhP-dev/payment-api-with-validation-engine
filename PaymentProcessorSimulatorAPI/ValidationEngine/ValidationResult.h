#pragma once
#include <string>
#include <vector>

class ValidationResult {
public:
    bool isValid = true;
    std::vector<std::string> errors;

    void addError(const std::string& message) {
        isValid = false;
        errors.push_back(message);
    }
};