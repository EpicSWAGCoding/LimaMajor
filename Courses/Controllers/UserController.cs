using Courses.Data;
using Courses.Models;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers
{
    public class UserController : Controller
    {
        private readonly CoursesDbContext _context;

        public UserController(CoursesDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        { // список курсов (accordion) добавить кнопку просмотреть курс (изучать)
            List<int> courseList = _context.userCourses.Where(x => x.UserId == 1).Select(a=>a.CourseId).ToList();
            List<Course> courses= new List<Course>();
            foreach (var course in courseList)
            {
                courses.Add(_context.courses.Where(a=>a.Id==course).FirstOrDefault());
            }
            ViewData["courses"] = courses;
            return View();
            //return Redirect("~/User/LearnCourse");
        }

        public IActionResult LearnCourse(int id)
        { //изучать
           Course courses = _context.courses.Where(a=>a.Id==id).FirstOrDefault(); //by id
           List<Lesson> lessons = _context.lessons.Where(a => a.CourseId == id).ToList();
            List<Image> images = _context.images.ToList();
            List<Video> videos = _context.videos.ToList();
            List<Content> contents = _context.contents.ToList();
            ViewData["course"] = courses;
            ViewData["lesson"] = lessons;
            ViewData["image"] = images;
            ViewData["video"] = videos;
            ViewData["content"] = contents;
            return View();
        }

        public IActionResult ListCourses()
        { //кнопкка купить
            List<Course> courses = new List<Course>(); 
            List<Lesson> lessons = new List<Lesson>(); //чисто тайтлы (accordion) и выпадает кнопка купить

            courses = _context.courses.ToList();
            lessons = _context.lessons.ToList();
            return View();
        }

        public IActionResult PayCourse(int courseId)
        {
            //для подтверждения оплаты введите пароль
            //ваши средства
            Wallet wallets = _context.wallets.Where(a=>a.UserId==1).FirstOrDefault();
            Course course = _context.courses.Where(a => a.Id == courseId).FirstOrDefault();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Paying(int money,string password) //walletId
        {
            if(_context.users.Any(a=>a.Id==1&&a.Password==password))
            {
                Wallet wallet = _context.wallets.Where(a => a.UserId == 1).FirstOrDefault(); //userId
                                                                                             //wallet.Money = 
                wallet.Money = wallet.Money - money;
                _context.wallets.Update(wallet);
                await _context.SaveChangesAsync();
                return Redirect("~/User/SuccessPay");
            } else
            {
                return Redirect("~/User/ErrorPay");
            }
        }

        public IActionResult SuccessPay()
        {
            return View();
        }
        //ошибка неверный пароль
        public IActionResult ErrorPay()
        {
            return View();
        }
    }
}
