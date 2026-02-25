using Microsoft.OpenApi.Models;

namespace PaymentApi.Models
{
    /// <summary>
    /// Represents a payment transaction.
    /// </summary>
    /// <example>
    /// {
    ///   "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///   "customerName": "John Doe",
    ///   "amount": 100.50,
    ///   "currency": "USD",
    ///   "createdAt": "2026-02-24T17:17:53.970Z",
    ///   "status": "pending"
    /// }
    /// </example>
    public class Payment
    {
        /// <summary>
        /// The unique identifier for the payment.
        /// </summary>
        /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
        public Guid Id { get; set; }
        
        /// <summary>
        /// The name of the customer making the payment.
        /// </summary>
        /// <example>John Doe</example>
        public string CustomerName { get; set; } = string.Empty;
        
        /// <summary>
        /// The payment amount.
        /// </summary>
        /// <example>100.50</example>
        public double Amount { get; set; }
        
        /// <summary>
        /// The currency code for the payment.
        /// </summary>
        /// <example>USD</example>
        public string Currency { get; set; } = "USD";
        
        /// <summary>
        /// The date and time when the payment was created.
        /// </summary>
        /// <example>2026-02-24T17:17:53.970Z</example>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// The current status of the payment.
        /// </summary>
        /// <example>pending</example>
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