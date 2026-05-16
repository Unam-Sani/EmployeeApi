using EmployeeApi.Models;

namespace EmployeeApi.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAllAsync(); //Select * from Employees
    Task<Employee?> GetByIdAsync(int id); //Selct * from Employees where Id = 1
    Task<List<Employee>> GetFilteredAsync(int? departmentId, string? search);

    Task AddAsync(Employee employee);//Insert into Employees values (...)   
    Task UpdateAsync(Employee employee);    //Update Employees set ... where Id = 1
    Task DeleteAsync(Employee employee); //Delete from Employees where Id = 1
    
    Task SaveChangesAsync();
}
/*
The Service classes represent the business logic layer of the application.
They contain methods that perform operations on the data, such as retrieving, adding, updating, and deleting records. 
The Service classes interact with the Repository classes to access the database and perform these operations.

They:

- Validate rules
- Apply logic
- Map DTOs ↔ Models
- Call repositories
- Throw exceptions

Services are where the thinking happens.
*/