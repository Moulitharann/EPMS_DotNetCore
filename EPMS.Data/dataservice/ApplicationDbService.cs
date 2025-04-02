using System.Data.SqlTypes;
using EmployeePerformancesManagementSystem.MVC.Models.EmployeeReviewSystem.Models;
using EPMS.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeePerformancesManagementSystem.MVC.DataService
{
    public class ApplicationDbService
    {
        private readonly DBConnection _context;

        public ApplicationDbService(DBConnection context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "Database context is not initialized.");
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            try
            {
                return await _context.Users.AnyAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                throw new Exception("Error checking user existence.", ex);
            }
        }

        public async Task AddUserAsync(User user)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user), "User object cannot be null.");
                }

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding user to the database.", ex);
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                return await _context.Users.Where(u => u.Email == email)
                                               .Select(u => new User
                                               {
                                                   Id = u.Id,
                                                   Name = u.Name,
                                                   Email = u.Email ?? string.Empty,
                                                   PasswordHash = u.PasswordHash,
                                                   Role = u.Role,
                                                   ManagerId = u.ManagerId
                                               })
                                               .SingleOrDefaultAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

    }    
}
