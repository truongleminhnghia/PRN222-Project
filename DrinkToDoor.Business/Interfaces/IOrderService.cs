
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Data.enums;

namespace DrinkToDoor.Business.Interfaces
{
    public interface IOrderService
    {
        Task<Guid> CreateOrder(OrderRequest request);
        Task<OrderResponse?> GetById(Guid id);
        Task<Tuple<IEnumerable<OrderResponse>, int>> GetWithParams(Guid? userId, EnumOrderStatus? status, int pageCurrent, int pageSize);
        Task<bool> ConfirmOrder(Guid id, string? reason);
        Task<bool> CancelOrder(Guid id, string reason);
        Task<List<MonthlyRevenueComparison>> CompareMonthlyRevenueAsync(int year);
        Task<List<ChartData>> GetMonthlyProfit(int year, int month);
    }
}