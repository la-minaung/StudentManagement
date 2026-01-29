using System.ComponentModel.DataAnnotations;

namespace StudentManagement.DTOs
{
    public class CreateCourseDto
    {
        [Required]
        [MaxLength(100)]
        public required string Title { get; set; }

        [Range(1, 10)]
        public int Credits { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public int TeacherId { get; set; }
    }
}
