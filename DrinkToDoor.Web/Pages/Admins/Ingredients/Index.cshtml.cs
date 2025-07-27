
using DrinkToDoor.Business.Dtos.ParamSearchs;
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using DrinkToDoor.Business.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DrinkToDoor.Web.Pages.Admins.Ingredients
{
    public class IndexModel : PageModel
    {
        private readonly IIngredientService _ingredientService;
        private readonly ICategoryService _categoryService;

        public IndexModel(IIngredientService ingredientService, ICategoryService categoryService)
        {
            _ingredientService = ingredientService;
            _categoryService = categoryService;
        }

        public IEnumerable<IngredientResponse> IngredientResponses { get; set; }

        public IngredientRequest IngredientRequest { get; set; } = new IngredientRequest();
        public IngredientResponse IngredientResponse { get; set; } = new IngredientResponse();
        public int TotalPages { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;
        [BindProperty(SupportsGet = true)]
        public int PageCurrent { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public IngredientParams? IngredientParams { get; set; }


        [BindProperty(SupportsGet = true)]
        public Guid? CategoryId { get; set; }

        public SelectList CategoryList { get; set; } = default!;

        public async Task OnGet()
        {
            await LoadDataAsync();
            var categories = await _categoryService.GetAll();
            CategoryList = new SelectList(categories, "Id", "Name", CategoryId);
        }

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
            IngredientResponses = result.Item1;
            TotalPages = (int)Math.Ceiling(result.Item2 / (double)PageSize);
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var entity = await _ingredientService.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            await _ingredientService.DeleteAsync(id);
            return RedirectToPage();
        }
    }
}