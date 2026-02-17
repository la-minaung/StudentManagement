namespace StudentManagement.DTOs
{
    public class CreateDepartmentDto
    {
        public required string Name { get; set; }

        public string? Code { get; set; }
    }
}
