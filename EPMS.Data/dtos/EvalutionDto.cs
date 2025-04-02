using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeePerformancesManagementSystem.MVC.DTOS
{
    public class EvaluationDto
    {
        public string ManagerName { get; set; }
        public string Comments { get; set; }
        public int Score { get; set; }
    }

}