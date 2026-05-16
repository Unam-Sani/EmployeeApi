using EmployeeApi.DTOs.Employees;

namespace EmployeeApi.Services.Interfaces;

public interface IEmployeeService
{
    Task<List<EmployeeReadDto>> GetAllAsync();
    Task<EmployeeReadDto> GetByIdAsync(int id);
    Task<EmployeeReadDto> CreateAsync(EmployeeCreateDto dto);
    Task<EmployeeReadDto> UpdateAsync(int id, EmployeeUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<List<EmployeeReadDto>> GetFilteredAsync(int? departmentId, string? search);

}
//This interface tells your API: "“Any EmployeeService must implement these methods.”