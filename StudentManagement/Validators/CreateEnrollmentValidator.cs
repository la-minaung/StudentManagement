using FluentValidation;
using StudentManagement.DTOs;
using StudentManagement.Models;

namespace StudentManagement.Validators
{
    public class CreateEnrollmentValidator : AbstractValidator<CreateEnrollmentDto>
    {
        public CreateEnrollmentValidator()
        {
            RuleFor(x => x.StudentId)
                .GreaterThan(0).WithMessage("Student ID must be greater than 0.");

            RuleFor(x => x.CourseId)
                .GreaterThan(0).WithMessage("Course ID must be greater than 0.");

            RuleFor(x => x.Grade)
                .IsInEnum().WithMessage("Grade must be a valid grade value.")
                .When(x => x.Grade.HasValue);
        }
    }
}
