using Flockbuster.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flockbuster.Services
{
    public class ContactService
    {
        public List<ContactCase> ContactCases { get; set; } = new();

        public void AddContactCase(string firstname, string lastname, string email, string msg)
        {
            ContactCases.Add(new ContactCase
            {
                Firstname = firstname,
                Lastname = lastname,
                Email = email,
                Message = msg
            });
        }

        public List<ContactCase> GetAllContactCases()
        {
            return ContactCases;
        }
    }
}

