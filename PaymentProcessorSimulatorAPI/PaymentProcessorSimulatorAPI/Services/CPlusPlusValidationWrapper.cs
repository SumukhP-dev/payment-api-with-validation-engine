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
    }
}