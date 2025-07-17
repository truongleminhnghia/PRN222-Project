using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrinkToDoor.BLL.ViewModel.Pages;
using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.BLL.ViewModel.Responses;
using DrinkToDoor.Data.enums;

namespace DrinkToDoor.BLL.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponse> Create(OrderRequest request);
        Task<OrderResponse?> GetById(Guid id);
        Task<PageResult<OrderResponse>> GetOrders(
            Guid? userId,
            EnumOrderStatus? status,
            int pageCurrent,
            int pageSize
        );
        Task<bool> Update(Guid id, OrderRequest request);
        Task<bool> Delete(Guid id);
    }
}
