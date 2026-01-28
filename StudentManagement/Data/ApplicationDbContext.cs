using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;

namespace StudentManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options)
        { 

        }   

        public DbSet<Student> Students { get; set; } 
        public DbSet<Department> Departments { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
    }
}
