namespace EmployeeApi.DTOs.Employees;

public class EmployeeReadDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
}
