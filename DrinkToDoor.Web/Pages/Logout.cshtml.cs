using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DrinkToDoor.Web.Pages
{
    public class LogoutModel : PageModel
    {

        private readonly INotyfService _toastNotification;

        public LogoutModel(INotyfService notyfService)
        {
            _toastNotification = notyfService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // xóa cookie authentication
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // hiển thị thông báo đăng xuất thành công
            _toastNotification.Success("Đăng xuất thành công", 5);
            // quay về trang login (hoặc homepage)
            return RedirectToPage("/Index");
        }
    }
}