using System.Runtime.InteropServices;
using System.Text;

namespace PaymentApi.Services
{
    public static class CPlusPlusValidationWrapper
    {
        private static bool _dllChecked = false;
        private static bool _dllAvailable = false;

        // Import the function from the native DLL
        [DllImport("ValidationEngine.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ValidatePayment(
            [MarshalAs(UnmanagedType.LPStr)] string name,
            double amount,
            [MarshalAs(UnmanagedType.LPStr)] string currency,
            [MarshalAs(UnmanagedType.LPStr)] StringBuilder errorBuffer,
            int bufferSize);

        private static void CheckDllAvailability()
        {
            if (!_dllChecked)
            {
                try
                {
                    // Try to load the DLL without calling any functions
                    var handle = LoadLibrary("ValidationEngine.dll");
                    _dllAvailable = handle != IntPtr.Zero;
                    if (handle != IntPtr.Zero)
                    {
                        FreeLibrary(handle);
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
                return (false, "ValidationEngine.dll not available, using C# fallback");
            }

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