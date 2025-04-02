using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagement.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Data
{
    public class MyApplicationDB:DbContext
    {
        public MyApplicationDB() { }

        public virtual DbSet<CustomerModel> CustomerModels { get; set; }
    }
}
