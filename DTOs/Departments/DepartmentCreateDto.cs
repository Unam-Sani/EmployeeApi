namespace EmployeeApi.DTOs.Departments;

public class DepartmentCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
