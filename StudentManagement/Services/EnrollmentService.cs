using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.DTOs;
using StudentManagement.Models;

namespace StudentManagement.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public EnrollmentService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            var enrollment = _mapper.Map<Enrollment>(createDto);
            if (enrollment.Grade == null)
            {
                enrollment.Grade = Models.Grade.InProgress;
            }

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            // Load navigation properties for mapping
            await _context.Entry(enrollment).Reference(e => e.Student).LoadAsync();
            await _context.Entry(enrollment).Reference(e => e.Course).LoadAsync();

            return _mapper.Map<EnrollmentDto>(enrollment);
        }

        public async Task<List<EnrollmentDto>> GetStudentEnrollments(int studentId)
        {
            var enrollments = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .Where(e => e.StudentId == studentId) // FILTER BY STUDENT
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<EnrollmentDto>>(enrollments);
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
