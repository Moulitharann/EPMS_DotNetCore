using System.ComponentModel.DataAnnotations;

namespace EmployeePerformancesManagementSystem.MVC.DTOs
{
    public class Register
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

        public string Category { get; set; }
    }
}
