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
        public ActionResult<List<StudentDto>> GetAll()
        {
            var students = _studentService.GetAllStudents();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public ActionResult<StudentDto> GetById(int id)
        {
            var student = _studentService.GetStudentById(id);

            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            return Ok(student);
        }

        [HttpPost]
        public ActionResult<StudentDto> Create(CreateStudentDto createStudentDto)
        {
            var createdStudent = _studentService.AddStudent(createStudentDto);
            return CreatedAtAction(nameof(GetById), new { id = createdStudent.Id }, createdStudent);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CreateStudentDto updatedStudent)
        {
            var result = _studentService.UpdateStudent(id, updatedStudent);
            if (!result)
            {
                return NotFound($"Cannot update. Student with ID {id} not found.");
            }

            return Ok("Student updated successfully.");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _studentService.DeleteStudent(id);
            if (!result)
            {
                return NotFound($"Cannot delete. Student with ID {id} not found.");
            }
            return Ok("Student deleted successfully.");
        }
    }
}
