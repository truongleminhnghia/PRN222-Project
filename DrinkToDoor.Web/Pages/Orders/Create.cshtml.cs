using System.Security.Claims;
using System.Text.Json;
using AspNetCoreHero.ToastNotification.Abstractions;
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DrinkToDoor.Data.enums;

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
        private readonly IIngredientProductService _ingredientProductService;

        public Create(ILogger<Create> logger,
                      ICartItemService cartItemService,
                      IUserService userService,
                      IOrderService orderService,
                      INotyfService toastNotification,
                      IPaymentService paymentService,
                      IIngredientProductService ingredientProductService)
        {
            _logger = logger;
            _cartItemService = cartItemService;
            _userService = userService;
            _orderService = orderService;
            _toastNotification = toastNotification;
            _paymentService = paymentService;
            _ingredientProductService = ingredientProductService;
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

        public IngredientProductResponse IngredientProduct { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = GetUserIdOrRedirect();
            if (userId == Guid.Empty) return RedirectToPage("/Login");

            if (TempData.TryGetValue("SelectedIds", out var rawJson) && rawJson is string json)
            {
                TempData.Keep("SelectedIds");
                SelectedIds = JsonSerializer.Deserialize<List<Guid>>(json) ?? new List<Guid>();
                var cartItemsResult = await _cartItemService.GetByIds(SelectedIds);
                SelectedCartItems = cartItemsResult ?? new List<CartItemResponse>();
                TotalPrice = SelectedCartItems.Sum(item => (double)(item.IngredientProduct?.TotalAmount ?? 0));
            }
            else if (TempData.TryGetValue("IngredientProductId", out var rawSingle) &&
                     Guid.TryParse(rawSingle.ToString(), out var ingredientProductId))
            {
                TempData.Keep("IngredientProductId");
                var product = await _ingredientProductService.GetById(ingredientProductId);
                if (product == null)
                {
                    _toastNotification.Error("Sản phẩm không tồn tại", 5);
                    return RedirectToPage("/Carts/Index");
                }
                IngredientProduct = product;
                TotalPrice = (double)(product.Price * product.QuantityPackage);
            }
            else
            {
                return RedirectToPage("/Carts/Index");
            }

            UserResponse = await _userService.GetByIdAsync(userId);
            if (UserResponse == null) return RedirectToPage("/Login");

            return Page();
        }

        public async Task<IActionResult> OnPostBuyOrder()
        {
            var userId = GetUserIdOrRedirect();
            if (userId == Guid.Empty)
                return RedirectToPage("/Login");

            var orderDetails = new List<OrderDetailRequest>();

            if (TempData.TryGetValue("SelectedIds", out var rawJson) && rawJson is string jsonSelected)
            {
                SelectedIds = JsonSerializer.Deserialize<List<Guid>>(jsonSelected) ?? new List<Guid>();
                var cartItems = await _cartItemService.GetByIds(SelectedIds);
                SelectedCartItems = cartItems ?? new List<CartItemResponse>();

                if (!SelectedCartItems.Any())
                {
                    _toastNotification.Error("Không có sản phẩm nào để đặt hàng", 5);
                    return RedirectToPage("/Carts/Index");
                }

                orderDetails = SelectedCartItems.Select(item => new OrderDetailRequest
                {
                    Quantity = item.Quantity,
                    Price = item.IngredientProduct?.Price ?? 0,
                    IngredientProductId = item.IngredientProduct?.Id ?? Guid.Empty
                }).ToList();
            }
            else if (TempData.TryGetValue("IngredientProductId", out var rawSingle) &&
                     Guid.TryParse(rawSingle.ToString(), out var ingredientProductId))
            {
                var product = await _ingredientProductService.GetById(ingredientProductId);
                if (product == null)
                {
                    _toastNotification.Error("Sản phẩm không tồn tại", 5);
                    return RedirectToPage("/Carts/Index");
                }
                IngredientProduct = product;
                orderDetails.Add(new OrderDetailRequest
                {
                    Quantity = product.QuantityPackage,
                    Price = product.Price,
                    IngredientProductId = product.Id
                });
            }
            else
            {
                _toastNotification.Error("Không thể xác định sản phẩm cần đặt hàng", 5);
                return RedirectToPage("/Carts/Index");
            }

            // UserResponse = await _userService.GetByIdAsync(userId);
            // if (UserResponse == null)
            // {
            //     _toastNotification.Error("Không tìm thấy người dùng", 5);
            //     return RedirectToPage("/Login");
            // }

            var order = new OrderRequest
            {
                ShippingAddress = UserResponse.Address,
                EmailShipping = UserResponse.Email,
                PhoneShipping = UserResponse.Phone,
                FullNameShipping = $"{UserResponse.LastName} {UserResponse.FirstName}",
                UserId = userId,
                OrderDetailsRequest = orderDetails
            };

            var orderId = await _orderService.CreateOrder(order);
            if (orderId != Guid.Empty)
            {
                _toastNotification.Success("Tạo đơn hàng thành công", 5);
                var payment = new PaymentRequest
                {
                    PaymentMethod = EnumPaymentMethod.PAY_OS,
                    OrderId = orderId,
                    UserId = userId,
                };
                var checkoutUrl = await _paymentService.CreatePaymentAsync(payment);
                return Redirect(checkoutUrl);
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
