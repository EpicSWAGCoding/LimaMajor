namespace Courses.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public int LessonId { get; set; }
        public Lesson lesson { get; set; }
    }
}
