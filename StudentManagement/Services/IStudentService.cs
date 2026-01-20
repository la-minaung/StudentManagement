using StudentManagement.DTOs;

namespace StudentManagement.Services
{
    public interface IStudentService
    {
        List<StudentDto> GetAllStudents();
        StudentDto? GetStudentById(int id);
        StudentDto AddStudent(CreateStudentDto newStudent);
        bool UpdateStudent(int id, CreateStudentDto updateStudent);
        bool DeleteStudent(int id);
    }
}
