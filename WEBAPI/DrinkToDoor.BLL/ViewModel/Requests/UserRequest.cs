using DrinkToDoor.Data.enums;
using System.ComponentModel.DataAnnotations;

namespace DrinkToDoor.BLL.ViewModel.Requests
{
    public class UserRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Avatar { get; set; }
        [Required]
        [EnumDataType(typeof(EnumRoleName))]
        public EnumRoleName RoleName { get; set; }
        public EnumGender? Gender { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }
    }
}
