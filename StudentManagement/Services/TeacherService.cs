using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.DTOs;
using StudentManagement.Models;

namespace StudentManagement.Services
{
    public class TeacherService : ITeacherService
    {

        private readonly ApplicationDbContext _context;

        public TeacherService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TeacherDto?> AddTeacher(CreateTeacherDto createDto)
        {
            var departmentExists = await _context.Departments.AnyAsync(d => d.Id == createDto.DepartmentId);
            if (!departmentExists)
            {
                return null;
            }

            var teacher = new Teacher
            {
                FirstName = createDto.FirstName,
                LastName = createDto.LastName,
                Email = createDto.Email,
                PhoneNumber = createDto.PhoneNumber,
                Specialization = createDto.Specialization,
                DepartmentId = createDto.DepartmentId
            };

            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();

            var departmentName = await _context.Departments
                .Where(d => d.Id == createDto.DepartmentId)
                .Select(d => d.Name)
                .FirstOrDefaultAsync();

            return new TeacherDto
            {
                Id = teacher.Id,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Email = teacher.Email,
                PhoneNumber = teacher.PhoneNumber,
                Specialization = teacher.Specialization,
                DepartmentId = teacher.DepartmentId,
                DepartmentName = departmentName ?? "Unknown"
            };
        }

        public async Task<List<TeacherDto>> GetAllTeachers()
        {
            var teachers = await _context.Teachers
                .Include(t => t.Department)
                .AsNoTracking()
                .ToListAsync();

            return teachers.Select(t => new TeacherDto
            {
                Id = t.Id,
                FirstName = t.FirstName,
                LastName = t.LastName,
                Email = t.Email,
                PhoneNumber = t.PhoneNumber,
                Specialization = t.Specialization,
                DepartmentId = t.DepartmentId,
                DepartmentName = t.Department?.Name ?? "Unknown"
            }).ToList();
        }

        public async Task<TeacherDto?> GetTeacherById(int id)
        {
            var teacher = await _context.Teachers
                .Include(t => t.Department)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null) return null;

            return new TeacherDto
            {
                Id = teacher.Id,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Email = teacher.Email,
                PhoneNumber = teacher.PhoneNumber,
                Specialization = teacher.Specialization,
                DepartmentId = teacher.DepartmentId,
                DepartmentName = teacher.Department?.Name ?? "Unknown"
            };
        }

        public async Task<TeacherDto> UpdateTeacher(int id, CreateTeacherDto updateDto)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return null;

            if (teacher.DepartmentId != updateDto.DepartmentId)
            {
                var departmentExists = await _context.Departments.AnyAsync(d => d.Id == updateDto.DepartmentId);
                if (!departmentExists) return null;
            }

            teacher.FirstName = updateDto.FirstName;
            teacher.LastName = updateDto.LastName;
            teacher.Email = updateDto.Email;
            teacher.PhoneNumber = updateDto.PhoneNumber;
            teacher.Specialization = updateDto.Specialization;
            teacher.DepartmentId = updateDto.DepartmentId;

            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();

            var departmentName = await _context.Departments
                .Where(d => d.Id == teacher.DepartmentId)
                .Select(d => d.Name)
                .FirstOrDefaultAsync();

            return new TeacherDto
            {
                Id = teacher.Id,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Email = teacher.Email,
                PhoneNumber = teacher.PhoneNumber,
                Specialization = teacher.Specialization,
                DepartmentId = teacher.DepartmentId,
                DepartmentName = departmentName ?? "Unknown"
            };
        }

        public async Task<bool> DeleteTeacher(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return false;

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
