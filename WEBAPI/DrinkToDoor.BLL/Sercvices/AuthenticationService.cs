using AutoMapper;
using DrinkToDoor.BLL.Exceptions;
using DrinkToDoor.BLL.HashPasswords;
using DrinkToDoor.BLL.Interfaces;
using DrinkToDoor.BLL.ViewModel.Requests;
using DrinkToDoor.BLL.ViewModel.Responses;
using DrinkToDoor.Data;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.enums;
using Microsoft.Extensions.Logging;

namespace DrinkToDoor.BLL.Sercvices
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        public AuthenticationService(ILogger<AuthenticationService> logger, IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher passwordHasher)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }
        public async Task<UserResponse> Login(string email, string password)
        {
            try
            {
                var account = await _unitOfWork.Users.FindByEmail(email);
                if (account == null) throw new AppException(ErrorCode.NOT_FOUND, "Người dùng ko tồn tại");
                bool checkPassword = _passwordHasher.VerifyPassword(password, account.Password);
                if (!checkPassword) throw new AppException(ErrorCode.INVALID, "Mật khẩu không đúng");
                return _mapper.Map<UserResponse>(account);
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

        public async Task<bool> Register(RegisterRequest request)
        {
            try
            {
                var userExisting = await _unitOfWork.Users.FindByEmail(request.Email);
                if (userExisting != null)
                {
                    _logger.LogWarning("Không thể tạo tài khoản, email {Email} đã tồn tại.", request.Email);
                    throw new AppException(ErrorCode.HAS_EXISTED, "Email đã tồn tại");
                }
                var user = _mapper.Map<User>(request);
                user.RoleName = EnumRoleName.ROLE_USER;
                user.EnumAccountStatus = EnumAccountStatus.ACTIVE;
                user.Password = _passwordHasher.HashPassword(request.Password);
                await _unitOfWork.Users.Add(user);
                await _unitOfWork.SaveChangesWithTransactionAsync();
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
