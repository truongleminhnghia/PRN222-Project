using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.BLL.ViewModel.Responses;

namespace DrinkToDoor.BLL.Interfaces
{
    public interface IAuthenticationService
    {
        Task<UserResponse> Login(string email, string password);
        Task<bool> Register(RegisterRequest request);
    }
}
