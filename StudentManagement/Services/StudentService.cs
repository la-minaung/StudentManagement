using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.DTOs;
using StudentManagement.Models;

namespace StudentManagement.Services
{
    public class StudentService : IStudentService
    {

        private readonly ApplicationDbContext _context;

        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StudentDto> AddStudent(CreateStudentDto newStudentDto)
        {
            var newStudent = new Student
            {
                FirstName = newStudentDto.FirstName,
                LastName = newStudentDto.LastName,
                Email = newStudentDto.Email,
                Age = newStudentDto.Age
            };

            await _context.Students.AddAsync(newStudent);
            await _context.SaveChangesAsync();

            return new StudentDto
            {
                Id = newStudent.Id,
                FullName = $"{newStudent.FirstName} {newStudent.LastName}",
                Email = newStudent.Email,
                Age = newStudent.Age
            };
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

            return students.Select(x => new StudentDto
            {
                Id = x.Id,
                FullName = $"{x.FirstName} {x.LastName}",
                Email = x.Email,
                Age = x.Age
            }).ToList();
        }

        public async Task<StudentDto?> GetStudentById(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null) return null;

            return new StudentDto
            {
                Id = student.Id,
                FullName = $"{student.FirstName} {student.LastName}",
                Age = student.Age,
                Email = student.Email
            };
        }

        public async Task<bool> UpdateStudent(int id, CreateStudentDto updateStudent)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            student.FirstName = updateStudent.FirstName;
            student.LastName = updateStudent.LastName;
            student.Email = updateStudent.Email;
            student.Age = updateStudent.Age;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
