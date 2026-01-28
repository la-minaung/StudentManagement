using StudentManagement.DTOs;

namespace StudentManagement.Services
{
    public interface ITeacherService
    {
        Task<List<TeacherDto>> GetAllTeachers();
        Task<TeacherDto?> GetTeacherById(int id);
        Task<TeacherDto?> AddTeacher(CreateTeacherDto createDto);
        Task<TeacherDto> UpdateTeacher(int id, CreateTeacherDto updateDto);
        Task<bool> DeleteTeacher(int id);
    }
}