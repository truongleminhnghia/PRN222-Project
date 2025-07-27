
using AutoMapper;
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using DrinkToDoor.Data;
using DrinkToDoor.Data.Entities;
using Microsoft.Extensions.Logging;

namespace DrinkToDoor.Business.Services
{
    public class IngredientProductService : IIngredientProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IngredientProductService> _logger;
        private readonly IMapper _mapper;

        public IngredientProductService(IUnitOfWork unitOfWork, ILogger<IngredientProductService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Guid> Create(IngredientProductRequest request)
        {
            try
            {
                var ingredientExisting = await _unitOfWork.Ingredients.FindById(request.IngredientId);
                if (ingredientExisting == null) throw new Exception("Nguyên liệu không tồn tại");
                var ingredientProduct = _mapper.Map<IngredientProduct>(request);
                ingredientProduct.Price = ingredientExisting.Price;
                ingredientProduct.TotalAmount = ingredientExisting.Price * request.QuantityPackage;
                await _unitOfWork.IngredientProducts.AddAsync(ingredientProduct);
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
                return result > 0 ? ingredientProduct.Id : Guid.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<IngredientProductResponse?> GetById(Guid id)
        {
            try
            {
                var ingredientProductExisting = await _unitOfWork.IngredientProducts.FindById(id);
                if (ingredientProductExisting == null) throw new Exception("Sản phẩm ko tồn tại");
                return _mapper.Map<IngredientProductResponse>(ingredientProductExisting);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }
    }
}