using EmployeePerformancesManagementSystem.MVC.DTOs;
using EmployeePerformancesManagementSystem.MVC.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EmployeePerformancesManagementSystem.MVC.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [EnableCors("AllowFrontend")] 
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid registration request.");

                var result = await _authService.Register(model);
                if (result == "User already exists")
                    return BadRequest("User already exists.");

                return Ok(new { message = "User registered successfully!" });
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid login request.");

                var user = await _authService.ValidateUser(model);
                if (user == null)
                    return Unauthorized();

                return Ok(new
                {
                    message = "Login successful!",
                    user = new
                    {
                        id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        Role = user.Role,
                        ManagerId = user.ManagerId
                    }
                });
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
