using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.DTOs;
using StudentManagement.Models;

namespace StudentManagement.Services
{
    public class CourseService : ICourseService
    {
        private readonly ApplicationDbContext _context;

        public CourseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CourseDto>> GetAllCourses()
        {
            var courses = await _context.Courses
                .Include(c => c.Department)
                .Include(c => c.Teacher)
                .AsNoTracking()
                .ToListAsync();

            return courses.Select(c => MapToDto(c)).ToList();
        }

        public async Task<CourseDto?> GetCourseById(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Department)
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null) return null;

            return MapToDto(course);
        }

        public async Task<CourseDto?> AddCourse(CreateCourseDto createDto)
        {
            // 1. Validate Department
            var deptExists = await _context.Departments.AnyAsync(d => d.Id == createDto.DepartmentId);
            if (!deptExists) return null;

            // 2. Validate Teacher
            var teacherExists = await _context.Teachers.AnyAsync(t => t.Id == createDto.TeacherId);
            if (!teacherExists) return null;

            // 3. Create Entity
            var course = new Course
            {
                Title = createDto.Title,
                Credits = createDto.Credits,
                DepartmentId = createDto.DepartmentId,
                TeacherId = createDto.TeacherId
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return await GetCourseById(course.Id);
        }

        public async Task<CourseDto?> UpdateCourse(int id, CreateCourseDto updateDto)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return null;

            // Validate Foreign Keys if they changed
            if (course.DepartmentId != updateDto.DepartmentId)
            {
                if (!await _context.Departments.AnyAsync(d => d.Id == updateDto.DepartmentId)) return null;
            }
            if (course.TeacherId != updateDto.TeacherId)
            {
                if (!await _context.Teachers.AnyAsync(t => t.Id == updateDto.TeacherId)) return null;
            }

            course.Title = updateDto.Title;
            course.Credits = updateDto.Credits;
            course.DepartmentId = updateDto.DepartmentId;
            course.TeacherId = updateDto.TeacherId;

            _context.Courses.Update(course);
            await _context.SaveChangesAsync();

            return await GetCourseById(id);
        }

        public async Task<bool> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return false;

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }

        private static CourseDto MapToDto(Course course)
        {
            return new CourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Credits = course.Credits,
                DepartmentId = course.DepartmentId,
                DepartmentName = course.Department?.Name ?? "Unknown",
                TeacherId = course.TeacherId,
                TeacherName = (course.Teacher?.FirstName + " " + course.Teacher?.LastName).Trim()
            };
        }
    }
}
