
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
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

        public Detail(ILogger<Detail> logger, IIngredientService ingredientService, INotyfService notyfService,
                        ICartService cartService)
        {
            _logger = logger;
            _ingredientService = ingredientService;
            _toastNotification = notyfService;
            _cartService = cartService;
        }

        public IEnumerable<PackagingOptionResponse> PackagingOptions { get; set; }

        public IngredientResponse IngredientResponse { get; set; } = new IngredientResponse();

        [BindProperty]
        public CartRequest CartRequest { get; set; } = new CartRequest();

        [BindProperty]
        public EnumPackageType SelectedPackage { get; set; }

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
                CartRequest.UnitPackage = EnumPackageType.BỊCH.ToString();
                CartRequest.UserId = userId;
                CartRequest.IngredientId = id;
                if (CartRequest == null)
                {
                    _toastNotification.Error("Thêm giỏ hàng thất bại", 5);
                }
                var result = await _cartService.AddToCart(CartRequest);
                if (result)
                {
                    _toastNotification.Success("Thêm giỏ hàng thành công", 5);
                }
                else
                {
                    _toastNotification.Error("Thêm giỏ hàng thất bại", 5);
                }
                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: ", ex.Message);
                return null;
            }
        }

        public IActionResult OnPostBuyNow()
        {
            _toastNotification.Success("Tạo order thành công", 5);
            return Page();
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