using FluentValidation;
using StudentManagement.DTOs;

namespace StudentManagement.Validators
{
    public class CreateTeacherValidator : AbstractValidator<CreateTeacherDto>
    {
        public CreateTeacherValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .MaximumLength(50).WithMessage("First Name cannot exceed 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .MaximumLength(50).WithMessage("Last Name cannot exceed 50 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.");

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20).WithMessage("Phone Number cannot exceed 20 characters.")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

            RuleFor(x => x.Specialization)
                .MaximumLength(100).WithMessage("Specialization cannot exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.Specialization));

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Department ID must be greater than 0.");
        }
    }
}
