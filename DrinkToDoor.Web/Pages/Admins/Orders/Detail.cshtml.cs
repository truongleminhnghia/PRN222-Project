
using AspNetCoreHero.ToastNotification.Abstractions;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DrinkToDoor.Web.Pages.Admins.Orders
{
    public class Detail : PageModel
    {
        private readonly ILogger<Detail> _logger;
        private readonly IOrderService _orderService;
        private readonly INotyfService _toastNotification;
        public Detail(ILogger<Detail> logger, IOrderService orderService, INotyfService toastNotification)
        {
            _logger = logger;
            _orderService = orderService;
            _toastNotification = toastNotification;
        }

        public OrderResponse OrderResponse { get; set; }

        [BindProperty]
        public string? Reason { get; set; }

        public async Task OnGet(Guid id)
        {
            await LoadData(id);
        }

        public async Task LoadData(Guid id)
        {
            var order = await _orderService.GetById(id);
            OrderResponse = order;
        }

        public async Task<IActionResult> OnPostCancel(Guid id)
        {
            try
            {
                await LoadData(id);
                if (string.IsNullOrWhiteSpace(Reason))
                {
                    ModelState.AddModelError(nameof(Reason), "Vui lòng nhập lý do hủy đơn hàng.");
                    return Page();
                }
                var result = await _orderService.CancelOrder(id, Reason);
                if (result)
                {
                    _toastNotification.Success("Đã từ chối đơn hàng thành công", 5);

                }
                else
                {
                    _toastNotification.Success("Đã từ chối đơn hàng thất bại", 5);
                }
                await LoadData(id);
                return RedirectToPage("./Detail", new { id = id });
            }
            catch (Exception ex)
            {
                _toastNotification.Warning(ex.Message, 5);
                return RedirectToPage("./Detail", new { id = id });
            }
        }

        public async Task<IActionResult> OnPostApprove(Guid id)
        {
            try
            {
                var result = await _orderService.ConfirmOrder(id, Reason);
                if (result)
                {
                    _toastNotification.Success("Đã duyệt đơn hàng thành công", 5);
                }
                else
                {
                    _toastNotification.Success("Đã duyệt đơn hàng thất bại", 5);
                }
                await LoadData(id);
                return RedirectToPage("./Detail", new { id = id });
            }
            catch (Exception ex)
            {
                _toastNotification.Warning(ex.Message, 5);
                return RedirectToPage("./Detail", new { id = id });
            }
        }

    }
}