using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.Data.Interfaces
{
    public interface IKitIngredientRepository
    {
        Task<int> Create(KitIngredient kitIngredient);
        Task<KitIngredient?> FindById(Guid id);
        Task<IEnumerable<KitIngredient>> FindAll();
        Task<bool> Update(KitIngredient kitIngredient);
        Task<bool> Delete(KitIngredient kitIngredient);
    }
}