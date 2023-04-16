using Courses.Data;
using Courses.Models;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers
{
    public class AdminController : Controller
    {
        private readonly CoursesDbContext _context;

        public AdminController(CoursesDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Users()
        {
            ViewData["users"]= _context.users.ToList();
            return View();
        }

        public IActionResult Managers() 
        {
            ViewData["managers"] = _context.managers.ToList();
            return View();
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            _context.users.Remove(_context.users.Where(a => a.Id == id).FirstOrDefault());
            await _context.SaveChangesAsync();
            return Redirect("~/Admin/Users");
        }

        public async Task<IActionResult> DeleteManager(int id)
        {
            _context.managers.Remove(_context.managers.Where(a => a.Id == id).FirstOrDefault());
            await _context.SaveChangesAsync();
            return Redirect("~/Admin/Managers");
        }

        [HttpPost]
        public async Task<IActionResult> AddManager(string Email,string Password,string Surname, string Name, string FatherName)
        {
            Manager manager = new Manager();
            manager.Email = Email;
            manager.Password = Password;
            manager.Surname = Surname;
            manager.Name = Name;
            manager.FatherName = FatherName;

            _context.managers.Add(manager);
            await _context.SaveChangesAsync();
            return Redirect("~/Admin/Managers");
        }

        [HttpPost]
        public async Task<IActionResult> EditManager(int id,string Email, string Password, string Surname, string Name, string FatherName)
        {
            Manager manager = new Manager();
            manager.Email = Email;
            manager.Password = Password;
            manager.Surname = Surname;
            manager.Name = Name;
            manager.FatherName = FatherName;
            manager.Id = id;

            _context.managers.Update(manager);
            await _context.SaveChangesAsync();
            return Redirect("~/Admin/Managers");
        }
    }
}
