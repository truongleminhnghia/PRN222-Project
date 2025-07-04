using DrinkToDoor.BLL.Interfaces;
using DrinkToDoor.BLL.ViewModel.ApiResponse;
using DrinkToDoor.BLL.ViewModel.Requests;
using Microsoft.AspNetCore.Mvc;

namespace DrinToDoor.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/ingredients")]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;

        public IngredientController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateIngredient([FromBody] IngredientRequest request)
        {
            var result = await _ingredientService.CreateIngredient(request);
            if (result)
            {
                return Ok(new ApiResponse
                {
                    Code = StatusCodes.Status201Created,
                    Message = "Create Ingredient successfully",
                    Success = true,
                    Data = null
                });
            }
            return BadRequest(new ApiResponse
            {
                Code = StatusCodes.Status400BadRequest,
                Message = "Create Ingredient failed",
                Success = false,
                Data = null
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _ingredientService.GetById(id);
            if (result != null)
            {
                return Ok(new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Message = "Get data successfully",
                    Success = true,
                    Data = result
                });
            }
            return BadRequest(new ApiResponse
            {
                Code = StatusCodes.Status400BadRequest,
                Message = "Get data failed",
                Success = false,
                Data = null
            });
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll([FromQuery] int pageCurrent, [FromQuery] int pageSize)
        {
            var result = await _ingredientService.GetIngredients(pageCurrent, pageSize);
            if (result != null)
            {
                return Ok(new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Message = "Get data successfully",
                    Success = true,
                    Data = result
                });
            }
            return BadRequest(new ApiResponse
            {
                Code = StatusCodes.Status400BadRequest,
                Message = "Get data failed",
                Success = false,
                Data = null
            });
        }
    }
}