
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