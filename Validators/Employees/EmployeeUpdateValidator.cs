using EmployeeApi.DTOs.Employees;
using FluentValidation;

namespace EmployeeApi.Validators.Employees;

public class EmployeeUpdateValidator : AbstractValidator<EmployeeUpdateDto>
{
    public EmployeeUpdateValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Salary)
            .GreaterThan(0);

        RuleFor(x => x.DepartmentId)
            .GreaterThan(0);
    }
}
