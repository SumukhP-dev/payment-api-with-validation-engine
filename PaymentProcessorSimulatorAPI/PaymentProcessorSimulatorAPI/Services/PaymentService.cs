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
                CPlusPlusValidationWrapper.EnableDebugModeSafe(logFilePath);
                
                Console.WriteLine($"[DEBUG] PaymentService initialized with debug logging to: {logFilePath}");
                Console.WriteLine($"[DEBUG] Debug mode enabled: {CPlusPlusValidationWrapper.IsDebugModeEnabledSafe()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to initialize debug mode: {ex.Message}");
            }
        }

        public Payment CreatePayment(Payment payment)
        {
            Console.WriteLine($"[DEBUG] Creating payment for customer: {payment.CustomerName}, amount: {payment.Amount}, currency: {payment.Currency}");
            
            // Call into C++ validation engine with proper parameters
            var (isValid, errorMessage) = CPlusPlusValidationWrapper.ValidatePaymentSafe(
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

        // Debug methods for memory leak detection
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