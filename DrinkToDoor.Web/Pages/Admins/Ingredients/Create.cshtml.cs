using System.Drawing;
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Interfaces;
using DrinkToDoor.Business.Services;
using DrinkToDoor.Web.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;

namespace DrinkToDoor.Web.Pages.Admins.Ingredients
{
    public class Create : PageModel
    {
        private readonly ILogger<Create> _logger;
        private readonly IIngredientService _ingredientService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;
        private readonly IHubContext<IngredientHub> _hubContext;

        public Create(ILogger<Create> logger, IIngredientService ingredientService,
                      ICategoryService categoryService, IHubContext<IngredientHub> hubContext,
                      IWebHostEnvironment env)
        {
            _ingredientService = ingredientService;
            _categoryService = categoryService;
            _logger = logger;
            _env = env;
            _hubContext = hubContext;
        }

        [BindProperty]
        public IngredientRequest IngredientRequest { get; set; } = new IngredientRequest();

        public SelectList Categories { get; set; }

        [BindProperty]
        public PackagingOptionRequest PackagingOptionRequest { get; set; } = new PackagingOptionRequest();

        [BindProperty]
        public List<IFormFile> ImagesRequest { get; set; } = new List<IFormFile>();

        public async Task<IActionResult> OnGet()
        {
            var categories = await _categoryService.GetAll();
            Categories = new SelectList(categories, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var categories = await _categoryService.GetAll();
            Categories = new SelectList(categories, "Id", "Name");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                if (ImagesRequest != null && ImagesRequest.Any())
                {
                    var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "ingredients");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var savedPaths = new List<string>();

                    foreach (var formFile in ImagesRequest)
                    {
                        if (formFile.Length > 0)
                        {
                            var uniqueFileName = Guid.NewGuid() + Path.GetExtension(formFile.FileName);
                            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                            using (var fs = new FileStream(filePath, FileMode.Create))
                            {
                                await formFile.CopyToAsync(fs);
                            }

                            savedPaths.Add($"/images/ingredients/{uniqueFileName}");
                        }
                    }

                    IngredientRequest.ImagesRequest = savedPaths
                        .Select(path => new ImageRequest { ImageUrl = path })
                        .ToList();
                }

                IngredientRequest.PackagingOptionsRequest.Add(PackagingOptionRequest);
                var ingredientResponse = await _ingredientService.CreateAsync(IngredientRequest);
                await _hubContext.Clients.All.SendAsync("ReceiveNewIngredient", new
                {
                    Id = ingredientResponse.Id,
                    Name = ingredientResponse.Name,
                    Price = ingredientResponse.Price,
                    Description = ingredientResponse.Description,
                    ImagePath = ingredientResponse.Images?.FirstOrDefault()?.Url
                });
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating ingredient");
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
