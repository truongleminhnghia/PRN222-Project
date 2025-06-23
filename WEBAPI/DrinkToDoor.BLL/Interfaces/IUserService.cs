using DrinkToDoor.BLL.ViewModel.Pages;
using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.BLL.ViewModel.Responses;
using DrinkToDoor.Data.enums;

namespace DrinkToDoor.BLL.Interfaces
{
    public interface IUserService
    {
        Task<bool> CreateAccount(UserRequest request);
        Task<UserResponse> GetById(Guid id);
        Task<PageResult<UserResponse>> GetUsers(string? lastName, string? firstName, EnumRoleName? roleName,
                                                EnumAccountStatus? status, EnumGender? gender,
                                                int pageCurrent, int pageSize);
        Task<bool> DeleteById(Guid id);
        Task<bool> ChangeStatus(Guid id, EnumAccountStatus status);
        Task<bool> UpdateUser(Guid id, UserRequest request);
    }
}
