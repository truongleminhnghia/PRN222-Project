using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using DrinkToDoor.Data;
using DrinkToDoor.Data.enums;
using Microsoft.Extensions.Logging;

namespace DrinkToDoor.Business.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ILogger<DashboardService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;

        public DashboardService(ILogger<DashboardService> logger, IUnitOfWork unitOfWork, IOrderService orderService, IUserService userService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _orderService = orderService;
            _userService = userService;
        }

        public async Task<IEnumerable<UserResponse>> ListUser(EnumAccountStatus status, int limit)
        {
            try
            {
                var (user, _) = await _userService.GetAsync(null, EnumRoleName.ROLE_USER, status, null, 1, limit);
                return user ?? Enumerable.Empty<UserResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: {Message}", ex.Message);
                throw new Exception("Server Error");
            }
        }

        public async Task<IEnumerable<OrderResponse>> ListOrder(EnumOrderStatus status, int limit)
        {
            try
            {
                var (orders, _) = await _orderService.GetWithParams(null, status, 1, limit);
                return orders ?? Enumerable.Empty<OrderResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: {Message}", ex.Message);
                throw new Exception("Server Error");
            }
        }

        public async Task<int> TotalConfirm()
        {
            try
            {
                var orders = await _unitOfWork.Orders.GetAllAsync(null, EnumOrderStatus.WAITING_DELIVER);

                return orders?.Count() ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: {Message}", ex.Message);
                throw new Exception("Server Error");
            }
        }

        public async Task<int> TotalOrder()
        {
            try
            {
                var orders = await _unitOfWork.Orders.GetAllAsync(null, null);

                return orders?.Count() ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: {Message}", ex.Message);
                throw new Exception("Server Error");
            }
        }

        public async Task<decimal> TotalRevenue()
        {
            try
            {
                var orders = await _unitOfWork.Orders.GetAllAsync(null, EnumOrderStatus.DELIVERED);

                return orders?.Sum(o => o.TotalAmount) ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tính tổng doanh thu: {Message}", ex.Message);
                throw new Exception("Server Error");
            }
        }

        public async Task<int> TotalUser()
        {
            try
            {
                var result = await _unitOfWork.Users.TotalUser();
                return result > 0 ? result : 0;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<List<MonthlyRevenueComparison>> ListChartRevenue(int year)
        {
            try
            {
                var result = await _orderService.CompareMonthlyRevenueAsync(year);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<List<ChartData>> ListProfitList(int year, int month)
        {
            try
            {
                var result = await _orderService.GetMonthlyProfit(year, month);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }
    }
}