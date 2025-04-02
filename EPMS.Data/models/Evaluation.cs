using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using EmployeePerformancesManagementSystem.MVC.Models.EmployeeReviewSystem.Models; // Ensure this namespace is included

namespace EmployeePerformancesManagementSystem.MVC.Models
{
    public class Evaluation
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        [JsonIgnore] // Exclude from JSON requests
        public virtual User? Employee { get; set; }

        public int ManagerId { get; set; }

        [JsonIgnore] // Exclude from JSON requests
        public virtual User? Manager { get; set; }

        public string? Comments { get; set; }
        public double Score { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
