using Flockbuster.Services;
using Flockbuster.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Flockbuster.Pages
{
    public class LoginPageModel : PageModel
    {
        private readonly AdminServices _adminServices;

        public LoginPageModel(AdminServices adminServices)
        {
            _adminServices = adminServices;
        }

        [BindProperty]
        [MaxLength(20)]
        public string Username { get; set; }

        [BindProperty]
        [MaxLength(20)]
        public string Password { get; set; }

        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                User foundUser = _adminServices.HtmlLogin(Username.ToLower(), Password);

                if (foundUser is not null)
                {
                    HttpContext.Session.SetInt32("ID", foundUser.accountID);
                    HttpContext.Session.SetInt32("Type", (int)foundUser.Type);

                    return RedirectToPage("/ProfilePage");
                }
            }
            return Page();
        }
    }
}
