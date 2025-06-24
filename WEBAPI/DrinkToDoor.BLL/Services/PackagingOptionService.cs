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
    public class PackagingOptionService : IPackagingOptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PackagingOptionService> _logger;

        public PackagingOptionService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PackagingOptionService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> Create(PackagingOptionRequest request)
        {
            try
            {
                var ingredientExisting = await _unitOfWork.Ingredients.FindById(request.IngredientId);
                if (ingredientExisting == null) throw new AppException(ErrorCode.NOT_FOUND, "Ingredient not found");
                var packagingOption = _mapper.Map<PackagingOption>(request);
                await _unitOfWork.PackagingOptions.Add(packagingOption);
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
                if (result == 0) return false;
                return true;
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "AppException occurred: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR, "Internal server error");
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                var existing = await _unitOfWork.PackagingOptions.FindById(id);
                if (existing == null) throw new AppException(ErrorCode.NOT_FOUND, "Package option not found");
                var result = await _unitOfWork.PackagingOptions.Delete(existing);
                if (!result) return false;
                return true;
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "AppException occurred: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR, "Internal server error");
            }
        }

        public async Task<PageResult<PackagingOptionResponse>> GetAll(int pageCurrent, int pageSize)
        {
            try
            {
                var result = await _unitOfWork.PackagingOptions.FindAll();
                if (result == null) throw new AppException(ErrorCode.LIST_EMPTY, "List empty");
                var pagedResult = result.Skip((pageCurrent - 1) * pageSize).Take(pageSize).ToList();
                var total = result.Count();
                var data = _mapper.Map<List<PackagingOptionResponse>>(pagedResult);
                if (data == null || !data.Any()) throw new AppException(ErrorCode.LIST_EMPTY);
                var pageResult = new PageResult<PackagingOptionResponse>(data, pageSize, pageCurrent, total);
                return pageResult;
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "AppException occurred: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR, "Internal server error");
            }
        }

        public async Task<PackagingOptionResponse?> GetById(Guid id)
        {
            try
            {
                var packagingOption = await _unitOfWork.PackagingOptions.FindById(id);
                if (packagingOption == null) throw new AppException(ErrorCode.NOT_FOUND, "Supplier not found");
                return _mapper.Map<PackagingOptionResponse>(packagingOption);
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "AppException occurred: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR, "Internal server error");
            }
        }

        public async Task<bool> Update(Guid id, PackagingOptionRequest request)
        {
            try
            {
                var packagingOption = await _unitOfWork.PackagingOptions.FindById(id);
                if (packagingOption == null) throw new AppException(ErrorCode.NOT_FOUND, "Supplier not found");
                var ingredientExisting = await _unitOfWork.Ingredients.FindById(request.IngredientId);
                if (ingredientExisting == null) throw new AppException(ErrorCode.NOT_FOUND, "Ingredient not found");
                if (request.Label != null) packagingOption.Label = request.Label;
                if (request.Size != null) packagingOption.Size = request.Size;
                if (request.Unit != null) packagingOption.Unit = request.Unit;
                if (request.Type != null) packagingOption.Type = request.Type;
                if (request.Price != null) packagingOption.Price = request.Price;
                if (request.Cost != null) packagingOption.Cost = request.Cost;
                if (request.StockQty != null) packagingOption.StockQty = request.StockQty;
                await _unitOfWork.PackagingOptions.Update(packagingOption);
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
                if (result == 0) return false;
                return true;
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "AppException occurred: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR, "Internal server error");
            }
        }
    }
}