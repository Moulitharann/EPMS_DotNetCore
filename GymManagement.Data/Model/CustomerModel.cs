using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.Data.Model
{
    public class CustomerModel
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Gender {  get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
    }
}
