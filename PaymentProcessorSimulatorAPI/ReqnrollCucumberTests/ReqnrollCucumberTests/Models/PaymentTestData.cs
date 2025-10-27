using Newtonsoft.Json;

namespace ReqnrollCucumberTests.Models
{
    public class PaymentTestData
    {
        [JsonProperty("customerName")]
        public string CustomerName { get; set; } = string.Empty;

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; } = "USD";

        [JsonProperty("status")]
        public string Status { get; set; } = "Pending";
    }

    public class PaymentResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; } = string.Empty;

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; } = string.Empty;

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;
    }

    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
        public int StatusCode { get; set; }
    }
}
