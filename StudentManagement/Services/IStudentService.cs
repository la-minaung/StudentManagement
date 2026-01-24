using StudentManagement.DTOs;

namespace StudentManagement.Services
{
    public interface IStudentService
    {
        Task<List<StudentDto>> GetAllStudents();
        Task<StudentDto?> GetStudentById(int id);
        Task<StudentDto> AddStudent(CreateStudentDto newStudent);
        Task<bool> UpdateStudent(int id, CreateStudentDto updateStudent);
        Task<bool> DeleteStudent(int id);
    }
}
