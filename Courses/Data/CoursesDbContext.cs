using Courses.Models;
using Microsoft.EntityFrameworkCore;

namespace Courses.Data
{
    public class CoursesDbContext : DbContext
    {
        public CoursesDbContext(DbContextOptions<CoursesDbContext> options) : base(options) { }

        //поля из таблиц
        public DbSet<Admin> admins { get; set; }
        public DbSet<Course> courses{ get; set; }
        public DbSet<Lesson> lessons{ get; set; }
        public DbSet<Manager> managers { get; set; }
        public DbSet<User> users { get;set; }
        public DbSet<Wallet> wallets { get; set; }
    }
}
