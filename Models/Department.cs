namespace EmployeeApi.Models;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public List<Employee> Employees { get; set; } = new();
}

/*
Last Line   public List<Employee> Employees { get; set; } = new();
The new() is shorthand for new List<Employee>(). It creates an empty bucket the moment a Department is created.
It’s like buying a filing cabinet. You want it to come with empty folders already inside so you can immediately start putting papers in them.
*/
