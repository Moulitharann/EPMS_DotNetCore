using Microsoft.AspNetCore.Mvc;

namespace EmployeePerformancesManagementSystem.MVC.Controllers
{
    public class ManagerController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dashboard() {
            return View();
        }
        public ActionResult AssignGoals()
        {
            return View("Pages/AssignGoals"); 
        }

        public ActionResult Feedback()
        {
            return View("Pages/Feedback"); 
        }

        public ActionResult PerformanceReview()
        {
            return View("Pages/PerformanceReview"); 
        }
    }
}