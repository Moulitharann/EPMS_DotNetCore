using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeePerformancesManagementSystem.MVC.DTOS
{
    public class EvaluationRequest
    {
        public int ManagerId { get; set; }
        public string Category { get; set; }
        public string FeedbackText { get; set; }
    }

}