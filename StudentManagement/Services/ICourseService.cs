using StudentManagement.DTOs;

namespace StudentManagement.Services
{
    public interface ICourseService
    {
        Task<List<CourseDto>> GetAllCourses();
        Task<CourseDto?> GetCourseById(int id);
        Task<CourseDto?> AddCourse(CreateCourseDto createDto);
        Task<CourseDto?> UpdateCourse(int id, CreateCourseDto updateDto);
        Task<bool> DeleteCourse(int id);
    }
}
