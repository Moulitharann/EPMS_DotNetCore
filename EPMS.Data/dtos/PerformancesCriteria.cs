using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPMS.Data.dtos
{
    public class PerformanceCriteriaDTO
    {
        public int Id { get; set; }
        public string CriteriaName { get; set; }
        public int Weightage { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int? ManagerId { get; set; } // Nullable for safety
        public string ManagerName { get; set; }
        public int Score { get; set; }
    }

}
