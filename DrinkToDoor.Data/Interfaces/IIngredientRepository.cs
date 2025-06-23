
using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.Data.Interfaces
{
    public interface IIngredientRepository
    {
        Task<int> AddAsync(Ingredient ingredient);
        Task<bool> DeleteAsync(Ingredient ingredient);
        Task<IEnumerable<Ingredient>> GetAllAsync();
        Task<Ingredient?> FindById(Guid id);
        Task<int> UpdateAsync(Ingredient ingredient);
    }
}