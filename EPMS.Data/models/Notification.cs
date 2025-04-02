using EmployeePerformancesManagementSystem.MVC.Models.EmployeeReviewSystem.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Notification
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column("Employee")]  
    public int EmployeeId { get; set; }

    [JsonIgnore]
    [ValidateNever]
    [ForeignKey("EmployeeId")]
    public virtual User? User { get; set; }

    public int ManagerId { get; set; }

    [Required]
    public string Message { get; set; }

    public bool IsRead { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
