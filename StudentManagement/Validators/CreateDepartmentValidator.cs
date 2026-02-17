using FluentValidation;
using StudentManagement.DTOs;

namespace StudentManagement.Validators
{
    public class CreateDepartmentValidator : AbstractValidator<CreateDepartmentDto>
    {
        public CreateDepartmentValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Department Name is required.")
                .MaximumLength(50).WithMessage("Department Name cannot exceed 50 characters.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Department Code is required.")
                .MaximumLength(10).WithMessage("Department Code cannot exceed 10 characters.");
        }
    }
}
