using EmployeeApi.Data;
using EmployeeApi.Models;
using EmployeeApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApi.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly ApplicationDbContext _context;

    public DepartmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Department>> GetAllAsync()
    {
        return await _context.Departments
            .Include(d => d.Employees)
            .ToListAsync();
    }

    public async Task<Department?> GetByIdAsync(int id)
    {
        return await _context.Departments
            .Include(d => d.Employees)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task AddAsync(Department department)
    {
        await _context.Departments.AddAsync(department);
    }

    public Task UpdateAsync(Department department)
    {
        _context.Departments.Update(department);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Department department)
    {
        _context.Departments.Remove(department);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
