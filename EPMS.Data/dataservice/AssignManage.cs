using System.Data;
using EmployeePerformancesManagementSystem.MVC.Models.EmployeeReviewSystem.Models;
using EPMS.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeePerformancesManagementSystem.MVC.Data
{
    public class AssignManagerDataService
    {
        private readonly DBConnection _db;

        public AssignManagerDataService(DBConnection context)
        {
            try
            {
                _db = context ?? throw new ArgumentNullException(nameof(context));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<User> GetEmployees()
        {
            try
            {
                return _db.Users.Where(u => u.Role == "Employee").ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in GetEmployees: {ex.Message}");
                return new List<User>();
            }
        }

        public List<User> GetManagers()
        {
            try
            {
                return _db.Users.FromSqlRaw("EXEC GetManagerList").ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in GetManagers: {ex.Message}");
                return new List<User>();
            }
        }

        public bool AssignManager(int employeeId, int managerId)
        {
            try
            {
                var employee = _db.Users.FirstOrDefault(u => u.Id == employeeId);
                if (employee == null)
                {
                    Console.WriteLine($"❌ Employee with ID {employeeId} not found.");
                    return false;
                }

                employee.ManagerId = managerId;
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in AssignManager: {ex.Message}");
                return false;
            }
        }
    }
}
