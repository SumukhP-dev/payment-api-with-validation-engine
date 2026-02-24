using System.Runtime.InteropServices;
using System.Text;

namespace PaymentApi.Services
{
    public static class CPlusPlusValidationWrapper
    {
        private static bool _dllChecked = false;
        private static bool _dllAvailable = false;

        private static void CheckDllAvailability()
        {
            if (!_dllChecked)
            {
                try
                {
                    // Check if we're in Azure/Production environment
                    var isAzure = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME"));
                    var isProduction = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";
                    var enableCppValidation = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ENABLE_CPP_VALIDATION"));

                    if ((isAzure || isProduction) && !enableCppValidation)
                    {
                        // Disable native DLL in Azure/Production unless explicitly enabled
                        _dllAvailable = false;
                    }
                    else
                    {
                        // Try to load DLL in development/local environments or when explicitly enabled
                        var handle = LoadLibrary("ValidationEngine.dll");
                        _dllAvailable = handle != IntPtr.Zero;
                        if (handle != IntPtr.Zero)
                        {
                            FreeLibrary(handle);
                        }
                    }
                }
                catch
                {
                    _dllAvailable = false;
                }
                finally
                {
                    _dllChecked = true;
                }
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeLibrary(IntPtr hModule);

        // Wrapper method that handles the conversion and error buffer
        public static (bool isValid, string errorMessage) ValidatePaymentSafe(string name, double amount, string currency)
        {
            CheckDllAvailability();

            if (!_dllAvailable)
            {
                return (false, "C++ validation engine disabled in Azure/Production environment, using C# fallback");
            }

            // Import the function from the native DLL only when needed and available
            [DllImport("ValidationEngine.dll", CallingConvention = CallingConvention.Cdecl)]
            static extern bool ValidatePayment(
                [MarshalAs(UnmanagedType.LPStr)] string name,
                double amount,
                [MarshalAs(UnmanagedType.LPStr)] string currency,
                [MarshalAs(UnmanagedType.LPStr)] StringBuilder errorBuffer,
                int bufferSize);

            try
            {
                const int bufferSize = 1024;
                var errorBuffer = new StringBuilder(bufferSize);
                
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
    }
}