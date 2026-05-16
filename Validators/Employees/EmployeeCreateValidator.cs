using EmployeeApi.DTOs.Employees;
using FluentValidation;

namespace EmployeeApi.Validators.Employees;

public class EmployeeCreateValidator : AbstractValidator<EmployeeCreateDto>
{
    public EmployeeCreateValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Salary)
            .GreaterThan(0).WithMessage("Salary must be greater than 0.");

        RuleFor(x => x.DepartmentId)
            .GreaterThan(0).WithMessage("DepartmentId must be valid.");
    }
}
