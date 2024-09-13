using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flockbuster.Services.Models
{
    public class NeverForget
    {
        public int UserID { get; set; }
        public int ItemID { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public DateOnly? RentDate { get; set; }
        public DateOnly? ReturnDate { get; set; }
    }

}
