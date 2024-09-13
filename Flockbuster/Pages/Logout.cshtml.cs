using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Flockbuster.Pages
{
    public class LogoutModel : PageModel
    {

        public IActionResult OnGet()
        {
            HttpContext.Session.Remove("ID");

            return Redirect("/LoginPage");
        }

    }
}
