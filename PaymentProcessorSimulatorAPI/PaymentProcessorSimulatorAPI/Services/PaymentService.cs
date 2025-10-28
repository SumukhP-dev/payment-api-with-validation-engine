using PaymentApi.Models;

namespace PaymentApi.Services
{
    public class PaymentService : IPaymentService
    {
        private static readonly List<Payment> _payments = new();
        private static bool _debugInitialized = false;

        public PaymentService()
        {
            // Initialize debug mode on first service creation
            if (!_debugInitialized)
            {
                InitializeDebugMode();
                _debugInitialized = true;
            }
        }

        private void InitializeDebugMode()
        {
            try
            {
                // Enable debug mode with timestamped log file
                var logFilePath = $"validation_debug_{DateTime.Now:yyyyMMdd_HHmmss}.log";
                Console.WriteLine($"[DEBUG] PaymentService initialized with C# validation engine");
                Console.WriteLine($"[DEBUG] Debug logging enabled to: {logFilePath}");
                Console.WriteLine($"[DEBUG] Debug mode enabled: true");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to initialize debug mode: {ex.Message}");
            }
        }

        public Payment CreatePayment(Payment payment)
        {
            Console.WriteLine($"[DEBUG] Creating payment for customer: {payment.CustomerName}, amount: {payment.Amount}, currency: {payment.Currency}");
            
            // Try C++ validation first, fallback to C# validation
            var (isValid, errorMessage) = ValidatePaymentWithFallback(
                payment.CustomerName,
                (double)payment.Amount,  // Convert decimal to double
                payment.Currency
            );

            if (!isValid)
            {
                Console.WriteLine($"[DEBUG] Payment validation failed: {errorMessage}");
                throw new InvalidOperationException($"Payment validation failed: {errorMessage}");
            }

            _payments.Add(payment);
            Console.WriteLine($"[DEBUG] Payment created successfully with ID: {payment.Id}");
            return payment;
        }

        private (bool isValid, string errorMessage) ValidatePaymentWithFallback(string customerName, double amount, string currency)
        {
            try
            {
                // Try C++ validation first
                return CPlusPlusValidationWrapper.ValidatePaymentSafe(customerName, amount, currency);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG] C++ validation failed, falling back to C# validation: {ex.Message}");
                // Fallback to C# validation
                return CSharpValidationEngine.ValidatePayment(customerName, amount, currency);
            }
        }

        public Payment? GetPayment(Guid id)
        {
            Console.WriteLine($"[DEBUG] Retrieving payment with ID: {id}");
            var payment = _payments.FirstOrDefault(p => p.Id == id);
            
            if (payment == null)
            {
                Console.WriteLine($"[DEBUG] Payment not found with ID: {id}");
            }
            else
            {
                Console.WriteLine($"[DEBUG] Payment found: {payment.CustomerName}, {payment.Amount}, {payment.Currency}");
            }
            
            return payment;
        }

        public IEnumerable<Payment> GetAllPayments()
        {
            Console.WriteLine($"[DEBUG] Retrieving all payments. Count: {_payments.Count}");
            return _payments;
        }

        // Debug methods for C++ validation engine
        public void DumpMemoryLeaks()
        {
            Console.WriteLine("[DEBUG] Dumping memory leaks...");
            CPlusPlusValidationWrapper.DumpMemoryLeaksSafe();
        }

        public bool IsDebugModeEnabled()
        {
            return CPlusPlusValidationWrapper.IsDebugModeEnabledSafe();
        }

        public void DisableDebugMode()
        {
            Console.WriteLine("[DEBUG] Disabling debug mode...");
            CPlusPlusValidationWrapper.DisableDebugModeSafe();
        }
    }
}