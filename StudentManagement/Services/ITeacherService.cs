using StudentManagement.DTOs;
using StudentManagement.Helpers;

namespace StudentManagement.Services
{
    public interface ITeacherService
    {
        Task<List<TeacherDto>> GetAllTeachers(TeacherQueryObject query);
        Task<TeacherDto?> GetTeacherById(int id);
        Task<TeacherDto?> AddTeacher(CreateTeacherDto createDto);
        Task<TeacherDto> UpdateTeacher(int id, CreateTeacherDto updateDto);
        Task<bool> DeleteTeacher(int id);
    }
}