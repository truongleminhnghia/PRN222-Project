using System.ComponentModel.DataAnnotations;

namespace DrinkToDoor.Data.enums
{
    public enum EnumStatus
    {
        [Display(Name = "Hoạt động")]
        ACTIVE = 1,

        [Display(Name = "Tạm dừng")]
        INACTIVE = 2,
    }
}