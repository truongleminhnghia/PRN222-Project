using System.Text.Json;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using DrinkToDoor.Business.Dtos.ParamSearchs;
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using DrinkToDoor.Data.enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DrinkToDoor.Web.Pages.Ingredients
{
    public class Detail : PageModel
    {
        private readonly ILogger<Detail> _logger;
        private readonly IIngredientService _ingredientService;
        private readonly ICartService _cartService;
        private readonly INotyfService _toastNotification;
        private readonly IIngredientProductService _ingredientProductService;
        private readonly ICartItemService _cartItemService;

        public Detail(ILogger<Detail> logger, IIngredientService ingredientService, INotyfService notyfService,
                        ICartService cartService, IIngredientProductService ingredientProductService,
                        ICartItemService cartItemService)
        {
            _logger = logger;
            _ingredientService = ingredientService;
            _toastNotification = notyfService;
            _cartService = cartService;
            _ingredientProductService = ingredientProductService;
            _cartItemService = cartItemService;
        }

        public IEnumerable<PackagingOptionResponse> PackagingOptions { get; set; } = Enumerable.Empty<PackagingOptionResponse>();

        public IngredientResponse IngredientResponse { get; set; } = new IngredientResponse();

        [BindProperty]
        public CartRequest CartRequest { get; set; } = new CartRequest();

        public IngredientProductRequest IngredientProduct { get; set; } = new IngredientProductRequest();

        [BindProperty]
        public EnumPackageType SelectedPackage { get; set; }

        public IEnumerable<IngredientResponse> Ingredients { get; set; }

        [BindProperty(SupportsGet = true)]
        public IngredientParams? IngredientParams { get; set; }

        public int TotalPages { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;
        [BindProperty(SupportsGet = true)]
        public int PageCurrent { get; set; } = 1;

        private async Task LoadDataAsync()
        {
            var result = await _ingredientService.GetAsync(
                keyword: "",
                name: IngredientParams?.Name,
                categoryId: IngredientParams?.CategoryId,
                minPirce: IngredientParams?.MinPrice,
                maxPrice: IngredientParams?.MaxPrice,
                minCost: IngredientParams?.MinCost,
                maxCost: IngredientParams?.MaxCost,
                minQuantity: IngredientParams?.MinStockQty,
                maxQuantity: IngredientParams?.MaxStockQty,
                status: IngredientParams?.Status,
                pageCurrent: PageCurrent,
                pageSize: PageSize
            );
            Ingredients = result.Item1;
            TotalPages = (int)Math.Ceiling(result.Item2 / (double)PageSize);
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound("Ingredient ID is required.");
            }

            try
            {
                IngredientResponse = await _ingredientService.GetByIdAsync(id);
                if (IngredientResponse == null)
                {
                    return NotFound($"Ingredient with ID {id} not found.");
                }
                PackagingOptions = IngredientResponse.PackagingOptions;
                SelectedPackage = IngredientResponse.PackagingOptions.FirstOrDefault()?.Type
                      ?? EnumPackageType.BỊCH;
                await LoadDataAsync();
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ingredient details");
                ModelState.AddModelError(string.Empty, "An error occurred while retrieving the ingredient details.");
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAddToCart(Guid id)
        {
            try
            {
                if (!await LoadIngredientAsync(id))
                    return NotFound("Ingredient not found.");
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return RedirectToPage("/Login");
                }
                if (!Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    return BadRequest("Invalid user ID.");
                }
                var selectedPackaging = PackagingOptions.FirstOrDefault(p => p.Type == SelectedPackage);
                if (selectedPackaging == null)
                {
                    _toastNotification.Error("Loại đóng gói không hợp lệ.", 5);
                    return RedirectToPage("/Ingredients/Detail", new { id });
                }
                if (selectedPackaging.StockQty < CartRequest.Quantity)
                {
                    _toastNotification.Warning($"Chỉ còn {selectedPackaging.StockQty} {selectedPackaging.Type} trong kho.", 5);
                    return RedirectToPage("/Ingredients/Detail", new { id });
                }
                CartRequest.UnitPackage = SelectedPackage.ToString();
                CartRequest.UserId = userId;
                CartRequest.IngredientId = id;
                var result = await _cartService.AddToCart(CartRequest);
                if (result)
                {
                    _toastNotification.Success("Thêm giỏ hàng thành công", 5);
                    var cart = await _cartService.GetByUser(userId);
                    if (cart != null)
                    {
                        int count = await _cartItemService.Count(cart.Id);
                        ViewData["CartCount"] = count;
                    }
                    else
                    {
                        ViewData["CartCount"] = 0;
                    }
                }
                else
                {
                    _toastNotification.Error("Thêm giỏ hàng thất bại", 5);
                }
                await LoadDataAsync();
                return RedirectToPage("/Ingredients/Detail", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm vào giỏ hàng");
                _toastNotification.Error("Có lỗi xảy ra", 5);
                return RedirectToPage("/Ingredients/Detail", new { id });
            }
        }

        public async Task<IActionResult> OnPostBuyNow(Guid id)
        {
            try
            {
                if (!await LoadIngredientAsync(id))
                    return NotFound("Ingredient not found.");
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return RedirectToPage("/Login");
                if (!Guid.TryParse(userIdClaim.Value, out var userId))
                    return BadRequest("Invalid user ID.");
                var selectedPackaging = PackagingOptions.FirstOrDefault(p => p.Type == SelectedPackage);
                if (selectedPackaging == null)
                {
                    _toastNotification.Error("Loại đóng gói không hợp lệ.", 5);
                    return RedirectToPage("/Ingredients/Detail", new { id });
                }
                if (selectedPackaging.StockQty < CartRequest.Quantity)
                {
                    _toastNotification.Warning($"Chỉ còn {selectedPackaging.StockQty} {selectedPackaging.Type} trong kho.", 5);
                    return RedirectToPage("/Ingredients/Detail", new { id });
                }
                IngredientProduct = new IngredientProductRequest
                {
                    UnitPackage = SelectedPackage.ToString(),
                    IngredientId = id,
                    QuantityPackage = CartRequest.Quantity,
                    Name = IngredientResponse.Name
                };
                Guid ingredientProductId = await _ingredientProductService.Create(IngredientProduct);
                if (ingredientProductId == Guid.Empty)
                {
                    _toastNotification.Warning("Tạo đơn hàng thất bại");
                    return RedirectToPage("/Ingredients/Detail", new { id });
                }
                _toastNotification.Success("Tạo order thành công", 5);
                TempData["IngredientProductId"] = ingredientProductId.ToString();
                return RedirectToPage("/Orders/Create");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo đơn hàng giỏ hàng");
                _toastNotification.Error("Có lỗi xảy ra", 5);
                return RedirectToPage("/Ingredients/Detail", new { id });
            }
        }

        private async Task<bool> LoadIngredientAsync(Guid id)
        {
            if (id == Guid.Empty)
                return false;

            IngredientResponse = await _ingredientService.GetByIdAsync(id);
            PackagingOptions = IngredientResponse?.PackagingOptions ?? Enumerable.Empty<PackagingOptionResponse>();
            return IngredientResponse != null;
        }
    }
}
