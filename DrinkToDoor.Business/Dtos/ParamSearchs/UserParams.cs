
using DrinkToDoor.Data.enums;

namespace DrinkToDoor.Business.Dtos.ParamSearchs
{
    public class UserParams
    {
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public EnumRoleName? RoleName { get; set; }
        public EnumAccountStatus? Status { get; set; }
        public EnumGender? Gender { get; set; }
    }
}