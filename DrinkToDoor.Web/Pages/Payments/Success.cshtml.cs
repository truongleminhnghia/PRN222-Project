
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using DrinkToDoor.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DrinkToDoor.Web.Pages.Payments
{
    public class Success : PageModel
    {
        private readonly ILogger<Success> _logger;
        private readonly IPaymentService _paymentService;
        private readonly INotyfService _toastNotification;

        public Success(ILogger<Success> logger, IPaymentService paymentService, INotyfService toastNotification)
        {
            _logger = logger;
            _paymentService = paymentService;
            _toastNotification = toastNotification;
        }

        public string Code { get; set; }
        public string Id { get; set; }
        public bool Cancel { get; set; }
        public string Status { get; set; }
        public string OrderCode { get; set; }

        public bool Paid { get; set; } = false;

        public async Task OnGet(string code, string id, bool cancel, string status, string orderCode)
        {
            Code = code;
            Id = id;
            Paid = !cancel;
            Status = status;
            OrderCode = orderCode;

            await LoadDataToDatabase(Code, Status, OrderCode, Paid);
        }

        private async Task<IActionResult> LoadDataToDatabase(string code, string status, string orderCode, bool Paid)
        {
            var result = await _paymentService.VerifyPaymentAsync(code, status, orderCode, Paid);
            if (result)
            {
                _toastNotification.Success("Thanh toán đơn hàng thành công", 5);
                return Page();
            }
            else
            {
                _toastNotification.Error("Thanh toán đơn hàng không thành công", 5);
                return Page();
            }
        }
    }
}