
using DrinkToDoor.BLL.ViewModel.Responses;

namespace DrinkToDoor.Business.Interfaces
{
    public interface IBannerService
    {
        Task<List<BannerResponse>> GetAllBannersAsync();
    }
}