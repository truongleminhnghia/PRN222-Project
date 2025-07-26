
using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DrinkToDoor.Web.Pages.Users
{
    public class Profile : PageModel
    {
        private readonly ILogger<Profile> _logger;
        private readonly IUserService _userService;
        private readonly INotyfService _toast;


        public Profile(ILogger<Profile> logger, IUserService userService, INotyfService toast)
        {
            _logger = logger;
            _userService = userService;
            _toast = toast;
        }

        [BindProperty]
        public UserResponse UserResponse { get; set; }

        public async Task OnGet()
        {
            var userId = GetUserIdOrRedirect();
            await EnsureCartExists(userId);
            await LoadDataAsync(userId);
        }

        private async Task LoadDataAsync(Guid? userId)
        {
            var result = await _userService.GetByIdAsync(userId.Value);
            UserResponse = result;
        }

        private async Task EnsureCartExists(Guid userId)
        {
            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
            {
                _toast.Warning("User không tồn tại hoặc chưa đăng nhập.");
                RedirectToPage("/Login");
            }
        }

        private Guid GetUserIdOrRedirect()
        {
            var str = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(str, out var userId))
            {
                throw new InvalidOperationException("User ID không hợp lệ");
            }
            return userId;
        }
    }
}