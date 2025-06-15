using AutoMapper;
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Data;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrinkToDoor.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllAsync(EnumCategoryType? categoryType = null)
        {
            var entities = await _unitOfWork.Categories.GetAllAsync(categoryType);
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
    }
}
