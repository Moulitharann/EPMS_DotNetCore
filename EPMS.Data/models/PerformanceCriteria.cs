using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Web;
using EmployeePerformancesManagementSystem.MVC.Models.EmployeeReviewSystem.Models;

namespace EmployeePerformancesManagementSystem.MVC.Models
{
    public class PerformanceCriteria
    {
        public int Id { get; set; }
        public string CriteriaName { get; set; }
        public int Weightage { get; set; }

        public int EmployeeId { get; set; }  
        public int? ManagerId { get; set; }  

        public int Score { get; set; }
        [JsonIgnore]
        public virtual User? Employee { get; set; }
        [JsonIgnore]
        public virtual User? Manager { get; set; }
    }


}