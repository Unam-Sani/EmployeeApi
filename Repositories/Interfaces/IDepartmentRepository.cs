using EmployeeApi.Models;

namespace EmployeeApi.Repositories.Interfaces;

public interface IDepartmentRepository
{
    Task<List<Department>> GetAllAsync();
    Task<Department?> GetByIdAsync(int id);
    Task AddAsync(Department department);
    Task UpdateAsync(Department department);
    Task DeleteAsync(Department department);
    Task SaveChangesAsync();
}
