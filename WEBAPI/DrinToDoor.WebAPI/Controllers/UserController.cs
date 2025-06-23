using DrinkToDoor.BLL.Interfaces;
using DrinkToDoor.BLL.ViewModel.ApiResponse;
using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.Data.enums;
using Microsoft.AspNetCore.Mvc;

namespace DrinToDoor.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateUser([FromBody] UserRequest request)
        {
            var user = await _userService.CreateAccount(request);
            if (user)
            {
                return Ok(new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "",
                    Data = null
                });
            }
            return BadRequest(new ApiResponse
            {
                Code = StatusCodes.Status400BadRequest,
                Success = false,
                Message = "Create user failed",
                Data = null
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _userService.GetById(id);
            if (result != null)
            {
                return Ok(new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "",
                    Data = result
                });
            }
            return BadRequest(new ApiResponse
            {
                Code = StatusCodes.Status400BadRequest,
                Success = false,
                Message = "User not found",
                Data = null
            });
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll([FromQuery] string? lastName, [FromQuery] string? firstName, [FromQuery] EnumRoleName? roleName,
                                                [FromQuery] EnumAccountStatus? status, [FromQuery] EnumGender? gender,
                                                [FromQuery] int pageCurrent = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _userService.GetUsers(lastName, firstName, roleName, status, gender, pageCurrent, pageSize);
            if (result != null)
            {
                return Ok(new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "",
                    Data = result
                });
            }
            return BadRequest(new ApiResponse
            {
                Code = StatusCodes.Status400BadRequest,
                Success = false,
                Message = "Load data failed",
                Data = null
            });
        }

        [HttpPut("change-status/{id}")]
        public async Task<IActionResult> ChangeStatus(Guid id, EnumAccountStatus status)
        {
            var result = await _userService.ChangeStatus(id, status);
            if (result)
            {
                return Ok(new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "",
                    Data = null,
                });
            }
            return BadRequest(new ApiResponse
            {
                Code = StatusCodes.Status400BadRequest,
                Success = false,
                Message = "Change status user failed",
                Data = null
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, UserRequest request)
        {
            var result = await _userService.UpdateUser(id, request);
            if (result)
            {
                return Ok(new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "",
                    Data = null,
                });
            }
            return BadRequest(new ApiResponse
            {
                Code = StatusCodes.Status400BadRequest,
                Success = false,
                Message = "update failed",
                Data = null
            });
        }
    }
}