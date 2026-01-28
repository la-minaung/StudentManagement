using StudentManagement.DTOs;
using StudentManagement.Models;

namespace StudentManagement.Services
{
    public interface IDepartmentService
    {
        Task<List<DepartmentDto>> GetAllDepartments();
        Task<DepartmentDto?> GetDepartmentById(int id);
        Task<DepartmentDto> AddDepartment(CreateDepartmentDto createDto);
        Task<DepartmentDto?> UpdateDepartment(int id, CreateDepartmentDto updateDto);
        Task<bool> DeleteDepartment(int id);
    }
}
