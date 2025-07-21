
using AutoMapper;
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using DrinkToDoor.Business.Utils;
using DrinkToDoor.Data;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.enums;
using Microsoft.Extensions.Logging;

namespace DrinkToDoor.Business.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly ILogger<IngredientService> _logger;
        private readonly IPackagingOptionService _packagingOptionService;

        public IngredientService(IUnitOfWork unitOfWork, IMapper mapper,
                                ILogger<IngredientService> logger,
                                IImageService imageService, IPackagingOptionService packagingOptionService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _imageService = imageService;
            _packagingOptionService = packagingOptionService;
        }

        public async Task<bool> CreateAsync(IngredientRequest request)
        {
            try
            {
                if (request.CategoryId == null || request.CategoryId == Guid.Empty)
                {
                    throw new ArgumentException("CategoryId không được bỏ trống");
                }
                var cateogry = await _unitOfWork.Categories.FindById(request.CategoryId.Value);
                if (cateogry == null)
                {
                    throw new ArgumentException($"Danh mục không tồn tại với ID {request.CategoryId}");
                }
                if (request.SupplierId == null || request.SupplierId == Guid.Empty)
                {
                    throw new ArgumentException("SupplierId không được bỏ trống");
                }
                var supplier = await _unitOfWork.Suppliers.FindById(request.SupplierId.Value);
                if (supplier == null)
                {
                    throw new ArgumentException($"Nhà cung cấp không tồn tại với ID {request.SupplierId}");
                }
                var ingredient = _mapper.Map<Ingredient>(request);
                ingredient.Code = Base.GenerateCode('P');
                ingredient.Status = EnumStatus.ACTIVE;
                ingredient.CategoryId = cateogry.Id;
                ingredient.SupplierId = supplier.Id;
                await _unitOfWork.Ingredients.AddAsync(ingredient);
                var image = await _imageService.AddImages(ingredient.Id, request.ImagesRequest);
                var package = await _packagingOptionService.Add(ingredient.Id, request.PackagingOptions);
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
                return result > 0 ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    throw new ArgumentException("Id không được bỏ trống");
                }
                var ingredient = await _unitOfWork.Ingredients.FindById(id);
                if (ingredient == null)
                {
                    throw new ArgumentException($"Nguyên liệu không tồn tại với ID {id}");
                }
                if (ingredient.Status == EnumStatus.INACTIVE)
                {
                    throw new ArgumentException($"Nguyên liệu với ID {id} đã bị tạm dừng");
                }
                ingredient.Status = EnumStatus.INACTIVE;
                await _unitOfWork.Ingredients.UpdateAsync(ingredient);
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
                return result > 0 ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<Tuple<IEnumerable<IngredientResponse>, int>> GetAsync(string? name, int pageCurrent, int pageSize)
        {
            try
            {
                var result = await _unitOfWork.Ingredients.GetAllAsync();
                if (result == null)
                {
                    throw new ArgumentException("Danh sách rỗng");
                }
                var total = result.Count();
                var paged = result.Skip((pageCurrent - 1) * pageSize).Take(pageSize).ToList();
                var pagedResponses = _mapper.Map<List<IngredientResponse>>(paged);
                return Tuple.Create<IEnumerable<IngredientResponse>, int>(pagedResponses, total);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<IngredientResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    throw new ArgumentException("Id không được bỏ trống");
                }
                var ingredient = await _unitOfWork.Ingredients.FindById(id);
                if (ingredient == null)
                {
                    throw new ArgumentException($"Nguyên liệu không tồn tại với ID {id}");
                }
                return _mapper.Map<IngredientResponse>(ingredient);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<bool> UpdateAsync(Guid id, IngredientUpdateRequest request)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    throw new ArgumentException("Id không được bỏ trống");
                }
                var ingredient = await _unitOfWork.Ingredients.FindById(id);
                if (ingredient == null)
                {
                    throw new ArgumentException($"Nguyên liệu không tồn tại với ID {id}");
                }
                if (request.Name != null) ingredient.Name = request.Name;
                if (request.Description != null) ingredient.Description = request.Description;
                if (request.Status != null) ingredient.Status = request.Status.Value;
                if (request.Price != null) ingredient.Price = request.Price.Value;
                if (request.Cost != null) ingredient.Cost = request.Cost.Value;
                if (request.StockQty != null) ingredient.StockQty = request.StockQty.Value;
                await _unitOfWork.Ingredients.UpdateAsync(ingredient);
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
                return result > 0 ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }
    }
}