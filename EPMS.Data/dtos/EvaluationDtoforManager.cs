using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeePerformancesManagementSystem.MVC.DTOS
{
    public class EvaluationDtoforManager
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Comments { get; set; }
        public double Score { get; set; }
    }
}