
using DrinkToDoor.Business.Dtos.Responses;
using Microsoft.AspNetCore.Identity.Data;

namespace DrinkToDoor.Business.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponse?> Login(string email, string password);
        Task<bool> Register(RegisterRequest request);
    }
}