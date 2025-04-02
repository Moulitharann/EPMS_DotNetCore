using EmployeePerformancesManagementSystem.MVC.DTOS;
using EmployeePerformancesManagementSystem.MVC.Models;
using EmployeePerformancesManagementSystem.MVC.Models.EmployeeReviewSystem.Models;
using EPMS.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeePerformancesManagementSystem.MVC
{
    public class EmployeeDashboardDataService
    {
        private readonly DBConnection _context;

        public EmployeeDashboardDataService(DBConnection context)
        {
            try
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string SaveGoal(Goal goal)
        {
            try
            {
                if (goal == null) return "Invalid goal data";

                goal.Deadline = DateTime.Now;
                _context.Goals.Add(goal);
                _context.SaveChanges();
                return "Goal set successfully!";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in SaveGoal: {ex.Message}");
                return "Goal Not Set Successfully From DataService Layer";
            }
        }

        public string UpdateGoal(int id, Goal goal)
        {
            try
            {
                if (goal == null) return "Invalid goal data"; 

                var existingGoal = _context.Goals.FirstOrDefault(g => g.Id == id);
                if (existingGoal == null) return "Goal not found";

                existingGoal.GoalText = goal.GoalText;
                existingGoal.Deadline = goal.Deadline;
                existingGoal.GoalStatus = goal.GoalStatus;
                existingGoal.IsPersonalGoal = goal.IsPersonalGoal;

                _context.SaveChanges();
                return "Goal updated successfully!";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in UpdateGoal: {ex.Message}");
                return "Not Updated";
            }
        }

        public List<Goal> FetchGoals()
        {
            try
            {
                var goals = _context.Goals.FromSqlRaw("EXEC GetGoal").ToList();
                return goals;
            }
            catch (Exception ex)
             {
                Console.WriteLine($"❌ Error in FetchGoals: {ex.Message}");
                return new List<Goal>();
            }
        }

        public string StoreFeedback(Notification notification)
        {
            try
            {
                

                _context.Notifications.Add(notification);
                _context.SaveChanges();
                return "Feedback request sent successfully!";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in StoreFeedback: {ex.Message}");
                return "Not Sent";
            }
        }



        public string GetManagerName(int employeeId)
        {
            try
            {
                var managerName = _context.Users
                    .Where(u => u.Id == employeeId)
                    .Select(u => u.Manager.Name)  
                    .FirstOrDefault();  

                return managerName ?? "No Manager";  
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in GetManagerName: {ex.Message}");
                return "Not Fetched";
            }
        }


        public List<User> FetchManagers()
        {
            try
            {
                return _context.Users.Where(u => u.Role == "Manager").ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in FetchManagers: {ex.Message}");
                return new List<User>();
            }
        }

        public List<EvaluationDto> GetEmployeeEvaluations(int employeeId)
        {
            try
            {
                var evaluations = _context.Evaluations
                    .Where(e => e.EmployeeId == employeeId)
                    .Join(_context.Users, e => e.ManagerId, u => u.Id, (e, u) => new EvaluationDto
                    {
                        ManagerName = u.Name ?? "Unknown", 
                        Comments = e.Comments,
                        Score = (int)e.Score
                    })
                    .ToList();

                return evaluations;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in GetEmployeeEvaluations: {ex.Message}");
                return new List<EvaluationDto>();
            }
        }
    }
}
