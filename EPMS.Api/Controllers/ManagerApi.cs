using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EmployeePerformancesManagementSystem.MVC.Models;
using EmployeePerformancesManagementSystem.MVC.Services;
using EmployeePerformancesManagementSystem.Core.Services;

namespace EmployeePerformancesManagementSystem.MVC.Controllers
{
    [Route("api/manager")]
    [ApiController]
    public class ManagerApiController : ControllerBase
    {
        private readonly ManagerService _managerService;

        public ManagerApiController(ManagerService managerService)
        {
            _managerService = managerService;
        }

        [HttpGet("employees/{managerId}")]
        public async Task<IActionResult> GetEmployees(int managerId)
        {
            try
            {
                var employees = await _managerService.GetEmployeesAsync(managerId);
                return employees == null || !employees.Any() ? NotFound() : Ok(employees);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("assignGoal")]
        public async Task<IActionResult> AssignGoal([FromBody] Goal goal)
        {
            try
            {
                if (goal == null || string.IsNullOrEmpty(goal.GoalText) || goal.EmployeeId <= 0)
                    return BadRequest("Invalid goal data.");

                var result = await _managerService.AssignGoalAsync(goal);
                return result ? Ok("Goal assigned successfully!") : BadRequest("Failed to assign goal.");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("submitFeedback")]
        public async Task<IActionResult> SubmitFeedback([FromBody] Feedback feedback)
        {
            try
            {
                if (feedback == null || string.IsNullOrWhiteSpace(feedback.Comment))
                    return BadRequest("Invalid feedback data.");

                var result = await _managerService.SubmitFeedbackAsync(feedback);
                return result ? Ok("Feedback submitted successfully.") : BadRequest("Failed to submit feedback.");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut("updateGoal/{goalId}")]
        public async Task<IActionResult> UpdateGoal(int goalId, [FromBody] Goal goal)
        {
            try
            {
                if (goal == null || string.IsNullOrEmpty(goal.GoalText) || goal.EmployeeId <= 0)
                    return BadRequest("Invalid goal data.");

                var result = await _managerService.UpdateGoalAsync(goalId, goal);
                return result ? Ok("Goal updated successfully!") : BadRequest("Failed to update goal.");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpDelete("deleteGoal/{goalId}")]
        public async Task<IActionResult> DeleteGoal(int goalId)
        {
            try
            {
                var result = await _managerService.DeleteGoalAsync(goalId);
                return result ? Ok("Goal deleted successfully.") : BadRequest("Failed to delete goal.");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("goals/{managerId}")]
        public async Task<IActionResult> GetGoals(int managerId)
        {
            try
            {
                var goals = await _managerService.GetGoalsAsync(managerId);
                return goals == null || !goals.Any() ? NotFound() : Ok(goals);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("getEmployeeGoals")]
        public async Task<IActionResult> GetEmployeeGoals([FromQuery] int managerId, [FromQuery] int employeeId)
        {
            try
            {
                var goals = await _managerService.GetEmployeeGoalsAsync(managerId, employeeId);

                return goals == null || !goals.Any() ? NotFound() : Ok(goals);
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet("notifications/{managerId}")]
        public async Task<IActionResult> GetNotifications(int managerId)
        {
            try
            {
                var notifications = await _managerService.GetNotificationsByManagerIdAsync(managerId);
                return notifications == null || !notifications.Any() ? NotFound() : Ok(notifications);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("submitEvaluation")]
        public async Task<IActionResult> SubmitEvaluation([FromBody] Evaluation evaluation)
        {
            try
            {
                if (evaluation == null) return BadRequest("Invalid evaluation data.");

                var success = await _managerService.SubmitEvaluationAsync(evaluation);
                return success ? Ok("Evaluation submitted successfully.") : BadRequest("Failed to submit evaluation.");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("generateReport/{employeeId}")]
        public async Task<IActionResult> GenerateReport(int employeeId)
        {
            try
            {
                var reportData = await _managerService.GeneratePerformanceReportAsync(employeeId);
                if (reportData == null) return NotFound();

                return File(reportData, "application/pdf", $"PerformanceReport_{employeeId}.pdf");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("getEvaluationRecord/{managerId}")]
        public async Task<IActionResult> GetEmployeePerformanceRecord(int managerId)
        {
            try
            {
                var response = await _managerService.GetEmployeePerformancesAsync(managerId);
                return response == null || !response.Any() ? NotFound() : Ok(response);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
