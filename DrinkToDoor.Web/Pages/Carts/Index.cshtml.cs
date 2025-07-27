
using System.Security.Claims;
using System.Text.Json;
using AspNetCoreHero.ToastNotification.Abstractions;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DrinkToDoor.Web.Pages.Carts
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ICartService _cartService;
        private readonly IUserService _userService;
        private readonly INotyfService _toast;
        private readonly ICartItemService _cartItemService;

        public IndexModel(
            ILogger<IndexModel> logger,
            ICartService cartService,
            IUserService userService,
            INotyfService toastNotification,
            ICartItemService cartItemService)
        {
            _logger = logger;
            _cartService = cartService;
            _userService = userService;
            _toast = toastNotification;
            _cartItemService = cartItemService;
        }

        public IEnumerable<CartItemResponse> CartItemResponses { get; set; }

        [BindProperty]
        public List<Guid> SelectedIds { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = GetUserIdOrRedirect();
            await EnsureCartExists(userId);
            return Page();
        }

        public async Task<IActionResult> OnPostProcessSelectionAsync()
        {
            var userId = GetUserIdOrRedirect();
            if (SelectedIds == null || !SelectedIds.Any())
            {
                _toast.Warning("Bạn chưa chọn sản phẩm nào.");
                return RedirectToPage();
            }

            // await _cartService.SaveFavoritesAsync(userId, SelectedIds);
            _toast.Success("Đã lưu vào mục Đã thích.");
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostBuyAsync()
        {
            var userId = GetUserIdOrRedirect();
            if (SelectedIds == null || !SelectedIds.Any())
            {
                _toast.Warning("Bạn chưa chọn sản phẩm nào để mua.", 5);
                return RedirectToPage();
            }
            TempData["SelectedIds"] = JsonSerializer.Serialize(SelectedIds);
            return RedirectToPage("/Orders/Create");
            // return RedirectToPage("/Carts/Index");
        }

        public async Task<IActionResult> OnPostRemoveAsync(Guid id)
        {
            var userId = GetUserIdOrRedirect();
            var result = await _cartItemService.DeleteItem(id);
            if (result)
            {
                _toast.Success("Đã xóa sản phẩm khỏi giỏ hàng.", 5);
            }
            else
            {
                _toast.Error("Xóa giỏ hàng thất bại.", 5);
            }
            await EnsureCartExists(userId);
            return RedirectToPage(); 
        }

        private async Task EnsureCartExists(Guid userId)
        {
            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
            {
                _toast.Warning("User không tồn tại hoặc chưa đăng nhập.");
                RedirectToPage("/Login");
            }

            var cart = await _cartService.GetByUser(userId);
            if (cart == null)
            {
                await _cartService.CreateCart(userId);
            }

            CartItemResponses = (await _cartService.GetByUser(userId)).CartItems;
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