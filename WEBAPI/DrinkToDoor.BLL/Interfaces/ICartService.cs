
using DrinkToDoor.BLL.ViewModel.Pages;
using DrinkToDoor.BLL.ViewModel.Responses;

namespace DrinkToDoor.BLL.Interfaces
{
    public interface ICartService
    {
        Task<PageResult<CartResponse>> GetAllWithParamsAsync(Guid? userId, string? sortBy, bool isDescending, int pageNumber, int pageSize);
    }
}
