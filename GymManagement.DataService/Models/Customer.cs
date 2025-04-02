using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DataService.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public string Description { get; set; }

        public string Address { get; set; }

        public int Phone { get; set; }
        public string City { get; set; }

        public int Age { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
    }
}
