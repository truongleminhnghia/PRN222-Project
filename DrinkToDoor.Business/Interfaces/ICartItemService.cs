
using DrinkToDoor.Business.Dtos.Responses;

namespace DrinkToDoor.Business.Interfaces
{
    public interface ICartItemService
    {
        Task<IEnumerable<CartItemResponse>> GetByIds(List<Guid> ids);
        Task<bool> DeleteItem(Guid id);
        Task<int> Count(Guid cartId);
    }
}