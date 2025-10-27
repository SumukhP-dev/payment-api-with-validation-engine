using Reqnroll;
using ReqnrollCucumberTests.Models;
using ReqnrollCucumberTests.Support;
using RestSharp;
using FluentAssertions;
using Newtonsoft.Json;

namespace ReqnrollCucumberTests.StepDefinitions
{
    [Binding]
    public class PaymentStepDefinitions
    {
        private readonly Support.TestContext _testContext;

        public PaymentStepDefinitions(Support.TestContext testContext)
        {
            _testContext = testContext;
        }

        [Given("the Payment API is running")]
        public void GivenThePaymentApiIsRunning()
        {
            _testContext.Initialize();
        }

        [Given("I have a payment with the following details:")]
        public void GivenIHaveAPaymentWithTheFollowingDetails(Table table)
        {
            var row = table.Rows[0];
            _testContext.CurrentPayment = new PaymentTestData
            {
                CustomerName = row["CustomerName"],
                Amount = decimal.Parse(row["Amount"]),
                Currency = row["Currency"]
            };
        }


        [Given("I have a payment ID that does not exist")]
        public void GivenIHaveAPaymentIdThatDoesNotExist()
        {
            _testContext.CreatedPayment = new PaymentResponse
            {
                Id = Guid.NewGuid() // Generate a random ID that won't exist
            };
        }

        [When("I create the payment")]
        public void WhenICreateThePayment()
        {
            var request = new RestRequest("/api/payment", Method.Post);
            request.AddJsonBody(_testContext.CurrentPayment!);

            _testContext.LastResponse = _testContext.ApiClient!.Execute(request);
            _testContext.StatusCode = (int)_testContext.LastResponse.StatusCode;

            if (_testContext.LastResponse.IsSuccessful && _testContext.LastResponse.Content != null)
            {
                _testContext.CreatedPayment = JsonConvert.DeserializeObject<PaymentResponse>(_testContext.LastResponse.Content);
            }
            else
            {
                _testContext.ErrorMessage = _testContext.LastResponse.ErrorMessage ?? _testContext.LastResponse.Content;
            }
        }


        [When("I request the payment by its ID")]
        public void WhenIRequestThePaymentByItsId()
        {
            var request = new RestRequest($"/api/payment/{_testContext.CreatedPayment!.Id}", Method.Get);
            _testContext.LastResponse = _testContext.ApiClient!.Execute(request);
            _testContext.StatusCode = (int)_testContext.LastResponse.StatusCode;

            if (_testContext.LastResponse.IsSuccessful && _testContext.LastResponse.Content != null)
            {
                _testContext.CreatedPayment = JsonConvert.DeserializeObject<PaymentResponse>(_testContext.LastResponse.Content);
            }
            else
            {
                _testContext.ErrorMessage = _testContext.LastResponse.ErrorMessage ?? _testContext.LastResponse.Content;
            }
        }

        [Then("the payment should be created successfully")]
        public void ThenThePaymentShouldBeCreatedSuccessfully()
        {
            _testContext.LastResponse!.IsSuccessful.Should().BeTrue($"Payment creation should succeed. Status: {_testContext.StatusCode}, Error: {_testContext.ErrorMessage}");
            _testContext.CreatedPayment.Should().NotBeNull("Created payment should not be null");
        }

        [Then("the payment should be rejected")]
        public void ThenThePaymentShouldBeRejected()
        {
            _testContext.LastResponse!.IsSuccessful.Should().BeFalse("Payment should be rejected");
            _testContext.StatusCode.Should().BeOneOf(new[] { 400, 500 }, "Should return client or server error");
        }

        [Then("the response should be successful")]
        public void ThenTheResponseShouldBeSuccessful()
        {
            _testContext.LastResponse!.IsSuccessful.Should().BeTrue($"Request should succeed. Status: {_testContext.StatusCode}, Error: {_testContext.ErrorMessage}");
        }

        [Then("the response should be not found")]
        public void ThenTheResponseShouldBeNotFound()
        {
            _testContext.StatusCode.Should().Be(404, "Should return not found status");
        }

        [Then("the response should contain the payment details")]
        public void ThenTheResponseShouldContainThePaymentDetails()
        {
            _testContext.CreatedPayment.Should().NotBeNull("Payment details should be present");
            _testContext.CreatedPayment!.CustomerName.Should().Be(_testContext.CurrentPayment!.CustomerName);
            _testContext.CreatedPayment.Amount.Should().Be(_testContext.CurrentPayment.Amount);
            _testContext.CreatedPayment.Currency.Should().Be(_testContext.CurrentPayment.Currency);
        }

        [Then("the response should contain a validation error")]
        public void ThenTheResponseShouldContainAValidationError()
        {
            _testContext.ErrorMessage.Should().NotBeNullOrEmpty("Should contain validation error message");
            _testContext.ErrorMessage.Should().Contain("validation", "Error should mention validation");
        }

        [Then("the response should contain an error")]
        public void ThenTheResponseShouldContainAnError()
        {
            _testContext.ErrorMessage.Should().NotBeNullOrEmpty("Should contain error message");
        }


        [Then("the payment status should be \"([^\"]*)\"")]
        public void ThenThePaymentStatusShouldBe(string expectedStatus)
        {
            _testContext.CreatedPayment.Should().NotBeNull("Payment should not be null");
            _testContext.CreatedPayment!.Status.Should().Be(expectedStatus, $"Payment status should be {expectedStatus}");
        }

        [Then("the payment currency should be \"([^\"]*)\"")]
        public void ThenThePaymentCurrencyShouldBe(string expectedCurrency)
        {
            _testContext.CreatedPayment.Should().NotBeNull("Payment should not be null");
            _testContext.CreatedPayment!.Currency.Should().Be(expectedCurrency, $"Payment currency should be {expectedCurrency}");
        }

        [Then("the C++ validation engine should validate the payment")]
        public void ThenTheCValidationEngineShouldValidateThePayment()
        {
            _testContext.LastResponse!.IsSuccessful.Should().BeTrue("C++ validation engine should validate the payment");
        }

        [Then("the C++ validation engine should reject the payment")]
        public void ThenTheCValidationEngineShouldRejectThePayment()
        {
            _testContext.LastResponse!.IsSuccessful.Should().BeFalse("C++ validation engine should reject the payment");
        }

        [Then("the validation error should mention customer name")]
        public void ThenTheValidationErrorShouldMentionCustomerName()
        {
            _testContext.ErrorMessage.Should().NotBeNullOrEmpty("Should contain validation error");
            _testContext.ErrorMessage.Should().ContainAny(new[] { "customer", "name", "CustomerName", "customer name" });
        }

        [Then("the validation error should mention amount")]
        public void ThenTheValidationErrorShouldMentionAmount()
        {
            _testContext.ErrorMessage.Should().NotBeNullOrEmpty("Should contain validation error");
            _testContext.ErrorMessage.Should().ContainAny(new[] { "amount", "Amount", "positive", "negative" });
        }

        [Then("the validation error should mention currency")]
        public void ThenTheValidationErrorShouldMentionCurrency()
        {
            _testContext.ErrorMessage.Should().NotBeNullOrEmpty("Should contain validation error");
            _testContext.ErrorMessage.Should().ContainAny(new[] { "currency", "Currency", "invalid" });
        }

    }
}
