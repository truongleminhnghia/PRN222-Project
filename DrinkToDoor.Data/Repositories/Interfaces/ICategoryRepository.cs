
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.enums;

namespace DrinkToDoor.Data.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<int> AddAsync(Category category);
        Task<int> UpdateAsync(Category category);
        Task<bool> DeleteAsync(Category category);
        Task<Category?> FindById(Guid id);
        Task<IEnumerable<Category>> GetAllAsync(EnumCategoryType? categoryType);
    }
}