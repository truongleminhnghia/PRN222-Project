using System.Security.Claims;
using System.Text.Json;
using AspNetCoreHero.ToastNotification.Abstractions;
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using DrinkToDoor.Data.enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DrinkToDoor.Web.Pages.Orders
{
    public class Create : PageModel
    {
        private readonly ILogger<Create> _logger;
        private readonly ICartItemService _cartItemService;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly INotyfService _toastNotification;
        private readonly IPaymentService _paymentService;

        public Create(ILogger<Create> logger, ICartItemService cartItemService, IUserService userService,
                      IOrderService orderService, INotyfService toastNotification, IPaymentService paymentService)
        {
            _logger = logger;
            _cartItemService = cartItemService;
            _userService = userService;
            _orderService = orderService;
            _toastNotification = toastNotification;
            _paymentService = paymentService;
        }

        [BindProperty]
        public IEnumerable<CartItemResponse> SelectedCartItems { get; set; } = new List<CartItemResponse>();

        [BindProperty]
        public UserResponse UserResponse { get; set; } = new UserResponse();

        [BindProperty]
        public double TotalPrice { get; set; }

        [BindProperty]
        public EnumPaymentMethod PaymentMethod { get; set; }

        public List<Guid> SelectedIds { get; set; } = new();

        public List<OrderDetailRequest> OrderDetailRequests { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (!TempData.TryGetValue("SelectedIds", out var rawJson) || rawJson is not string json)
            {
                return RedirectToPage("/Carts/Index");
            }
            TempData.Keep("SelectedIds");

            SelectedIds = JsonSerializer.Deserialize<List<Guid>>(json) ?? new List<Guid>();

            var userId = GetUserIdOrRedirect();
            if (userId == Guid.Empty) return RedirectToPage("/Login");

            var cartItemsResult = await _cartItemService.GetByIds(SelectedIds);
            SelectedCartItems = cartItemsResult ?? new List<CartItemResponse>();
            TotalPrice = SelectedCartItems.Sum(item => (double)(item.IngredientProduct?.TotalAmount ?? 0));

            UserResponse = await _userService.GetByIdAsync(userId);
            if (UserResponse == null) return RedirectToPage("/Login");

            return Page();
        }

        public async Task<IActionResult> OnPostBuyOrder()
        {
            // Bạn cần logic để lấy lại SelectedIds từ TempData hoặc form (nếu bạn dùng hidden field để giữ lại dữ liệu)
            if (!TempData.TryGetValue("SelectedIds", out var rawJson) || rawJson is not string json)
            {
                _toastNotification.Error("Không thể xác định sản phẩm cần đặt hàng", 5);
                return RedirectToPage("/Carts/Index");
            }

            SelectedIds = JsonSerializer.Deserialize<List<Guid>>(json) ?? new List<Guid>();
            var cartItems = await _cartItemService.GetByIds(SelectedIds);
            SelectedCartItems = cartItems ?? new List<CartItemResponse>();

            if (!SelectedCartItems.Any())
            {
                _toastNotification.Error("Không có sản phẩm nào để đặt hàng", 5);
                return RedirectToPage("/Carts/Index");
            }

            OrderDetailRequests = new List<OrderDetailRequest>();
            foreach (var item in SelectedCartItems)
            {
                var orderDetail = new OrderDetailRequest
                {
                    Quantity = item.Quantity,
                    Price = item.IngredientProduct?.Price ?? 0,
                    IngredientProductId = item.IngredientProduct?.Id ?? Guid.Empty
                };
                OrderDetailRequests.Add(orderDetail);
            }

            var userId = GetUserIdOrRedirect();
            if (userId == Guid.Empty) return RedirectToPage("/Login");

            var order = new OrderRequest
            {
                ShippingAddress = UserResponse.Address,
                EmailShipping = UserResponse.Email,
                PhoneShipping = UserResponse.Phone,
                FullNameShipping = $"{UserResponse.LastName} {UserResponse.FirstName}",
                UserId = userId,
                OrderDetailsRequest = OrderDetailRequests
            };

            var result = await _orderService.CreateOrder(order);

            if (result != Guid.Empty)
            {
                _toastNotification.Success("Tạo đơn hàng thành công", 5);
                var payment = new PaymentRequest
                {
                    PaymentMethod = EnumPaymentMethod.PAY_OS,
                    OrderId = result,
                    UserId = userId,
                };
                var urlCheckout = await _paymentService.CreatePaymentAsync(payment);
                return Redirect(urlCheckout);
            }
            else
            {
                _toastNotification.Error("Tạo đơn hàng thất bại", 5);
                return Page();
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
