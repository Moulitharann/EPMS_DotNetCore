using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeePerformancesManagementSystem.MVC.DTOS
{
    public class GoalWithEmployeeDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }  // This is from the Users table
        public string GoalText { get; set; }
        public int Weightage { get; set; }
        public string GoalStatus { get; set; }
        public DateTime Deadline { get; set; }
    }

}