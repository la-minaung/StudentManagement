using System.ComponentModel.DataAnnotations;

namespace StudentManagement.DTOs
{
    public class CreateTeacherDto
    {
        [Required]
        [MaxLength(50)]
        public required string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public required string LastName { get; set;}

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Specialization { get; set; }

        [Required]
        public int DepartmentId { get; set; }
    }
}
