using EmployeePerformancesManagementSystem.MVC.DTOs;
using EmployeePerformancesManagementSystem.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeePerformancesManagementSystem.MVC.Controllers
{
    public class AuthController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly AuthService _authService;

        public AuthController()
        {
           
        }

        [HttpGet]
        public ActionResult Login()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<ActionResult> Login(UserLogin model)
        {
            
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<ActionResult> Register(Register model)
        {
            
                return View();
           
        }

        [HttpPost]
        public ActionResult Logout()
        {
            
            
            return RedirectToAction("Login");
        }
    }
}
