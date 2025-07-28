
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.enums;

namespace DrinkToDoor.Data.Interfaces
{
    public interface IIngredientRepository
    {
        Task<int> AddAsync(Ingredient ingredient);
        Task<bool> DeleteAsync(Ingredient ingredient);
        Task<IEnumerable<Ingredient>> GetAllAsync(string? keyword, string? name, Guid? categoryId, decimal? minPirce, decimal? maxPrice,
                                                decimal? minCost, decimal? maxCost, int? minQuantity, int? maxQuantity, EnumStatus? status);
        Task<Ingredient?> FindById(Guid id);
        Task<int> UpdateAsync(Ingredient ingredient);
        Task<IEnumerable<Ingredient>> FindByName(string? name);
    }
}