







using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using PaymentApi.Models;
using PaymentApi.Services;

namespace PaymentApi.Controllers
{
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        /// <summary>
        /// Creates a new payment.
        /// </summary>
        /// <param name="payment">The payment to create.</param>
        /// <returns>The created payment.</returns>
        /// <response code="201">The payment was created successfully.</response>
        /// <response code="400">The payment was invalid.</response>
        [HttpPost("api/payment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreatePayment([FromBody] Payment payment)
        {
            try
            {
                var created = _paymentService.CreatePayment(payment);
                return CreatedAtAction(nameof(GetPayment), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An unexpected error occurred while processing payment." });
            }
        }

        [HttpGet("api/payment/{id:guid}")]
        public IActionResult GetPayment(Guid id)
        {
            var payment = _paymentService.GetPayment(id);
            if (payment == null)
                return NotFound();

            return Ok(payment);
        }

        [HttpGet("api/payments")]
        public IActionResult GetAllPayments()
        {
            return Ok(_paymentService.GetAllPayments());
        }

    }
}