
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.enums;

namespace DrinkToDoor.Data.Interfaces
{
    public interface ICategoryRepository
    {
        Task<int> AddAsync(Category category);
        Task<int> UpdateAsync(Category category);
        Task<bool> DeleteAsync(Category category);
        Task<Category?> FindById(Guid id);
        Task<Category> FindByName(string name);
        Task<IEnumerable<Category>> FindAll();
        Task<IEnumerable<Category>> GetAllAsync(string? name, EnumCategoryType? categoryType);
    }
}