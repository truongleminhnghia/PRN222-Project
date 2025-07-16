using DrinkToDoor.BLL.Interfaces;
using DrinkToDoor.BLL.ViewModel.ApiResponse;
using DrinkToDoor.BLL.ViewModel.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrinToDoor.WebAPI.Controllers
{
    [Route("api/v1/messages")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _service;

        public MessageController(IMessageService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
                                                [FromQuery] Guid? messageId,
                                                [FromQuery] Guid? senderId,
                                                [FromQuery] Guid? receiverId,
                                                [FromQuery] string? message,
                                                [FromQuery] bool? readed,
                                                [FromQuery] string? sortBy = "createdAt",
                                                [FromQuery] bool isDescending = true
                                                )
        {
            var list = await _service.GetAllAsync(messageId, senderId, receiverId, message, readed, sortBy, isDescending);
            return Ok(new ApiResponse
            {
                Code = StatusCodes.Status200OK,
                Data = list,
                Message = "Messages retrieved successfully."
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var message = await _service.GetByIdAsync(id);
                if (message == null) return NotFound();
                return Ok(new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Data = message,
                    Message = "Message retrieved successfully."
                });
            }
            catch (ApplicationException ex)
            {
                return NotFound(new ApiResponse
                {
                    Code = StatusCodes.Status404NotFound,
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MessageRequest request)
        {
            try
            {
                var created = await _service.CreateAsync(request);
                return Ok(new ApiResponse
                {
                    Code = StatusCodes.Status201Created,
                    Message = "Message created successfully."
                });
            }
            catch (ApplicationException ex) 
            { 
                return BadRequest(new ApiResponse
                {
                    Code = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }
        }

        [HttpPatch("{id}/read")]
        public async Task<IActionResult> UpdateReadStatus(Guid id, [FromQuery] bool readed)
        {
            var updated = await _service.UpdateReadStatusAsync(id, readed);
            if (!updated) return NotFound(new ApiResponse
            {
                Code = StatusCodes.Status404NotFound,
                Message = "Message not found."
            });
            return Ok(new ApiResponse
            {
                Code = StatusCodes.Status200OK,
                Message = "Message read status updated successfully."
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound(new ApiResponse
            {
                Code = StatusCodes.Status404NotFound,
                Message = "Message not found."
            });
            return Ok(new ApiResponse
            {
                Code = StatusCodes.Status200OK,
                Message = "Message deleted successfully."
            });
        }
    }
}
