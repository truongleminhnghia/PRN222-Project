
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DrinkToDoor.Web.Pages.Admins.Ingredients
{
    public class EditModel : PageModel
    {
        private readonly IIngredientService _ingredientService;
        private readonly ILogger<EditModel> _logger;

        public EditModel(IIngredientService ingredientService, ILogger<EditModel> logger)
        {
            _logger = logger;
            _ingredientService = ingredientService;
        }

        [BindProperty]
        public IngredientUpdateRequest IngredientUpdateRequest { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var response = await _ingredientService.GetByIdAsync(id);
            if (response == null)
            {
                return NotFound();
            }

            IngredientUpdateRequest = new IngredientUpdateRequest
            {
                Id = response.Id,
                Code = response.Code,
                Name = response.Name,
                Price = response.Price,
                Cost = response.Cost,
                StockQty = response.StockQty,
                Description = response.Description,
                Status = response.Status,
                Images = response.Images,
                PackagingOptions = response.PackagingOptions
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _ingredientService.UpdateAsync(id, IngredientUpdateRequest);
            _logger.LogInformation($"Ingredient with ID {id} updated .", IngredientUpdateRequest);
            return RedirectToPage("Detail", new { id });
        }
    }
}
