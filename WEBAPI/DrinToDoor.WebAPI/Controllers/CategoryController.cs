using DrinkToDoor.BLL.Interfaces;
using DrinkToDoor.BLL.ViewModel.ApiResponse;
using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.Data.enums;
using Microsoft.AspNetCore.Mvc;

namespace DrinToDoor.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryRequest request)
        {
            var category = await _categoryService.Create(request);
            if (!category)
            {
                return BadRequest(new ApiResponse
                {
                    Code = StatusCodes.Status400BadRequest,
                    Message = "Create category failed",
                    Success = false,
                    Data = null
                });
            }
            return Ok(new ApiResponse
            {
                Code = StatusCodes.Status200OK,
                Message = "Create category successfully",
                Success = true,
                Data = null
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var categoryExisting = await _categoryService.GetById(id);
            if (categoryExisting == null)
            {
                return BadRequest(new ApiResponse
                {
                    Code = StatusCodes.Status400BadRequest,
                    Message = "Get data problem",
                    Success = false,
                    Data = null
                });
            }
            return Ok(new ApiResponse
            {
                Code = StatusCodes.Status200OK,
                Success = true,
                Data = categoryExisting
            });
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll([FromQuery] string? name, [FromQuery] EnumCategoryType? type, [FromQuery] int pageCurrent = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _categoryService.GetCategories(name, type, pageCurrent, pageSize);
            if (result == null)
            {
                return BadRequest(new ApiResponse
                {
                    Code = StatusCodes.Status400BadRequest,
                    Message = "Get data problem",
                    Success = false,
                    Data = null
                });
            }
            return Ok(new ApiResponse
            {
                Code = StatusCodes.Status200OK,
                Success = true,
                Data = result
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, CategoryRequest request)
        {
            var result = await _categoryService.Update(id, request);
            if (!result)
            {
                return BadRequest(new ApiResponse
                {
                    Code = StatusCodes.Status400BadRequest,
                    Message = "Update category failed",
                    Success = false,
                    Data = null
                });
            }
            return Ok(new ApiResponse
            {
                Code = StatusCodes.Status200OK,
                Message = "Update category successfully",
                Success = true,
                Data = null
            });
        }
    }
}