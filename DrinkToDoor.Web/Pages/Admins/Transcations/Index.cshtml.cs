
using System.Threading.Tasks;
using DrinkToDoor.Business.Dtos.ParamSearchs;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DrinkToDoor.Web.Pages.Admins.Transcations
{
    public class Index : PageModel
    {
        private readonly ILogger<Index> _logger;
        private readonly IPaymentService _paymentService;

        public Index(ILogger<Index> logger, IPaymentService paymentService)
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        public int TotalPages { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        [BindProperty(SupportsGet = true)]
        public int PageCurrent { get; set; } = 1;

        public IEnumerable<PaymentResponse> Payments { get; set; }

        [BindProperty(SupportsGet = true)]
        public PaymentParams PaymentParams { get; set; }

        public async Task OnGet()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var result = await _paymentService.GetParams(
                method: PaymentParams?.PaymentMethod,
                minPrice: PaymentParams?.MinPrice,
                maxPrice: PaymentParams?.MaxPrice,
                status: PaymentParams?.Status,
                fromDate: PaymentParams?.FromDate,
                toDate: PaymentParams?.ToDate,
                currency: PaymentParams?.Currency,
                userId: null,
                pageCurrent: PageCurrent,
                pageSize: PageSize
            );
            Payments = result.Item1;
            TotalPages = (int)Math.Ceiling(result.Item2 / (double)PageSize);
        }
    }
}