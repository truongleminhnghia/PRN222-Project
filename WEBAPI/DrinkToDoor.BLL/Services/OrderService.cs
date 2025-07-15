using AutoMapper;
using DrinkToDoor.BLL.Exceptions;
using DrinkToDoor.BLL.Interfaces;
using DrinkToDoor.BLL.ViewModel.Pages;
using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.BLL.ViewModel.Responses;
using DrinkToDoor.Data;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.enums;
using Microsoft.Extensions.Logging;

namespace DrinkToDoor.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<OrderService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> Create(OrderRequest request)
        {
            try
            {
                var order = _mapper.Map<Order>(request);
                var now = DateTime.UtcNow;
                order.CreatedAt = now;
                order.UpdatedAt = now;
                await _unitOfWork.Orders.AddAsync(order);
                if (request.OrderDetails != null && request.OrderDetails.Any())
                {
                    foreach (var detailReq in request.OrderDetails)
                    {
                        var detail = _mapper.Map<OrderDetail>(detailReq);
                        detail.OrderId = order.Id;
                        detail.CreatedAt = now;
                        detail.UpdatedAt = now;
                        await _unitOfWork.OrderDetails.AddAsync(detail);
                    }
                }
                var saved = await _unitOfWork.SaveChangesWithTransactionAsync();
                return saved > 0;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order with details: {Message}", ex.Message);
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR, "Could not create order");
            }
        }

        public async Task<OrderResponse?> GetById(Guid id)
        {
            try
            {
                var existing = await _unitOfWork.Orders.FindById(id);
                if (existing == null)
                    throw new AppException(ErrorCode.NOT_FOUND, "Order not found");
                var dto = _mapper.Map<OrderResponse>(existing);
                dto.UserName = existing.User?.LastName ?? string.Empty;
                return dto;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving order: {Message}", ex.Message);
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR, "Could not get order");
            }
        }

        public async Task<PageResult<OrderResponse>> GetOrders(
            Guid? userId,
            EnumOrderStatus? status,
            int pageCurrent,
            int pageSize
        )
        {
            try
            {
                var list = await _unitOfWork.Orders.GetAllAsync(userId, status);
                if (!list.Any())
                    throw new AppException(ErrorCode.LIST_EMPTY);

                var total = list.Count();
                var paged = list.Skip((pageCurrent - 1) * pageSize).Take(pageSize).ToList();
                var data = _mapper.Map<List<OrderResponse>>(paged);
                for (int i = 0; i < paged.Count; i++)
                    data[i].UserName = paged[i].User?.LastName ?? string.Empty;

                return new PageResult<OrderResponse>(data, pageSize, pageCurrent, total);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listing orders: {Message}", ex.Message);
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR, "Could not list orders");
            }
        }

        public async Task<bool> Update(Guid id, OrderRequest request)
        {
            try
            {
                var existing = await _unitOfWork.Orders.FindById(id);
                if (existing == null)
                    throw new AppException(ErrorCode.NOT_FOUND, "Order not found");

                _mapper.Map(request, existing);
                await _unitOfWork.Orders.UpdateAsync(existing);
                return (await _unitOfWork.SaveChangesWithTransactionAsync()) > 0;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order: {Message}", ex.Message);
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR, "Could not update order");
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                var existing = await _unitOfWork.Orders.FindById(id);
                if (existing == null)
                    throw new AppException(ErrorCode.NOT_FOUND, "Order not found");
                await _unitOfWork.Orders.DeleteAsync(existing);
                return (await _unitOfWork.SaveChangesWithTransactionAsync()) > 0;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order: {Message}", ex.Message);
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR, "Could not delete order");
            }
        }
    }
}
