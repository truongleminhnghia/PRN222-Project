

using AutoMapper;
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using DrinkToDoor.Data;
using DrinkToDoor.Data.Entities;
using Microsoft.Extensions.Logging;

namespace DrinkToDoor.Business.Services
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

        public async Task<bool> Add(Guid id, List<PackagingOptionRequest> request)
        {
            var packages = new List<PackagingOption>();
            try
            {

                foreach (var item in request)
                {
                    var package = new PackagingOption
                    {
                        IngredientId = id,
                        Label = item.Label,
                        Size = item.Size,
                        Unit = item.Unit,
                        Type = item.Type,
                        Price = item.Price,
                        Cost = item.Cost,
                        StockQty = item.StockQty,
                    };
                    await _unitOfWork.PackagingOptions.Add(package);
                    packages.Add(package);
                }
                // await _unitOfWork.SaveChangesWithTransactionAsync();
                return packages.Count > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                var existing = await _unitOfWork.PackagingOptions.FindById(id);
                if (existing == null) throw new Exception("Package option not found");
                var result = await _unitOfWork.PackagingOptions.Delete(existing);
                if (!result) return false;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        // public async Task<PageResult<PackagingOptionResponse>> GetAll(int pageCurrent, int pageSize)
        // {
        //     try
        //     {
        //         var result = await _unitOfWork.PackagingOptions.FindAll();
        //         if (result == null) throw new AppException(ErrorCode.LIST_EMPTY, "List empty");
        //         var pagedResult = result.Skip((pageCurrent - 1) * pageSize).Take(pageSize).ToList();
        //         var total = result.Count();
        //         var data = _mapper.Map<List<PackagingOptionResponse>>(pagedResult);
        //         if (data == null || !data.Any()) throw new AppException(ErrorCode.LIST_EMPTY);
        //         var pageResult = new PageResult<PackagingOptionResponse>(data, pageSize, pageCurrent, total);
        //         return pageResult;
        //     }
        //     catch (AppException ex)
        //     {
        //         _logger.LogWarning(ex, "AppException occurred: {Message}", ex.Message);
        //         throw;
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);
        //         throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR, "Internal server error");
        //     }
        // }

        public async Task<PackagingOptionResponse?> GetById(Guid id)
        {
            try
            {
                var packagingOption = await _unitOfWork.PackagingOptions.FindById(id);
                if (packagingOption == null) throw new Exception("Supplier not found");
                return _mapper.Map<PackagingOptionResponse>(packagingOption);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }
    }
}