using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeePerformancesManagementSystem.MVC.DTOS
{
    public class PerformancesReportDto
    {
        public string UserName { get; set; }
        public string Category { get; set; }
        public int star {  get; set; }
        public int score { get; set; }
    }
}