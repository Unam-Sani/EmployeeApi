using EmployeeApi.Data;
using EmployeeApi.Models;
using EmployeeApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApi.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _context;

    public EmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Employee>> GetAllAsync()
    {
        return await _context.Employees
            .Include(e => e.Department)
            .ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _context.Employees
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task AddAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
    }

    public Task UpdateAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Employee employee)
    {
        _context.Employees.Remove(employee);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    // 🔥 NEW: Filtering logic
    public async Task<List<Employee>> GetFilteredAsync(int? departmentId, string? search)
    {
        var query = _context.Employees
            .Include(e => e.Department)
            .AsQueryable();

        if (departmentId.HasValue)
            query = query.Where(e => e.DepartmentId == departmentId.Value);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(e =>
                e.FirstName.Contains(search) ||
                e.LastName.Contains(search) ||
                e.Email.Contains(search));

        return await query.ToListAsync();
    }
}
