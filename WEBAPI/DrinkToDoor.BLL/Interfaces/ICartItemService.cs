

using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.BLL.ViewModel.Responses;
using DrinkToDoor.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DrinkToDoor.BLL.Interfaces
{
    public interface ICartItemService
    {
        Task<bool> CreateCartItemAsync(CartItemRequest request);
        Task<bool> UpdateCartItemAsync(Guid id, int quantity);
        Task<bool> DeleteCartItemAsync(Guid id);

    }
}
