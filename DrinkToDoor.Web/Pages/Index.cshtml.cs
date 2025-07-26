
using DrinkToDoor.BLL.ViewModel.Responses;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DrinkToDoor.Web.Pages;

public class Index : PageModel
{
    private readonly ILogger<Index> _logger;
    private readonly IBannerService _bannerService;
    private readonly IIngredientService _ingredientService;

    public Index(ILogger<Index> logger, IBannerService bannerService, IIngredientService ingredientService)
    {
        _bannerService = bannerService;
        _logger = logger;
        _ingredientService = ingredientService;
    }

    public IList<BannerResponse> Banners { get; set; } = new List<BannerResponse>();

    public IEnumerable<IngredientResponse>? Ingredients { get; set; }

    public async Task OnGet()
    {
        Banners = await _bannerService.GetAllBannersAsync();
        var listIngredient = await _ingredientService.GetAsync(null, 1, 10);
        Ingredients = listIngredient.Item1;
    }
}
