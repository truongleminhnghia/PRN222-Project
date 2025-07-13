using DrinkToDoor.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrinToDoor.WebAPI.Controllers
{
    [Route("api/v1/carts")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWithParams(  [FromQuery] Guid? userId,
                                                            [FromQuery] string? sortBy = "id",
                                                            [FromQuery] bool isDescending = false,
                                                            [FromQuery] int pageNumber = 1,
                                                            [FromQuery] int pageSize = 10)
        {
            var result = await _cartService.GetAllWithParamsAsync(
        userId, sortBy, isDescending, pageNumber, pageSize);

            return Ok(result);
        }
    }
}
