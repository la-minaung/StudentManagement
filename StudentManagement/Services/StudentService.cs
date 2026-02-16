using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.DTOs;
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

        public async Task<List<StudentDto>> GetAllStudents()
        {
            var students = await _context.Students.AsNoTracking().ToListAsync();

            return _mapper.Map<List<StudentDto>>(students);
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
