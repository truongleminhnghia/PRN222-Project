

using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Data.enums;

namespace DrinkToDoor.Business.Interfaces
{
    public interface IDashboardService
    {
        Task<int> TotalUser();
        Task<decimal> TotalRevenue();
        Task<int> TotalOrder();
        Task<int> TotalConfirm();
        Task<IEnumerable<OrderResponse>> ListOrder(EnumOrderStatus status, int limit);
        Task<IEnumerable<UserResponse>> ListUser(EnumAccountStatus status, int limit);
        Task<List<MonthlyRevenueComparison>> ListChartRevenue(int year);
        Task<List<ChartData>> ListProfitList(int year, int month);
    }
}