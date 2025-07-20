using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkToDoor.Business.Dtos.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace DrinkToDoor.Web.Pages.Admins.Ingredients
{
    public class Create : PageModel
    {
        private readonly ILogger<Create> _logger;

        public Create(ILogger<Create> logger)
        {
            _logger = logger;
        }

        public IngredientRequest IngredientRequest { get; set; } = new IngredientRequest();

        public void OnGet()
        {
        }
    }
}