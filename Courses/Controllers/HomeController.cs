using Courses.Data;
using Courses.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Courses.Controllers
{
    public class HomeController : Controller
    {
        private readonly CoursesDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, CoursesDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(string Email, string Password)
        {
            if (_context.users.Any(a => a.Password == Password && a.Email == Email))
            {
                int id = _context.users.Where(a => a.Password == Password && a.Email == Email).Select(a => a.Id).FirstOrDefault();
                Response.Cookies.Append("userId", id.ToString());
                return RedirectToAction("Index", "User", new { id = id });
            }
            else if (_context.managers.Any(a => a.Password == Password && a.Email == Email))
            {
                int id = _context.managers.Where(a => a.Password == Password && a.Email == Email).Select(a => a.Id).FirstOrDefault();
                Response.Cookies.Append("userId", id.ToString());
                return RedirectToAction("Index", "Manager", new { id = id });
            }
            else if (_context.admins.Any(a => a.Password == Password && a.Email == Email))
            {
                int id = _context.admins.Where(a => a.Password == Password && a.Email == Email).Select(a => a.Id).FirstOrDefault();
                Response.Cookies.Append("userId", id.ToString());
                return RedirectToAction("Index", "Admin", new { id = id });
            }
            return View();
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("userId");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Register(string Surname,string Name,string FatherName, string Email, string Password)
        {
            User user= new User();
            user.Email = Email;
            user.Password = Password;
            user.Surname = Surname;
            user.Name = Name;
            user.FatherName = FatherName;
            _context.users.Add(user);

            _context.SaveChanges();
            int id = _context.users.Where(a => a.Password == Password && a.Email == Email).Select(a => a.Id).FirstOrDefault();

            Wallet wallet = new Wallet();
            wallet.UserId = id;
            wallet.Money = 0;
            wallet.WalletNum = "0";
            

            _context.Add(wallet);
            _context.SaveChanges();
            Response.Cookies.Append("userId", id.ToString());
            return RedirectToAction("Index", "User", new { id = id });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}