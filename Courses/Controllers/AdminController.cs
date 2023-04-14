using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TableUsers()
        {
            return View();
        }

        public IActionResult TableManagers() 
        {
            return View();
        }

        public IActionResult TableCourses()
        {
            return View();
        }

        public IActionResult Capital()
        { //зароботок 30% от цены курса, выводится общая сумма
            return View();
        }
    }
}
