using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkToDoor.BLL.ViewModel.ApiResponse;
using DrinkToDoor.BLL.ViewModel.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DrinToDoor.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/banners")]
    public class BannerController : ControllerBase
    {
        private static readonly List<BannerResponse> _banners = new()
        {
            new BannerResponse { Url = "https://res.cloudinary.com/dkikc5ywq/image/upload/v1741202773/banner_1_vqsiey.png" },
            new BannerResponse { Url = "https://res.cloudinary.com/dkikc5ywq/image/upload/v1741337427/banner_2_ikyyvu.png" }
        };

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(new ApiResponse
            {
                Code = StatusCodes.Status200OK,
                Success = true,
                Data = _banners
            });
        }

    }
}