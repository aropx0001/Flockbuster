using Flockbuster.Services.Models;
using Flockbuster.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Flockbuster.Pages.AdminControlPanel
{
    public class OccupiedObjectsPageModel : PageModel
    {
        private readonly AdminServices _adminServices;

        public OccupiedObjectsPageModel(AdminServices adminServices)
        {
            _adminServices = adminServices;
        }

        [BindProperty]
        public RentalObject rentalObject { get; set; }

        public List<RentalObject> ListOfRO { get; set; } = new();

        public void OnGet()
        {
            ListOfRO = _adminServices.GetAllFalses();
        }
    }
}
