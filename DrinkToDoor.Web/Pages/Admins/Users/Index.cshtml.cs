
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
        private readonly ILogger<IndexModel> _logger;
        public IndexModel(IUserService userService, ILogger<IndexModel> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public IEnumerable<UserResponse> UserResponses { get; set; }
        = Enumerable.Empty<UserResponse>();
        public int TotalPages { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;
        [BindProperty(SupportsGet = true)]
        public int PageCurrent { get; set; } = 1;

        [BindProperty]
        public UserRequest UserRequest { get; set; } = new UserRequest();

        [BindProperty]
        public bool ShowCreateModal { get; set; }

        [BindProperty(SupportsGet = true)]
        public UserParams? UserParams { get; set; }

        public async Task OnGet()
        {
            await LoadDataAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ShowCreateModal = true;
                await LoadDataAsync();
                return Page();
            }

            UserRequest.Password = "123456";

            try
            {
                await _userService.CreateAsync(UserRequest);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                // 1) Log lỗi chi tiết
                _logger.LogError(ex, "Lỗi khi tạo người dùng: {@UserRequest}", UserRequest);

                // 2) Thêm model error để hiển thị trong ValidationSummary
                ModelState.AddModelError(string.Empty, "Đã có lỗi xảy ra khi tạo người dùng: " + ex.Message);

                // 3) Bật lại modal và load dữ liệu cần thiết
                ShowCreateModal = true;
                await LoadDataAsync();
                return Page();
            }
        }

        private async Task LoadDataAsync()
        {
            var (items, total) = await _userService.GetAsync(
                keyword: UserParams?.Keyword,
               roleName: UserParams?.RoleName,
               status: UserParams?.Status,
               gender: UserParams?.Gender,
               pageCurrent: PageCurrent,
               pageSize: PageSize);
            UserResponses = items ?? Enumerable.Empty<UserResponse>();
            TotalPages = (int)Math.Ceiling(total / (double)PageSize);
        }


        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            await _userService.DeleteAsync(id);
            // await _hubContext.Clients.All.SendAsync("GameDeleted", id);
            return RedirectToPage();
        }
    }
}