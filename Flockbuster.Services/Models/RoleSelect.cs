using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flockbuster.Services.Models
{
    
    public class Standard : User
    {
        public Standard(int accountID, string firstname, string lastname, string username, string password) : base(accountID, firstname, lastname, username, password)
        { 
            this.Type = UserType.Standard;
            History = new();
        }

    }

    public class Admin : User
    {
        public Admin(int accountID, string firstname, string lastname, string username, string password) : base(accountID, firstname, lastname, username, password)
        {
            this.Type = UserType.Admin;
            History = new();
        }

    }
}
