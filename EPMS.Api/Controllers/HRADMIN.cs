using EmployeePerformancesManagementSystem.Core.Services;
using EmployeePerformancesManagementSystem.MVC.DTOS;
using EmployeePerformancesManagementSystem.MVC.Models;
using EmployeePerformancesManagementSystem.MVC.Models.EmployeeReviewSystem.Models;
using EmployeePerformancesManagementSystem.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeePerformancesManagementSystem.Core.ApiControllers
{
    [ApiController]
    [Route("api/assignmanager")]
    public class UserApiController : ControllerBase
    {
        private readonly UserService _userService;

        public UserApiController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
      
        public IActionResult GetUsers()
        {
            try
            {
                var users = _userService.GetAllUsers();
                if (users == null || users.Count == 0)
                {
                    return NotFound("No users found.");
                }
                return Ok(users);
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        public IActionResult AddUser([FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _userService.AddUser(user);
                return Ok(new { message = "User added successfully" });
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _userService.UpdateUser(id, user);
                return Ok(new { message = "User updated successfully" });
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                _userService.DeleteUser(id);
                return Ok(new { message = "User deleted successfully" });
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("DownloadEmployeeData/{format}")]
        public async Task<IActionResult> DownloadEmployeeData(string format)
        {
            try
            {
                var employees = await _userService.GetEmployeesAsync();

                if (employees == null || !employees.Any())
                    return NotFound();

                byte[] fileContent;
                string fileName;
                string contentType;

                if (format.ToLower() == "excel")
                {
                    fileContent = _userService.GenerateExcelForEmployees(employees);
                    fileName = "Employees.xlsx";
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                }
                else if (format.ToLower() == "pdf")
                {
                    fileContent = _userService.GeneratePdfForEmployees(employees);
                    fileName = "Employees.pdf";
                    contentType = "application/pdf";
                }
                else
                {
                    return BadRequest("Invalid format. Use 'excel' or 'pdf'.");
                }

                return File(fileContent, contentType, fileName);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("GetCriteria")]
        public IActionResult GetCriteria(int pageNumber = 1, int pageSize = 5)
        {
            try
            {
                var pagedResult = _userService.GetCriteriaList(pageNumber, pageSize);
                return Ok(pagedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving criteria: {ex.Message}");
            }
        }


        [HttpPost("DefineCriteria")]
        public IActionResult DefineCriteria([FromBody] PerformanceCriteria criteria)
        {
            try
            {
                _userService.DefineCriteria(criteria.CriteriaName, criteria.Weightage, criteria.EmployeeId);
                _userService.CalculatePerformance(criteria.Weightage);
                return Ok(new { message = "Criteria defined successfully" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Employee not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", error = ex.Message });
            }
        }

        [HttpGet("GetPassword/{id}")]
        public IActionResult GetPassword(int id)
        {
            try
            {
                var data = _userService.GetPassword(id);
                return Ok(data);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("DownloadPerformanceReportPdf/{departmentId}")]
        public async Task<IActionResult> DownloadPerformanceReportPdf(string departmentId)
        {
            try
            {
                var pdfContent = await _userService.GeneratePerformanceReportPdfAsync(departmentId);

                if (pdfContent == null || pdfContent.Length == 0)
                    return StatusCode(500, new { message = "Failed to generate the PDF report." });

                return File(pdfContent, "application/pdf", "PerformanceReport.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", error = ex.Message });
            }
        }


        [HttpGet("DownloadPerformanceReportExcel/{departmentId}")]
        public IActionResult DownloadPerformanceReportExcel(string departmentId)
        {
            try
            {
                var excelContent = _userService.GeneratePerformanceReportExcel(departmentId);

                if (excelContent == null || excelContent.Length == 0)
                    return StatusCode(500, new { message = "Failed to generate the Excel report." });

                return File(excelContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PerformanceReport.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", error = ex.Message });
            }
        }

        [HttpGet("categories")]
        public IActionResult GetCategories()
        {
            try
            {
                var categories = _userService.GetDistinctCategories();
                return Ok(categories);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("SubmitEvaluation")]
        public IActionResult SubmitEvaluation([FromBody] EvaluationRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.Category) || string.IsNullOrEmpty(request.FeedbackText))
                    return BadRequest("Invalid data.");

                bool result = _userService.SubmitEvaluation(request.ManagerId, request.Category, request.FeedbackText);

                if (result)
                {
                    return Ok("Evaluation submitted successfully.");
                }
                else
                {
                    return BadRequest("Failed to submit evaluation.");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("Performances")]
        public async Task<IActionResult> GetPerformanceReports([FromQuery] string category = "")
        {
            try
            {
                var result = await _userService.GetPerformanceReportsAsync(category);
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
