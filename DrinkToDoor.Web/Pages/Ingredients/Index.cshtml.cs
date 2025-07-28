using DrinkToDoor.Business.Dtos.ParamSearchs;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public int PageSize { get; set; } = 12;
        [BindProperty(SupportsGet = true)]
        public int PageCurrent { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public Guid? CategoryId { get; set; }

        public SelectList CategoryList { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string Keyword { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Type { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; }

        public async Task OnGet()
        {
            await LoadDataAsync();
        }

        [BindProperty(SupportsGet = true)]
        public IngredientParams? IngredientParams { get; set; }

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
    }
}