namespace Courses.Models
{
    public class Content
    {
        public int Id { get; set; }
        public string ContentText { get; set; }
        public int LessonId { get; set; }
        public Lesson lesson { get; set; }
    }
}
