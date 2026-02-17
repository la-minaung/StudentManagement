using Microsoft.AspNetCore.Mvc;
using StudentManagement.DTOs;
using StudentManagement.Helpers;
using StudentManagement.Services;

namespace StudentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] TeacherQueryObject query)
        {
            var teachers = await _teacherService.GetAllTeachers(query);
            return Ok(teachers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var teacher = await _teacherService.GetTeacherById(id);

            if (teacher == null)
            {
                return NotFound($"Teacher with ID {id} not found.");
            }

            return Ok(teacher);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTeacherDto createDto)
        {
            var createdTeacher = await _teacherService.AddTeacher(createDto);

            if (createdTeacher == null)
            {
                return BadRequest($"Department with ID {createDto.DepartmentId} does not exist.");
            }

            return CreatedAtAction(nameof(GetById), new { id = createdTeacher.Id }, createdTeacher);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateTeacherDto updateDto)
        {
            var updatedTeacher = await _teacherService.UpdateTeacher(id, updateDto);

            if (updatedTeacher == null)
            {
                return NotFound($"Teacher with ID {id} not found or Department ID is invalid.");
            }

            return Ok(updatedTeacher);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _teacherService.DeleteTeacher(id);

            if (!isDeleted)
            {
                return NotFound($"Teacher with ID {id} not found.");
            }

            return Ok("Teacher deleted successfully.");
        }
    }
}
