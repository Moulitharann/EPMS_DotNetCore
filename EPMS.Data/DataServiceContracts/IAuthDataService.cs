using System.Threading.Tasks;
using EmployeePerformancesManagementSystem.MVC.Models.EmployeeReviewSystem.Models;

namespace EPMS.Data.DataServiceContracts
{
    public interface IAuthDataService
    {
        Task<User?> GetUserByEmailAsync(string email); // Added 'email' parameter
        Task<bool> UserExistsAsync(string email);
        Task AddUserAsync(User user);
    }
}
