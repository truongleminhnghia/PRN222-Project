
using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using DrinkToDoor.Data.enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DrinkToDoor.Web.Pages.Orders
{
    public class Index : PageModel
    {
        private readonly ILogger<Index> _logger;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly INotyfService _toast;

        public Index(ILogger<Index> logger, IOrderService orderService, IUserService userService,
                    INotyfService toast)
        {
            _logger = logger;
            _orderService = orderService;
            _userService = userService;
            _toast = toast;
        }

        public int TotalPages { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        [BindProperty(SupportsGet = true)]
        public int PageCurrent { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public EnumOrderStatus? Status { get; set; }
        public IEnumerable<OrderResponse> Orders { get; set; }

        public async Task OnGet(string status)
        {
            var userId = GetUserIdOrRedirect();
            await EnsureCartExists(userId);
            if (!string.IsNullOrEmpty(status) && Enum.TryParse<EnumOrderStatus>(status, out var parsedStatus))
            {
                Status = parsedStatus;
            }
            await LoadDataAsync(userId, Status);
        }

        private async Task LoadDataAsync(Guid? userId, EnumOrderStatus? status)
        {
            var result = await _orderService.GetWithParams(userId, status, PageCurrent, PageSize);
            Orders = result.Item1;
            TotalPages = (int)Math.Ceiling(result.Item2 / (double)PageSize);
        }

        private async Task EnsureCartExists(Guid userId)
        {
            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
            {
                _toast.Warning("User không tồn tại hoặc chưa đăng nhập.");
                RedirectToPage("/Login");
            }
        }

        private Guid GetUserIdOrRedirect()
        {
            var str = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(str, out var userId))
            {
                throw new InvalidOperationException("User ID không hợp lệ");
            }
            return userId;
        }
    }
}