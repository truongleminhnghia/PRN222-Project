
using DrinkToDoor.Data.enums;

namespace DrinkToDoor.Business.Dtos.Responses
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Avatar { get; set; }
        public EnumRoleName RoleName { get; set; }
        public EnumAccountStatus EnumAccountStatus { get; set; }
        public EnumGender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}