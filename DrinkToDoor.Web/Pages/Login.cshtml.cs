

using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using DrinkToDoor.Business.Interfaces;
using DrinkToDoor.Data.enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DrinkToDoor.Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _service;

        public LoginModel(IAuthService service)
        {
            _service = service;
        }

        [TempData]
        public string ToastMessage { get; set; }
        [TempData]
        public string ToastType { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Bạn phải nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Bạn phải nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [TempData]
        public string? ErrorMessage { get; set; }


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            var user = await _service.Login(Email, Password);
            if (user == null)
            {
                ErrorMessage = "Email hoặc mật khẩu không đúng.";
                return RedirectToPage();
            }
            //creating the scurity context
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.RoleName.ToString()),
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal,
            new AuthenticationProperties { IsPersistent = false }
            );
            ToastMessage = "Đăng nhập thành công!";
            ToastType = "success";
            if (user.RoleName == EnumRoleName.ROLE_ADMIN)
            {
                return RedirectToPage("/Admins/Dashboard");
            }
            return RedirectToPage("/Index");
        }
    }
}