

using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.Data.Interfaces
{
    public interface ICartItemRepository
    {
        Task<bool> CreateAsync(CartItem item);
        Task<bool> UpdateQuantityAsync(CartItem item);
        Task<CartItem?> FindByIdAsync(Guid id);
        Task<bool> DeleteAsync(CartItem item);

    }
}
