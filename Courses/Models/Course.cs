namespace Courses.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ManagerId { get; set; }
        public Manager manager { get; set; }

        public double Cost { get; set; }
    }
}
