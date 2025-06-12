
using System.Threading.Tasks;
using DrinkToDoor.Business.Dtos.ParamSearchs;
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DrinkToDoor.Web.Pages.Admins.Users
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        public IEnumerable<UserResponse> UserResponses { get; set; }
        public int TotalPages { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;
        [BindProperty(SupportsGet = true)]
        public int PageCurrent { get; set; } = 1;

        [BindProperty]
        public UserRequest UserRequest { get; set; }

        [BindProperty]
        public bool ShowCreateModal { get; set; }

        [BindProperty(SupportsGet = true)]
        public UserParams? UserParams { get; set; }

        public async Task OnGet()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var result = await _userService.GetAsync(
                lastName: UserParams?.LastName,
                firstName: UserParams?.FirstName,
                roleName: UserParams?.RoleName,
                status: UserParams?.Status,
                gender: UserParams?.Gender,
                pageCurrent: PageCurrent,
                pageSize: PageSize);
            UserResponses = result.Item1;
            TotalPages = (int)Math.Ceiling(result.Item2 / (double)PageSize);
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                ShowCreateModal = true;
                await LoadDataAsync();
                return Page();
            }
            await _userService.CreateAsync(UserRequest);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            await _userService.DeleteAsync(id);
            // await _hubContext.Clients.All.SendAsync("GameDeleted", id);
            return RedirectToPage();
        }
    }
}