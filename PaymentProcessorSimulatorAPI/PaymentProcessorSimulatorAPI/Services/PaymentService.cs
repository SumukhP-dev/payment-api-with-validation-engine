using PaymentApi.Models;

namespace PaymentApi.Services
{
    public class PaymentService : IPaymentService
    {
        private static readonly List<Payment> _payments = new();

        public Payment CreatePayment(Payment payment)
        {
            // Call into C++ validation engine with proper parameters
            var (isValid, errorMessage) = CPlusPlusValidationWrapper.ValidatePaymentSafe(
                payment.CustomerName,
                (double)payment.Amount,  // Convert decimal to double
                payment.Currency
            );

            if (!isValid)
            {
                throw new InvalidOperationException($"Payment validation failed: {errorMessage}");
            }

            _payments.Add(payment);
            return payment;
        }

        public Payment? GetPayment(Guid id)
        {
            return _payments.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Payment> GetAllPayments()
        {
            return _payments;
        }
    }
}