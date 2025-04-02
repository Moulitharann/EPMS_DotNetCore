using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EmployeePerformancesManagementSystem.MVC.Models.EmployeeReviewSystem.Models;

namespace EmployeePerformancesManagementSystem.MVC.Models
{
    public class ReviewCycle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public virtual ICollection<User> Participants { get; set; }

        public virtual ICollection<Goal> Goals { get; set; }

        public ReviewCycle()
        {
            Participants = new HashSet<User>();
            Goals = new HashSet<Goal>();
        }
    }

}
