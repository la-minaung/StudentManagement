using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public required string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public required string LastName { get; set;}

        [Required]
        [MaxLength(50)]
        public required string Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Specialization { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }
    }
}
