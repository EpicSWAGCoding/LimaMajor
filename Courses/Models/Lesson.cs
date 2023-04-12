namespace Courses.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string Video { get; set; }
        public int LessNum { get; set; }
        public int CourseId { get; set; }
        public Course course { get; set; }
    }
}
