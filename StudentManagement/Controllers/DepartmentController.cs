using Microsoft.AspNetCore.Mvc;
using StudentManagement.DTOs;
using StudentManagement.Services;

namespace StudentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _departmentService.GetAllDepartments();
            return Ok(departments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var department = await _departmentService.GetDepartmentById(id);
            if (department == null)
            {
                return NotFound($"Department with ID {id} not found.");
            }

            return Ok(department);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentDto createDto)
        {
            var createdDepartment = await _departmentService.AddDepartment(createDto);
            return CreatedAtAction(nameof(GetById), new { id = createdDepartment.Id }, createdDepartment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateDepartmentDto updateDto)
        {
            var updateDepartment = await _departmentService.UpdateDepartment(id, updateDto);

            if (updateDepartment == null)
            {
                return NotFound($"Department with ID {id} not found.");
            }

            return Ok(updateDepartment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _departmentService.DeleteDepartment(id);

            if (!isDeleted)
            {
                return NotFound($"Department with ID {id} not found.");
            }

            return Ok("Department deleted successfully.");
        }
    }
}
