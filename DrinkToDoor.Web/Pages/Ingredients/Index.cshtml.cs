using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
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

        public int TotalPages { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;
        [BindProperty(SupportsGet = true)]
        public int PageCurrent { get; set; } = 1;

        public async Task OnGet()
        {
            var listIngredient = await _ingredientService.GetAsync(null, PageCurrent, PageSize);
            Ingredients = listIngredient.Item1;
            TotalPages = (int)Math.Ceiling(listIngredient.Item2 / (double)PageSize);
        }
    }
}