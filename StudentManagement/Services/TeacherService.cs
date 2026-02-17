using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.DTOs;
using StudentManagement.Helpers;
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

        public async Task<List<TeacherDto>> GetAllTeachers(TeacherQueryObject query)
        {

            // 1. Start with Queryable + Include
            var teachers = _context.Teachers
                .Include(t => t.Department)
                .AsQueryable();

            // 2. Filter by Department (if provided)
            if (query.DepartmentId.HasValue)
            {
                teachers = teachers.Where(t => t.DepartmentId == query.DepartmentId.Value);
            }

            // 3. Search (Name, Specialization, or Department Name)
            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                var lowerSearch = query.SearchTerm.ToLower();
                teachers = teachers.Where(t =>
                    t.FirstName.ToLower().Contains(lowerSearch) ||
                    t.LastName.ToLower().Contains(lowerSearch) ||
                    t.Specialization.ToLower().Contains(lowerSearch) ||
                    t.Department.Name.ToLower().Contains(lowerSearch));
            }

            // 4. Sorting
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    teachers = query.IsDescending
                        ? teachers.OrderByDescending(t => t.FirstName)
                        : teachers.OrderBy(t => t.FirstName);
                }
                else if (query.SortBy.Equals("Specialization", StringComparison.OrdinalIgnoreCase))
                {
                    teachers = query.IsDescending
                        ? teachers.OrderByDescending(t => t.Specialization)
                        : teachers.OrderBy(t => t.Specialization);
                }
            }

            // 5. Pagination
            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            var result = await teachers
                .Skip(skipNumber)
                .Take(query.PageSize)
                .ToListAsync();

            return _mapper.Map<List<TeacherDto>>(result);
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
