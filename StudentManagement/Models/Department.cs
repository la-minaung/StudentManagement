using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [MaxLength(20)]
        public string? Code { get; set; }
    }
}
