using PaymentApi.Models;

namespace PaymentApi.Services
{
    public interface IPaymentService
    {
        Payment CreatePayment(Payment payment);
        Payment? GetPayment(Guid id);
        IEnumerable<Payment> GetAllPayments();
    }
}