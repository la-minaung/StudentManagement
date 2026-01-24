using Microsoft.AspNetCore.Mvc;
using StudentManagement.DTOs;
using StudentManagement.Services;

namespace StudentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {

        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<ActionResult<List<StudentDto>>> GetAll()
        {
            var students = await _studentService.GetAllStudents();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetById(int id)
        {
            var student = await _studentService.GetStudentById(id);

            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            return Ok(student);
        }

        [HttpPost]
        public async Task<ActionResult<StudentDto>> Create(CreateStudentDto createStudentDto)
        {
            var createdStudent = await _studentService.AddStudent(createStudentDto);
            return CreatedAtAction(nameof(GetById), new { id = createdStudent.Id }, createdStudent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateStudentDto updatedStudent)
        {
            var result = await _studentService.UpdateStudent(id, updatedStudent);
            if (!result)
            {
                return NotFound($"Cannot update. Student with ID {id} not found.");
            }

            return Ok("Student updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _studentService.DeleteStudent(id);
            if (!result)
            {
                return NotFound($"Cannot delete. Student with ID {id} not found.");
            }
            return Ok("Student deleted successfully.");
        }
    }
}
