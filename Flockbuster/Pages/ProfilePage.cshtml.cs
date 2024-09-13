using Flockbuster.Services;
using Flockbuster.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;

namespace Flockbuster.Pages
{
    public class ProfilePageModel : PageModel
    {
        private readonly AdminServices _adminServices;

        public ProfilePageModel(AdminServices adminServices)
        {
            _adminServices = adminServices;
        }

        public User User { get; set; } = new();

        [BindProperty]
        [MaxLength(20)]
        public string Firstname { get; set; }

        [BindProperty]
        [MaxLength(20)]
        public string Lastname { get; set; }

        [BindProperty]
        public double? Balance { get; set; }

        public IActionResult OnGet()
        {
            int? foundID = HttpContext.Session.GetInt32("ID");

            if (foundID is null)
            {
                return Redirect("/LoginPage");
            }

            User = _adminServices.IdentifyUserByID(foundID.Value);
            User.History = _adminServices.GetHistoryFromUser(foundID.Value);
            return Page();
        }

        public IActionResult OnPostUpdateBalance()
        {
            int? foundID = HttpContext.Session.GetInt32("ID");

            if (foundID is null)
            {
                return Redirect("/LoginPage");
            }

            if (!ModelState.IsValid)
            {
                User = _adminServices.IdentifyUserByID(foundID.Value);

                if (Balance.HasValue)
                {
                    _adminServices.AddBalance(User.accountID, Balance);
                }

                _adminServices.UpdateUser(User);

                User = _adminServices.IdentifyUserByID(foundID.Value);

                return RedirectToPage();
            }

            return Page();
        }


        public IActionResult OnPostNameChange()
        {
            int? foundID = HttpContext.Session.GetInt32("ID");

            if (foundID is null)
            {
                return Redirect("/LoginPage");
            }

            if (ModelState.IsValid)
            {
                User = _adminServices.IdentifyUserByID(foundID.Value);

                User.firstname = Firstname;
                User.lastname = Lastname;
                
                _adminServices.UpdateUser(User);

                return RedirectToPage();
            }

            return Page();
        }
    }
}
