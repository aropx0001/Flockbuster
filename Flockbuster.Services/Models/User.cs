using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flockbuster.Services.Models
{
    public class User
    {
        public int accountID;
        public string firstname;
        public string lastname;
        public string username;
        public string password;
        public UserType Type;
        public double? balance = 100000;
        public List<RentalObject> MyLoans;
        public List<NeverForget>? History;

        public User(int accountID, string firstname, string lastname, string username, string password)
        {
            this.accountID = accountID;
            this.firstname = firstname;
            this.lastname = lastname;
            this.balance = 0;
            this.username = username;
            this.password = password;
        }

        public User()
        {

        }
    }

    
}
