using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EmployeePerformancesManagementSystem.MVC.Models.EmployeeReviewSystem.Models;

namespace EmployeePerformancesManagementSystem.MVC.Models
{
    public class PerformanceReport
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        public virtual User Employee { get; set; }

        public DateTime GeneratedDate { get; set; } = DateTime.UtcNow;

        public int OverallScore { get; set; } 

        public string ReportData { get; set; }

        public int? GeneratedByHRId { get; set; }
        public int HRAdminId { get; set; }  
        public User HRAdmin { get; set; }
    }

}