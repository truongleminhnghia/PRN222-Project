using DrinkToDoor.BLL.ViewModel.Pages;
using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.BLL.ViewModel.Responses;

namespace DrinkToDoor.BLL.Interfaces
{
    public interface IUserService
    {
        Task<bool> CreateAccount(UserRequest request);
        Task<UserResponse> GetById(Guid id);
        Task<PageResult<UserResponse>> GetUsers();
        Task<bool> DeleteById(Guid id);
    }
}
