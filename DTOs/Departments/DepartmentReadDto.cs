namespace EmployeeApi.DTOs.Departments;

public class DepartmentReadDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
