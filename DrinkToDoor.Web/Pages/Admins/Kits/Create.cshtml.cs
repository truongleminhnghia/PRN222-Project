using AspNetCoreHero.ToastNotification.Abstractions;
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DrinkToDoor.Web.Pages.Admins.Kits
{
    public class Create : PageModel
    {
        private readonly ILogger<Create> _logger;
        private readonly IKitService _kitService;
        private readonly IIngredientService _ingredientService;
        private readonly INotyfService _toastNotification;

        public Create(ILogger<Create> logger, IKitService kitService, INotyfService toastNotification,
                      IIngredientService ingredientService)
        {
            _logger = logger;
            _kitService = kitService;
            _toastNotification = toastNotification;
            _ingredientService = ingredientService;
        }

        [BindProperty]
        public KitRequest KitRequest { get; set; }

        public void OnGet()
        {
        }

        public async Task<JsonResult> OnGetSearchIngredientsAsync(string term)
        {
            var ingredients = await _ingredientService.OnGetSearchIngredientsAsync(term); 
            var result = ingredients.Select(i => new
            {
                i.Id,
                i.Name,
                i.Code,
                i.Price,
                ImageUrl = i.Images.FirstOrDefault()?.Url ?? "/images/no-image.png"
            });

            return new JsonResult(result);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                _toastNotification.Warning("Vui lòng kiểm tra lại thông tin.");
                return Page();
            }

            var success = await _kitService.CreateKit(KitRequest);
            if (success)
            {
                _toastNotification.Success("Tạo Kit thành công!");
                return RedirectToPage("Index");
            }

            _toastNotification.Error("Tạo Kit thất bại!");
            return Page();
        }
    }
}
