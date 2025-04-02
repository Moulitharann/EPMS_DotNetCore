using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using EmployeePerformancesManagementSystem.MVC.Models.EmployeeReviewSystem.Models;

namespace EmployeePerformancesManagementSystem.MVC.Models
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [JsonIgnore] 
        public virtual User? Employee { get; set; }

        public int? ReviewerId { get; set; }

        [JsonIgnore] 
        public virtual User? Reviewer { get; set; }

        [JsonPropertyName("star")]
        public int star { get; set; } 

        [Required]
        public string TaskName_forFeedback { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters.")]
        public string Comment { get; set; }

        public DateTime FeedbackDate { get; set; } = DateTime.UtcNow;
    }
}
