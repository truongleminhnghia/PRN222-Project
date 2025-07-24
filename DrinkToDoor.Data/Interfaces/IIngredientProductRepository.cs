using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.Data.Interfaces
{
    public interface IIngredientProductRepository
    {
        Task<int> AddAsync(IngredientProduct entity);
        Task<IngredientProduct> Create(IngredientProduct entity);
        Task<int> UpdateAsync(IngredientProduct entity);
        Task<bool> DeleteAsync(IngredientProduct entity);
        Task<IngredientProduct?> FindById(Guid id);
        Task<IngredientProduct> FindByName(string name);
        Task<IEnumerable<IngredientProduct>> GetAllAsync(string? name, Guid? ingredientId);
    }
}
