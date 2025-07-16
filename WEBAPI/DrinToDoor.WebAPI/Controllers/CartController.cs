using DrinkToDoor.BLL.Interfaces;
using DrinkToDoor.BLL.Services;
using DrinkToDoor.BLL.ViewModel.ApiResponse;
using DrinkToDoor.BLL.ViewModel.Requests;
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
        public async Task<IActionResult> GetAllWithParams([FromQuery] Guid? userId,
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

        [HttpGet("getByUserId")]
        public async Task<IActionResult> GetCartByUserId([FromQuery] Guid userId)
        {
            var result = await _cartService.GetCartByUserIdAsync(userId);
            if (result != null)
            {
                return Ok(new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Cart retrieved successfully",
                    Data = result
                });
            }
            else
            {
                return NotFound(new ApiResponse
                {
                    Code = StatusCodes.Status404NotFound,
                    Success = false,
                    Message = "Cart not found for this user"
                });
            }
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
            }
            else
            {
                return BadRequest(new ApiResponse
                {
                    Code = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = "Cart already exists for this user"
                });
            }
        }

        [HttpPost("createCartItem")]
        public async Task<IActionResult> CreateCartItem([FromBody] CartItemRequest request)
        {
            try
            {
                var result = await _cartItemService.CreateCartItemAsync(request);
                if (result)
                {
                    return Ok(new ApiResponse
                    {
                        Code = StatusCodes.Status200OK,
                        Success = true,
                        Message = "Cart item created successfully"
                    });
                }
                else
                {
                    return BadRequest(new ApiResponse
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Success = false,
                        Message = "Failed to create cart item"
                    });
                }
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new ApiResponse
                {
                    Code = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = ex.Message
                });
            }
        }


        [HttpPut("{id}/cartItem")]
        public async Task<IActionResult> UpdateCartItemQuantity(Guid id, [FromQuery] int quantity)
        {
            try
            {
                if (quantity < 1)
                    return BadRequest("Quantity must be at least 1.");

                var updated = await _cartItemService.UpdateCartItemAsync(id, quantity);
                if (updated)
                {
                    return Ok(new ApiResponse
                    {
                        Code = StatusCodes.Status200OK,
                        Success = true,
                        Message = "Cart item updated successfully"
                    });
                }
                else
                {
                    return NotFound(new ApiResponse
                    {
                        Code = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "Cart item not found"
                    });
                }
            } catch (ApplicationException ex)
            {
                return BadRequest(new ApiResponse
                {
                    Code = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("{id}/cartItem")]
        public async Task<IActionResult> DeleteCartItem(Guid id)
        {
            try
            {
                var deleted = await _cartItemService.DeleteCartItemAsync(id);
                if (deleted)
                {
                    return Ok(new ApiResponse
                    {
                        Code = StatusCodes.Status200OK,
                        Success = true,
                        Message = "Cart item deleted successfully"
                    });
                }
                else
                {
                    return NotFound(new ApiResponse
                    {
                        Code = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "Cart item not found"
                    });
                }
            } catch (ApplicationException ex)
            {
                return BadRequest(new ApiResponse
                {
                    Code = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}
