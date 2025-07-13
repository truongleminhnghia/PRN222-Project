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
    public class IngredientProductService : IIngredientProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<IngredientProductService> _logger;

        public IngredientProductService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<IngredientProductService> logger
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> Create(IngredientProductRequest request)
        {
            try
            {
                var existing = await _unitOfWork.IngredientProducts.FindByName(request.Name);
                if (existing != null)
                    throw new AppException(ErrorCode.HAS_EXISTED, "Ingredient product existed");

                var entity = _mapper.Map<IngredientProduct>(request);
                await _unitOfWork.IngredientProducts.AddAsync(entity);
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
                return result > 0;
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "AppException: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: {Message}", ex.Message);
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR, "Internal server error");
            }
        }

        public async Task<IngredientProductResponse?> GetById(Guid id)
        {
            try
            {
                var existing = await _unitOfWork.IngredientProducts.FindById(id);
                if (existing == null)
                    throw new AppException(ErrorCode.NOT_FOUND, "Ingredient product not found");

                var dto = _mapper.Map<IngredientProductResponse>(existing);
                dto.IngredientName = existing.Ingredient?.Name ?? string.Empty;
                return dto;
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "AppException: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: {Message}", ex.Message);
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR, "Internal server error");
            }
        }

        public async Task<PageResult<IngredientProductResponse>> GetIngredientProducts(
            string? name,
            Guid? ingredientId,
            int pageCurrent,
            int pageSize
        )
        {
            try
            {
                var items = await _unitOfWork.IngredientProducts.GetAllAsync(name, ingredientId);
                if (items == null || !items.Any())
                    throw new AppException(ErrorCode.LIST_EMPTY);

                var total = items.Count();
                var paged = items.Skip((pageCurrent - 1) * pageSize).Take(pageSize).ToList();
                var data = _mapper.Map<List<IngredientProductResponse>>(paged);

                for (int i = 0; i < paged.Count; i++)
                    data[i].IngredientName = paged[i].Ingredient?.Name ?? string.Empty;

                return new PageResult<IngredientProductResponse>(
                    data,
                    pageSize,
                    pageCurrent,
                    total
                );
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "AppException: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: {Message}", ex.Message);
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR, "Internal server error");
            }
        }

        public async Task<bool> Update(Guid id, IngredientProductRequest request)
        {
            try
            {
                var existing = await _unitOfWork.IngredientProducts.FindById(id);
                if (existing == null)
                    throw new AppException(ErrorCode.NOT_FOUND, "Ingredient product not found");

                if (!string.IsNullOrEmpty(request.Name))
                    existing.Name = request.Name;
                existing.Price = request.Price;
                existing.TotalAmount = request.TotalAmount;
                existing.QuantityPackage = request.QuantityPackage;
                if (!string.IsNullOrEmpty(request.UnitPackage))
                    existing.UnitPackage = request.UnitPackage;
                existing.IngredientId = request.IngredientId;

                await _unitOfWork.IngredientProducts.UpdateAsync(existing);
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
                return result > 0;
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "AppException: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: {Message}", ex.Message);
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR, "Internal server error");
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                var existing = await _unitOfWork.IngredientProducts.FindById(id);
                if (existing == null)
                    throw new AppException(ErrorCode.NOT_FOUND, "Ingredient product not found");

                var removed = await _unitOfWork.IngredientProducts.DeleteAsync(existing);
                if (!removed)
                    return false;

                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
                return result > 0;
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "AppException in Delete: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in Delete: {Message}", ex.Message);
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR, "Internal server error");
            }
        }
    }
}
