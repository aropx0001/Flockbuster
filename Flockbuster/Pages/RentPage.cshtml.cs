using Flockbuster.Services;
using Flockbuster.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.Xml;

namespace Flockbuster.Pages
{
    public class RentPageModel : PageModel
    {
        private readonly AdminServices _adminServices;

        public RentPageModel(AdminServices adminServices)
        {
            _adminServices = adminServices;
        }
        [BindProperty]
        public List<RentalObject> rentalObjects { get; set; } = new();

        [BindProperty]
        public int ItemID { get; set; }

        [BindProperty(SupportsGet = true)]
        [MaxLength(100)]
        public string SearchString { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string SelectedCategory { get; set; } = string.Empty;

        public List<string> AvailableCategories { get; set; } = new();

        public IActionResult OnGet()
        {
            AvailableCategories = Enum.GetNames(typeof(Category)).Select(c => c.Replace('_', ' ')).ToList();
            
            rentalObjects = _adminServices.ListOfRentalObjects;
            
            if (!string.IsNullOrEmpty(SearchString))
            {
                rentalObjects = rentalObjects.Where(rO => rO.Titel.Contains(SearchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(SelectedCategory))
            {
                rentalObjects = rentalObjects.Where(rO => rO.Category.Any(c => c.ToString() == SelectedCategory)).ToList();
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            int? userID = HttpContext.Session.GetInt32("ID");

            if (userID is null)
            {
                return Redirect("/LoginPage");
            }

            RentalObject rentalObject = _adminServices.FindROWithID(ItemID);

            if (rentalObjects is not null)
            {
                _adminServices.RentObject(rentalObject.ItemID, userID.Value);
            }

            //reload changes
            rentalObjects = _adminServices.ListOfRentalObjects;
            return Page();
        }

    }
}
