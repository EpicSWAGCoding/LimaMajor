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
        {
            List<int> courseList = _context.userCourses.Where(x => x.UserId ==Convert.ToInt32(Request.Cookies["userId"])).Select(a=>a.CourseId).ToList();
            List<Course> courses= new List<Course>();
            foreach (var course in courseList)
            {
                courses.Add(_context.courses.Where(a=>a.Id==course).FirstOrDefault());
            }
            Wallet wallet = _context.wallets.Where(a => a.UserId == Convert.ToInt32(Request.Cookies["userId"])).FirstOrDefault(); //userId
            ViewData["wallet"] = wallet;
            ViewData["courses"] = courses;
            return View();
        }

        public IActionResult LearnCourse(int id)
        {
            Course courses = _context.courses.Where(a=>a.Id==id).FirstOrDefault();
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
            List<Lesson> lessons = new List<Lesson>();

            lessons = _context.lessons.ToList();

          
            foreach(var item in _context.courses.Select(a=>a.Id).ToList())
            {
                if (!_context.userCourses.Any(a => a.CourseId == item && a.UserId == Convert.ToInt32(Request.Cookies["userId"])))
                {
                    courses.Add(_context.courses.Where(a=>a.Id==item).FirstOrDefault());
                }
            }

            Wallet wallets = _context.wallets.Where(a => a.UserId == Convert.ToInt32(Request.Cookies["userId"])).FirstOrDefault();

            ViewData["courses"] = courses;
            ViewData["lesson"] = lessons;
            ViewData["wallet"] = wallets;
            return View();
        }

        public IActionResult PayCourse(int courseId)
        {
            //для подтверждения оплаты введите пароль
            //ваши средства
            Wallet wallets = _context.wallets.Where(a=>a.UserId==Convert.ToInt32(Request.Cookies["userId"])).FirstOrDefault();
            Course course = _context.courses.Where(a => a.Id == courseId).FirstOrDefault();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Paying(int CourseId, string password)
        {
            Course course = _context.courses.Where(a=>a.Id==CourseId).FirstOrDefault();
            if (_context.users.Any(a => a.Id == Convert.ToInt32(Request.Cookies["userId"])&&a.Password==password))
            {
                Wallet wallet = _context.wallets.Where(a => a.UserId == Convert.ToInt32(Request.Cookies["userId"])).FirstOrDefault(); //userId
                if(wallet.Money<course.Cost)
                {
                    return Redirect("~/User/ErrorPay");
                } else
                {
                    wallet.Money = wallet.Money - course.Cost;
                    _context.wallets.Update(wallet);

                    UserCourse userCoursse = new UserCourse();
                    userCoursse.UserId = Convert.ToInt32(Request.Cookies["userId"]);
                    userCoursse.CourseId = CourseId;
                    _context.userCourses.Add(userCoursse);
                    await _context.SaveChangesAsync();
                    return Redirect("~/User/SuccessPay");
                }             
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

        public async Task<IActionResult> AddMoneyWallet(int sum, string password)
        {
            if (_context.users.Any(a => a.Id == Convert.ToInt32(Request.Cookies["userId"]) && a.Password == password))
            {
                Wallet wallet = _context.wallets.Where(a => a.UserId == Convert.ToInt32(Request.Cookies["userId"])).FirstOrDefault();
                wallet.Money += sum;
                _context.wallets.Update(wallet);
                await _context.SaveChangesAsync();
                return Redirect("~/User/SuccessPay");
            }
            else
            {
                return Redirect("~/User/ErrorPay");
            }

        }
    }
}
