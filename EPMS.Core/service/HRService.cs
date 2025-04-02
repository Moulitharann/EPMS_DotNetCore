using EmployeePerformancesManagementSystem.MVC;
using EmployeePerformancesManagementSystem.MVC.Data;
using EmployeePerformancesManagementSystem.MVC.DTOS;
using EmployeePerformancesManagementSystem.MVC.Models;
using EmployeePerformancesManagementSystem.MVC.Models.EmployeeReviewSystem.Models;
using EPMS.Data;
using EPMS.Data.dtos;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace EmployeePerformancesManagementSystem.Core.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly DBConnection _dbConnection;

        public UserService(UserRepository userRepository, DBConnection dbConnection)
        {
            _userRepository = userRepository;
            _dbConnection = dbConnection;
        }

        public List<UserDto> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public void AddUser(User user)
        {
            _userRepository.AddUser(user);
        }

        public void UpdateUser(int id, User user)
        {
            _userRepository.UpdateUser(id, user);
        }

        public void DeleteUser(int id)
        {
            _userRepository.DeleteUser(id);
        }

        public async Task<List<EmployeeDto>> GetEmployeesAsync()
        {
            return await _userRepository.GetEmployeesAsync();
        }

        public byte[] GenerateExcelForEmployees(List<EmployeeDto> employees)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Employees");

                // Adding Headers
                string[] headers = { "ID", "Name", "Email", "Role", "Category" };
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Adding Employee Data
                int row = 2;
                foreach (var emp in employees)
                {
                    worksheet.Cells[row, 1].Value = emp.Id;
                    worksheet.Cells[row, 2].Value = emp.Name;
                    worksheet.Cells[row, 3].Value = emp.Email;
                    worksheet.Cells[row, 4].Value = emp.Role;
                    worksheet.Cells[row, 5].Value = emp.Category;
                    row++;
                }

                return package.GetAsByteArray();
            }
        }

        public byte[] GeneratePdfForEmployees(List<EmployeeDto> employees)
        {
            using (var memoryStream = new MemoryStream())
            {
                Document document = new Document(PageSize.A4);
                PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                document.Add(new Paragraph("Employee Data\n\n"));

                PdfPTable table = new PdfPTable(5) { WidthPercentage = 100 };

                string[] headers = { "ID", "Name", "Email", "Role", "Category" };
                foreach (var header in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(header))
                    {
                        BackgroundColor = new BaseColor(211, 211, 211),
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    table.AddCell(cell);
                }

                foreach (var emp in employees)
                {
                    table.AddCell(emp.Id.ToString());
                    table.AddCell(emp.Name);
                    table.AddCell(emp.Email);
                    table.AddCell(emp.Role);
                    table.AddCell(emp.Category);
                }

                document.Add(table);
                document.Close();

                return memoryStream.ToArray();
            }
        }

        public PagedResult<PerformanceCriteriaDTO> GetCriteriaList(int pageNumber, int pageSize)
        {
            return _userRepository.GetAllCriteria(pageNumber, pageSize);
        }


        public void DefineCriteria(string name, int weightage, int employeeId)
        {
            var employee = _userRepository.GetAllUsers().FirstOrDefault(u => u.Id == employeeId);
            if (employee == null)
                throw new Exception("Employee not found");

            PerformanceCriteria criteria = new PerformanceCriteria
            {
                CriteriaName = name,
                Weightage = weightage,
                EmployeeId = employeeId,
                ManagerId = employee.ManagerId,
                Score = 0
            };
            _userRepository.AddCriteria(criteria);
        }

        public void CalculatePerformance(int weightage)
        {
            var criteriaList = _userRepository.GetAllCriteria(1, 100); 

            foreach (var criteria in criteriaList.Items) 
            {
                int employeeId = criteria.EmployeeId;

                var feedbackList = _dbConnection.Feedbacks
                                    .Where(f => f.EmployeeId == employeeId)
                                    .ToList();

                int goalScore = _dbConnection.Goals
                                    .Where(g => g.EmployeeId == employeeId)
                                    .Select(g => g.Weightage)
                                    .FirstOrDefault();

                double feedbackScore = 0;
                if (feedbackList.Count > 0)
                {
                    double totalStars = feedbackList.Sum(f => f.star);
                    double avgFeedbackScore = totalStars / feedbackList.Count;
                    feedbackScore = (avgFeedbackScore / 5) * 50;
                }

                double goalAchievementScore = (goalScore / 100) * 50;
                double finalScore = feedbackScore + goalAchievementScore;
                finalScore /= 100;

                _userRepository.UpdateScore(employeeId, (int)(finalScore * weightage));
            }
        }

        public List<string> GetDistinctCategories()
        {
            return _userRepository.FetchDistinctCategories();
        }

        public string GetPassword(int id)
        {
            return _userRepository.GetPasswordForUser(id);
        }

        public bool SubmitEvaluation(int managerId, string category, string feedbackText)
        {
            return _userRepository.AddPerformanceEvaluation(managerId, category, feedbackText);
        }

        public async Task<byte[]> GeneratePerformanceReportPdfAsync(string departmentId)
        {
            try
            {
                var data =  _userRepository.GetPerformanceReports(departmentId);
                if (!data.Any()) throw new InvalidOperationException("No performance reports found for the given department.");

                using (var memoryStream = new MemoryStream())
                {
                    Document document = new Document(PageSize.A4);
                    PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

                    document.Open();
                    document.Add(new Paragraph("Performance Report\n\n"));

                    PdfPTable table = new PdfPTable(4) { WidthPercentage = 100 };

                    string[] headers = { "Employee Name", "Manager Name", "Rating", "Score" };
                    foreach (var header in headers)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(header))
                        {
                            BackgroundColor = new BaseColor(211, 211, 211),
                            HorizontalAlignment = Element.ALIGN_CENTER
                        };
                        table.AddCell(cell);
                    }

                    foreach (var item in data)
                    {
                        table.AddCell(item.EmployeeName ?? "N/A");
                        table.AddCell(item.ManagerName ?? "N/A");
                        table.AddCell(item.Rating.ToString());
                        table.AddCell(item.Score.ToString());
                    }

                    document.Add(table);
                    document.Close();
                    writer.Close();

                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while generating the performance report PDF.", ex);
            }
        }

        public byte[] GeneratePerformanceReportExcel(string departmentId)
        {
            try
            {
                var data = _userRepository.GetPerformanceReports(departmentId);

                if (data == null )
                {
                    throw new Exception("No performance reports found for the given department.");
                }
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Performance Report");

                    string[] headers = { "Employee Name", "Manager Name", "Rating", "Score" };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = headers[i];
                        worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                        worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    }

                    int row = 2;
                    foreach (var item in data)
                    {
                        worksheet.Cells[row, 1].Value = item.EmployeeName ?? "N/A";
                        worksheet.Cells[row, 2].Value = item.ManagerName ?? "N/A";
                        worksheet.Cells[row, 3].Value = item.Rating.ToString() ?? "N/A";
                        worksheet.Cells[row, 4].Value = item.Score.ToString() ?? "N/A";
                        row++;
                    }

                    return package.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while generating the performance report Excel file.", ex);
            }
        }
        public async Task<List<PerformancesReportDto>> GetPerformanceReportsAsync(string category)
        {
            return await _userRepository.GetPerformanceReportsAsync(category);
        }
    }
}
