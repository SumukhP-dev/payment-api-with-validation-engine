using Microsoft.OpenApi.Models;

namespace PaymentApi.Models
{
    public class Payment
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public double Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Completed, Failed
    }

    public class PaymentExample
    {
        public static readonly Payment Default = new Payment
        {
            Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
            CustomerName = "John Doe",
            Amount = 100.50,
            Currency = "USD",
            CreatedAt = DateTime.Parse("2026-02-24T17:17:53.970Z"),
            Status = "pending"
        };
    }
}