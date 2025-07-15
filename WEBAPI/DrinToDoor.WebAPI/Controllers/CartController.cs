using DrinkToDoor.BLL.Interfaces;
using DrinkToDoor.BLL.Services;
using DrinkToDoor.BLL.ViewModel.ApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrinToDoor.WebAPI.Controllers
{
    [Route("api/v1/carts")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ICartItemService _cartItemService;

        public CartController(ICartService cartService, ICartItemService cartItemService)
        {
            _cartService = cartService;
            _cartItemService = cartItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWithParams(  [FromQuery] Guid? userId,
                                                            [FromQuery] string? sortBy = "createdAt",
                                                            [FromQuery] bool isDescending = false,
                                                            [FromQuery] int pageNumber = 1,
                                                            [FromQuery] int pageSize = 10)
        {
            var result = await _cartService.GetAllWithParamsAsync(
        userId, sortBy, isDescending, pageNumber, pageSize);

            return Ok(new ApiResponse
            {
                Code = StatusCodes.Status200OK,
                Success = true,
                Message = "Successful",
                Data = result
            });
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCart([FromQuery] Guid userId)
        {
                var result = await _cartService.CreateCartAsync(userId);
                if (result)
                {
                    return Ok(new ApiResponse
                    {
                        Code = StatusCodes.Status200OK,
                        Success = true,
                        Message = "Cart created successfully"
                    });
                } else
                {
                    return BadRequest(new ApiResponse
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Success = false,
                        Message = "Cart already exists for this user"
                    });
                }           
        }


    }
}
