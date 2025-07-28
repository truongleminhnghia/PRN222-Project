
using AspNetCoreHero.ToastNotification.Abstractions;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DrinkToDoor.Web.Pages.Admins.Kits
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IKitService _kitService;
        private readonly INotyfService _toastNotification;

        public IndexModel(ILogger<IndexModel> logger, IKitService kitService, INotyfService toastNotification)
        {
            _logger = logger;
            _kitService = kitService;
            _toastNotification = toastNotification;
        }

        public int TotalPages { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        [BindProperty(SupportsGet = true)]
        public int PageCurrent { get; set; } = 1;

        public IEnumerable<KitResponse> Kits { get; set; }

        public async Task OnGet()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            var result = await _kitService.GetAll(PageCurrent, PageSize);
            Kits = result.Item1;
            TotalPages = (int)Math.Ceiling(result.Item2 / (double)PageSize);
        }
    }
}