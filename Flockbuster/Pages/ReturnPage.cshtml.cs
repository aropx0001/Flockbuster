using Flockbuster.Services;
using Flockbuster.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Flockbuster.Pages
{
    public class ReturnPageModel : PageModel
    {
        private readonly AdminServices _adminServices;

        public ReturnPageModel(AdminServices adminServices)
        {
            _adminServices = adminServices;
        }

        [BindProperty]
        public int ItemID { get; set; }

        [BindProperty]
        public User user { get; set; } = new User();

        [BindProperty]
        public List<RentalObject> rentalObjects { get; set; } = new();

        public IActionResult OnGet()
        {
            int? userID = HttpContext.Session.GetInt32("ID");

            if (userID is null)
            {
                return Redirect("/LoginPage");
            }
            user = _adminServices.IdentifyUserByID(userID.Value);
            rentalObjects = _adminServices.ListOfRentalObjects;

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
            if (rentalObject is not null)
            {   
                _adminServices.ReturnObject(rentalObject.ItemID, userID.Value);
            }

            //reload changes
            rentalObjects = _adminServices.ListOfRentalObjects;
            

            return RedirectToPage("/ReturnPage");
        }
    }
}
