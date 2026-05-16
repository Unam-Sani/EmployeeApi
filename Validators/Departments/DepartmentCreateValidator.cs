using EmployeeApi.DTOs.Departments;
using FluentValidation;

namespace EmployeeApi.Validators.Departments;

public class DepartmentCreateValidator : AbstractValidator<DepartmentCreateDto>
{
    public DepartmentCreateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
    }
}
