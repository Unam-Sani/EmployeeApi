using AutoMapper;
using EmployeeApi.DTOs.Employees;
using EmployeeApi.Exceptions;
using EmployeeApi.Models;
using EmployeeApi.Repositories.Interfaces;
using EmployeeApi.Services.Interfaces;

namespace EmployeeApi.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IMapper _mapper;

    public EmployeeService(
        IEmployeeRepository employeeRepository,
        IDepartmentRepository departmentRepository,
        IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
        _mapper = mapper;
    }

    public async Task<List<EmployeeReadDto>> GetAllAsync()
    {
        var employees = await _employeeRepository.GetAllAsync();
        return _mapper.Map<List<EmployeeReadDto>>(employees);
    }

    public async Task<EmployeeReadDto> GetByIdAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);

        if (employee == null)
            throw new NotFoundException($"Employee with ID {id} not found.");

        return _mapper.Map<EmployeeReadDto>(employee);
    }

    public async Task<EmployeeReadDto> CreateAsync(EmployeeCreateDto dto)
    {
        // Validate department exists
        var department = await _departmentRepository.GetByIdAsync(dto.DepartmentId);
        if (department == null)
            throw new NotFoundException($"Department with ID {dto.DepartmentId} does not exist.");

        var employee = _mapper.Map<Employee>(dto);

        await _employeeRepository.AddAsync(employee);
        await _employeeRepository.SaveChangesAsync();

        return _mapper.Map<EmployeeReadDto>(employee);
    }

    public async Task<EmployeeReadDto> UpdateAsync(int id, EmployeeUpdateDto dto)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);

        if (employee == null)
            throw new NotFoundException($"Employee with ID {id} not found.");

        // Validate department exists
        var department = await _departmentRepository.GetByIdAsync(dto.DepartmentId);
        if (department == null)
            throw new NotFoundException($"Department with ID {dto.DepartmentId} does not exist.");

        _mapper.Map(dto, employee);

        await _employeeRepository.UpdateAsync(employee);
        await _employeeRepository.SaveChangesAsync();

        return _mapper.Map<EmployeeReadDto>(employee);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);

        if (employee == null)
            throw new NotFoundException($"Employee with ID {id} not found.");

        await _employeeRepository.DeleteAsync(employee);
        await _employeeRepository.SaveChangesAsync();

        return true;
    }

public async Task<List<EmployeeReadDto>> GetFilteredAsync(int? departmentId, string? search)
{
    var employees = await _employeeRepository.GetFilteredAsync(departmentId, search);
    return _mapper.Map<List<EmployeeReadDto>>(employees);
}
}