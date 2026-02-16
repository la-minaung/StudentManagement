using AutoMapper;
using StudentManagement.DTOs;
using StudentManagement.Models;

namespace StudentManagement.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // 1. Source -> Destination
            // This handles GET requests (Database -> User)
            CreateMap<Student, StudentDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            // 2. Destination -> Source
            // This handles POST/PUT requests (User -> Database)
            CreateMap<CreateStudentDto, Student>();

            // Note: If property names match (e.g. "Email" -> "Email"), 
            // AutoMapper does it automatically! No need to type anything else.

            CreateMap<Teacher, TeacherDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));

            CreateMap<CreateTeacherDto, Teacher>();

            // Course mappings
            CreateMap<Course, CourseDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : "Unknown"))
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher != null ? $"{src.Teacher.FirstName} {src.Teacher.LastName}".Trim() : ""));

            CreateMap<CreateCourseDto, Course>();

            // Department mappings
            CreateMap<Department, DepartmentDto>();
            CreateMap<CreateDepartmentDto, Department>();

            // Enrollment mappings
            CreateMap<Enrollment, EnrollmentDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? $"{src.Student.FirstName} {src.Student.LastName}" : ""))
                .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course != null ? src.Course.Title : ""))
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade != null ? src.Grade.ToString() : "Not Graded"));

            CreateMap<CreateEnrollmentDto, Enrollment>();
        }
    }
}
