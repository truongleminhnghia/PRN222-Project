
using System.Threading.Tasks;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using DrinkToDoor.Data.enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DrinkToDoor.Web.Pages.Admins.Orders
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IOrderService _orderService;
        public IndexModel(ILogger<IndexModel> logger, IOrderService orderService)
        {
            _orderService = orderService;
            _logger = logger;
        }

        public IEnumerable<OrderResponse> Orders { get; set; }

        public int TotalPages { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        [BindProperty(SupportsGet = true)]
        public int PageCurrent { get; set; } = 1;

        public async Task OnGet()
        {
            await LoadDataAsync(null, null);
        }

        private async Task LoadDataAsync(Guid? userId, EnumOrderStatus? status)
        {
            var result = await _orderService.GetWithParams(userId, status, PageCurrent, PageSize);
            Orders = result.Item1;
            TotalPages = (int)Math.Ceiling(result.Item2 / (double)PageSize);
        }
    }
}