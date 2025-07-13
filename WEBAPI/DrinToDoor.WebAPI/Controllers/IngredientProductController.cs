using DrinkToDoor.BLL.Interfaces;
using DrinkToDoor.BLL.ViewModel.ApiResponse;
using DrinkToDoor.BLL.ViewModel.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrinToDoor.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/ingredient-products")]
    public class IngredientProductController : ControllerBase
    {
        private readonly IIngredientProductService _iingredientProductService;

        public IngredientProductController(IIngredientProductService iingredientProductService)
        {
            _iingredientProductService = iingredientProductService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] IngredientProductRequest request)
        {
            var ok = await _iingredientProductService.Create(request);
            if (!ok)
                return BadRequest(
                    new ApiResponse
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Message = "Create failed",
                        Success = false,
                    }
                );

            return Ok(
                new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Message = "Created",
                    Success = true,
                }
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _iingredientProductService.GetById(id);
            if (item == null)
                return BadRequest(
                    new ApiResponse
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Message = "Not found",
                        Success = false,
                    }
                );

            return Ok(
                new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Data = item,
                }
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? name,
            [FromQuery] Guid? ingredientId,
            [FromQuery] int pageCurrent = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var result = await _iingredientProductService.GetIngredientProducts(
                name,
                ingredientId,
                pageCurrent,
                pageSize
            );
            if (result == null)
                return BadRequest(
                    new ApiResponse
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Message = "Get data problem",
                        Success = false,
                    }
                );

            return Ok(
                new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Data = result,
                }
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] IngredientProductRequest request
        )
        {
            var ok = await _iingredientProductService.Update(id, request);
            if (!ok)
                return BadRequest(
                    new ApiResponse
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Message = "Update failed",
                        Success = false,
                    }
                );

            return Ok(
                new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Message = "Updated",
                    Success = true,
                }
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await _iingredientProductService.Delete(id);
            if (!ok)
                return BadRequest(
                    new ApiResponse
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Message = "Delete ingredient product failed",
                        Success = false,
                    }
                );

            return Ok(
                new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Message = "Deleted ingredient product successfully",
                    Success = true,
                }
            );
        }
    }
}
