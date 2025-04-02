using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Data.dtos
{
    public class AssignManagerRequest
    {
        public int EmployeeId { get; set; }
        public int ManagerId { get; set; }
    }
}
