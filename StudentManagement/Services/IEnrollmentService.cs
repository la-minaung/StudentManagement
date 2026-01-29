using StudentManagement.DTOs;

namespace StudentManagement.Services
{
    public interface IEnrollmentService
    {
        Task<EnrollmentDto?> EnrollStudent(CreateEnrollmentDto createDto);
        Task<List<EnrollmentDto>> GetStudentEnrollments(int studentId);
        Task<bool> UnenrollStudent(int id);
    }
}
