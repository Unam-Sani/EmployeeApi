using EmployeeApi.DTOs.Departments;

namespace EmployeeApi.Services.Interfaces;

public interface IDepartmentService
{
    Task<List<DepartmentReadDto>> GetAllAsync();
    Task<DepartmentReadDto> GetByIdAsync(int id);
    Task<DepartmentReadDto> CreateAsync(DepartmentCreateDto dto);
    Task<DepartmentReadDto> UpdateAsync(int id, DepartmentUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
