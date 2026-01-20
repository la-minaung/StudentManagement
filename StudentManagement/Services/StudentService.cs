using StudentManagement.DTOs;
using StudentManagement.Models;

namespace StudentManagement.Services
{
    public class StudentService : IStudentService
    {

        private static List<Student> _students = new List<Student>
        {
            new Student { Id = 1, FirstName = "John", LastName = "Doe", Age = 20, Email = "john@example.com" },
            new Student { Id = 2, FirstName = "Jane", LastName = "Smith", Age = 22, Email = "jane@example.com" }
        };

        public StudentDto AddStudent(CreateStudentDto newStudentDto)
        {
            var newId = _students.Count > 0 ? _students.Max(s => s.Id) + 1 : 1;
            var newStudent = new Student
            {
                Id = newId,
                FirstName = newStudentDto.FirstName,
                LastName = newStudentDto.LastName,
                Email = newStudentDto.Email,
                Age = newStudentDto.Age
            };

            _students.Add(newStudent);

            return new StudentDto
            {
                Id = newStudent.Id,
                FullName = $"{newStudent.FirstName} {newStudent.LastName}",
                Email = newStudent.Email,
                Age = newStudent.Age
            };
        }

        public bool DeleteStudent(int id)
        {
            var student = _students.FirstOrDefault(x => x.Id == id);
            if (student == null) return false;

            _students.Remove(student);
            return true;

        }

        public List<StudentDto> GetAllStudents()
        {
            return _students.Select(x => new StudentDto
            {
                Id = x.Id,
                FullName = $"{x.FirstName} {x.LastName}",
                Email = x.Email,
                Age = x.Age
            }).ToList();
        }

        public StudentDto? GetStudentById(int id)
        {
            var student = _students.FirstOrDefault(x => x.Id == id);
            if (student == null) return null;

            return new StudentDto
            {
                Id = student.Id,
                FullName = $"{student.FirstName} {student.LastName}",
                Age = student.Age,
                Email = student.Email
            };
        }

        public bool UpdateStudent(int id, CreateStudentDto updateStudent)
        {
            var student = _students.FirstOrDefault(x => x.Id == id);
            if (student == null) return false;

            student.FirstName = updateStudent.FirstName;
            student.LastName = updateStudent.LastName;
            student.Email = updateStudent.Email;
            student.Age = updateStudent.Age;
            return true;
        }
    }
}
