using FluentValidation;
using StudentManagement.DTOs;

namespace StudentManagement.Validators
{
    public class CreateStudentValidator : AbstractValidator<CreateStudentDto>
    {
        public CreateStudentValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .MaximumLength(50).WithMessage("First Name cannot exceed 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.");

            RuleFor(x => x.Age)
                .InclusiveBetween(5, 100).WithMessage("Age must be between 5 and 100.");
        }
    }
}