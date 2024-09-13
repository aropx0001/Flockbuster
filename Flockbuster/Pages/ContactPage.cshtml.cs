using Flockbuster.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Flockbuster.Pages
{
    public class ContactPageModel : PageModel
    {
        private readonly ContactService _contactService;
        public ContactPageModel(ContactService contactService)
        {
            _contactService = contactService;
        }

        [BindProperty]
        [MaxLength(20)]
        public string Firstname { get; set; }
        [BindProperty]
        [MaxLength(20)]
        public string Lastname { get; set; }
        [BindProperty]
        [MaxLength(100)]
        public string Email { get; set; }
        [BindProperty]
        [MaxLength(2400)]
        public string Message { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _contactService.AddContactCase(Firstname, Lastname, Email, Message);
                return RedirectToPage("/ContactPageSucces");
            }
            return Page();
        }
    }
}
