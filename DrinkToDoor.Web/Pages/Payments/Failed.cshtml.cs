using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace DrinkToDoor.Web.Pages.Payments
{
    public class Failed : PageModel
    {
        private readonly ILogger<Failed> _logger;

        public Failed(ILogger<Failed> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}