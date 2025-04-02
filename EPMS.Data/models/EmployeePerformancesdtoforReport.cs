using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeePerformancesManagementSystem.MVC.Models
{
    public class EmployeePerformancesdtoforReport
    {
        public String EmployeeName { get; set; }
        public string ManagerName { get; set; }
        public int DepartmentId { get; set; }
        public int Rating { get; set; }
        public int Score { get; set; }
    }
}