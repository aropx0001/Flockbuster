using Flockbuster.Services;
using Flockbuster.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;

namespace Flockbuster.Pages
{
    public class Sign_upModel : PageModel
    {
        private readonly AdminServices _adminServices;
        
        public Sign_upModel(AdminServices adminServices)
        {
            _adminServices = adminServices;
        }

        [BindProperty]
        [MaxLength(12)]
        public string Username { get; set; }

        [BindProperty]
        [MaxLength(20)]
        public string Password { get; set; }

        [BindProperty]
        [MaxLength(20)]
        public string Firstname { get; set; }

        [BindProperty]
        [MaxLength(20)]
        public string Lastname { get; set; }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            if (ModelState.IsValid)
            {

            }
            int listMax = _adminServices.UserList.Count();

            _adminServices.CreateUser(Firstname, Lastname, UserType.Standard, Username, Password).ToString();

            if (listMax < _adminServices.UserList.Count())
            {
                Response.Redirect("/LoginPage");
            }
        }

    }
}
