using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers
{
    public class ManagerController : Controller
    {
        public IActionResult Index()
        { //список созданных им курсов
            return View();
        }

        [HttpPost]
        public IActionResult AddCourse()
        { //модалка будет
            return View();
        }

        [HttpPost]
        public IActionResult EditCourse()
        { // модалка будет
            return View();
        }

        [HttpPost]
        public IActionResult DeleteCourse()
        { // модалка будет
            return View();
        }

        public IActionResult Capital()
        { //зароботок за продажи курсов (список продаж)
            return View();
        }
    }
}
