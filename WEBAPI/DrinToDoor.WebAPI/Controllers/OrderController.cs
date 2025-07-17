using DrinkToDoor.BLL.Interfaces;
using DrinkToDoor.BLL.ViewModel.ApiResponse;
using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.Data.enums;
using Microsoft.AspNetCore.Mvc;

namespace DrinToDoor.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderRequest req)
        {
            var response = await _orderService.Create(req);
            if (response == null)
                return BadRequest(
                    new ApiResponse
                    {
                        Code = 400,
                        Message = "Create order failed",
                        Success = false,
                    }
                );
            return Ok(
                new ApiResponse
                {
                    Code = 200,
                    Message = "Order created",
                    Success = true,
                    Data = response
                }
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var dto = await _orderService.GetById(id);
            if (dto == null)
                return NotFound(
                    new ApiResponse
                    {
                        Code = 404,
                        Message = "Order not found",
                        Success = false,
                    }
                );
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
            [FromQuery] Guid? userId,
            [FromQuery] EnumOrderStatus? status,
            [FromQuery] int pageCurrent = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var page = await _orderService.GetOrders(userId, status, pageCurrent, pageSize);
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
        public async Task<IActionResult> Update(Guid id, [FromBody] OrderRequest req)
        {
            if (!await _orderService.Update(id, req))
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
                    Message = "Order updated",
                    Success = true,
                }
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!await _orderService.Delete(id))
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
                    Message = "Order deleted",
                    Success = true,
                }
            );
        }
    }
}
