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
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CategoryService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> Create(CategoryRequest request)
        {
            try
            {
                var categoryExist = await _unitOfWork.Categories.FindByName(request.Name);
                if (categoryExist != null) throw new AppException(ErrorCode.HAS_EXISTED, "Category existed");
                var category = _mapper.Map<Category>(request);
                await _unitOfWork.Categories.AddAsync(category);
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
                if (result == 0)
                {
                    return false;
                }
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

        public async Task<CategoryResponse?> GetById(Guid id)
        {
            try
            {
                var categoryExist = await _unitOfWork.Categories.FindById(id);
                if (categoryExist == null) throw new AppException(ErrorCode.NOT_FOUND, "Category not found");
                return _mapper.Map<CategoryResponse>(categoryExist);
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

        public async Task<PageResult<CategoryResponse>> GetCategories(string? name, EnumCategoryType? type, int pageCurrent, int pageSize)
        {
            try
            {
                var result = await _unitOfWork.Categories.GetAllAsync(name, type);
                if (result == null) throw new AppException(ErrorCode.LIST_EMPTY);
                var pagedResult = result.Skip((pageCurrent - 1) * pageSize).Take(pageSize).ToList();
                var total = result.Count();
                var data = _mapper.Map<List<CategoryResponse>>(pagedResult);
                if (data == null || !data.Any()) throw new AppException(ErrorCode.LIST_EMPTY);
                var pageResult = new PageResult<CategoryResponse>(data, pageSize, pageCurrent, total);
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

        public async Task<bool> Update(Guid id, CategoryRequest request)
        {
            try
            {
                var categoryExisting = await _unitOfWork.Categories.FindById(id);
                if (categoryExisting == null) throw new AppException(ErrorCode.NOT_FOUND, "Category not found");
                if (request.Name != null) categoryExisting.Name = request.Name;
                if (request.CategoryType != null) categoryExisting.CategoryType = request.CategoryType;
                await _unitOfWork.Categories.UpdateAsync(categoryExisting);
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
                if (result == 0)
                {
                    return false;
                }
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
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }
    }
}