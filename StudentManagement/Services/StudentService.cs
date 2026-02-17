using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.DTOs;
using StudentManagement.Helpers;
using StudentManagement.Models;

namespace StudentManagement.Services
{
    public class StudentService : IStudentService
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public StudentService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<StudentDto> AddStudent(CreateStudentDto newStudentDto)
        {
            var newStudent = _mapper.Map<Student>(newStudentDto);

            await _context.Students.AddAsync(newStudent);
            await _context.SaveChangesAsync();

            return _mapper.Map<StudentDto>(newStudent);
        }

        public async Task<bool> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            _context.Students.Remove(student);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<StudentDto>> GetAllStudents(StudentQueryObject query)
        {

            // 1. Start with AsQueryable
            var students = _context.Students.AsQueryable();

            // 2. Filtering
            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                var lowerSearch = query.SearchTerm.ToLower();
                students = students.Where(s =>
                    s.FirstName.ToLower().Contains(lowerSearch) ||
                    s.LastName.ToLower().Contains(lowerSearch) ||
                    s.Email.ToLower().Contains(lowerSearch));
            }

            // 3. Sorting
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    students = query.IsDescending
                        ? students.OrderByDescending(s => s.FirstName)
                        : students.OrderBy(s => s.FirstName);
                }
                else if (query.SortBy.Equals("Age", StringComparison.OrdinalIgnoreCase))
                {
                    students = query.IsDescending
                        ? students.OrderByDescending(s => s.Age)
                        : students.OrderBy(s => s.Age);
                }
            }

            // 4. Pagination
            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            var result = await students
                .Skip(skipNumber)
                .Take(query.PageSize)
                .ToListAsync();

            return _mapper.Map<List<StudentDto>>(result);
        }

        public async Task<StudentDto?> GetStudentById(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null) return null;

            return _mapper.Map<StudentDto>(student);
        }

        public async Task<bool> UpdateStudent(int id, CreateStudentDto updateStudent)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            _mapper.Map(updateStudent, student);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
