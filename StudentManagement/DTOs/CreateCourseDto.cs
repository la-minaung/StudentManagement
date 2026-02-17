namespace StudentManagement.DTOs
{
    public class CreateCourseDto
    {
        public required string Title { get; set; }

        public int Credits { get; set; }

        public int DepartmentId { get; set; }

        public int TeacherId { get; set; }
    }
}
