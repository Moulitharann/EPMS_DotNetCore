using EmployeePerformancesManagementSystem.MVC.Models;
using EmployeePerformancesManagementSystem.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeePerformancesManagementSystem.MVC.Controllers
{
    [ApiController]
    [Route("api/EmployeeDashboard")]
    public class EmployeeDashboardController : ControllerBase
    {
        private readonly EmployeeDashboardService _service;

        public EmployeeDashboardController(EmployeeDashboardService service)
        {
            _service = service;
        }

        [HttpPost("SetGoal")]
        public IActionResult SetGoal([FromBody] Goal goal)
            {
            
            try
            {
                var result = _service.SetGoal(goal);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetGoals")]
        public IActionResult GetGoals()
        {
            try
            {
                var goals = _service.GetGoals();
                return Ok(goals);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SubmitFeedback")]
        public IActionResult SubmitFeedback([FromBody] Notification notification)
        {
            try
            {
                var result = _service.SaveFeedback(notification);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error saving feedback: {ex.Message}");
            }
        }

        [HttpGet("GetManager/{employeeId}")]
        public IActionResult GetManager(int employeeId)
        {
            try
            {
                var managerName = _service.GetManager(employeeId);
                if (string.IsNullOrEmpty(managerName))
                {
                    return NotFound();
                }
                return Ok(new { ManagerName = managerName });
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut("UpdateGoal/{id}")]
        public IActionResult UpdateGoal(int id, [FromBody] Goal goal)
        {
            if (goal == null || id <= 0 || goal.EmployeeId <= 0)
            {
                return BadRequest("Invalid goal data.");
            }

            try
            {
                var result = _service.UpdateGoal(id, goal);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetManagers")]
        public IActionResult GetManagers()
        {
            try
            {
                var managers = _service.GetManagers();
                return Ok(managers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetEmployeeEvaluations/{employeeId}")]
        public IActionResult GetEmployeeEvaluations(int employeeId)
        {
            try
            {
                var evaluations = _service.GetEmployeeEvaluations(employeeId);
                if (evaluations == null || !evaluations.Any())
                {
                    return NotFound();
                }
                return Ok(evaluations);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
    