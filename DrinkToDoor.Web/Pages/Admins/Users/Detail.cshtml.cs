
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DrinkToDoor.Web.Pages.Admins.Users
{
    public class DetailModel : PageModel
    {
        private readonly IUserService _userService;
        public DetailModel(IUserService userService)
        {
            _userService = userService;
        }

        public UserResponse UserResponse { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            UserResponse = await _userService.GetByIdAsync(id)!;
            if (UserResponse == null) return NotFound();
            return Page();
        }
    }
}