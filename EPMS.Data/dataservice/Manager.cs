using EmployeePerformancesManagementSystem.MVC.DTOS;
using EmployeePerformancesManagementSystem.MVC.Models;
using EmployeePerformancesManagementSystem.MVC.Models.EmployeeReviewSystem.Models;
using EPMS.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeePerformancesManagementSystem.MVC.DataService
{
    public class ManagerDataService
    {
        private readonly DBConnection _context;

        public ManagerDataService(DBConnection context)
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

        public async Task<List<User>> GetAllEmployeesAsync(int managerId)
        {
            try
            {
                return await _context.Users
                       .Where(u => u.Role == "Employee" && u.ManagerId == managerId)
                       .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<NotificationDto>> GetNotificationsByManagerIdAsync(int managerId)
        {
            try
            {
                return await _context.Notifications
                       .Where(r => r.ManagerId == managerId)
                       .Join(_context.Users,
                             r => r.EmployeeId,
                             u => u.Id,
                             (r, u) => new NotificationDto
                             {
                                 Id = r.Id,
                                 EmployeeName = u.Name,
                                 Message = r.Message,
                                 IsRead = r.IsRead
                             })
                       .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Goal>> GetEmployeeGoalsAsync(int managerId, int employeeId)
        {
            try
            {
                return await _context.Goals
                        .Where(g => g.ManagerId == managerId && g.EmployeeId == employeeId)
                        .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> SaveGoalAsync(Goal goal)
        {
            try
            {
                if (goal == null) return false;
                goal.Employee = await _context.Users.FindAsync(goal.EmployeeId);
                goal.Manager = goal.ManagerId.HasValue ? await _context.Users.FindAsync(goal.ManagerId) : null;

                if (goal.Employee == null) return false;

                goal.EmployeeId = goal.Employee.Id;
                goal.ManagerId = goal.Manager?.Id;

                await _context.Goals.AddAsync(goal);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }



        public async Task<bool> SaveFeedbackAsync(Feedback feedback)
        {
            try
            {
                if (feedback == null) return false;
                await _context.Feedbacks.AddAsync(feedback);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<GoalWithEmployeeDto>> GetGoalsAsync(int managerId)
        {
            try
            {
                return await _context.Goals
                        .Where(goal => goal.ManagerId == managerId)
                        .Join(_context.Users,
                              goal => goal.EmployeeId,
                              user => user.Id,
                              (goal, user) => new GoalWithEmployeeDto
                              {
                                  Id = goal.Id,
                                  EmployeeId = goal.EmployeeId,
                                  EmployeeName = user.Name,
                                  GoalText = goal.GoalText,
                                  Weightage = goal.Weightage,
                                  GoalStatus = goal.GoalStatus,
                                  Deadline = goal.Deadline
                              })
                        .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> UpdateGoalAsync(int id, Goal goal)
        {
            try
            {
                var existingGoal = await _context.Goals.FindAsync(id);
                if (existingGoal == null) return false;

                existingGoal.GoalText = goal.GoalText;
                existingGoal.Deadline = goal.Deadline;
                existingGoal.Weightage = goal.Weightage;
                existingGoal.IsPersonalGoal = goal.IsPersonalGoal;
                existingGoal.GoalStatus = goal.GoalStatus;

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> DeleteGoalAsync(int id)
        {
            try
            {
                var goal = await _context.Goals.FindAsync(id);
                if (goal == null) return false;

                _context.Goals.Remove(goal);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<(string Feedback, string UserName)> GetEmployeeReviewAsync(int employeeId, int managerId)
        {
            try
            {
                var result = await _context.Notifications
                        .Where(r => r.EmployeeId == employeeId && r.ManagerId == managerId)
                        .Join(_context.Users,
                              r => r.EmployeeId,
                              u => u.Id,
                              (r, u) => new { r.Message, u.Name })
                        .FirstOrDefaultAsync();

                return result != null ? (result.Message, result.Name) : (null, null);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> SaveEvaluationAsync(Evaluation evaluation)
        {
            try
            {
                if (evaluation == null) return false;

                await _context.Evaluations.AddAsync(evaluation);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<User> GetUserByIdAsync(int employeeId)
        {
            try
            {
                return await _context.Users.FindAsync(employeeId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Feedback>> GetFeedbacksByManagerAsync(int managerId, int employeeId)
        {
            try
            {
                return await _context.Feedbacks
                       .Where(f => f.ReviewerId == managerId && f.EmployeeId == employeeId)
                       .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<EvaluationDtoforManager>> GetEvaluationsByEmployeeAsync(int employeeId)
        {
            try
            {

            
            return await _context.Evaluations
                .Where(e => e.EmployeeId == employeeId)
                .Join(_context.Users,
                      e => e.EmployeeId,
                      u => u.Id,
                      (e, u) => new EvaluationDtoforManager
                      {
                          EmployeeId = e.EmployeeId,
                          EmployeeName = u.Name,
                          Comments = e.Comments,
                          Score = e.Score
                      })
                .ToListAsync();
            }
            catch (Exception er)
            {

                Console.WriteLine(er);
                return new List<EvaluationDtoforManager>();
            }
        }

        public async Task<List<EmployeePerformancesdtoforlist>> GetEmployeePerformanceReviewAsync(int managerId)
        {
            try
            {
                return await _context.Evaluations
                       .Where(p => p.ManagerId == managerId)
                       .Join(_context.Users,
                             p => p.EmployeeId,
                             u => u.Id,
                             (p, u) => new EmployeePerformancesdtoforlist
                             {
                                 EmployeeId = p.EmployeeId,
                                 EmployeeName = u.Name,
                                 Score = (int)p.Score
                             })
                       .OrderByDescending(p => p.Score)
                       .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
