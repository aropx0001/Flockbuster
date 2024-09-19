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

        [BindProperty]
        public User User { get; set; }

        [BindProperty]
        [Range(0, double.MaxValue)]
        public double? Balance { get; set; }

        [BindProperty]
        [MaxLength(20, ErrorMessage = "Max 20 characters")]
        public string Firstname { get; set; }

        [BindProperty]
        [MaxLength(20, ErrorMessage = "Max 20 characters")]
        public string Lastname { get; set; }

        public IActionResult OnGet()
        {
            int? foundID = HttpContext.Session.GetInt32("ID");

            if (foundID is null)
            {
                return Redirect("/LoginPage");
            }

            User = _adminServices.IdentifyUserByID(foundID.Value);
            User.History = _adminServices.GetHistoryFromUser(foundID.Value);

            Firstname = User.firstname;
            Lastname = User.lastname;

            return Page();
        }

        public IActionResult OnPostUpdateBalance()
        {
            int? foundID = HttpContext.Session.GetInt32("ID");

            if (foundID == null)
                return Redirect("/LoginPage");

            User = _adminServices.IdentifyUserByID(foundID.Value);

            if (Balance.HasValue)
            {
                double? balance = _adminServices.AddBalanceV2(User.accountID, Balance.Value);
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
            Balance = 0;

            if (ModelState.IsValid)
            {
                var userToUpdate = _adminServices.IdentifyUserByID(foundID.Value);
                userToUpdate.firstname = Firstname;
                userToUpdate.lastname = Lastname;

                _adminServices.UpdateUserName(userToUpdate);

                return RedirectToPage();
            }

            User = _adminServices.IdentifyUserByID(foundID.Value);
            User.History = _adminServices.GetHistoryFromUser(foundID.Value);

            return Page();
        }
    }
}
