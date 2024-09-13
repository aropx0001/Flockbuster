using Flockbuster.Services;
using Flockbuster.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Flockbuster.Pages.AdminControlPanel
{
    public class UserOverviewPageModel : PageModel
    {
        private readonly AdminServices _adminServices;

        public UserOverviewPageModel(AdminServices adminServices)
        {
            _adminServices = adminServices;
        }

        [BindProperty]
        public List<User> UserList { get; set; } = new();

        [BindProperty]
        public List<RentalObject>? History { get; set; } = new();

        [BindProperty]
        public User user { get; set; } = new();
        public void OnGet()
        {
            UserList = _adminServices.UserList;

        }

        
    }
}
