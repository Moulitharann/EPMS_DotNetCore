using System.Threading.Tasks;
using System.Web.Http.Cors;
using EmployeePerformancesManagementSystem.MVC.DataService;
using EmployeePerformancesManagementSystem.MVC.DTOs;
using EmployeePerformancesManagementSystem.MVC.Models;
using EmployeePerformancesManagementSystem.MVC.Models.EmployeeReviewSystem.Models;

namespace EmployeePerformancesManagementSystem.MVC.Services
{
    public class AuthService
    {
       
        private readonly ApplicationDbService _dbService;

        public AuthService(ApplicationDbService dbService)
        {
            _dbService = dbService ?? throw new ArgumentNullException(nameof(dbService));
        }
        
        public async Task<string> Register(Register model)
        {
            try
            {
                if (await _dbService.UserExistsAsync(model.Email))
                    return "User already exists";

                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    PasswordHash = model.Password,
                    Role = model.Role,
                    Category = model.Category
                };

                await _dbService.AddUserAsync(user);
                return "User registered successfully";
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<User?> ValidateUser(UserLogin model)
        {
            try
            {
                var user = await _dbService.GetUserByEmailAsync(model.Email);
                if (user == null || user.PasswordHash != model.Password)
                    return null;

                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
