using System.ComponentModel.DataAnnotations;

namespace DrinkToDoor.Data.enums
{
    public enum EnumRoleName
    {
        [Display(Name = "Quản trị viên")]
        ROLE_ADMIN,

        [Display(Name = "Nhân viên")]
        ROLE_STAFF,

        [Display(Name = "Người dùng")]
        ROLE_USER
    }
}