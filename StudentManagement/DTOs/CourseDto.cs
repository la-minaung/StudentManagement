namespace StudentManagement.DTOs
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Credits { get; set; }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;

        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
    }
}
