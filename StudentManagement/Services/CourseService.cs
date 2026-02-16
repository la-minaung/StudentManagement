using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.DTOs;
using StudentManagement.Models;

namespace StudentManagement.Services
{
    public class CourseService : ICourseService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CourseService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CourseDto>> GetAllCourses()
        {
            var courses = await _context.Courses
                .Include(c => c.Department)
                .Include(c => c.Teacher)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<CourseDto>>(courses);
        }

        public async Task<CourseDto?> GetCourseById(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Department)
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null) return null;

            return _mapper.Map<CourseDto>(course);
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
            var course = _mapper.Map<Course>(createDto);

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

            _mapper.Map(updateDto, course);

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

    }
}
