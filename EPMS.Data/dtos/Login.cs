using System.ComponentModel.DataAnnotations;

namespace EmployeePerformancesManagementSystem.MVC.DTOs
{
    public class UserLogin
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
