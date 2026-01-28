using System.ComponentModel.DataAnnotations;

namespace StudentManagement.DTOs
{
    public class CreateDepartmentDto
    {
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [MaxLength(10)]
        public string? Code { get; set; }
    }
}
