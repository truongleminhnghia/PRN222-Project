
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Data.enums;

namespace DrinkToDoor.Business.Interfaces
{
    public interface IUserService
    {
        Task<bool> CreateAsync(UserRequest request);
        Task<UserResponse?> GetByIdAsync(Guid id);
        Task<Tuple<IEnumerable<UserResponse>, int>> GetAsync(string? keyword, EnumRoleName? roleName,
                                            EnumAccountStatus? status, EnumGender? gender, int pageCurrent, int pageSize);
        Task<bool> UpdateAsync(Guid id, UserResponse response);
        Task<bool> DeleteAsync(Guid id);
    }
}