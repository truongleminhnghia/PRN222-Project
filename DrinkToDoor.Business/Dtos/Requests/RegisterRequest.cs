

using System.ComponentModel.DataAnnotations;

namespace DrinkToDoor.Business.Dtos.Requests
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string? LastName { get; set; }
        public string? FirstName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}