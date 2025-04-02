using EmployeePerformancesManagementSystem.MVC.DataService;
using EmployeePerformancesManagementSystem.MVC.DTOS;
using EmployeePerformancesManagementSystem.MVC.Models;
using EmployeePerformancesManagementSystem.MVC.Models.EmployeeReviewSystem.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EmployeePerformancesManagementSystem.Core.Services
{
    public class ManagerService
    {
        private readonly ManagerDataService _dataService;

        public ManagerService(ManagerDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<List<User>> GetEmployeesAsync(int managerId)
        {
            return managerId > 0 ? await _dataService.GetAllEmployeesAsync(managerId) : new List<User>();
        }

        public async Task<bool> AssignGoalAsync(Goal goal)
        {
            if (goal?.EmployeeId > 0)
                return await _dataService.SaveGoalAsync(goal);
            return false;
        }

        public async Task<List<GoalWithEmployeeDto>> GetGoalsAsync(int managerId)
        {
            return managerId > 0 ? await _dataService.GetGoalsAsync(managerId) : new List<GoalWithEmployeeDto>();
        }

        public async Task<bool> UpdateGoalAsync(int id, Goal goal)
        {
            if (goal != null && id > 0)
                return await _dataService.UpdateGoalAsync(id, goal);
            return false;
        }

        public async Task<bool> DeleteGoalAsync(int id)
        {
            return id > 0 && await _dataService.DeleteGoalAsync(id);
        }

        public async Task<bool> SubmitFeedbackAsync(Feedback feedback)
        {
            if (feedback?.EmployeeId > 0 && !string.IsNullOrWhiteSpace(feedback.Comment))
                return await _dataService.SaveFeedbackAsync(feedback);
            return false;
        }

        public async Task<(string Feedback, string UserName)> GenerateReviewAsync(int employeeId, int managerId)
        {
            return (employeeId > 0 && managerId > 0) ? await _dataService.GetEmployeeReviewAsync(employeeId, managerId) : (null, null);
        }

        public async Task<List<Goal>> GetEmployeeGoalsAsync(int managerId, int employeeId)
        {
            return (managerId > 0 && employeeId > 0) ? await _dataService.GetEmployeeGoalsAsync(managerId, employeeId) : new List<Goal>();
        }

        public async Task<List<NotificationDto>> GetNotificationsByManagerIdAsync(int managerId)
        {
            return managerId > 0 ? await _dataService.GetNotificationsByManagerIdAsync(managerId) : new List<NotificationDto>();
        }

        public async Task<bool> SubmitEvaluationAsync(Evaluation evaluation)
        {
            if (evaluation?.EmployeeId <= 0 || evaluation?.ManagerId <= 0)
                return false;  // Ensure both EmployeeId & ManagerId exist

            var employee = await _dataService.GetUserByIdAsync(evaluation.EmployeeId);
            int managerId = evaluation.ManagerId > 0 ? evaluation.ManagerId : employee?.ManagerId ?? 0;
            if (managerId == 0)
                return false;

            var goals = await _dataService.GetEmployeeGoalsAsync(managerId, evaluation.EmployeeId);
            double goalScore = goals.Any() ? goals.Average(g => g.Weightage) : 0;

            var feedbacks = await _dataService.GetFeedbacksByManagerAsync(managerId, evaluation.EmployeeId);
            double feedbackScore = feedbacks.Any() ? feedbacks.Average(f => f.star) : 0;

            double finalScore = (goalScore + feedbackScore) / 2;
            string comments = finalScore switch
            {
                >= 4.5 => "Excellent performance! Keep up the great work.",
                >= 3.5 => "Good job! A few areas for improvement.",
                >= 2.5 => "Satisfactory performance, but there is room for growth.",
                >= 1.5 => "Needs improvement. Consider focusing on key areas.",
                _ => "Poor performance. Immediate improvement is required."
            };

            evaluation.Score = finalScore;
            evaluation.CreatedAt = DateTime.UtcNow;
            evaluation.Comments = comments;
            evaluation.ManagerId = managerId; 

            return await _dataService.SaveEvaluationAsync(evaluation);
        }


        public async Task<byte[]> GeneratePerformanceReportAsync(int employeeId)
        {
            var evaluations = await _dataService.GetEvaluationsByEmployeeAsync(employeeId);
            return evaluations.Any() ? GeneratePDFReport(employeeId, evaluations, evaluations.Average(e => e.Score)) : null;
        }

        private byte[] GeneratePDFReport(int employeeId, List<EvaluationDtoforManager> evaluations, double overallScore)
        {
            using MemoryStream memoryStream = new();
            Document document = new(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
            document.Open();

            Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            document.Add(new Paragraph($"Performance Report - Employee ID: {employeeId}\n\n", titleFont) { Alignment = Element.ALIGN_CENTER });

            Font scoreFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            document.Add(new Paragraph($"Overall Score: {overallScore:F2}\n\n", scoreFont) { Alignment = Element.ALIGN_LEFT });

            PdfPTable table = new(4) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 25, 25, 30, 20 });

            Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            foreach (string header in new[] { "Employee Name", "Employee ID", "Comments", "Score" })
                table.AddCell(new PdfPCell(new Phrase(header, headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });

            foreach (var eval in evaluations)
            {
                table.AddCell(eval.EmployeeName);
                table.AddCell(eval.EmployeeId.ToString());
                table.AddCell(eval.Comments ?? "N/A");
                table.AddCell(eval.Score.ToString("F2"));
            }

            document.Add(table);
            document.Close();
            writer.Close();

            return memoryStream.ToArray();
        }

        public async Task<List<EmployeePerformancesdtoforlist>> GetEmployeePerformancesAsync(int managerId)
        {
            var performances = await _dataService.GetEmployeePerformanceReviewAsync(managerId);
            return performances.GroupBy(p => p.EmployeeId).Select(g => g.OrderByDescending(p => p.Score).First()).ToList();
        }
    }
}
