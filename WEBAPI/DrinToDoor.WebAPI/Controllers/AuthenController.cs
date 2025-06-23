using DrinkToDoor.BLL.Interfaces;
using DrinkToDoor.BLL.ViewModel.ApiResponse;
using DrinkToDoor.BLL.ViewModel.Requests;
using Microsoft.AspNetCore.Mvc;

namespace DrinToDoor.WebAPI.Controllers
{
    [Route("api/v1/auths")]
    [ApiController]
    public class AuthenController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authenticationService.Login(request.Email, request.Password);
            return Ok(new ApiResponse
            {
                Code = StatusCodes.Status200OK,
                Success = true,
                Message = "Login successful",
                Data = result
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _authenticationService.Register(request);
            return Ok(new ApiResponse
            {
                Code = StatusCodes.Status200OK,
                Success = true,
                Message = "Register successful",
                Data = null
            });
        }
    }
}
