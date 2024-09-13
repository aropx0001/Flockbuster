using Flockbuster.Services.Models;
using Flockbuster.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Flockbuster.Pages.AdminControlPanel
{
    public class DeleteROModel : PageModel
    {
        private readonly AdminServices _adminServices;

        public DeleteROModel(AdminServices adminServices)
        {
            _adminServices = adminServices;
        }
        [BindProperty]
        public int ItemID { get; set; }

        [BindProperty]
        public RentalObject FoundRO { get; set; }

        public void OnGet()
        {

        }
        

        public IActionResult OnPostGetID()
        {
            FoundRO = _adminServices.FindROWithID(ItemID);

            if (FoundRO is null)
            {
                ModelState.AddModelError(string.Empty, "No Rental Object found.");
            }
            return Page();
        }
        public IActionResult OnPostDelete()
        {
            FoundRO = _adminServices.FindROWithID(ItemID);

            if (FoundRO is not null)
            {
                _adminServices.DeleteRO(FoundRO.ItemID);
                FoundRO = null;
                ModelState.Clear();
            }
            return Page();
        }
    }
}
