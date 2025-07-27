
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using DrinkToDoor.Data.enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DrinkToDoor.Web.Pages.Admins
{
    public class DashboardModel : PageModel
    {
        private readonly ILogger<DashboardModel> _logger;
        private readonly IDashboardService _dashboardService;

        public DashboardModel(ILogger<DashboardModel> logger, IDashboardService dashboardService)
        {
            _logger = logger;
            _dashboardService = dashboardService;
        }

        public decimal TotalRevenue { get; set; } = 0;
        public int TotalOrders { get; set; } = 0;
        public int PendingOrders { get; set; } = 0;
        public int TotalUsers { get; set; } = 0;

        public IEnumerable<OrderResponse> Orders { get; set; }
        public IEnumerable<UserResponse> User { get; set; }

        [BindProperty(SupportsGet = true)]
        public EnumOrderStatus Status { get; set; } = EnumOrderStatus.DELIVERED;
        public List<MonthlyRevenueComparison> MonthlyRevenueList { get; set; } = new();
        public List<ChartData> MonthlyProfitList { get; set; } = new();

        public async Task OnGet()
        {
            await LoadData();
            await LoadDataAsync(5, Status);
            await LoadDataUserAsync(EnumAccountStatus.ACTIVE, 5);
            await LoadDataChartRevenueAsync();
            await LoadMonthlyProfitListAsync();
        }

        public async Task LoadData()
        {
            TotalRevenue = await _dashboardService.TotalRevenue();
            TotalOrders = await _dashboardService.TotalOrder();
            PendingOrders = await _dashboardService.TotalConfirm();
            TotalUsers = await _dashboardService.TotalUser();

        }

        private async Task LoadDataAsync(int limit, EnumOrderStatus status)
        {
            var result = await _dashboardService.ListOrder(status, limit);
            Orders = result;
        }

        private async Task LoadDataUserAsync(EnumAccountStatus status, int limit)
        {
            var result = await _dashboardService.ListUser(status, limit);
            User = result;
        }

        private async Task LoadDataChartRevenueAsync()
        {
            MonthlyRevenueList = await _dashboardService.ListChartRevenue(DateTime.Now.Year);
        }

        private async Task LoadMonthlyProfitListAsync()
        {
            MonthlyProfitList = await _dashboardService.ListProfitList(DateTime.Now.Year, DateTime.Now.Month);
        }
    }
}