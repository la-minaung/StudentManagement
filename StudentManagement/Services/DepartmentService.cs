using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.DTOs;
using StudentManagement.Models;

namespace StudentManagement.Services
{
    public class DepartmentService : IDepartmentService
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DepartmentService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DepartmentDto> AddDepartment(CreateDepartmentDto createDto)
        {
            var department = _mapper.Map<Department>(createDto);

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task<List<DepartmentDto>> GetAllDepartments()
        {
            var departments = await _context.Departments.AsNoTracking().ToListAsync();

            return _mapper.Map<List<DepartmentDto>>(departments);
        }

        public async Task<DepartmentDto?> GetDepartmentById(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null) return null;

            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task<DepartmentDto?> UpdateDepartment(int id, CreateDepartmentDto updateDto)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null) return null;

            _mapper.Map(updateDto, department);

            _context.Departments.Update(department);
            await _context.SaveChangesAsync();

            return _mapper.Map<DepartmentDto>(department);
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
