using EmployeePerformancesManagementSystem.MVC.Models.EmployeeReviewSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Goal
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(500)]
    public string GoalText { get; set; }

    [Required]
    public DateTime Deadline { get; set; }

    [Required]
    public int EmployeeId { get; set; }

    [JsonIgnore]
    public virtual User? Employee { get; set; }  

    public string? GoalStatus { get; set; }

    public int? ManagerId { get; set; }

    [JsonIgnore]
    public virtual User? Manager { get; set; }  

    public int IsPersonalGoal { get; set; }

    [Range(0, 100)]
    public int Weightage { get; set; }
}
