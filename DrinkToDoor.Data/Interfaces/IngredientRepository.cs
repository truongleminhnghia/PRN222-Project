
using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.Data.Interfaces
{
    public interface IngredientRepository
    {
        Task<bool> Create(Ingredient ingredient);
        Task<Ingredient?> FindById(Guid id);
        Task<IEnumerable<Ingredient>> FindAll(string? name, decimal? minPrice, decimal? maxPrice);
    }
}