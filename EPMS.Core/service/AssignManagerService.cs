using System.Collections.Generic;
using EmployeePerformancesManagementSystem.MVC.Data;
using EmployeePerformancesManagementSystem.MVC.Models.EmployeeReviewSystem.Models;

namespace EmployeePerformancesManagementSystem.MVC.Services
{
    public class AssignManagerService
    {
        private readonly AssignManagerDataService _dataService;

        public AssignManagerService(AssignManagerDataService dataService)
        {
            try
            {
                _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
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
                return _dataService.GetEmployees();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<User> GetManagers()
        {
            try
            {
                return _dataService.GetManagers();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool AssignManager(int employeeId, int managerId)
        {
            try
            {
                return _dataService.AssignManager(employeeId, managerId);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
