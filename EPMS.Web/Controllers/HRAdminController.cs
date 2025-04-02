using EmployeePerformancesManagementSystem.MVC.Models;
using EmployeePerformancesManagementSystem.MVC.Models.EmployeeReviewSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeePerformancesManagementSystem.MVC.Controllers
{
    public class HRAdminController : Controller
    {

        public ActionResult Employees()
        {
           
            return View();
        }
        public ActionResult Dashboard() {  return View(); }

        [HttpPost]
        public ActionResult AddEmployee(User employee)
        {
           
            return View(employee);
        }

        public ActionResult EditEmployee(int id)
        {
            
            return View();
        }

        public ActionResult EvaluationCriteria()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddReviewCycle(ReviewCycle cycle)
        {
            if (ModelState.IsValid)
            {
                
                return RedirectToAction("ReviewCycles");
            }
            return View(cycle);
        }

        public ActionResult PerformanceReports()
        {
            
            return View();
        }
        public ActionResult AssignManager()
        {
            return View();
        }

    }
}
