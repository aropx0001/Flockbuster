using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flockbuster.Services.Models
{
    public class RentalObject
    {
        public int ItemID { get; set; }

        public string Titel { get; set; }

        public List<Category> Category { get; set; } = new List<Category>();
        public int ReleaseYear { get; set; }

        public string Instructor { get; set; }

        public double Price { get; set; }

        public bool InStock { get; set; }

        public DateOnly? RentDate { get; set; }

        public DateOnly? ReturnDate { get; set; }

        public int? LoaningNow { get; set; }
        
    }


}
