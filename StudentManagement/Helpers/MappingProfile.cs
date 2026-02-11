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
        }
    }
}
