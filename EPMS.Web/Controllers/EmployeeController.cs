using Microsoft.AspNetCore.Mvc;

namespace EmployeePerformancesManagementSystem.MVC.Controllers
{
    public class EmployeeController : Controller
    {
       
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult ManagerGoal()
        {
            return View();
        }
        public ActionResult Feedback()
        {
            return View();
        }
        public ActionResult PerformanceHistory()
        {
            return View();
        }
    }
}