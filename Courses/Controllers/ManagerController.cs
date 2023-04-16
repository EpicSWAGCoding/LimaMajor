using Courses.Data;
using Courses.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace Courses.Controllers
{
    public class ManagerController : Controller
    {
        private readonly CoursesDbContext _context;
        IWebHostEnvironment _appEnvironment;

        public ManagerController(CoursesDbContext context,IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }
        public IActionResult Index()
        { //список созданных им курсов
           // List<Manager> managers = new List<Manager>();
            Manager manager = _context.managers.Where(a => a.Id == Convert.ToInt32(Request.Cookies["userId"])).FirstOrDefault(); //userId
            List<Course> courses = _context.courses.Where(a=>a.ManagerId==manager.Id).ToList();
            ViewData["course"] = courses;
            double AllSum = 0;
            List<Course> courseCost = new List<Course>();
            foreach(var item in courses)
            {
                List<UserCourse> userCourses = _context.userCourses.Where(a => a.CourseId==item.Id).ToList();
                AllSum += item.Cost * userCourses.Count;
                Course tempcourse = new Course();
                tempcourse.Id = item.Id;
                tempcourse.Cost = item.Cost * userCourses.Count;
                tempcourse.Title = item.Title;
                courseCost.Add(tempcourse);
            }
            ViewData["allCost"] = AllSum;
            ViewData["courseCost"] = courseCost;
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse(string Title,string Description,int Cost)
        {
            Course course = new Course();
            course.Title = Title;
            course.Description = Description;
            course.Cost = Cost;
            course.ManagerId = Convert.ToInt32(Request.Cookies["userId"]);
            _context.courses.Add(course);
            await _context.SaveChangesAsync();
            return Redirect("~/Manager/Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditCourse(int id, string Title, string Description, int Cost)
        {
            Course course = new Course();
            course.Id= id;
            course.Title = Title;
            course.Description = Description;
            course.Cost = Cost;
            course.ManagerId = Convert.ToInt32(Request.Cookies["userId"]);
            _context.courses.Update(course);
            await _context.SaveChangesAsync();
            return Redirect("~/Manager/Index");
        }

        public IActionResult Detail(int id)
        {
            List<Lesson> lessons = _context.lessons.Where(a => a.CourseId == id).ToList();
            Course course = _context.courses.Where(a=>a.Id== id).FirstOrDefault();
            List<Image> images = _context.images.ToList();
            List<Video> videos = _context.videos.ToList();
            List<Content> contents = _context.contents.ToList();

            ViewData["course"] = course;
            ViewData["lesson"] = lessons;
            ViewData["image"] = images;
            ViewData["video"] = videos;
            ViewData["content"] = contents;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCourse(int id)
        { // модалка 
            Course course = new Course();
            course.Id = id;
            _context.courses.Remove(course);
            await _context.SaveChangesAsync();
            return Redirect("~/Manager/Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddLesson(int CourseId, string Title,string Content,IFormFile Video,IFormFile Image)
        { //добавление урока в курс
            Lesson lesson = new Lesson();
            Image image = new Image();
            List<Image> listImage = new List<Image>();
            Video video = new Video();
            Content content = new Content();

            lesson.Title= Title;
            lesson.CourseId = CourseId;
            
            _context.lessons.Add(lesson);
            await _context.SaveChangesAsync();

            int maxLesson= _context.lessons.Max(a => a.Id);
            int maxImgId = _context.images.Max(a=>a.Id);

            var filePathVideo = "/video/"+String.Concat(Video.FileName.Where(a=>!Char.IsWhiteSpace(a)));
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath+filePathVideo,FileMode.Create))
            {
                await Video.CopyToAsync(fileStream);
            }

            var filePathImg = "/img/" + String.Concat(Image.FileName.Where(a => !Char.IsWhiteSpace(a)));
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + filePathImg, FileMode.Create))
            {
                await Image.CopyToAsync(fileStream);
            }

            image.FileName = filePathImg;
            image.LessonId = maxLesson;
            _context.images.Add(image);
            maxImgId++;
            

            content.ContentText = Content;
            content.LessonId= maxLesson;
            _context.contents.Add(content);

            video.FileName = filePathVideo;
            video.LessonId = maxLesson;
            _context.videos.Add(video);

            await _context.SaveChangesAsync();
            string url = CourseId.ToString();
            return Redirect("~/Manager/Detail?id="+url);
        }

        [HttpPost]
        public async Task<IActionResult> EditLesson(int id,int CourseId, string Title, string Content, IFormFile VideoEdit, IFormFile ImageEdit)
        { //добавление урока в курс
            Lesson lesson = new Lesson();
            Image image = new Image();
            List<Image> listImage = new List<Image>();
            Video video = new Video();
            Content content = new Content();

            lesson.Title = Title;
            lesson.CourseId = CourseId;
            lesson.Id= id;

            _context.lessons.Update(lesson);
            await _context.SaveChangesAsync();
            if(VideoEdit!=null)
            {
                var filePathVideo = "/video/" + String.Concat(VideoEdit.FileName.Where(a => !Char.IsWhiteSpace(a)));
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + filePathVideo, FileMode.Create))
                {
                    await VideoEdit.CopyToAsync(fileStream);
                }
                video.FileName = filePathVideo;
                video.LessonId = id;
                _context.videos.Update(video);
            }
            if(ImageEdit!=null)
            {
                var filePathImg = "/img/" + String.Concat(ImageEdit.FileName.Where(a => !Char.IsWhiteSpace(a)));
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + filePathImg, FileMode.Create))
                {
                    await ImageEdit.CopyToAsync(fileStream);
                }
                image.FileName = filePathImg;
                image.LessonId = id;
                _context.images.Update(image);
            }

            if(Content!=null)
            {
            content.ContentText = Content;
            content.LessonId = id;
            _context.contents.Update(content);
            }

            await _context.SaveChangesAsync();
            string url = CourseId.ToString();
            return Redirect("~/Manager/Detail?id=" + url);
        }

        
        public async Task<IActionResult> LessonDelete(int id)
        { //добавление урока в курс
            Lesson lesson = new Lesson();
            lesson.Id = id;
            _context.lessons.Remove(lesson);
            await _context.SaveChangesAsync();
            return Redirect("~/Manager/Index");
        }
    }
}
