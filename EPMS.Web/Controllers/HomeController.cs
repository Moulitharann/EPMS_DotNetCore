using Microsoft.AspNetCore.Mvc;

namespace EmployeePerformancesManagementSystem.MVC.Controllers
{
    public class HomeController : Controller
    {
       
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "This is DotNet Website";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact At Any Time";

            return View();
        }
    }
}