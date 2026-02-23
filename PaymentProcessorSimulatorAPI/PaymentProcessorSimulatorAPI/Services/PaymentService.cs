using PaymentApi.Models;
using System.Runtime.InteropServices;

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
            // Check if running on Windows and if ValidationEngine.dll is available
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && 
                System.IO.File.Exists("ValidationEngine.dll"))
            {
                try
                {
                    // Try C++ validation first on Windows if DLL is available
                    Console.WriteLine($"[DEBUG] 🔧 Using C++ Validation Engine (Native DLL)");
                    return CPlusPlusValidationWrapper.ValidatePaymentSafe(customerName, amount, currency);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DEBUG] ⚠️ C++ validation failed, falling back to C# validation: {ex.Message}");
                    // Fallback to C# validation
                    Console.WriteLine($"[DEBUG] 🔄 Using C# Validation Engine (Fallback)");
                    return CSharpValidationEngine.ValidatePayment(customerName, amount, currency);
                }
            }
            else
            {
                // On Linux/macOS or when DLL is not available, use C# validation directly
                var platform = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
                var dllExists = System.IO.File.Exists("ValidationEngine.dll");
                Console.WriteLine($"[DEBUG] 🔄 Using C# Validation Engine (platform: {platform}, dll exists: {dllExists})");
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

    }
}