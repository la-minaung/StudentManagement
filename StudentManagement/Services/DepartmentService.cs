using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.DTOs;
using StudentManagement.Models;

namespace StudentManagement.Services
{
    public class DepartmentService : IDepartmentService
    {

        private readonly ApplicationDbContext _context;

        public DepartmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DepartmentDto> AddDepartment(CreateDepartmentDto createDto)
        {
            var department = new Department
            {
                Name = createDto.Name,
                Code = createDto.Code
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                Code = department.Code
            };
        }

        public async Task<List<DepartmentDto>> GetAllDepartments()
        {
            var departments = await _context.Departments.AsNoTracking().ToListAsync();

            return departments.Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                Code = d.Code
            }).ToList();
        }

        public async Task<DepartmentDto?> GetDepartmentById(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null) return null;

            return new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                Code = department.Code
            };
        }

        public async Task<DepartmentDto?> UpdateDepartment(int id, CreateDepartmentDto updateDto)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null) return null;

            department.Name = updateDto.Name;
            department.Code = updateDto.Code;

            _context.Departments.Update(department);
            await _context.SaveChangesAsync();

            return new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                Code = department.Code
            };
        }

        public async Task<bool> DeleteDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null) return false;

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
