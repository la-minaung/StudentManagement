using StudentManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace StudentManagement.DTOs
{
    public class CreateEnrollmentDto
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        public Grade? Grade { get; set; }
    }
}
