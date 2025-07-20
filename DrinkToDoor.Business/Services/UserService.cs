
using AutoMapper;
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.HashPassword;
using DrinkToDoor.Business.Interfaces;
using DrinkToDoor.Data;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.enums;
using Microsoft.Extensions.Logging;

namespace DrinkToDoor.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<UserService> _logger;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher passwordHasher, ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<bool> CreateAsync(UserRequest request)
        {
            try
            {
                var userExisting = await _unitOfWork.Users.FindByEmail(request.Email);
                if (userExisting != null)
                {
                    throw new ArgumentException("Người dùng tồn tại với {email}", request.Email);
                }
                var user = _mapper.Map<User>(request);
                if (!string.IsNullOrEmpty(request.Password))
                {
                    user.Password = _passwordHasher.HashPassword(request.Password);
                }
                user.EnumAccountStatus = EnumAccountStatus.ACTIVE;
                await _unitOfWork.Users.Add(user);
                await _unitOfWork.SaveChangesWithTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while logging in user {email}", request.Email);
                throw new Exception("Server Error");
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    _logger.LogError("id trống");
                    throw new ArgumentException("Id không được bỏ trống");
                }
                var userExisting = await _unitOfWork.Users.FindById(id);
                if (userExisting == null)
                {
                    _logger.LogError("Người dùng không tồn tại với {id}", id);
                    throw new ArgumentException("Người dùng không tồn tại");
                }
                // bool result = await _unitOfWork.Users.Delete(userExisting);
                userExisting.EnumAccountStatus = EnumAccountStatus.INACTIVE;
                // if (!result)
                // {
                //     _logger.LogError("Không thể remove {id}", id);
                //     return false;
                // }
                _logger.LogInformation("DeleteAsync: user deleted successfully (id={UserId})", id);
                await _unitOfWork.Users.Update(userExisting);
                await _unitOfWork.SaveChangesWithTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while logging in user {id}", id);
                throw new Exception("Server Error");
            }
        }

        public async Task<Tuple<IEnumerable<UserResponse>, int>> GetAsync(string? keyword, EnumRoleName? roleName, EnumAccountStatus? status,
                                                                    EnumGender? gender, int pageCurrent, int pageSize)
        {
            try
            {
                var result = await _unitOfWork.Users.FindToList(keyword, roleName, status, gender);
                if (result == null)
                {
                    throw new ArgumentException("Danh sách rỗng");
                }
                var total = result.Count();
                var paged = result.Skip((pageCurrent - 1) * pageSize).Take(pageSize).ToList();
                var pagedResponses = _mapper.Map<List<UserResponse>>(paged);
                return Tuple.Create<IEnumerable<UserResponse>, int>(pagedResponses, total);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {ex}", ex);
                throw new Exception("Server Error");
            }
        }

        public async Task<UserResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    throw new ArgumentException("Id không được bỏ trống");
                }
                var userExisting = await _unitOfWork.Users.FindById(id);
                if (userExisting == null)
                {
                    throw new ArgumentException("Người dùng không tồn tại");
                }
                return _mapper.Map<UserResponse>(userExisting);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while logging in user {id}", id);
                throw new Exception("Server Error");
            }
        }

        public Task<bool> UpdateAsync(Guid id, UserResponse response)
        {
            throw new NotImplementedException();
        }
    }
}