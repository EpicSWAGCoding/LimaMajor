namespace Courses.Models
{
    public class UserCourse
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course course { get; set; }
        public int UserId { get; set; }
        public User user { get; set; }
    }
}
