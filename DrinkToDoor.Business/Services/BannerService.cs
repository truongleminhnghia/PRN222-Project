
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
            new BannerResponse { Url = "https://bizweb.dktcdn.net/100/486/168/themes/908225/assets/slider_2.jpg?1701164815113" },
            new BannerResponse { Url = "https://nguyenlieuhala.com/wp-content/uploads/2018/05/nguyen-lieu-pha-che-hala-1.png" },
            new BannerResponse { Url = "https://bizweb.dktcdn.net/100/486/168/themes/908225/assets/slider_1.jpg?1701164815113" },
            new BannerResponse { Url = "https://glofood.vn/upload/images/nguye%CC%82n%20lie%CC%A3%CC%82u%20tra%CC%80%20tra%CC%81i%20ca%CC%82y.png" },
        };

        public async Task<List<BannerResponse>> GetAllBannersAsync()
        {
            return _banners;
        }
    }
}