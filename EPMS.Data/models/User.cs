using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeePerformancesManagementSystem.MVC.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    namespace EmployeeReviewSystem.Models
    {
        public class User
        {
            [Key]
            public int Id { get; set; }

            [Required]
            public string Name { get; set; }

            [Required, EmailAddress]
            public string Email { get; set; }

            [Required]
            public string PasswordHash { get; set; }  

            [Required]
            public string Role { get; set; }

            public int? ManagerId { get; set; }
            [JsonIgnore]
            public virtual User? Manager { get; set; }

            public string Category { get; set; }
            [JsonIgnore]
            public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();

            [JsonIgnore]
            public virtual ICollection<Feedback> FeedbackReceived { get; set; } = new List<Feedback>();

            [JsonIgnore]
            public virtual ICollection<Feedback> FeedbackGiven { get; set; } = new List<Feedback>();

            [JsonIgnore]
            public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

            [JsonIgnore]
            public virtual ICollection<PerformanceReport> PerformanceReports { get; set; } = new List<PerformanceReport>();
        }
    }

}