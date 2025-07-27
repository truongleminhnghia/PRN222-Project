
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DrinkToDoor.Web.Pages.Admins.Kits
{
    public class Detail : PageModel
    {
        private readonly ILogger<Detail> _logger;
        private readonly IKitService _kitService;

        public Detail(ILogger<Detail> logger, IKitService kitService)
        {
            _logger = logger;
            _kitService = kitService;
        }

        public KitResponse KitResponse { get; set; } = new();

        public async Task OnGet()
        {
            Guid id = Guid.Parse("08ddcc80-a8f1-444a-8ecf-8b42493ca963");
            await LoadData(id);
        }

        public async Task LoadData(Guid id)
        {
            var kit = await _kitService.GetById(id);
            KitResponse = kit;
        }
    }
}