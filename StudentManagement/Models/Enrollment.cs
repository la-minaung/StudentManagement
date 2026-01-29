using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models
{
    // Define Grade as an Enum so we don't store random text like "Good" or "Bad"
    public enum Grade
    {
        A, B, C, D, F, InProgress
    }

    public class Enrollment
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student? Student { get; set; }

        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course? Course { get; set; }

        public Grade? Grade { get; set; }
    }
}
