using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.DTOs;
using StudentManagement.Models;

namespace StudentManagement.Services
{
    public class TeacherService : ITeacherService
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TeacherService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TeacherDto?> AddTeacher(CreateTeacherDto createDto)
        {
            var departmentExists = await _context.Departments.AnyAsync(d => d.Id == createDto.DepartmentId);
            if (!departmentExists)
            {
                return null;
            }

            var teacher = _mapper.Map<Teacher>(createDto);

            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();

            // load full department object
            await _context.Entry(teacher).Reference(t => t.Department).LoadAsync();

            var departmentName = await _context.Departments
                .Where(d => d.Id == createDto.DepartmentId)
                .Select(d => d.Name)
                .FirstOrDefaultAsync();

            return _mapper.Map<TeacherDto>(teacher);
        }

        public async Task<List<TeacherDto>> GetAllTeachers()
        {
            var teachers = await _context.Teachers
                .Include(t => t.Department)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<TeacherDto>>(teachers);
        }

        public async Task<TeacherDto?> GetTeacherById(int id)
        {
            var teacher = await _context.Teachers
                .Include(t => t.Department)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null) return null;

            return _mapper.Map<TeacherDto>(teacher);
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

            _mapper.Map(updateDto, teacher);

            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();

            await _context.Entry(teacher).Reference(t => t.Department).LoadAsync();

            return _mapper.Map<TeacherDto>(teacher);
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
