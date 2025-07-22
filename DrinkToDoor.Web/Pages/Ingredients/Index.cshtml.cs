using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DrinkToDoor.Web.Pages.Ingredients
{
    public class Index : PageModel
    {
        private readonly ILogger<Index> _logger;
        private readonly IIngredientService _ingredientService;

        public Index(ILogger<Index> logger, IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
            _logger = logger;
        }

        public IEnumerable<IngredientResponse> Ingredients { get; set; }

        public async Task OnGet()
        {
            var listIngredient = await _ingredientService.GetAsync(null, 1, 10);
            Ingredients = listIngredient.Item1;
        }
    }
}