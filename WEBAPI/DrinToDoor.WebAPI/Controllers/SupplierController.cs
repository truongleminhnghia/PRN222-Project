using DrinkToDoor.BLL.Interfaces;
using DrinkToDoor.BLL.ViewModel.ApiResponse;
using DrinkToDoor.BLL.ViewModel.Requests;
using Microsoft.AspNetCore.Mvc;

namespace DrinToDoor.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/suppliers")]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateSupplier([FromBody] SupplierRequest request)
        {
            var result = await _supplierService.CreateSupplier(request);
            if (result)
            {
                return Ok(new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Message = "",
                    Success = true,
                    Data = null
                });
            }
            return BadRequest(new ApiResponse
            {
                Code = StatusCodes.Status400BadRequest,
                Message = "Create supplier failed",
                Success = false,
                Data = null
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _supplierService.GetById(id);
            if (result == null)
            {
                return BadRequest(new ApiResponse
                {
                    Code = StatusCodes.Status400BadRequest,
                    Message = "Load data failed",
                    Success = false,
                    Data = null
                });
            }
            return Ok(new ApiResponse
            {
                Code = StatusCodes.Status200OK,
                Message = "",
                Success = true,
                Data = result
            });
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll([FromQuery] string? name, [FromQuery] string? contactPerson, [FromQuery] int pageCurrent = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _supplierService.GetSuppliers(name, contactPerson, pageCurrent, pageSize);
            if (result == null)
            {
                return BadRequest(new ApiResponse
                {
                    Code = StatusCodes.Status400BadRequest,
                    Message = "Load data failed",
                    Success = false,
                    Data = null
                });
            }
            return Ok(new ApiResponse
            {
                Code = StatusCodes.Status200OK,
                Message = "",
                Success = true,
                Data = result
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] SupplierRequest request)
        {
            var result = await _supplierService.UpdateSupplier(id, request);
            if (result)
            {
                return Ok(new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Message = "",
                    Success = true,
                    Data = null
                });
            }
            return BadRequest(new ApiResponse
            {
                Code = StatusCodes.Status400BadRequest,
                Message = "Update supplier failed",
                Success = false,
                Data = null
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _supplierService.DeleteSuplier(id);
            if (result)
            {
                return Ok(new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Message = "Delete successfully",
                    Success = true,
                    Data = null
                });
            }
            return BadRequest(new ApiResponse
            {
                Code = StatusCodes.Status400BadRequest,
                Message = "Delete Failed",
                Success = false,
                Data = null
            });
        }
    }
}