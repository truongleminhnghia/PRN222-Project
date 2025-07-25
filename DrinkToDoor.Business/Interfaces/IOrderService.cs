
using DrinkToDoor.Business.Dtos.Requests;

namespace DrinkToDoor.Business.Interfaces
{
    public interface IOrderService
    {
        Task<Guid> CreateOrder(OrderRequest request);
    }
}