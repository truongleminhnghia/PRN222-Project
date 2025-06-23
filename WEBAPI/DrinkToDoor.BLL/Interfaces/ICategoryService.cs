using DrinkToDoor.BLL.ViewModel.Pages;
using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.BLL.ViewModel.Responses;
using DrinkToDoor.Data.enums;

namespace DrinkToDoor.BLL.Interfaces
{
    public interface ICategoryService
    {
        Task<bool> Create(CategoryRequest request);
        Task<CategoryResponse?> GetById(Guid id);
        Task<PageResult<CategoryResponse>> GetCategories(string? name, EnumCategoryType? type, int pageCurrent, int pageSize);
        Task<bool> Update(Guid id, CategoryRequest request);
    }
}