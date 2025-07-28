
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DrinkToDoor.Web.Pages.Admins.Ingredients
{
    public class Detail : PageModel
    {
        private readonly ILogger<Detail> _logger;
        private readonly IIngredientService _ingredientService;

        public Detail(ILogger<Detail> logger, IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
            _logger = logger;
        }

        public IngredientResponse IngredientResponse { get; set; } = new IngredientResponse();

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
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ingredient details");
                ModelState.AddModelError(string.Empty, "An error occurred while retrieving the ingredient details.");
                return Page();
            }
        }
    }
}