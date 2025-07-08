

using AutoMapper;
using DrinkToDoor.BLL.Exceptions;
using DrinkToDoor.BLL.Interfaces;
using DrinkToDoor.BLL.Utils;
using DrinkToDoor.BLL.ViewModel.Pages;
using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.BLL.ViewModel.Responses;
using DrinkToDoor.Data;
using DrinkToDoor.Data.Entities;
using Microsoft.Extensions.Logging;

namespace DrinkToDoor.BLL.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<IngredientService> _logger;
        private readonly Base _utils;
        private readonly IImageService _imageService;
        private readonly ISupplierService _supplierService;
        private readonly ICategoryService _categoryService;
        private readonly IPackagingOptionService _packagingOptionService;

        public IngredientService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<IngredientService> logger
                                , Base utils, IImageService imageService, ISupplierService supplierService,
                                ICategoryService categoryService, IPackagingOptionService packagingOptionService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _utils = utils;
            _imageService = imageService;
            _supplierService = supplierService;
            _categoryService = categoryService;
            _packagingOptionService = packagingOptionService;
        }

        public async Task<bool> CreateIngredient(IngredientRequest request)
        {
            try
            {
                var ingredient = _mapper.Map<Ingredient>(request);
                ingredient.Code = Base.GenerateCode('P');
                await _unitOfWork.Ingredients.AddAsync(ingredient);
                //var supplier = await _unitOfWork.Suppliers.FindByEmail(request.Supplier.Email);
                //if (supplier == null)
                //{
                //    supplier = await _supplierService.CreateSupplier(request.Supplier);
                //}
                //ingredient.SupplierId = supplier.Id;
                var category = await _unitOfWork.Categories.FindByName(request.Category.Name);
                if (category == null) throw new AppException(ErrorCode.NOT_FOUND, "Category not found with name");
                ingredient.CategoryId = category.Id;
                var image = await _imageService.AddImages(ingredient.Id, request.ImagesRequest);
                var package = await _packagingOptionService.Add(ingredient.Id, request.PackagingOptions);
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
                return result > 0 ? true : false;
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

        public async Task<IngredientResponse?> GetById(Guid id)
        {
            try
            {
                var ingredient = await _unitOfWork.Ingredients.FindById(id);
                if (ingredient == null) throw new AppException(ErrorCode.NOT_FOUND, "Ingredient not found");
                return _mapper.Map<IngredientResponse>(ingredient);
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

        public async Task<PageResult<IngredientResponse>> GetIngredients(int pageCurrent, int pageSize)
        {
            try
            {
                var result = await _unitOfWork.Ingredients.GetAllAsync();
                if (result == null) throw new AppException(ErrorCode.LIST_EMPTY);
                var pagedResult = result.Skip((pageCurrent - 1) * pageSize).Take(pageSize).ToList();
                var total = result.Count();
                var data = _mapper.Map<List<IngredientResponse>>(pagedResult);
                if (data == null || !data.Any()) throw new AppException(ErrorCode.LIST_EMPTY);
                var pageResult = new PageResult<IngredientResponse>(data, pageSize, pageCurrent, total);
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
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }
    }
}