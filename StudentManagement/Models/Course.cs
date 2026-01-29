using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Title { get; set; }

        [Required]
        public int Credits { get; set; }

        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }

        public int TeacherId { get; set; }
        [ForeignKey("TeacherId")]
        public Teacher? Teacher { get; set; }
    }
}
