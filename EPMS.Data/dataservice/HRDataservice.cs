using EmployeePerformancesManagementSystem.MVC.DTOS;
using EmployeePerformancesManagementSystem.MVC.Models;
using EmployeePerformancesManagementSystem.MVC.Models.EmployeeReviewSystem.Models;
using EPMS.Data;
using EPMS.Data.dtos;
using Microsoft.EntityFrameworkCore;

namespace EmployeePerformancesManagementSystem.MVC.Data
{
    public class UserRepository
    {
        private readonly DBConnection _db;
        public UserRepository(DBConnection context)
        {
            try
            {
                _db = context ?? throw new ArgumentNullException(nameof(context), "Database context cannot be null.");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<UserDto> GetAllUsers()
        {
            try
            {
                if (_db?.Users == null)
                    throw new Exception("Database context is null or Users table is inaccessible.");

                var managerNames = _db.Users
                    .Where(u => u.Role == "Manager")
                    .ToDictionary(m => m.Id, m => m.Name ?? "Unknown"); 

                return _db.Users
                    .Where(user => user.Role == "Manager" || user.Role == "Employee")
                    .Select(user => new UserDto
                    {
                        Id = user.Id,
                        Name = user.Name ?? "Unknown", 
                        Email = user.Email ?? "No Email", 
                        PasswordHash = user.PasswordHash ?? "N/A", 
                        Role = user.Role ?? "Unassigned", 
                        ManagerId = user.ManagerId,
                        ManagerName = user.ManagerId.HasValue && managerNames.ContainsKey(user.ManagerId.Value)
                                      ? managerNames[user.ManagerId.Value]
                                      : "No Manager", 
                        Category = user.Category ?? "Uncategorized" 
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in GetAllUsers: {ex.Message}");
                return new List<UserDto>();
            }
        }



        public void AddUser(User user)
        {
            try
            {
                if (user == null) throw new ArgumentNullException(nameof(user), "User object cannot be null.");
                _db.Users.Add(user);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddUser: {ex.Message}");
            }
        }

        public void UpdateUser(int id, User user)
        {
            try
            {
                var existingUser = _db.Users.Find(id);
                if (existingUser == null) throw new Exception("User not found.");

                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                if (!string.IsNullOrEmpty(user.PasswordHash))
                {
                    existingUser.PasswordHash = user.PasswordHash;
                }
                existingUser.Role = user.Role;
                existingUser.Category = user.Category;

                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateUser: {ex.Message}");
            }
        }

        public void DeleteUser(int id)
        {
            try
            {
                var user = _db.Users.Find(id);
                if (user == null) throw new Exception("User not found.");

                var relatedRecords = _db.PerformanceCriteria.Where(p => p.ManagerId == id);
                foreach (var record in relatedRecords)
                {
                    record.ManagerId = null;
                }

                _db.Users.Remove(user);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteUser: {ex.Message}");
            }
        }

        public async Task<List<EmployeeDto>> GetEmployeesAsync()
        {
            try
            {
                return await _db.Users
                    .Where(u => u.Role == "Employee")
                    .Select(u => new EmployeeDto
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Email = u.Email,
                        Role = u.Role,
                        Category = u.Category
                    }).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetEmployeesAsync: {ex.Message}");
                return new List<EmployeeDto>();
            }
        }

        public PagedResult<PerformanceCriteriaDTO> GetAllCriteria(int pageNumber, int pageSize)
        {
            try
            {
                var query = from pc in _db.PerformanceCriteria
                            join e in _db.Users on pc.EmployeeId equals e.Id
                            join m in _db.Users on pc.ManagerId equals m.Id into managers
                            from m in managers.DefaultIfEmpty() 
                            select new PerformanceCriteriaDTO
                            {
                                Id = pc.Id,
                                CriteriaName = pc.CriteriaName,
                                Weightage = pc.Weightage,
                                EmployeeId = pc.EmployeeId,
                                EmployeeName = e.Name,
                                ManagerId = pc.ManagerId,
                                ManagerName = m != null ? m.Name : "N/A", 
                                Score = pc.Score
                            };

                int totalRecords = query.Count();
                var items = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                return new PagedResult<PerformanceCriteriaDTO>
                {
                    Items = items,
                    TotalRecords = totalRecords,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllCriteria: {ex.Message}");
                return new PagedResult<PerformanceCriteriaDTO>
                {
                    Items = new List<PerformanceCriteriaDTO>(),
                    TotalRecords = 0,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
        }

        public void AddCriteria(PerformanceCriteria criteria)
        {
            try
            {
                if (criteria == null) throw new ArgumentNullException(nameof(criteria), "Criteria object cannot be null.");
                _db.PerformanceCriteria.Add(criteria);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddCriteria: {ex.Message}");
            }
        }

        public string GetPasswordForUser(int userId)
        {
            try
            {
                return _db.Users.Where(u => u.Id == userId).Select(u => u.PasswordHash).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPasswordForUser: {ex.Message}");
                return string.Empty;
            }
        }
        public void UpdateScore(int employeeId, int score)
        {
            try
            {
                var latestCriteria = _db.PerformanceCriteria
                                        .Where(c => c.EmployeeId == employeeId)
                                        .OrderByDescending(c => c.Id) 
                                        .FirstOrDefault();

                if (latestCriteria != null)
                {
                    latestCriteria.Score = score;
                    _db.SaveChanges();
                }
            }
            catch (Exception er)
            {
                Console.WriteLine(er);
            }
        }
        
        public List<string> FetchDistinctCategories()
        {
            try
            {
                return _db.Users
                    .Where(u => !string.IsNullOrEmpty(u.Category) && u.Category != "Uncategorized" && u.Category!="null" && u.Category!="string")
                    .Select(u => u.Category)
                    .Distinct()
                    .ToList();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public bool AddPerformanceEvaluation(int managerId, string category, string feedbackText)
        {
            try
            {
                var employees = _db.Users.Where(u => u.Category == category).ToList();
                if (!employees.Any()) return false;

                var manager = _db.Users.FirstOrDefault(m => m.Id == managerId);
                if (manager == null) return false;

                foreach (var employee in employees)
                {
                    var feedbackScores = _db.Feedbacks
                        .Where(f => f.EmployeeId == employee.Id)
                        .Select(f => (int?)f.star)
                        .ToList();

                    int avgRating = feedbackScores.Any()
                        ? (int)Math.Round(feedbackScores.Average() ?? 3) 
                        : 3;

                    Console.WriteLine($"Employee: {employee.Id}, Feedback Count: {feedbackScores.Count}, Calculated Avg Rating: {avgRating}");

                    var existingPerformance = _db.EmployeePerformances
                        .FirstOrDefault(ep => ep.EmployeeId == employee.Id && ep.ManagerId == manager.Id);

                    if (existingPerformance != null)
                    {
                        existingPerformance.Rating = avgRating;
                        existingPerformance.Score = avgRating * 10;
                        existingPerformance.FeedbackText = feedbackText;
                        existingPerformance.DateEvaluated = DateTime.Now;
                    }
                    else
                    {
                        var newEvaluation = new EmployeePerformance
                        {
                            EmployeeId = employee.Id,
                            ManagerId = manager.Id,
                            DepartmentId = employee.Id,
                            Rating = avgRating,
                            Score = avgRating * 10,
                            FeedbackText = feedbackText,
                            DateEvaluated = DateTime.Now
                        };

                        _db.EmployeePerformances.Add(newEvaluation);
                    }
                }

                int savedRecords = _db.SaveChanges();
                Console.WriteLine($"Records Updated/Saved: {savedRecords}");
                return savedRecords > 0;
            }
            catch (Exception er)
            {
                Console.WriteLine(er);
                return false;
            }
        }

        public  List<EmployeePerformancesdtoforReport> GetPerformanceReports(string departmentName)
        {
            try
            {

                var departmentIds = _db.Users
                                       .Where(u => u.Category == departmentName)
                                       .Select(u => u.Id)
                                       .Distinct()
                                       .ToList();

                if (!departmentIds.Any())
                {
                    throw new Exception("Department not found.");
                }

                var reports = (from emp in _db.EmployeePerformances
                               join empUser in _db.Users on emp.EmployeeId equals empUser.Id into employees
                               from employee in employees.DefaultIfEmpty()
                               join mgr in _db.Users on emp.ManagerId equals mgr.Id into managers
                               from manager in managers.DefaultIfEmpty()
                               where departmentIds.Contains(emp.DepartmentId)
                               select new EmployeePerformancesdtoforReport
                               {
                                   EmployeeName = employee != null ? employee.Name : "N/A",
                                   ManagerName = manager != null ? manager.Name : "N/A",
                                   DepartmentId = emp.DepartmentId,
                                   Rating = emp.Rating,
                                   Score = emp.Score
                               }).ToList();

                return reports;


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching the performance reports.", ex);
            }


        }

        public async Task<List<PerformancesReportDto>> GetPerformanceReportsAsync(string category)
        {
            Console.WriteLine($"🔹 Received Category: '{category}'");

            return await Task.Run(() =>
            {
                var usersInCategory = _db.Users
                    .Where(u => u.Category == category)
                    .Select(u => new { u.Id, u.Name })
                    .ToList();

                Console.WriteLine($"🔹 Users in category '{category}': " +
                    (usersInCategory.Any() ? string.Join(", ", usersInCategory.Select(u => $"{u.Id} - {u.Name}")) : "None"));

                var employeeIds = usersInCategory.Select(u => u.Id).ToList();
                Console.WriteLine($"🔹 Extracted Employee IDs: " + (employeeIds.Any() ? string.Join(", ", employeeIds) : "None"));

                if (!employeeIds.Any())
                {
                    Console.WriteLine($"❌ No employees found in category '{category}'");
                    return new List<PerformancesReportDto>(); 
                }

                var reports = _db.EmployeePerformances
                    .Where(r => employeeIds.Contains(r.EmployeeId))
                    .Select(r => new
                    {
                        r.EmployeeId,
                        r.Rating,
                        r.Score,
                    })
                    .ToList();

                Console.WriteLine($"🔹 Reports Found: {reports.Count}");

                if (!reports.Any())
                {
                    Console.WriteLine($"❌ No performance reports found for employees in category '{category}'");
                    return new List<PerformancesReportDto>();
                }

                var result = reports
                    .Join(usersInCategory,
                          report => report.EmployeeId,
                          user => user.Id,
                          (report, user) => new PerformancesReportDto
                          {
                              UserName = user.Name,
                              Category = category,
                              star = report.Rating,
                              score = report.Score
                          })
                    .ToList();

                Console.WriteLine($"🔹 Final Result Count: {result.Count}");
                return result;
            });
        }

    }
}