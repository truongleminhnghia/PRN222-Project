
using System.Text.Json;
using DrinkToDoor.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Net.payOS.Types;

namespace DrinkToDoor.Web.Pages.Payments
{
    public class Verify : PageModel
    {
        private readonly ILogger<Verify> _logger;
        private readonly IPaymentService _paymentService;

        public Verify(ILogger<Verify> logger, IPaymentService paymentService)
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        public async Task<IActionResult> OnPostAsync([FromBody] JsonElement body)
        {
            try
            {
                var raw = body.ToString();
                _logger.LogInformation("Webhook raw payload: {Body}", raw);

                // Sau này bạn có thể deserialize ra WebhookType
                var model = JsonSerializer.Deserialize<WebhookType>(raw);

                var result = await _paymentService.VerifyPaymentAsync(model);

                return new JsonResult(new
                {
                    message = result ? "Payment processed." : "Payment failed or canceled"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Webhook xử lý lỗi.");
                return StatusCode(500, new { message = "Internal Server Error (logged)." });
            }
        }
    }
}