
using DrinkToDoor.BLL.ViewModel.Responses;
using DrinkToDoor.Business.Interfaces;

namespace DrinkToDoor.Business.Services
{
    public class BannerService : IBannerService
    {
        public BannerService()
        {

        }
        private static readonly List<BannerResponse> _banners = new()
        {
            new BannerResponse { Url = "https://res.cloudinary.com/dkikc5ywq/image/upload/v1741202773/banner_1_vqsiey.png" },
            new BannerResponse { Url = "https://res.cloudinary.com/dkikc5ywq/image/upload/v1741337427/banner_2_ikyyvu.png" }
        };

        public async Task<List<BannerResponse>> GetAllBannersAsync()
        {
            return _banners;
        }
    }
}