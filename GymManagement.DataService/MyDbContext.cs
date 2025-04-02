using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagement.DataService.Models;

namespace GymManagement.DataService
{
    public class MyDbContext:DbContext
    {
       public virtual DbSet<CustomerDB> Customer { get; set; }
        
    }
}
