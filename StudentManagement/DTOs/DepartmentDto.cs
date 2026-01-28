using System.ComponentModel.DataAnnotations;

namespace StudentManagement.DTOs
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public required string Name { get; set; } = string.Empty;
        public string? Code { get; set; }
    }
}
