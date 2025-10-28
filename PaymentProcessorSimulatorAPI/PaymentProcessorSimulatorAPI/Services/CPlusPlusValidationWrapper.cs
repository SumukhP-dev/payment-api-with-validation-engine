using System.Runtime.InteropServices;
using System.Text;

namespace PaymentApi.Services
{
    public static class CPlusPlusValidationWrapper
    {
        // Import the function from the native DLL
        [DllImport("ValidationEngine.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ValidatePayment(
            [MarshalAs(UnmanagedType.LPStr)] string name,
            double amount,
            [MarshalAs(UnmanagedType.LPStr)] string currency,
            [MarshalAs(UnmanagedType.LPStr)] StringBuilder errorBuffer,
            int bufferSize);

        // Debug functions
        [DllImport("ValidationEngine.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void EnableDebugMode([MarshalAs(UnmanagedType.LPStr)] string logFilePath);

        [DllImport("ValidationEngine.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DisableDebugMode();

        [DllImport("ValidationEngine.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DumpMemoryLeaks();

        [DllImport("ValidationEngine.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool IsDebugModeEnabled();

        // Wrapper method that handles the conversion and error buffer
        public static (bool isValid, string errorMessage) ValidatePaymentSafe(string name, double amount, string currency)
        {
            const int bufferSize = 1024;
            var errorBuffer = new StringBuilder(bufferSize);
            
            try
            {
                bool isValid = ValidatePayment(name, amount, currency, errorBuffer, bufferSize);
                string errorMessage = errorBuffer.ToString();
                return (isValid, errorMessage);
            }
            catch (DllNotFoundException)
            {
                return (false, "ValidationEngine.dll not found or not accessible.");
            }
            catch (Exception ex)
            {
                return (false, $"Error calling validation engine: {ex.Message}");
            }
        }

        // Debug wrapper methods
        public static void EnableDebugModeSafe(string logFilePath = "validation_debug.log")
        {
            try
            {
                EnableDebugMode(logFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to enable debug mode: {ex.Message}");
            }
        }

        public static void DisableDebugModeSafe()
        {
            try
            {
                DisableDebugMode();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to disable debug mode: {ex.Message}");
            }
        }

        public static void DumpMemoryLeaksSafe()
        {
            try
            {
                DumpMemoryLeaks();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to dump memory leaks: {ex.Message}");
            }
        }

        public static bool IsDebugModeEnabledSafe()
        {
            try
            {
                return IsDebugModeEnabled();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to check debug mode status: {ex.Message}");
                return false;
            }
        }
    }
}