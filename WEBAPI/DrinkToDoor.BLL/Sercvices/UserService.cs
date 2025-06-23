
using DrinkToDoor.BLL.Interfaces;
using DrinkToDoor.BLL.ViewModel.Pages;
using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.BLL.ViewModel.Responses;
using DrinkToDoor.Data.Repositories.Interfaces;

namespace DrinkToDoor.BLL.Sercvices
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<bool> CreateAccount(UserRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<UserResponse> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PageResult<UserResponse>> GetUsers()
        {
            throw new NotImplementedException();
        }
    }
}
