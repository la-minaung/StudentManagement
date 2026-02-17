using StudentManagement.Models;

namespace StudentManagement.DTOs
{
    public class CreateEnrollmentDto
    {
        public int StudentId { get; set; }

        public int CourseId { get; set; }

        public Grade? Grade { get; set; }
    }
}
