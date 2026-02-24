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
}