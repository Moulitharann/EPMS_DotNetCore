using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EmployeePerformancesManagementSystem.MVC.Models.EmployeeReviewSystem.Models;

namespace EmployeePerformancesManagementSystem.MVC.Models
{
    public class EmployeePerformance
    {
        [Key]
        public int Id { get; set; }
        
        public int EmployeeId { get; set; }  
        public int ManagerId { get; set; }   
        public int DepartmentId { get; set; }
        public int Rating { get; set; } 
        public string FeedbackText { get; set; }
        public int Score { get; set; }
        public DateTime DateEvaluated { get; set; } = DateTime.Now;

    }


}