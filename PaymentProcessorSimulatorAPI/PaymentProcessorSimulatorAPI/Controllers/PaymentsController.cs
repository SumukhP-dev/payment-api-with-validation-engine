













using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("api/payment")]
        public IActionResult CreatePayment([FromBody] Payment payment)
        {
            var created = _paymentService.CreatePayment(payment);
            return CreatedAtAction(nameof(GetPayment), new { id = created.Id }, created);
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

        // Debug endpoints
        [HttpGet("api/payment/debug/status")]
        public IActionResult GetDebugStatus()
        {
            var isDebugEnabled = _paymentService.IsDebugModeEnabled();
            return Ok(new { DebugModeEnabled = isDebugEnabled });
        }

        [HttpPost("api/payment/debug/dump-memory-leaks")]
        public IActionResult DumpMemoryLeaks()
        {
            _paymentService.DumpMemoryLeaks();
            return Ok(new { Message = "Memory leak dump initiated. Check console and log files." });
        }

        [HttpPost("api/payment/debug/disable")]
        public IActionResult DisableDebugMode()
        {
            _paymentService.DisableDebugMode();
            return Ok(new { Message = "Debug mode disabled." });
        }
    }
}