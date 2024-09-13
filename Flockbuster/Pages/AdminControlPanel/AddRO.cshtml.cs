using Flockbuster.Services;
using Flockbuster.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;

namespace Flockbuster.Pages.AdminControlPanel
{
    public class AddROModel : PageModel
    {
        private readonly AdminServices _adminServices;

        public AddROModel(AdminServices adminServices)
        {
            _adminServices = adminServices;
        }

        [BindProperty]
        public RentalObject NewRO { get; set; } = new RentalObject();

        [BindProperty]
        public List<Category> SelectedCategories { get; set; } = new List<Category>();

        public IActionResult OnGet()
        {
            int? userType = HttpContext.Session.GetInt32("Type");

            if (userType is not 1) return Redirect("/AccessDenied");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            NewRO.Category = SelectedCategories;
            _adminServices.AddRO(NewRO);

            return Page();
        }

        
    }
}