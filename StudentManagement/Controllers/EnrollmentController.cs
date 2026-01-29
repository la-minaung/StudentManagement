using Microsoft.AspNetCore.Mvc;
using StudentManagement.DTOs;
using StudentManagement.Services;

namespace StudentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpPost]
        public async Task<ActionResult<EnrollmentDto>> Enroll(CreateEnrollmentDto createDto)
        {
            var enrollment = await _enrollmentService.EnrollStudent(createDto);

            if (enrollment == null)
            {              
                return BadRequest("Invalid Student/Course ID, or Student is already enrolled in this course.");
            }

            return Ok(enrollment);
        }

        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<List<EnrollmentDto>>> GetStudentSchedule(int studentId)
        {
            var enrollments = await _enrollmentService.GetStudentEnrollments(studentId);
            return Ok(enrollments);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Unenroll(int id)
        {
            var success = await _enrollmentService.UnenrollStudent(id);

            if (!success)
            {
                return NotFound("Enrollment record not found.");
            }

            return NoContent();
        }
    }
}
