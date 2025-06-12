
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.enums;

namespace DrinkToDoor.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<int> Add(User user);
        Task<User?> FindById(Guid id);
        Task<IEnumerable<User>> FindToList(string? lastName, string? firstName, EnumRoleName? roleName,
                                            EnumAccountStatus? status, EnumGender? gender);
        Task<int> Update(User user);
        Task<bool> Delete(User user);
        Task<User?> FindByEmail(string email);
    }
}