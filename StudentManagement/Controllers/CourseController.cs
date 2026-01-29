using Microsoft.AspNetCore.Mvc;
using StudentManagement.DTOs;
using StudentManagement.Services;

namespace StudentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }


        [HttpGet]
        public async Task<ActionResult<List<CourseDto>>> GetAll()
        {
            var courses = await _courseService.GetAllCourses();
            return Ok(courses);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetById(int id)
        {
            var course = await _courseService.GetCourseById(id);

            if (course == null)
            {
                return NotFound($"Course with ID {id} not found.");
            }

            return Ok(course);
        }


        [HttpPost]
        public async Task<ActionResult<CourseDto>> Create(CreateCourseDto createDto)
        {

            var createdCourse = await _courseService.AddCourse(createDto);

            if (createdCourse == null)
            {
                return BadRequest("Invalid Department ID or Teacher ID.");
            }

            return CreatedAtAction(nameof(GetById), new { id = createdCourse.Id }, createdCourse);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<CourseDto>> Update(int id, CreateCourseDto updateDto)
        {
            var updatedCourse = await _courseService.UpdateCourse(id, updateDto);

            if (updatedCourse == null)
            {
                return NotFound($"Course with ID {id} not found, or invalid Department/Teacher ID.");
            }

            return Ok(updatedCourse);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _courseService.DeleteCourse(id);

            if (!isDeleted)
            {
                return NotFound($"Course with ID {id} not found.");
            }

            return NoContent();
        }
    }
}
