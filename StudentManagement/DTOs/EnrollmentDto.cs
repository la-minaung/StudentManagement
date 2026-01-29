namespace StudentManagement.DTOs
{
    public class EnrollmentDto
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;

        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;

        // Display "A", "B" or "In Progress"
        public string Grade { get; set; } = string.Empty;
    }
}
