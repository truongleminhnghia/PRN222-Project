using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace DrinkToDoor.Web.Pages.Admins.Kits
{
    public class IndexModel : PageModel
    {

        public IndexModel(ILogger<Index> logger)
        {
        }

        public void OnGet()
        {
        }
    }
}