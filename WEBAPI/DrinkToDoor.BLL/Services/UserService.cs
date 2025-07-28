
using AutoMapper;
using DrinkToDoor.BLL.Exceptions;
using DrinkToDoor.BLL.Interfaces;
using DrinkToDoor.BLL.ViewModel.Pages;
using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.BLL.ViewModel.Responses;
using DrinkToDoor.Data;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.enums;
using DrinkToDoor.Data.Interfaces;
using Microsoft.Extensions.Logging;

namespace DrinkToDoor.BLL.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;
        private IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateAccount(UserRequest request)
        {
            try
            {
                var userExisting = await _unitOfWork.Users.FindByEmail(request.Email);
                if (userExisting != null) throw new AppException(ErrorCode.NOT_FOUND, "User existed");
                var user = _mapper.Map<User>(request);
                if (user == null) throw new AppException(ErrorCode.NOT_NULL, "body request not null");
                user.EnumAccountStatus = EnumAccountStatus.ACTIVE;
                await _unitOfWork.Users.Add(user);
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
                if (result == 0) return false;
                return true;
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "AppException occurred: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR, "Internal server error");
            }
        }

        public Task<bool> DeleteById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<UserResponse> GetById(Guid id)
        {
            try
            {
                var userExisting = await _userRepository.FindById(id);
                if (userExisting == null) throw new AppException(ErrorCode.NOT_FOUND, "User not found");
                return _mapper.Map<UserResponse>(userExisting);
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "AppException occurred: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR, "Internal server error");
            }
        }

        public async Task<bool> ChangeStatus(Guid id, EnumAccountStatus status)
        {
            try
            {
                var userExisting = await _userRepository.FindById(id);
                if (userExisting == null) throw new AppException(ErrorCode.NOT_FOUND, "User not found");
                if (userExisting.EnumAccountStatus == status) throw new AppException(ErrorCode.HAS_INACTIVE, "Status user like");
                userExisting.EnumAccountStatus = status;
                await _unitOfWork.Users.Update(userExisting);
                return true;
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "AppException occurred: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR, "Internal server error");
            }
        }

        public async Task<PageResult<UserResponse>> GetUsers(string? keyword, EnumRoleName? roleName, EnumAccountStatus? status, EnumGender? gender, int pageCurrent, int pageSize)
        {
            try
            {
                var result = await _unitOfWork.Users.FindToList(keyword, roleName, status, gender);
                if (result == null) throw new AppException(ErrorCode.LIST_EMPTY);
                var pagedResult = result.Skip((pageCurrent - 1) * pageSize).Take(pageSize).ToList();
                var total = result.Count();
                var data = _mapper.Map<List<UserResponse>>(pagedResult);
                if (data == null || !data.Any()) throw new AppException(ErrorCode.LIST_EMPTY);
                var pageResult = new PageResult<UserResponse>(data, pageSize, pageCurrent, total);
                return pageResult;
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "AppException occurred: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<bool> UpdateUser(Guid id, UserRequest request)
        {
            try
            {
                var userExisting = await _userRepository.FindById(id);
                if (userExisting == null) throw new AppException(ErrorCode.NOT_FOUND, "User not found");
                if (request.LastName != null) userExisting.LastName = request.LastName;
                if (request.FirstName != null) userExisting.FirstName = request.FirstName;
                if (request.Password != null) userExisting.Password = request.Password;
                if (request.Phone != null) userExisting.Phone = request.Phone;
                if (request.Address != null) userExisting.Address = request.Address;
                if (request.Avatar != null) userExisting.Avatar = request.Avatar;
                if (request.RoleName != null) userExisting.RoleName = request.RoleName.Value;
                if (request.Gender != null) userExisting.Gender = request.Gender;
                if (request.DateOfBirth != null) userExisting.DateOfBirth = request.DateOfBirth.Value;
                await _unitOfWork.Users.Update(userExisting);
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();
                if (result == 0)
                {
                    return false;
                }
                return true;
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "AppException occurred: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }
    }
}
