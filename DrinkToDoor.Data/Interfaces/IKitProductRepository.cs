using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.Data.Interfaces
{
    public interface IKitProductRepository
    {
        Task<int> Create(KitProduct kitProduct);
        Task<KitProduct?> FindById(Guid id);
        Task<IEnumerable<KitProduct>> FindAll();
        Task<bool> Update(KitProduct kitProduct);
        Task<bool> Delete(KitProduct kitProduct);
    }
}