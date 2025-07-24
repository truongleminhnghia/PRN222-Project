
using AspNetCoreHero.ToastNotification.Abstractions;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DrinkToDoor.Web.Pages.Carts
{
    public class Index : PageModel
    {
        private readonly ILogger<Index> _logger;
        private readonly ICartService _cartService;
        private readonly IUserService _userService;
        private readonly INotyfService _toastNotification;

        public Index(ILogger<Index> logger, ICartService cartService, IUserService userService,
                    INotyfService toastNotification)
        {
            _logger = logger;
            _cartService = cartService;
            _userService = userService;
            _toastNotification = toastNotification;
        }

        public CartResponse CartResponse { get; set; } = new CartResponse();

        public IEnumerable<CartItemResponse> CartItemResponses { get; set; }

        [BindProperty]
        public List<Guid> SelectedIds { get; set; } = new();

        [BindProperty]
        public string Action { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToPage("/Login");
            }
            if (!Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return BadRequest("Invalid user ID.");
            }
            return await LoadData(userId);
        }

        private async Task<IActionResult> LoadData(Guid userId)
        {
            var userExisting = await _userService.GetByIdAsync(userId);
            if (userExisting == null)
            {
                _toastNotification.Warning("User không tồn tại hoặc chưa đăng nhập");
                return RedirectToPage("/Login");
            }
            var cartExisting = await _cartService.GetByUser(userExisting.Id);
            if (cartExisting == null)
            {
                await _cartService.CreateCart(userExisting.Id);
                cartExisting = await _cartService.GetByUser(userExisting.Id);
            }
            CartItemResponses = cartExisting.CartItems;
            return Page();
        }

        public async Task<IActionResult> OnPostProcessSelectionAsync()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToPage("/Login");
            }
            if (!Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return BadRequest("Invalid user ID.");
            }

            if (SelectedIds == null || !SelectedIds.Any())
            {
                TempData["Error"] = "Bạn chưa chọn sản phẩm nào.";
                return RedirectToPage();
            }
            else if (Action == "buy")
            {
                // VD: tạo order từ các cartItemId
                // var orderId = await _cartService.CreateOrderFromCartAsync(userId, SelectedIds);
                // return RedirectToPage("/Orders/Detail", new { id = orderId });
                Console.WriteLine("cart item id: ", SelectedIds);
                return null;
            }

            return RedirectToPage();
        }
    }
}