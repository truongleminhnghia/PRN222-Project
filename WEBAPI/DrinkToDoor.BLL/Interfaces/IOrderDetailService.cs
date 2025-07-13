using DrinkToDoor.BLL.ViewModel.Pages;
using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.BLL.ViewModel.Responses;

namespace DrinkToDoor.BLL.Interfaces
{
    public interface IOrderDetailService
    {
        Task<bool> Create(OrderDetailRequest request);
        Task<OrderDetailResponse?> GetById(Guid id);
        Task<PageResult<OrderDetailResponse>> GetOrderDetails(
            Guid? orderId,
            Guid? kitProductId,
            Guid? ingredientProductId,
            int pageCurrent,
            int pageSize
        );
        Task<bool> Update(Guid id, OrderDetailRequest request);
        Task<bool> Delete(Guid id);
    }
}
