using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;

namespace DrinkToDoor.Business.Interfaces
{
    public interface ICartService
    {
        Task<bool> CreateCart(Guid userId);
        Task<CartResponse?> GetById(Guid cartId);
        Task<IEnumerable<CartResponse>> GetAll();
        Task<CartResponse?> GetByUser(Guid userId);
        Task<bool> AddToCart(CartRequest requst);
    }
}