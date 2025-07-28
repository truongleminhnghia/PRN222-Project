using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Data.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrinkToDoor.Business.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponse>> GetAllAsync(string? name, EnumCategoryType? categoryType = null);
        Task<CategoryResponse?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(CategoryRequest request);
        Task<bool> UpdateAsync(Guid id, CategoryRequest request);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<CategoryResponse>> GetAll();
        Task<Tuple<IEnumerable<CategoryResponse>, int>> GetParams(string? name, int pageCurrent, int pageSize, EnumCategoryType? categoryType = null);
    }
}
