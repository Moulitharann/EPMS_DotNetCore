using EmployeePerformancesManagementSystem.MVC.Services;
using EPMS.Data.dtos;
using Microsoft.AspNetCore.Mvc;

namespace EmployeePerformancesManagementSystem.MVC.Controllers
{
    [Route("api/assignmanager")]
    [ApiController] 
    public class AssignManagerController : ControllerBase
    {
        private readonly AssignManagerService _service;

       
        public AssignManagerController(AssignManagerService service)
        {
            _service = service;
        }

        [HttpGet("employees")]
        public ActionResult GetEmployees()
        {
            try
            {
                var employees = _service.GetEmployees();
                if (employees == null) return NotFound("No employees found.");
                return Ok(employees);
            }
            catch (Exception)
            {

                throw;
            }
        }   

        [HttpGet("managers")]
        public ActionResult GetManagers()
        {
            try
            {
                var managers = _service.GetManagers();
                if (managers == null) return NotFound("No managers found.");
                return Ok(managers);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("assign")]
        public ActionResult AssignManager([FromBody] AssignManagerRequest request)
        {
            try
            {
                if (request == null || request.EmployeeId <= 0 || request.ManagerId <= 0)
                    return BadRequest("Invalid request data.");

                bool result = _service.AssignManager(request.EmployeeId, request.ManagerId);

                if (result)
                    return Ok(new { message = "Manager assigned successfully." });
                else
                    return BadRequest("Error assigning manager.");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    
    
}
