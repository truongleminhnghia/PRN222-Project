
using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.Data.Interfaces
{
    public interface ICartRepository
    {
        Task<int> AddAsync(Cart cart);
        Task<Cart?> FindById(Guid cartId);
        Task<Cart?> FindByUserId(Guid userId);
        Task<bool> Delete(Cart cart);
    }
}
