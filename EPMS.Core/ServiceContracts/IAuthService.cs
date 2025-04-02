using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeePerformancesManagementSystem.MVC.DTOs;
using EmployeePerformancesManagementSystem.MVC.Models.EmployeeReviewSystem.Models;

namespace EmployeePerformancesManagementSystem.Core.ServiceContracts
{
    public interface IAuthService
    {
        Task<string> RegisterUser(Register model);
        Task<User?> LoginUser(UserLogin model);
    }
}
