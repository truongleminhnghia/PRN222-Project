using AutoMapper;
using DrinkToDoor.BLL.Exceptions;
using DrinkToDoor.BLL.Interfaces;
using DrinkToDoor.BLL.ViewModel.Pages;
using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.BLL.ViewModel.Responses;
using DrinkToDoor.Data;
using DrinkToDoor.Data.Entities;
using Microsoft.Extensions.Logging;

namespace DrinkToDoor.BLL.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderDetailService> _logger;

        public OrderDetailService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<OrderDetailService> logger
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> Create(OrderDetailRequest request)
        {
            try
            {
                var entity = _mapper.Map<OrderDetail>(request);
                await _unitOfWork.OrderDetails.AddAsync(entity);
                return (await _unitOfWork.SaveChangesWithTransactionAsync()) > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order detail: {Message}", ex.Message);
                throw new AppException(
                    ErrorCode.INTERNAL_SERVER_ERROR,
                    "Could not create order detail"
                );
            }
        }

        public async Task<OrderDetailResponse?> GetById(Guid id)
        {
            var entity = await _unitOfWork.OrderDetails.FindById(id);
            if (entity == null)
                throw new AppException(ErrorCode.NOT_FOUND, "OrderDetail not found");

            var dto = _mapper.Map<OrderDetailResponse>(entity);
            dto.KitProductName = entity.KitProduct?.Name ?? string.Empty;
            dto.IngredientProductName = entity.IngredientProduct?.Name ?? string.Empty;
            return dto;
        }

        public async Task<PageResult<OrderDetailResponse>> GetOrderDetails(
            Guid? orderId,
            Guid? kitProductId,
            Guid? ingredientProductId,
            int pageCurrent,
            int pageSize
        )
        {
            var list = await _unitOfWork.OrderDetails.GetAllAsync(
                orderId,
                kitProductId,
                ingredientProductId
            );
            if (!list.Any())
                throw new AppException(ErrorCode.LIST_EMPTY);

            var total = list.Count();
            var paged = list.Skip((pageCurrent - 1) * pageSize).Take(pageSize).ToList();
            var data = _mapper.Map<List<OrderDetailResponse>>(paged);

            for (int i = 0; i < paged.Count; i++)
            {
                data[i].KitProductName = paged[i].KitProduct?.Name ?? string.Empty;
                data[i].IngredientProductName = paged[i].IngredientProduct?.Name ?? string.Empty;
            }

            return new PageResult<OrderDetailResponse>(data, pageSize, pageCurrent, total);
        }

        public async Task<bool> Update(Guid id, OrderDetailRequest request)
        {
            var existing = await _unitOfWork.OrderDetails.FindById(id);
            if (existing == null)
                throw new AppException(ErrorCode.NOT_FOUND, "OrderDetail not found");

            _mapper.Map(request, existing);
            await _unitOfWork.OrderDetails.UpdateAsync(existing);
            return (await _unitOfWork.SaveChangesWithTransactionAsync()) > 0;
        }

        public async Task<bool> Delete(Guid id)
        {
            var existing = await _unitOfWork.OrderDetails.FindById(id);
            if (existing == null)
                throw new AppException(ErrorCode.NOT_FOUND, "OrderDetail not found");

            await _unitOfWork.OrderDetails.DeleteAsync(existing);
            return (await _unitOfWork.SaveChangesWithTransactionAsync()) > 0;
        }
    }
}
