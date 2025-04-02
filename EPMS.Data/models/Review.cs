using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EmployeePerformancesManagementSystem.MVC.Models.EmployeeReviewSystem.Models;

namespace EmployeePerformancesManagementSystem.MVC.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        public virtual User Employee { get; set; }

        public int? ReviewerId { get; set; }
        public virtual User Reviewer { get; set; }

        [Required]
        public string Feedback { get; set; }

        [Required]
        [Range(1, 5)] 
        public int Rating { get; set; }
        public int? ReviewCycle_Id { get; set; }  
        [ForeignKey("ReviewCycle_Id")]
        public virtual ReviewCycle ReviewCycle { get; set; }

        public DateTime ReviewDate { get; set; } = DateTime.UtcNow;
    }


}