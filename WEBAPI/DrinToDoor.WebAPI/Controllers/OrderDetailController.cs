using DrinkToDoor.BLL.Interfaces;
using DrinkToDoor.BLL.ViewModel.ApiResponse;
using DrinkToDoor.BLL.ViewModel.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrinToDoor.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/order-details")]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderDetailRequest req)
        {
            if (!await _orderDetailService.Create(req))
                return BadRequest(
                    new ApiResponse
                    {
                        Code = 400,
                        Message = "Create failed",
                        Success = false,
                    }
                );
            return Ok(
                new ApiResponse
                {
                    Code = 200,
                    Message = "Created",
                    Success = true,
                }
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var dto = await _orderDetailService.GetById(id);
            return Ok(
                new ApiResponse
                {
                    Code = 200,
                    Success = true,
                    Data = dto,
                }
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] Guid? orderId,
            [FromQuery] Guid? kitProductId,
            [FromQuery] Guid? ingredientProductId,
            [FromQuery] int pageCurrent = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var page = await _orderDetailService.GetOrderDetails(
                orderId,
                kitProductId,
                ingredientProductId,
                pageCurrent,
                pageSize
            );
            return Ok(
                new ApiResponse
                {
                    Code = 200,
                    Success = true,
                    Data = page,
                }
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] OrderDetailRequest req)
        {
            if (!await _orderDetailService.Update(id, req))
                return BadRequest(
                    new ApiResponse
                    {
                        Code = 400,
                        Message = "Update failed",
                        Success = false,
                    }
                );
            return Ok(
                new ApiResponse
                {
                    Code = 200,
                    Message = "Updated",
                    Success = true,
                }
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!await _orderDetailService.Delete(id))
                return BadRequest(
                    new ApiResponse
                    {
                        Code = 400,
                        Message = "Delete failed",
                        Success = false,
                    }
                );
            return Ok(
                new ApiResponse
                {
                    Code = 200,
                    Message = "Deleted",
                    Success = true,
                }
            );
        }
    }
}
