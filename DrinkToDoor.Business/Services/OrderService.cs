
using AutoMapper;
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using DrinkToDoor.Data;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.enums;
using Microsoft.Extensions.Logging;

namespace DrinkToDoor.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(ILogger<OrderService> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CancelOrder(Guid id, string reason)
        {
            try
            {
                var orderExisting = await _unitOfWork.Orders.FindById(id);
                if (orderExisting == null) throw new Exception("Đơn hàng không tồn tại");
                if (orderExisting.Status == EnumOrderStatus.CANCELED) throw new Exception("Đơn hàng đã bị hủy trước đó");
                orderExisting.Status = EnumOrderStatus.CANCELED;
                // orderExisting.Reaso = reason;
                await _unitOfWork.Orders.UpdateAsync(orderExisting);
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
                return result > 0 ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<List<MonthlyRevenueComparison>> CompareMonthlyRevenueAsync(int year)
        {
            try
            {
                var orders = await _unitOfWork.Orders.FindByYear(year);
                var monthlyTotals = orders.GroupBy(o => o.OrderDate.Month)
                                            .Select(g => new { Month = g.Key, Revenue = g.Sum(o => o.TotalAmount) })
                                            .ToDictionary(g => g.Month, g => g.Revenue);
                var results = new List<MonthlyRevenueComparison>();
                for (int i = 1; i <= 12; i++)
                {
                    decimal currentRevenue = monthlyTotals.ContainsKey(i) ? monthlyTotals[i] : 0;
                    decimal? percentChange = null;

                    if (i > 1)
                    {
                        decimal prevRevenue = results[i - 2].Revenue;
                        if (prevRevenue > 0)
                        {
                            percentChange = ((currentRevenue - prevRevenue) / prevRevenue) * 100;
                        }
                        else if (currentRevenue > 0)
                        {
                            percentChange = 100;
                        }
                    }

                    results.Add(new MonthlyRevenueComparison
                    {
                        Month = i,
                        Revenue = currentRevenue,
                        PercentageChange = percentChange
                    });
                }
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<bool> ConfirmOrder(Guid id, string? reason)
        {
            try
            {
                var orderExisting = await _unitOfWork.Orders.FindById(id);
                if (orderExisting == null) throw new Exception("Đơn hàng không tồn tại");
                if (orderExisting.Status == EnumOrderStatus.CANCELED && reason == null) throw new Exception("Đơn hàng đã bị hủy trước đó cần có lí do");
                orderExisting.Status = EnumOrderStatus.DELIVERING;
                // orderExisting.Reaso = reason;
                await _unitOfWork.Orders.UpdateAsync(orderExisting);
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
                return result > 0 ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<Guid> CreateOrder(OrderRequest request)
        {
            try
            {
                var userExisting = await _unitOfWork.Users.FindById(request.UserId);
                if (userExisting == null) throw new Exception($"Người dùng không tồn tại với {request.UserId}");
                var order = _mapper.Map<Order>(request);
                order.OrderDate = DateTime.UtcNow;
                order.Status = EnumOrderStatus.PEDING;
                await _unitOfWork.Orders.AddAsync(order);
                var orderDetails = new List<OrderDetail>();
                foreach (var item in request.OrderDetailsRequest)
                {
                    var orderDetail = _mapper.Map<OrderDetail>(item);
                    orderDetail.OrderId = order.Id;
                    orderDetail.TotalPrice = item.Price * item.Quantity;
                    await _unitOfWork.OrderDetails.AddAsync(orderDetail);
                    orderDetails.Add(orderDetail);
                    order.TotalAmount += orderDetail.TotalPrice;
                }
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
                return result > 0 ? order.Id : Guid.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<OrderResponse?> GetById(Guid id)
        {
            try
            {
                var orderExisting = await _unitOfWork.Orders.FindById(id);
                if (orderExisting == null) throw new Exception("Đơn hàng không tồn tại");
                return _mapper.Map<OrderResponse>(orderExisting);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<List<ChartData>> GetMonthlyProfit(int year, int month)
        {
            try
            {
                var orders = await _unitOfWork.Orders.FindByMonthOfYear(month, year);

                if (orders == null || !orders.Any())
                    return new List<ChartData>();

                var totalProfit = orders
                    .Where(o => o.Status == EnumOrderStatus.DELIVERED)
                    .SelectMany(o => o.OrderDetails)
                    .Where(d => d.IngredientProduct != null && d.IngredientProduct.Ingredient != null)
                    .Sum(d =>
                    {
                        var cost = d.IngredientProduct.Ingredient.Cost;
                        return (d.Price - cost) * d.Quantity;
                    });

                return new List<ChartData>
        {
            new ChartData
            {
                Label = $"Lợi nhuận tháng {month}",
                Value = (decimal)totalProfit
            }
        };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tính tổng lợi nhuận tháng");
                throw new Exception("Server Error");
            }
        }

        public async Task<Tuple<IEnumerable<OrderResponse>, int>> GetWithParams(Guid? userId, EnumOrderStatus? status, int pageCurrent, int pageSize)
        {
            try
            {
                var orders = await _unitOfWork.Orders.GetAllAsync(userId, status);
                if (orders == null) return null;
                var total = orders.Count();
                var paged = orders.Skip((pageCurrent - 1) * pageSize).Take(pageSize).ToList();
                var pagedResponses = _mapper.Map<List<OrderResponse>>(paged);
                return Tuple.Create<IEnumerable<OrderResponse>, int>(pagedResponses, total);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }
    }
}