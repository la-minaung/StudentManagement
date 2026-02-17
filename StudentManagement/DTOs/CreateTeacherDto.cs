namespace StudentManagement.DTOs
{
    public class CreateTeacherDto
    {
        public required string FirstName { get; set; }

        public required string LastName { get; set;}

        public required string Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Specialization { get; set; }

        public int DepartmentId { get; set; }
    }
}
