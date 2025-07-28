using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Business.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace DrinkToDoor.Web.Pages.Admins.Categories
{
    public class Index : PageModel
    {
        private readonly ILogger<Index> _logger;
        private readonly ICategoryService _categoryService;
        private readonly INotyfService _toastNotification;

        public Index(ILogger<Index> logger, ICategoryService categoryService, INotyfService toastNotification)
        {
            _logger = logger;
            _categoryService = categoryService;
            _toastNotification = toastNotification;
        }

        public int TotalPages { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        [BindProperty(SupportsGet = true)]
        public int PageCurrent { get; set; } = 1;

        [BindProperty]
        public CategoryRequest CategoryRequest { get; set; } = new();

        public IEnumerable<CategoryResponse> Categories { get; set; }

        [BindProperty]
        public bool ShowCreateModal { get; set; }

        public async Task OnGet()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            var result = await _categoryService.GetParams(null, PageCurrent, PageSize, null);
            Categories = result.Item1;
            TotalPages = (int)Math.Ceiling(result.Item2 / (double)PageSize);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ShowCreateModal = true;
                await LoadData();
                return Page();
            }

            try
            {
                Guid result = await _categoryService.CreateAsync(CategoryRequest);
                if (result != null)
                {
                    _toastNotification.Success("Tạo mới thành công", 5);
                }
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                // 1) Log lỗi chi tiết
                _logger.LogError(ex, "Lỗi khi tạo loại: {@CategoryRequest}", CategoryRequest);
                _toastNotification.Error("Lỗi: " + ex.Message, 5);
                // 2) Thêm model error để hiển thị trong ValidationSummary
                ModelState.AddModelError(string.Empty, "Đã có lỗi xảy ra khi tạo loại: " + ex.Message);

                // 3) Bật lại modal và load dữ liệu cần thiết
                ShowCreateModal = true;
                await LoadData();
                return Page();
            }
        }
    }
}