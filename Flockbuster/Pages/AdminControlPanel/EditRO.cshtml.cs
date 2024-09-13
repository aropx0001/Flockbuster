using Flockbuster.Services.Models;
using Flockbuster.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Flockbuster.Pages.AdminControlPanel
{
    public class EditROModel : PageModel
    {
        private readonly AdminServices _adminServices;

        public EditROModel(AdminServices adminServices)
        {
            _adminServices = adminServices;
        }
        [BindProperty]
        public int ItemID { get; set; }

        [BindProperty]
        public List<Category> SelectedCategories { get; set; } = new List<Category>();

        [BindProperty]
        public RentalObject NewRO { get; set; } = new RentalObject();

        [BindProperty]
        public RentalObject FoundRO { get; set; }

        public void OnGet()
        {

        }
        public IActionResult OnPostEdit()
        {
            NewRO.Category = SelectedCategories;
            _adminServices.UpdateRO(NewRO);

            return Page();
        }

        public IActionResult OnPostGetID()
        {
            FoundRO = _adminServices.FindROWithID(ItemID);

            if (FoundRO != null)
            {
                // Copy values from FoundRO to NewRO
                NewRO.ItemID = FoundRO.ItemID;
                NewRO.Titel = FoundRO.Titel;
                NewRO.ReleaseYear = FoundRO.ReleaseYear;
                NewRO.Instructor = FoundRO.Instructor;
                NewRO.Price = FoundRO.Price;
                NewRO.InStock = FoundRO.InStock;
                NewRO.Category = FoundRO.Category;
            }
            else
            {
                // Handle case where no RentalObject was found
                ModelState.AddModelError(string.Empty, "No Rental Object found with the given ID.");
            }

            return Page();
        }
    }
}
