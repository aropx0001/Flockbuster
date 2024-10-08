﻿using Flockbuster.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Flockbuster.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly AdminServices _adminServices;
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger, AdminServices adminServices)
        {
            _adminServices = adminServices;
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}