using AutoMapper;
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Data;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.enums;
using Microsoft.Extensions.Logging;

namespace DrinkToDoor.Business.Services
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

        public async Task<IEnumerable<CategoryResponse>> GetAllAsync(string? name, EnumCategoryType? categoryType = null)
        {
            var entities = await _unitOfWork.Categories.GetAllAsync(name, categoryType);
            return _mapper.Map<IEnumerable<CategoryResponse>>(entities);
        }

        public async Task<CategoryResponse?> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Categories.FindById(id);
            return entity == null ? null : _mapper.Map<CategoryResponse>(entity);
        }

        public async Task<Guid> CreateAsync(CategoryRequest request)
        {
            var category = _mapper.Map<Category>(request);
            category.Id = Guid.NewGuid(); // tạo ID mới

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return category.Id;
        }

        public async Task<bool> UpdateAsync(Guid id, CategoryRequest request)
        {
            var category = await _unitOfWork.Categories.FindById(id);
            if (category == null) return false;

            category.Name = request.Name;
            category.CategoryType = request.CategoryType;

            await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var category = await _unitOfWork.Categories.FindById(id);
            if (category == null) return false;

            await _unitOfWork.Categories.DeleteAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAll()
        {
            try
            {
                var categories = await _unitOfWork.Categories.FindAll();
                if (categories == null || !categories.Any())
                {
                    _logger.LogWarning("No categories found.");
                    return Enumerable.Empty<CategoryResponse>();
                }
                return _mapper.Map<IEnumerable<CategoryResponse>>(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<Tuple<IEnumerable<CategoryResponse>, int>> GetParams(string? name, int pageCurrent, int pageSize, EnumCategoryType? categoryType = null)
        {
            try
            {
                var result = await _unitOfWork.Categories.GetAllAsync(name, categoryType);
                if (result == null)
                {
                    throw new ArgumentException("Danh sách rỗng");
                }
                var total = result.Count();
                var paged = result.Skip((pageCurrent - 1) * pageSize).Take(pageSize).ToList();
                var pagedResponses = _mapper.Map<List<CategoryResponse>>(paged);
                return Tuple.Create<IEnumerable<CategoryResponse>, int>(pagedResponses, total);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }
    }
}
