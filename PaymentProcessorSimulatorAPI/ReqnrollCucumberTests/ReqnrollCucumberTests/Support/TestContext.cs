using ReqnrollCucumberTests.Models;
using RestSharp;

namespace ReqnrollCucumberTests.Support
{
    public class TestContext
    {
        public RestClient? ApiClient { get; set; }
        public RestResponse? LastResponse { get; set; }
        public PaymentTestData? CurrentPayment { get; set; }
        public PaymentResponse? CreatedPayment { get; set; }
        public string? ErrorMessage { get; set; }
        public int StatusCode { get; set; }

        public void Initialize()
        {
            ApiClient = new RestClient("http://localhost:5255");
        }

        public void Cleanup()
        {
            ApiClient?.Dispose();
            ApiClient = null;
            LastResponse = null;
            CurrentPayment = null;
            CreatedPayment = null;
            ErrorMessage = null;
            StatusCode = 0;
        }
    }
}
