using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrinkToDoor.BLL.ViewModel.Requests
{
    public class RegisterRequest
    {
        public string? LastName { get; set; }
        public string? FirstName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email không được để trống")]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
