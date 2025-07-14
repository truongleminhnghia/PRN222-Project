
using DrinkToDoor.Business.Dtos.ParamSearchs;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DrinkToDoor.Web.Pages.Admins.Ingredients
{
    public class IndexModel : PageModel
    {
        private readonly IIngredientService _ingredientService;

        public IndexModel(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        public IEnumerable<IngredientResponse> IngredientResponses { get; set; }
        public int TotalPages { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;
        [BindProperty(SupportsGet = true)]
        public int PageCurrent { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public IngredientParams? IngredientParams { get; set; }

        public async Task OnGet()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var result = await _ingredientService.GetAsync(null, PageCurrent, PageSize);
            IngredientResponses = result.Item1;
            TotalPages = (int)Math.Ceiling(result.Item2 / (double)PageSize);
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            await _ingredientService.DeleteAsync(id);
            return RedirectToPage();
        }
    }
}