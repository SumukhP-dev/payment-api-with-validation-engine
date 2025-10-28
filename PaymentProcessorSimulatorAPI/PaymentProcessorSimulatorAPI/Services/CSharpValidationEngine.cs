using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PaymentApi.Services
{
    /// <summary>
    /// Pure C# validation engine that replaces the C++ DLL for cross-platform compatibility
    /// </summary>
    public static class CSharpValidationEngine
    {
        private static readonly List<string> ValidCurrencies = new List<string> { "USD", "EUR" };
        private static readonly double MinAmount = 0.01;
        private static readonly double MaxAmount = 999999.99;

        /// <summary>
        /// Validates a payment using pure C# logic
        /// </summary>
        /// <param name="customerName">Customer name</param>
        /// <param name="amount">Payment amount</param>
        /// <param name="currency">Currency code</param>
        /// <returns>Tuple of (isValid, errorMessage)</returns>
        public static (bool isValid, string errorMessage) ValidatePayment(string customerName, double amount, string currency)
        {
            try
            {
                // Validate customer name
                var nameValidation = ValidateCustomerName(customerName);
                if (!nameValidation.isValid)
                {
                    return (false, nameValidation.errorMessage);
                }

                // Validate amount
                var amountValidation = ValidateAmount(amount);
                if (!amountValidation.isValid)
                {
                    return (false, amountValidation.errorMessage);
                }

                // Validate currency
                var currencyValidation = ValidateCurrency(currency);
                if (!currencyValidation.isValid)
                {
                    return (false, currencyValidation.errorMessage);
                }

                return (true, "Payment validation successful");
            }
            catch (Exception ex)
            {
                return (false, $"Validation error: {ex.Message}");
            }
        }

        private static (bool isValid, string errorMessage) ValidateCustomerName(string customerName)
        {
            if (string.IsNullOrWhiteSpace(customerName))
            {
                return (false, "Customer name is required and cannot be empty");
            }

            if (customerName.Trim().Length < 2)
            {
                return (false, "Customer name must be at least 2 characters long");
            }

            if (customerName.Trim().Length > 100)
            {
                return (false, "Customer name cannot exceed 100 characters");
            }

            // Check for valid characters (letters, spaces, hyphens, apostrophes)
            if (!Regex.IsMatch(customerName.Trim(), @"^[a-zA-Z\s\-']+$"))
            {
                return (false, "Customer name can only contain letters, spaces, hyphens, and apostrophes");
            }

            return (true, "Customer name is valid");
        }

        private static (bool isValid, string errorMessage) ValidateAmount(double amount)
        {
            if (amount <= 0)
            {
                return (false, "Amount must be greater than zero");
            }

            if (amount < MinAmount)
            {
                return (false, $"Amount must be at least {MinAmount:C}");
            }

            if (amount > MaxAmount)
            {
                return (false, $"Amount cannot exceed {MaxAmount:C}");
            }

            // Check for reasonable precision (max 2 decimal places)
            if (Math.Round(amount, 2) != amount)
            {
                return (false, "Amount cannot have more than 2 decimal places");
            }

            return (true, "Amount is valid");
        }

        private static (bool isValid, string errorMessage) ValidateCurrency(string currency)
        {
            if (string.IsNullOrWhiteSpace(currency))
            {
                return (false, "Currency is required and cannot be empty");
            }

            var upperCurrency = currency.Trim().ToUpper();
            if (!ValidCurrencies.Contains(upperCurrency))
            {
                return (false, $"Currency '{currency}' is not supported. Supported currencies: {string.Join(", ", ValidCurrencies)}");
            }

            return (true, "Currency is valid");
        }

        /// <summary>
        /// Gets the list of supported currencies
        /// </summary>
        /// <returns>List of supported currency codes</returns>
        public static List<string> GetSupportedCurrencies()
        {
            return new List<string>(ValidCurrencies);
        }

        /// <summary>
        /// Gets the minimum allowed amount
        /// </summary>
        /// <returns>Minimum amount</returns>
        public static double GetMinimumAmount()
        {
            return MinAmount;
        }

        /// <summary>
        /// Gets the maximum allowed amount
        /// </summary>
        /// <returns>Maximum amount</returns>
        public static double GetMaximumAmount()
        {
            return MaxAmount;
        }
    }
}
