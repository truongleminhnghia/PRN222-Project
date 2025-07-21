
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Interfaces;
using DrinkToDoor.Business.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DrinkToDoor.Web.Pages.Admins.Ingredients
{
    public class Create : PageModel
    {
        private readonly ILogger<Create> _logger;
        private readonly IIngredientService _ingredientService;
        private readonly ICategoryService _categoryService;
        private readonly ISupplierService _supplierService;

        public Create(ILogger<Create> logger, IIngredientService ingredientService,
                      ICategoryService categoryService, ISupplierService supplierService)
        {
            _ingredientService = ingredientService;
            _categoryService = categoryService;
            _supplierService = supplierService;
            _logger = logger;
        }

        [BindProperty]
        public IngredientRequest IngredientRequest { get; set; } = new IngredientRequest();

        public SelectList Categories { get; set; }
        public SelectList Suppliers { get; set; }

        [BindProperty]
        public PackagingOptionRequest PackagingOptionRequest { get; set; } = new PackagingOptionRequest();

        public async Task<IActionResult> OnGet()
        {
            var supplier = await _supplierService.GetAll();
            Suppliers = new SelectList(supplier, "Id", "Name");
            var categories = await _categoryService.GetAll();
            Categories = new SelectList(categories, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var supplier = await _supplierService.GetAll();
                Suppliers = new SelectList(supplier, "Id", "Name");
                var categories = await _categoryService.GetAll();
                Categories = new SelectList(categories, "Id", "Name");
                return Page();
            }

            try
            {
                IngredientRequest.PackagingOptions.Add(PackagingOptionRequest);
                await _ingredientService.CreateAsync(IngredientRequest);
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating ingredient");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the ingredient.");
                return Page();
            }
        }
    }
}