
using AutoMapper;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.HashPassword;
using DrinkToDoor.Business.Interfaces;
using DrinkToDoor.Data;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.enums;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Logging;

namespace DrinkToDoor.Business.Services
{
    public class AuthenticationService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IPasswordHasher _passwordHasher;

        public AuthenticationService(IUnitOfWork unitOfWork, IMapper mapper,
                                    ILogger<AuthenticationService> logger, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserResponse?> Login(string email, string password)
        {
            try
            {
                User? user = await _unitOfWork.Users.FindByEmail(email);
                if (user == null)
                {
                    _logger.LogError("{Email} Không tồn tài", email);
                    throw new Exception("{Email} không tồn tại");
                }
                if (string.IsNullOrEmpty(user.Password))
                {
                    _logger.LogError("User password is null or empty for {Email}", email);
                    throw new Exception("Mật khẩu không hợp lệ");
                }
                bool checkPassword = _passwordHasher.VerifyPassword(password, user.Password);
                if (!checkPassword)
                {
                    _logger.LogError("Mật khẩu không khớp");
                    throw new Exception("Mật khẩu không khớp");
                }
                return _mapper.Map<UserResponse>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while logging in user {Email}", email);
                throw new Exception("Server Error");
            }
        }

        public async Task<bool> Register(RegisterRequest request)
        {
            try
            {
                User? userExisting = await _unitOfWork.Users.FindByEmail(request.Email);
                if (userExisting != null)
                {
                    _logger.LogError("{Email} đã tồn tại", request.Email);
                    throw new Exception("{Email} đã tồn tại");
                }
                var newUser = _mapper.Map<User>(request);
                newUser.EnumAccountStatus = EnumAccountStatus.ACTIVE;
                newUser.RoleName = EnumRoleName.ROLE_USER;
                newUser.Password = _passwordHasher.HashPassword(request.Password);
                await _unitOfWork.Users.Add(newUser);
                await _unitOfWork.SaveChangesWithTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while logging in user ");
                throw new Exception("Server Error");
            }
        }
    }
}