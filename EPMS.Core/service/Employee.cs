using System;
using System.Collections.Generic;
using EmployeePerformancesManagementSystem.MVC.DataService;
using EmployeePerformancesManagementSystem.MVC.DTOS;
using EmployeePerformancesManagementSystem.MVC.Models;
using EmployeePerformancesManagementSystem.MVC.Models.EmployeeReviewSystem.Models;

namespace EmployeePerformancesManagementSystem.MVC.Services
{
    public class EmployeeDashboardService
    {
        private readonly EmployeeDashboardDataService _dataService;

        public EmployeeDashboardService(EmployeeDashboardDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
        }

        public string SetGoal(Goal goal)
        {
            if (goal == null || string.IsNullOrWhiteSpace(goal.GoalText) || goal.EmployeeId <= 0)
            {
                throw new ArgumentException("Invalid goal details.");
            }
            return _dataService.SaveGoal(goal);
        }

        public List<Goal> GetGoals()
        {
            return _dataService.FetchGoals();
        }

        public string SaveFeedback(Notification notification)
        {
            if (notification == null)
            {
                throw new ArgumentException("Invalid notification data.");
            }
            return _dataService.StoreFeedback(notification);
        }

        public string GetManager(int employeeId)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentException("Invalid employee ID.");
            }
            return _dataService.GetManagerName(employeeId);
        }

        public string UpdateGoal(int id, Goal goal)
        {
            if (goal == null || id <= 0 || goal.EmployeeId <= 0)
            {
                throw new ArgumentException("Invalid goal data.");
            }
            return _dataService.UpdateGoal(id, goal);
        }

        public List<User> GetManagers()
        {
            return _dataService.FetchManagers();
        }

        public List<EvaluationDto> GetEmployeeEvaluations(int employeeId)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentException("Invalid employee ID.");
            }
            return _dataService.GetEmployeeEvaluations(employeeId);
        }
    }
}
