using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.DTOs;
using StudentManagement.Models;

namespace StudentManagement.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EnrollmentDto?> EnrollStudent(CreateEnrollmentDto createDto)
        {
            // 1. Validate Student Exists
            var student = await _context.Students.FindAsync(createDto.StudentId);
            if (student == null) return null;

            // 2. Validate Course Exists
            var course = await _context.Courses.FindAsync(createDto.CourseId);
            if (course == null) return null;

            // 3. Prevent Duplicates (Already enrolled?)
            bool exists = await _context.Enrollments.AnyAsync(e =>
                e.StudentId == createDto.StudentId &&
                e.CourseId == createDto.CourseId);

            if (exists) return null;

            // 4. Create Record
            var enrollment = new Enrollment
            {
                StudentId = createDto.StudentId,
                CourseId = createDto.CourseId,
                Grade = createDto.Grade ?? Models.Grade.InProgress
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return new EnrollmentDto
            {
                Id = enrollment.Id,
                StudentId = student.Id,
                StudentName = $"{student.FirstName} {student.LastName}",
                CourseId = course.Id,
                CourseTitle = course.Title,
                Grade = enrollment.Grade.ToString()!
            };
        }

        public async Task<List<EnrollmentDto>> GetStudentEnrollments(int studentId)
        {
            var enrollments = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .Where(e => e.StudentId == studentId) // FILTER BY STUDENT
                .AsNoTracking()
                .ToListAsync();

            return enrollments.Select(e => new EnrollmentDto
            {
                Id = e.Id,
                StudentId = e.StudentId,
                StudentName = $"{e.Student!.FirstName} {e.Student.LastName}",
                CourseId = e.CourseId,
                CourseTitle = e.Course!.Title,
                Grade = e.Grade?.ToString() ?? "Not Graded"
            }).ToList();
        }

        public async Task<bool> UnenrollStudent(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null) return false;

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
