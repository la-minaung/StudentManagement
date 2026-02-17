namespace StudentManagement.Helpers
{
    public class TeacherQueryObject
    {
        public string? SearchTerm { get; set; } = null;
        public string? SortBy { get; set; } = null;
        public bool IsDescending { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int? DepartmentId { get; set; } = null;
    }
}